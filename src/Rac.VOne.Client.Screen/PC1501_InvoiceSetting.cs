using GrapeCity.Win.Editors;
using Rac.VOne.Common;
using Rac.VOne.Client.Common;
using Rac.VOne.Message;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.Extensions;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.InvoiceSettingService;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.SettingMasterService;
using Rac.VOne.Client.Screen.BillingService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>請求書設定</summary>
    public partial class PC1501 : VOneScreenBase
    {
        #region メンバー
        private List<Category> CollectCategoryList { get; set; }
        private List<int> ExcludeCollectCategoryIds { get; set; }
        private List<Setting> DueDateFormatList { get; set; }
        private InvoiceCommonSetting CommonSetting { get; set; }
        private InvoiceNumberSetting NumberSetting { get; set; }
        private InvoiceTemplateSetting TemplateSetting { get; set; }
        private int CollectCategoryId { get; set; }
        private bool TemplateChangeFlg { get; set; }
        private bool CommonChangeFlg { get; set; }
        private bool NumberingChangeFlg { get; set; }

        private const int TemplateTabIndex = 0;
        private const int CommonTabIndex = 1;
        private const int NumberingTabIndex = 2;

        private class FKeyNames
        {
            internal const string F01 = "登録";
            internal const string F02 = "クリア";
            internal const string F02_2 = "再読込";
            internal const string F03 = "削除";
            internal const string F05 = "出力設定";
            internal const string F06 = "PDF設定";
            internal const string F07 = "印刷設定";
            internal const string F10 = "終了";
        }
        #endregion

        #region OwnConfirmDialog
        private XmlMessenger messenger = new XmlMessenger();
        private DialogResult ShowDialog(string messageId, params string[] args)
            => messenger.GetMessageInfo(messageId, args).ShowMessageBox(this);
        private bool ShownConfirmDialog(string messageId, params string[] args)
            => ShowDialog(messageId, args) == DialogResult.Yes;
        #endregion

        public PC1501()
        {
            InitializeComponent();
            Text = "請求書設定";
            AddHandlers();
        }

        #region 画面の初期化
        private void PC1501_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                SetFunctionKey();

                ProgressDialog.Start(ParentForm, InitializeLoadDataAsync(), false, SessionKey);

                SetFormat();
                SetCollectCategoryCombo();
                SetOtherCombos();
                SetCommonSetting();

                if (NumberSetting == null)
                    ClearTabs(NumberingTabIndex);
                else
                    SetNumberingSetting();

                SetNumberingPreview();

                ClearTabs(TemplateTabIndex);
                CommonChangeFlg = false;
                NumberingChangeFlg = false;
                InitializeEnable();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task InitializeLoadDataAsync()
        {
            var loadTask = new List<Task>();
            if (ApplicationControl == null)
                loadTask.Add(LoadApplicationControlAsync());
            if (Company == null)
                loadTask.Add(LoadCompanyAsync());
            loadTask.Add(LoadControlColorAsync());
            await Task.WhenAll(loadTask);

            CommonSetting = await GetCommonSettingAsync();
            NumberSetting = await GetNumberSettingAsync();

            var categorySearch = new CategorySearch();
            categorySearch.CompanyId = CompanyId;
            categorySearch.CategoryType = CategoryType.Collect;
            CollectCategoryList = await GetCollectCategoryListAsync(categorySearch);
            DueDateFormatList = await GetReportDateFormatAsync(new string[] {nameof(ReportDateFormat)});
        }

        private void SetFormat()
        {
            txtPatternNo.PaddingChar = '0';
        }

        private void SetCollectCategoryCombo()
        {
            cmbCollectCategory.Items.Clear();
            cmbCollectCategory.Items.Add(new ListItem(string.Empty, 0));
            foreach (var category in CollectCategoryList)
                cmbCollectCategory.Items.Add(new ListItem(category.CodeAndName, category.Id));
        }

        private void SetOtherCombos()
        {
            cmbDueDateFormat.Items.Clear();
            foreach (var dateFormat in DueDateFormatList)
                cmbDueDateFormat.Items.Add(new ListItem(dateFormat.ItemValue, dateFormat.ItemKey));

            cmbResetTypeMonths.Items.Clear();
            for (int i = 1; i <= 12; i++)
                cmbResetTypeMonths.Items.Add(new ListItem(i.ToString() + "月", i));

            cmbNumberingDateType.Items.Clear();
            cmbNumberingDateType.Items.Add(new ListItem("請求日", 0));
            cmbNumberingDateType.Items.Add(new ListItem("請求締日", 1));

            cmbNumberingDateFormat.Items.Clear();
            cmbNumberingDateFormat.Items.Add(new ListItem("yyyy", 0));
            cmbNumberingDateFormat.Items.Add(new ListItem("yyyyMM", 1));
        }

        private void SetCommonSetting()
        {
            if (CommonSetting != null)
            {
                cbxExcludeAmountZero.Checked = Convert.ToInt32(CommonSetting.ExcludeAmountZero) == 1 ? true : false;
                cbxExcludeMinusAmount.Checked = Convert.ToInt32(CommonSetting.ExcludeMinusAmount) == 1 ? true : false;
                cbxExcludeMatchedData.Checked = Convert.ToInt32(CommonSetting.ExcludeMatchedData) == 1 ? true : false;
                cbxControlInputCharacter.Checked = Convert.ToInt32(CommonSetting.ControlInputCharacter) == 1 ? true : false;
            }

            InitializeCollectCategorySelection();
        }

        private void InitializeCollectCategorySelection()
        {
            var excludeCollectCategoryList = CollectCategoryList.Where(x => x.ExcludeInvoicePublish == 1).ToList();

            if (excludeCollectCategoryList.Count == CollectCategoryList.Count)
            {
                lblCategoryName.Text = "すべて";
            }
            else if (excludeCollectCategoryList.Count == 1)
            {
                lblCategoryName.Text = excludeCollectCategoryList.First().Name;
            }
            else if (excludeCollectCategoryList.Count > 1)
            {
                lblCategoryName.Text = "対象外有";
            }
            else
            {
                lblCategoryName.Clear();
            }

            ExcludeCollectCategoryIds = excludeCollectCategoryList.Select(x => x.Id).ToList();
        }

        private void SetNumberingSetting()
        {
            if (NumberSetting == null)
                return;

            cbxUseNumbering.Checked = Convert.ToInt32(NumberSetting.UseNumbering) == 1 ? true : false;
            if (!cbxUseNumbering.Checked) return;

            nmbLength.Value = NumberSetting.Length;
            rdoZeroPadding.Checked = Convert.ToInt32(NumberSetting.ZeroPadding) == 1 ? true : false;
            rdoNotZeroPadding.Checked = Convert.ToInt32(NumberSetting.ZeroPadding) == 0 ? true : false;

            rdoResetTypeYear.Checked = Convert.ToInt32(NumberSetting.ResetType) == 0 ? true : false;
            if (rdoResetTypeYear.Checked)
            {
                ListItem resetMonth = cmbResetTypeMonths.Items.Cast<ListItem>().FirstOrDefault(i => (int)i.SubItems[1].Value == NumberSetting.ResetMonth);
                cmbResetTypeMonths.SelectedIndex = resetMonth != null ? cmbResetTypeMonths.Items.IndexOf(resetMonth) : -1;
            }
            cmbResetTypeMonths.Enabled = rdoResetTypeYear.Checked;

            rdoResetTypeMonth.Checked = Convert.ToInt32(NumberSetting.ResetType) == 1 ? true : false;
            rdoResetTypeMaxium.Checked = Convert.ToInt32(NumberSetting.ResetType) == 2 ? true : false;

            rdoFormatTypeNumber.Checked = Convert.ToInt32(NumberSetting.FormatType) == 0 ? true : false;

            rdoFormatTypeUsingDate.Checked = Convert.ToInt32(NumberSetting.FormatType) == 1 ? true : false;
            if (rdoFormatTypeUsingDate.Checked)
            {
                ListItem dateType = cmbNumberingDateType.Items.Cast<ListItem>().FirstOrDefault(i => (int)i.SubItems[1].Value == NumberSetting.DateType);
                cmbNumberingDateType.SelectedIndex = dateType != null ? cmbNumberingDateType.Items.IndexOf(dateType) : -1;
                ListItem dateFormat = cmbNumberingDateFormat.Items.Cast<ListItem>().FirstOrDefault(i => (int)i.SubItems[1].Value == NumberSetting.DateFormat);
                cmbNumberingDateFormat.SelectedIndex = dateFormat != null ? cmbNumberingDateFormat.Items.IndexOf(dateFormat) : -1;
            }
            else
            {
                cmbNumberingDateType.SelectedIndex = -1;
                cmbNumberingDateType.Enabled = false;
                cmbNumberingDateFormat.SelectedIndex = -1;
                cmbNumberingDateFormat.Enabled = false;
            }

            rdoFormatTypeFixedString.Checked = Convert.ToInt32(NumberSetting.FormatType) == 2 ? true : false;
            if (rdoFormatTypeFixedString.Checked)
            {
                rdoFixedString.Checked = NumberSetting.FixedStringType == null ? false : Convert.ToInt32(NumberSetting.FixedStringType) == 0 ? true : false;
                txtNumberingFixedString.Text = NumberSetting.FixedString;
                rdoPatternFixedString.Checked = NumberSetting.FixedStringType == null ? false : Convert.ToInt32(NumberSetting.FixedStringType) == 1 ? true : false;
            }
            else
            {
                rdoFixedString.Checked = false;
                rdoFixedString.Enabled = false;
                txtNumberingFixedString.Text = string.Empty;
                txtNumberingFixedString.Enabled = false;
                rdoPatternFixedString.Checked = false;
                rdoPatternFixedString.Enabled = false;
            }

            rdoFormatNumber.Checked = Convert.ToInt32(NumberSetting.DisplayFormat) == 0 ? true : false;
            rdoNumberFormat.Checked = Convert.ToInt32(NumberSetting.DisplayFormat) == 1 ? true : false;
            txtDelimiter.Text = NumberSetting.Delimiter;
        }

        #endregion

        #region ファンクションキー初期化
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption(FKeyNames.F01);
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Save;

            BaseContext.SetFunction05Caption(FKeyNames.F05);
            BaseContext.SetFunction05Enabled(true);
            OnF05ClickHandler = CallExportFieldSetting;

            BaseContext.SetFunction06Caption(FKeyNames.F06);
            BaseContext.SetFunction06Enabled(true);
            OnF06ClickHandler = CallPdfSetting;

            BaseContext.SetFunction07Caption(FKeyNames.F07);
            BaseContext.SetFunction07Enabled(true);
            OnF07ClickHandler = OpenPrintSetting;

            BaseContext.SetFunction10Caption(FKeyNames.F10);
            OnF10ClickHandler = Exit;
        }

        private void SetFunctionKey()
        {
            switch (tbcInvoiceSetting.SelectedIndex)
            {
                case TemplateTabIndex:
                    BaseContext.SetFunction02Caption(FKeyNames.F02);
                    BaseContext.SetFunction02Enabled(true);
                    OnF02ClickHandler = Clear;

                    BaseContext.SetFunction03Caption(FKeyNames.F03);
                    BaseContext.SetFunction03Enabled(false);
                    OnF03ClickHandler = DeleteTemplateSetting;
                    break;
                case CommonTabIndex:
                    BaseContext.SetFunction02Caption(FKeyNames.F02_2);
                    BaseContext.SetFunction02Enabled(true);
                    OnF02ClickHandler = ReloadCommonSetting;

                    BaseContext.SetFunction03Caption("");
                    BaseContext.SetFunction03Enabled(false);
                    break;
                case NumberingTabIndex:
                    BaseContext.SetFunction02Caption(FKeyNames.F02_2);
                    BaseContext.SetFunction02Enabled(true);
                    OnF02ClickHandler = ReloadNumberingSetting;

                    BaseContext.SetFunction03Caption("");
                    BaseContext.SetFunction03Enabled(false);
                    break;

            }
        }
        #endregion

        #region F01/登録
        [OperationLog(FKeyNames.F01)]
        private void Save()
        {
            ClearStatusMessage();
            try
            {
                switch (tbcInvoiceSetting.SelectedIndex)
                {
                    case TemplateTabIndex:
                        SaveTemplateSetting();
                        break;
                    case CommonTabIndex:
                        SaveCommonSetting();
                        break;
                    case NumberingTabIndex:
                        SaveNumberingSetting();
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrSaveError);
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private bool RequireFieldsChecking()
        {
            switch (tbcInvoiceSetting.SelectedIndex)
            {
                case TemplateTabIndex:
                    if (string.IsNullOrWhiteSpace(txtPatternNo.Text))
                    {
                        ShowWarningDialog(MsgWngInputRequired, lblPatternNo.Text);
                        txtPatternNo.Focus();
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(txtPatternName.Text))
                    {
                        ShowWarningDialog(MsgWngInputRequired, lblPatternName.Text);
                        txtPatternName.Focus();
                        return false;
                    }
                    if ((!string.IsNullOrWhiteSpace(txtDueDateComment.Text)) && 
                        string.IsNullOrWhiteSpace(cmbDueDateFormat.Text))
                    {
                        ShowWarningDialog(MsgWngSelectionRequired, lblDueDateFormat.Text);
                        cmbDueDateFormat.Focus();
                        return false;
                    }
                    break;
                case NumberingTabIndex:
                    if (string.IsNullOrWhiteSpace(nmbLength.Text))
                    {
                        ShowWarningDialog(MsgWngInputRequired, lblLength.Text);
                        nmbLength.Focus();
                        return false;
                    }
                    if (rdoResetTypeYear.Checked && cmbResetTypeMonths.SelectedIndex == -1)
                    {
                        ShowWarningDialog(MsgWngSelectionRequired, "リセットする月");
                        cmbResetTypeMonths.Focus();
                        return false;
                    }

                    if ((!rdoFormatTypeUsingDate.Checked) && (rdoResetTypeYear.Checked || rdoResetTypeMonth.Checked))
                    {
                        var resetType = rdoResetTypeYear.Checked ? rdoResetTypeYear.Text : rdoResetTypeMonth.Text;
                        ShowWarningDialog(MsgWngSelectingNeedAnotherAssigment, resetType, rdoFormatTypeUsingDate.Text);
                        return false;
                    }

                    if (rdoFormatTypeUsingDate.Checked &&
                        cmbNumberingDateType.SelectedIndex == -1)
                    {
                        ShowWarningDialog(MsgWngSelectionRequired, rdoFormatTypeUsingDate.Text + "の" + lblDateType.Text);
                        cmbNumberingDateType.Focus();
                        return false;
                    }
                    if (rdoFormatTypeUsingDate.Checked &&
                        cmbNumberingDateFormat.SelectedIndex == -1)
                    {
                        ShowWarningDialog(MsgWngSelectionRequired, rdoFormatTypeUsingDate.Text + "の" + lblDateFormat.Text);
                        cmbNumberingDateFormat.Focus();
                        return false;
                    }
                    if (rdoFormatTypeFixedString.Checked &&
                        !rdoFixedString.Checked &&
                        !rdoPatternFixedString.Checked)
                    {
                        ShowWarningDialog(MsgWngSelectionRequired, rdoFormatTypeFixedString.Text);
                        return false;
                    }
                    if (rdoFormatTypeFixedString.Checked &&
                        rdoFixedString.Checked &&
                        string.IsNullOrWhiteSpace(txtNumberingFixedString.Text))
                    {
                        ShowWarningDialog(MsgWngInputRequired, rdoFixedString.Text);
                        txtNumberingFixedString.Focus();
                        return false;
                    }
                    break;
            }
            return true;
        }

        private bool IsCollectCategoryRegistedAtTemplate()
        {
            if (cmbCollectCategory.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(cmbCollectCategory.SelectedItem.SubItems[0].Value.ToString()) ||
                (TemplateSetting != null && TemplateSetting.CollectCategoryId == CollectCategoryId))
                return false;

            Task<bool> task = ExistCollectCategoryAtTemplateAsync(CollectCategoryId);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (!task.Result)
                return false;

            return true;
        }

        private void SaveTemplateSetting()
        {
            if (!RequireFieldsChecking()) return;

            ZeroLeftPaddingWithoutValidated();

            if (IsCollectCategoryRegistedAtTemplate())
            {
                cmbCollectCategory.Select();
                ShowWarningDialog(MsgWngCollectCategoryRegistedOtherPattern);
                return;
            }

            if (!ShowConfirmDialog(MsgQstConfirmSave))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }

            InvoiceTemplateSetting templateSetting = PrepareTemplateSetting();
            Task<InvoiceTemplateSettingResult> task = SaveTemplateSettingAsync(templateSetting);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (task.Result.ProcessResult.Result)
            {
                ClearTabs(TemplateTabIndex);
                DispStatusMessage(MsgInfSaveSuccess);
            }
            else
                ShowWarningDialog(MsgErrSaveError);

        }

        private void SaveCommonSetting()
        {
            if (!ShowConfirmDialog(MsgQstConfirmSave))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }

            var commonResult = false;
            var categoryResult = false;
            InvoiceCommonSetting commonSetting = PrepareCommonSetting();
            Task task = SaveCommonSettingAsync(commonSetting)
            .ContinueWith(async t =>
            {
                commonResult = t.Result.ProcessResult.Result;
                if (commonResult)
                {
                    List<Category> collectCategories = PrepareCollectCategories();
                    var updateResult = await UpdateCollectCategoryAsync(collectCategories);
                    categoryResult = updateResult.ProcessResult.Result;
                    if (categoryResult)
                    {
                        CollectCategoryList = null;
                        CollectCategoryList = updateResult.Categories;
                        SetCollectCategoryCombo();
                    }
                }
            }, TaskScheduler.FromCurrentSynchronizationContext())
            .Unwrap();
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (commonResult && categoryResult)
            {
                DispStatusMessage(MsgInfSaveSuccess);
                CommonChangeFlg = false;
            }
            else
                ShowWarningDialog(MsgErrSaveError);
        }

        private void SaveNumberingSetting()
        {
            if (cbxUseNumbering.Checked && !RequireFieldsChecking()) return;

            Task task = GetInvoiceNumberHistoriesAsync()
            .ContinueWith(async t =>
            {
                if (t.Result.ProcessResult.Result &&
                    t.Result.InvoiceNumberHistories.Any() &&
                    !ShownConfirmDialog(MsgQstConfirmResetInvoiceNumberSetting)) return;

                var hasHistories = t.Result.InvoiceNumberHistories.Any();
                if (hasHistories)
                {
                    var deleteResult = await DeleteNumberHistoriesAsync();
                    if (deleteResult.Count <= 0)
                    {
                        ShowWarningDialog(MsgErrSaveError);
                        return;
                    }
                }
                else if (!hasHistories && !ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                InvoiceNumberSetting numberSetting = PrepareNumberSetting();
                var saveResult = await SaveNumberSettingAsync(numberSetting);
                if (saveResult.ProcessResult.Result)
                {
                    DispStatusMessage(MsgInfSaveSuccess);
                    NumberingChangeFlg = false;
                }
                else
                    ShowWarningDialog(MsgErrSaveError);

            }, TaskScheduler.FromCurrentSynchronizationContext())
            .Unwrap();
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

        }
        #endregion

        #region PrepareData
        private InvoiceTemplateSetting PrepareTemplateSetting()
        {
            var templateSetting = new InvoiceTemplateSetting();
            templateSetting.CompanyId = CompanyId;
            templateSetting.Code = txtPatternNo.Text;
            templateSetting.Name = txtPatternName.Text.Trim();

            if ((cmbCollectCategory.SelectedIndex != -1) &&
                (!string.IsNullOrWhiteSpace(cmbCollectCategory.SelectedItem.SubItems[0].Value.ToString())))
                templateSetting.CollectCategoryId = CollectCategoryId;

            templateSetting.Title = txtTitle.Text.Trim();
            templateSetting.Greeting = txtGreeting.Text.Trim();
            templateSetting.DisplayStaff = rdoDisplayStaff.Checked ? 1 : 0;
            templateSetting.DueDateComment = txtDueDateComment.Text.Trim();

            if ((!string.IsNullOrWhiteSpace(txtDueDateComment.Text)) &&
                (cmbDueDateFormat.SelectedIndex != -1) &&
                (!string.IsNullOrWhiteSpace(cmbDueDateFormat.SelectedItem.SubItems[0].Value.ToString())))
            {
                var dueDateFormat = cmbDueDateFormat.SelectedItem.SubItems[0].Value.ToString();
                templateSetting.DueDateFormat = int.Parse(DueDateFormatList.Where(x => x.ItemValue == dueDateFormat).Select(x => x.ItemKey).FirstOrDefault());
            }

            templateSetting.TransferFeeComment = txtTransferFeeComment.Text.Trim();
            templateSetting.FixedString = txtTemplateFixedString.Text.Trim();
            templateSetting.CreateBy = Login.UserId;
            templateSetting.UpdateBy = Login.UserId;

            return templateSetting;
        }

        private InvoiceCommonSetting PrepareCommonSetting()
        {
            var commonSetting = new InvoiceCommonSetting();
            commonSetting.CompanyId = CompanyId;
            commonSetting.ExcludeAmountZero = cbxExcludeAmountZero.Checked ? 1 : 0;
            commonSetting.ExcludeMinusAmount = cbxExcludeMinusAmount.Checked ? 1 : 0;
            commonSetting.ExcludeMatchedData = cbxExcludeMatchedData.Checked ? 1 : 0;
            commonSetting.ControlInputCharacter = cbxControlInputCharacter.Checked ? 1 : 0;
            commonSetting.CreateBy = Login.UserId;
            commonSetting.UpdateBy = Login.UserId;

            return commonSetting;
        }

        private List<Category> PrepareCollectCategories()
        {
            var collectCategories = new List<Category>();
            foreach (var category in CollectCategoryList)
            {
                if (ExcludeCollectCategoryIds.Any(x => x == category.Id))
                    category.ExcludeInvoicePublish = 1;
                else
                    category.ExcludeInvoicePublish = 0;

                collectCategories.Add(category);
            }

            return collectCategories;
        }

        private InvoiceNumberSetting PrepareNumberSetting()
        {
            decimal length = nmbLength.Value ?? 0M;
            var numberSetting = new InvoiceNumberSetting();
            numberSetting.CompanyId = CompanyId;
            numberSetting.UseNumbering = cbxUseNumbering.Checked ? 1 : 0;
            numberSetting.Length = Decimal.ToInt32(length);
            numberSetting.ZeroPadding = rdoZeroPadding.Checked ? 1 : 0;
            numberSetting.ResetType = rdoResetTypeYear.Checked ? 0 : rdoResetTypeMonth.Checked ? 1 : 2;

            if (rdoResetTypeYear.Checked)
                numberSetting.ResetMonth = Convert.ToInt32(cmbResetTypeMonths.SelectedItem.SubItems[1].Value);
            else
                numberSetting.ResetMonth = null;

            numberSetting.FormatType = rdoFormatTypeNumber.Checked ? 0 : rdoFormatTypeUsingDate.Checked ? 1 : 2;

            if (rdoFormatTypeUsingDate.Checked)
            {
                numberSetting.DateType = Convert.ToInt32(cmbNumberingDateType.SelectedItem.SubItems[1].Value);
                numberSetting.DateFormat = Convert.ToInt32(cmbNumberingDateFormat.SelectedItem.SubItems[1].Value);
            }
            else
            {
                numberSetting.DateType = null;
                numberSetting.DateFormat = null;
            }

            if (rdoFormatTypeFixedString.Checked)
            {
                numberSetting.FixedStringType = rdoFixedString.Checked ? 0 : 1;
                numberSetting.FixedString = rdoPatternFixedString.Checked ? string.Empty : txtNumberingFixedString.Text.Trim();
            }
            else
            {
                numberSetting.FixedStringType = null;
                numberSetting.FixedString = string.Empty;
            }

            numberSetting.DisplayFormat = rdoFormatNumber.Checked ? 0 : 1;
            numberSetting.Delimiter = txtDelimiter.Text;
            numberSetting.CreateBy = Login.UserId;
            numberSetting.UpdateBy = Login.UserId;

            return numberSetting;
        }

        #endregion

        #region F2 クリア処理
        [OperationLog(FKeyNames.F02)]
        private void Clear()
        {
            if (TemplateChangeFlg && !ShowConfirmDialog(MsgQstConfirmClear))
                return;
            ClearTabs(TemplateTabIndex);
        }

        private void ClearTabs(int tabIndex)
        {
            ClearStatusMessage();
            switch (tabIndex)
            {
                case TemplateTabIndex:
                    ClearStatusMessage();
                    txtPatternNo.Clear();
                    txtPatternName.Clear();
                    txtTitle.Clear();
                    txtGreeting.Clear();
                    txtDueDateComment.Clear();
                    txtTransferFeeComment.Clear();
                    txtTemplateFixedString.Clear();
                    rdoDisplayStaff.Checked = true;
                    cmbCollectCategory.SelectedIndex = -1;
                    cmbDueDateFormat.SelectedIndex = -1;
                    txtPatternNo.Enabled = true;
                    btnPatternSearch.Enabled = true;
                    BaseContext.SetFunction03Enabled(false);
                    txtPatternNo.Focus();
                    TemplateChangeFlg = false;
                    break;
                case NumberingTabIndex:

                    cbxUseNumbering.Checked = false;
                    lblNumberingPreview.Clear();

                    nmbLength.Clear();
                    rdoZeroPadding.Checked = true;
                    rdoNotZeroPadding.Checked = false;
                    gbxNumberingSetting.Enabled = false;

                    rdoResetTypeYear.Checked = true;
                    rdoResetTypeMonth.Checked = false;
                    rdoResetTypeMaxium.Checked = false;
                    cmbResetTypeMonths.SelectedIndex = -1;
                    gbxNumberingReset.Enabled = false;

                    rdoFormatTypeNumber.Checked = true;
                    rdoFormatTypeUsingDate.Checked = false;
                    cmbNumberingDateType.SelectedIndex = -1;
                    cmbNumberingDateFormat.SelectedIndex = -1;
                    rdoFormatTypeFixedString.Checked = false;
                    rdoFixedString.Checked = false;
                    rdoPatternFixedString.Checked = false;
                    txtNumberingFixedString.Clear();
                    gbxNumberingFormat.Enabled = false;

                    rdoFormatNumber.Checked = true;
                    rdoNumberFormat.Checked = false;
                    txtDelimiter.Clear();
                    gbxNumberingLayout.Enabled = false;
                    break;
            }
        }

        #endregion

        #region F2 再読込
        [OperationLog(FKeyNames.F02_2)]
        private void ReloadCommonSetting()
        {
            if (CommonChangeFlg && !ShowConfirmDialog(MsgQstConfirmUpdateData))
                return;

            try
            {
                CommonSetting = null;
                Task<InvoiceCommonSetting> task = GetCommonSettingAsync();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                CommonSetting = task.Result;
                SetCommonSetting();
                CommonChangeFlg = false;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void ReloadNumberingSetting()
        {
            if (NumberingChangeFlg && !ShowConfirmDialog(MsgQstConfirmUpdateData))
                return;

            try
            {
                NumberSetting = null;
                Task<InvoiceNumberSetting> task = GetNumberSettingAsync();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                NumberSetting = task.Result;
                SetNumberingSetting();
                SetNumberingPreview();
                NumberingChangeFlg = false;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        #endregion

        #region F3 削除処理
        [OperationLog(FKeyNames.F03)]
        private void DeleteTemplateSetting()
        {
            if (string.IsNullOrWhiteSpace(txtPatternNo.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblPatternNo.Text);
                txtPatternNo.Focus();
                return;
            }
            if (!ShowConfirmDialog(MsgQstConfirmDelete))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }

            try
            {
                //if (IsCollectCategoryAtBilling())
                //{
                //    ShowWarningDialog(MsgWngDeleteConstraint, new string[] { "請求データ", "回収区分" });
                //    return;
                //}

                Task<CountResult> task = DeleteTemplateSettingAsync(TemplateSetting.Id);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (task.Result.Count > 0)
                {
                    ClearTabs(TemplateTabIndex);
                    DispStatusMessage(MsgInfDeleteSuccess);
                }
                else
                    ShowWarningDialog(MsgErrDeleteError);

            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrDeleteError);
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private bool IsCollectCategoryAtBilling()
        {
            if (TemplateSetting.CollectCategoryId == null)
                return false;

            Task<bool> task = ExistBillingCollectCategoryAsync((int)TemplateSetting.CollectCategoryId);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (task.Result)
                return true;

            return false;
        }

        #endregion

        #region F5 出力設定
        [OperationLog(FKeyNames.F05)]
        private void CallExportFieldSetting()
        {
            ClearStatusMessage();
            using (var form = ApplicationContext.Create(nameof(PH9904)))
            {
                var screen = form.GetAll<PH9904>().First();
                screen.ExportFileType = 2;
                screen.InitializeParentForm("出力項目設定");
                ApplicationContext.ShowDialog(ParentForm, form, true);
            }
        }
        #endregion

        #region F6 PDF出力設定
        [OperationLog(FKeyNames.F06)]
        private void CallPdfSetting()
        {
            ClearStatusMessage();
            var form = ApplicationContext.Create(nameof(PC1503));
            var screen = form.GetAll<PC1503>().FirstOrDefault();
            screen.lblNumber.Text = "請求書番号";
            screen.lblDate.Text = "請求日";
            screen.ReturnScreen = this;
            var result = ApplicationContext.ShowDialog(BaseForm, form);
        }
        #endregion

        #region F7 印刷設定
        [OperationLog(FKeyNames.F07)]
        private void OpenPrintSetting()
        {
            using (var form = ApplicationContext.Create(nameof(PH9905)))
            {
                var screen = form.GetAll<PH9905>().First();
                screen.FormName = nameof(PC0401);
                screen.InitializeParentForm("帳票設定");
                var result = ApplicationContext.ShowDialog(ParentForm, form, true);
            }
        }
        #endregion

        #region F10 終了処理
        [OperationLog(FKeyNames.F10)]
        private void Exit()
        {
            bool changeFlg = TemplateChangeFlg || CommonChangeFlg || NumberingChangeFlg;
            if (changeFlg && !ShowConfirmDialog(MsgQstConfirmClose))
                return;

            ParentForm.Close();
        }
        #endregion

        #region  入力項目変更イベント処理
        private void AddHandlers()
        {
            foreach (var control in this.GetAll<Control>())
            {
                if (control is Common.Controls.VOneTextControl
                    || control is Common.Controls.VOneNumberControl)
                {
                    control.TextChanged += OnContentChanged;
                }
                else if (control is Common.Controls.VOneComboControl)
                {
                    ((Common.Controls.VOneComboControl)control).SelectedIndexChanged += OnContentChanged;
                }
                else if (control is CheckBox)
                {
                    ((CheckBox)control).CheckedChanged += OnContentChanged;
                }
                else if (control is RadioButton)
                {
                    ((RadioButton)control).CheckedChanged += OnContentChanged;
                }
            }
        }

        private void OnContentChanged(object sender, EventArgs e)
        {
            switch (tbcInvoiceSetting.SelectedIndex)
            {
                case TemplateTabIndex:
                    TemplateChangeFlg = true;
                    break;
                case CommonTabIndex:
                    CommonChangeFlg = true;
                    break;
                case NumberingTabIndex:
                    NumberingChangeFlg = true;
                    break;
            }
        }

        #endregion

        #region タブ変更イベント
        private void tbcInvoiceSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearStatusMessage();
            SetFunctionKey();
        }
        #endregion

        #region Validatedイベント
        private void ZeroLeftPaddingWithoutValidated()
        {
            if (IsNeedValidate(0, txtPatternNo.TextLength, txtPatternNo.MaxLength))
            {
                txtPatternNo.Text = ZeroLeftPadding(txtPatternNo);
                txtPatternNo_Validated(null, null);
            }
        }

        private void txtPatternNo_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            string code = txtPatternNo.Text;
            if (string.IsNullOrWhiteSpace(code))
                return;

            try
            {
                Task<InvoiceTemplateSetting> task = GetTemplateSettingByCodeAsync(code);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                TemplateSetting = task.Result;
                if (TemplateSetting == null)
                {
                    DispStatusMessage(MsgInfNewPatternNo, lblPatternNo.Text);
                    txtPatternName.Focus();
                    return;
                }
                SetTemplateSetting();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void nmbLength_Validated(object sender, EventArgs e)
        {
            SetNumberingPreview();
        }

        private void txtNumberingFixedString_Validated(object sender, EventArgs e)
        {
            SetNumberingPreview();
        }

        private void txtDelimiter_Validated(object sender, EventArgs e)
        {
            SetNumberingPreview();
        }

        private void txtDueDateComment_Validated(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtDueDateComment.Text))
            {
                cmbDueDateFormat.Required = true;
            }
            else
            {
                cmbDueDateFormat.SelectedIndex = -1;
                cmbDueDateFormat.Required = false;
            }
        }

        #endregion

        #region Combo_SelectedIndexChangedイベント
        private void cmbCollectCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCollectCategory.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(cmbCollectCategory.SelectedItem.SubItems[0].Value.ToString()))
                return;

            var category = cmbCollectCategory.SelectedItem.SubItems[0].Value.ToString();
            CollectCategoryId = CollectCategoryList.Where(y => y.Code == (category.Split('：')[0])).Select(y => y.Id).FirstOrDefault();
        }

        private void cmbNumberingDateFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbNumberingDateFormat.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(cmbNumberingDateFormat.SelectedItem.SubItems[0].Value.ToString()))
                return;
            SetNumberingPreview();
        }


        #endregion

        #region 検索ボタンClickイベント処理

        private void btnPatternSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                var templateSetting = this.ShowInvoiceTemplateSettingSearchDialog();
                if (templateSetting != null)
                {
                    TemplateSetting = templateSetting;
                    txtPatternNo.Text = TemplateSetting.Code;
                    SetTemplateSetting();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnCategorySearch_Click(object sender, EventArgs e)
        {
            var allSelected = CollectCategoryList.Count == ExcludeCollectCategoryIds.Count;
            var oldExcludeCollectCategoryIds = ExcludeCollectCategoryIds;
            using (var form = ApplicationContext.Create(nameof(PC1502)))
            {
                var screen = form.GetAll<PC1502>().First();
                screen.AllSelected = allSelected;
                screen.InitialIds = ExcludeCollectCategoryIds;
                screen.InitializeParentForm("対象外回収区分");

                var dialogResult = ApplicationContext.ShowDialog(ParentForm, form, true);
                if (dialogResult == DialogResult.OK)
                {
                    ClearStatusMessage();
                    lblCategoryName.Text = screen.SelectedState;
                    ExcludeCollectCategoryIds = screen.SelectedIds;
                    if (ExcludeCollectCategoryIds.Except(oldExcludeCollectCategoryIds).ToList().Any())
                        CommonChangeFlg = true;
                }
            }
        }

        #endregion

        #region CheckBox_CheckedChanged
        private void cbxUseNumbering_CheckedChanged(object sender, EventArgs e)
        {
            InitializeEnable();
        }

        private void InitializeEnable()
        {
            gbxNumberingSetting.Enabled = cbxUseNumbering.Checked;
            gbxNumberingReset.Enabled = cbxUseNumbering.Checked;
            gbxNumberingFormat.Enabled = cbxUseNumbering.Checked;
            gbxNumberingLayout.Enabled = cbxUseNumbering.Checked && !rdoFormatTypeNumber.Checked;
        }

        #endregion

        #region RadioButton_CheckChanged
        private void rdoResetTypeYear_CheckedChanged(object sender, EventArgs e)
        {
            cmbResetTypeMonths.Enabled = rdoResetTypeYear.Checked;

            SetUsingDateFixed(rdoResetTypeYear.Checked);
        }

        private void SetUsingDateFixed(bool doFixed)
        {
            if (doFixed)
            {
                rdoFormatTypeUsingDate.Checked = true;
                rdoFormatTypeNumber.Enabled = false;
                rdoFormatTypeUsingDate.Enabled = false;
                rdoFormatTypeFixedString.Enabled = false;

            }
            else
            {
                rdoFormatTypeNumber.Enabled = true;
                rdoFormatTypeUsingDate.Enabled = true;
                rdoFormatTypeFixedString.Enabled = true;
            }
        }

        private void rdoResetTypeMonth_CheckedChanged(object sender, EventArgs e)
        {
            cmbResetTypeMonths.SelectedIndex = -1;
            cmbResetTypeMonths.Enabled = false;
            SetUsingDateFixed(rdoResetTypeMonth.Checked);
        }

        private void rdoResetTypeMaxium_CheckedChanged(object sender, EventArgs e)
        {
            cmbResetTypeMonths.SelectedIndex = -1;
            cmbResetTypeMonths.Enabled = false;
        }

        private void rdoFormatTypeBilledDate_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoFormatTypeUsingDate.Checked)
            {
                cmbNumberingDateType.Enabled = true;
                cmbNumberingDateFormat.Enabled = true;
            }
            else
            {
                cmbNumberingDateType.SelectedIndex = -1;
                cmbNumberingDateType.Enabled = false;
                cmbNumberingDateFormat.SelectedIndex = -1;
                cmbNumberingDateFormat.Enabled = false;
            }
            SetNumberingPreview();
        }

        private void rdoFormatTypeFixedString_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoFormatTypeFixedString.Checked)
            {
                rdoFixedString.Checked = false;
                rdoFixedString.Enabled = true;
                rdoPatternFixedString.Checked = false;
                rdoPatternFixedString.Enabled = true;
            }
            else
            {
                rdoFixedString.Checked = false;
                rdoFixedString.Enabled = false;
                txtNumberingFixedString.Clear();
                txtNumberingFixedString.Enabled = false;
                rdoPatternFixedString.Checked = false;
                rdoPatternFixedString.Enabled = false;
            }
            SetNumberingPreview();
        }

        private void rdoFixedString_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoFixedString.Checked)
            {
                txtNumberingFixedString.Enabled = true;
                txtNumberingFixedString.Focus();
            }
            else
            {
                txtNumberingFixedString.Clear();
                txtNumberingFixedString.Enabled = false;
            }
            SetNumberingPreview();
        }

        private void rdoZeroPadding_CheckedChanged(object sender, EventArgs e)
        {
            SetNumberingPreview();
        }

        private void rdoPatternFixedString_CheckedChanged(object sender, EventArgs e)
        {
            SetNumberingPreview();
        }

        private void rdoFormatNumber_CheckedChanged(object sender, EventArgs e)
        {
            SetNumberingPreview();
        }

        private void rdoFormatTypeNumber_CheckedChanged(object sender, EventArgs e)
        {
            gbxNumberingLayout.Enabled = !rdoFormatTypeNumber.Checked;
        }

        #endregion

        #region 共通
        private void SetTemplateSetting()
        {
            txtPatternName.Text = TemplateSetting.Name;

            ListItem categoryItem = cmbCollectCategory.Items.Cast<ListItem>().FirstOrDefault(i => (int)i.SubItems[1].Value == TemplateSetting.CollectCategoryId);
            cmbCollectCategory.SelectedIndex = categoryItem != null ? cmbCollectCategory.Items.IndexOf(categoryItem) : -1;

            txtTitle.Text = TemplateSetting.Title;
            txtGreeting.Text = TemplateSetting.Greeting;

            rdoDisplayStaff.Checked = Convert.ToInt32(TemplateSetting.DisplayStaff) == 1;
            rdoNotDisplayStaff.Checked = Convert.ToInt32(TemplateSetting.DisplayStaff) == 0;
            txtDueDateComment.Text = TemplateSetting.DueDateComment;

            ListItem dueDateItem = cmbDueDateFormat.Items.Cast<ListItem>().FirstOrDefault(i => (string)i.SubItems[1].Value == TemplateSetting.DueDateFormat.ToString());
            cmbDueDateFormat.SelectedIndex = dueDateItem != null ? cmbDueDateFormat.Items.IndexOf(dueDateItem) : -1;

            txtTransferFeeComment.Text = TemplateSetting.TransferFeeComment;
            txtTemplateFixedString.Text = TemplateSetting.FixedString;

            BaseContext.SetFunction03Enabled(true);
            txtPatternNo.Enabled = false;
            btnPatternSearch.Enabled = false;
            TemplateChangeFlg = false;
        }

        private void SetNumberingPreview()
        {
            if (!cbxUseNumbering.Checked)
            {
                lblNumberingPreview.Text = string.Empty;
                return;
            }

            string value = "1";
            decimal length = nmbLength.Value ?? 0M;
            string fixedString = string.Empty;
            string dateFormat = string.Empty;
            string delimiter = !rdoFormatTypeNumber.Checked ? txtDelimiter.Text : string.Empty;

            if (rdoZeroPadding.Checked)
                value = value.PadLeft((int)length, '0');

            if (rdoFixedString.Checked)
                fixedString = txtNumberingFixedString.Text;

            if (rdoPatternFixedString.Checked)
                fixedString = "[文面パターン指定文字列]";

            if (rdoFormatTypeUsingDate.Checked && cmbNumberingDateFormat.SelectedIndex != -1)
            {
                dateFormat = Convert.ToInt32(cmbNumberingDateFormat.SelectedItem.SubItems[1].Value) == 0 ?
                    DateTime.Now.ToString("yyyy") :
                    DateTime.Now.ToString("yyyyMM");
            }

            if (rdoFormatNumber.Checked)
                value = fixedString + dateFormat + delimiter + value;
            else
                value = value + delimiter + fixedString + dateFormat;

            lblNumberingPreview.Text = value;
        }

        #endregion

        #region WebService

        #region 共通
        private async Task<List<Category>> GetCollectCategoryListAsync(CategorySearch categorySearch)
            => await ServiceProxyFactory.DoAsync(async (CategoryMasterClient client) =>
            {
                List<Category> list = null;
                var result = await client.GetItemsAsync(SessionKey, categorySearch);
                if (result.ProcessResult.Result)
                    list = result.Categories;
                return list ?? new List<Category>();
            });

        private async Task<List<Setting>> GetReportDateFormatAsync(string[] itemIds)
            => await ServiceProxyFactory.DoAsync(async (SettingMasterClient client) =>
            {
                List<Setting> list = null;
                var result = await client.GetItemsAsync(SessionKey, itemIds);
                if (result.ProcessResult.Result)
                    list = result.Settings;
                return list;
            });

        private async Task<bool> ExistBillingCollectCategoryAsync(int categoryId)
            => await ServiceProxyFactory.DoAsync(async (BillingServiceClient client) =>
            {
                var result = await client.ExistCollectCategoryAsync(SessionKey, categoryId);
                return result.Exist;
            });

        #endregion

        #region CommonSetting

        private async Task<InvoiceCommonSetting> GetCommonSettingAsync()
            => await ServiceProxyFactory.DoAsync(async (InvoiceSettingServiceClient client) =>
            {
                InvoiceCommonSetting commonSetting = null;
                var result = await client.GetInvoiceCommonSettingAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    commonSetting = result.InvoiceCommonSetting;
                return commonSetting;
            });

        private async Task<InvoiceCommonSettingResult> SaveCommonSettingAsync(InvoiceCommonSetting commonSetting)
            => await ServiceProxyFactory.DoAsync(async (InvoiceSettingServiceClient client) =>
            {
                var result = await client.SaveInvoiceCommonSettingAsync(SessionKey, commonSetting);
                return result;
            });

        private async Task<CategoriesResult> UpdateCollectCategoryAsync(IEnumerable<Category> collectCategories)
            => await ServiceProxyFactory.DoAsync(async (InvoiceSettingServiceClient client) =>
            {
                var result = await client.UpdateCollectCategoryAsync(SessionKey, collectCategories.ToArray());
                return result;
            });


        #endregion

        #region NumberSetting

        private async Task<InvoiceNumberSetting> GetNumberSettingAsync()
            => await ServiceProxyFactory.DoAsync(async (InvoiceSettingServiceClient client) =>
            {
                InvoiceNumberSetting numberSetting = null;
                var result = await client.GetInvoiceNumberSettingAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    numberSetting = result.InvoiceNumberSetting;
                return numberSetting;
            });

        private async Task<InvoiceNumberSettingResult> SaveNumberSettingAsync(InvoiceNumberSetting numberSetting)
            => await ServiceProxyFactory.DoAsync(async (InvoiceSettingServiceClient client) =>
            {
                var result = await client.SaveInvoiceNumberSettingAsync(SessionKey, numberSetting);
                return result;
            });

        private async Task<InvoiceNumberHistoriesResult> GetInvoiceNumberHistoriesAsync()
            => await ServiceProxyFactory.DoAsync(async (InvoiceSettingServiceClient client) =>
            {
                var result = await client.GetInvoiceNumberHistoriesAsync(SessionKey, CompanyId);
                return result;
            });

        private async Task<CountResult> DeleteNumberHistoriesAsync()
            => await ServiceProxyFactory.DoAsync(async (InvoiceSettingServiceClient client) =>
            {
                var result = await client.DeleteInvoiceNumberHistoriesAsync(SessionKey, CompanyId);
                return result;
            });

        #endregion

        #region TemplateSetting

        private async Task<bool> ExistCollectCategoryAtTemplateAsync(int collectCategoryId)
            => await ServiceProxyFactory.DoAsync(async (InvoiceSettingServiceClient client) =>
            {
                var result = await client.ExistCollectCategoryAtTemplateAsync(SessionKey, collectCategoryId);
                return result.Exist;
            });

        private async Task<InvoiceTemplateSetting> GetTemplateSettingByCodeAsync(string Code)
            => await ServiceProxyFactory.DoAsync(async (InvoiceSettingServiceClient client) =>
            {
                var result = await client.GetInvoiceTemplateSettingByCodeAsync(SessionKey, CompanyId, Code);
                return result.InvoiceTemplateSetting;
            });

        private async Task<InvoiceTemplateSettingResult> SaveTemplateSettingAsync(InvoiceTemplateSetting templateSetting)
            => await ServiceProxyFactory.DoAsync(async (InvoiceSettingServiceClient client) =>
            {
                var result = await client.SaveInvoiceTemplateSettingAsync(SessionKey, templateSetting);
                return result;
            });

        private async Task<CountResult> DeleteTemplateSettingAsync(int templateId)
            => await ServiceProxyFactory.DoAsync(async (InvoiceSettingServiceClient client) =>
            {
                var result = await client.DeleteInvoiceTemplateSettingAsync(SessionKey, templateId);
                return result;
            });


        #endregion

        #endregion

    }
}
