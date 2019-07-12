using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.ReminderService;
using Rac.VOne.Client.Screen.ReminderSettingService;
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
    /// <summary>督促設定</summary>
    public partial class PI0401 : VOneScreenBase
    {
        #region 変数宣言
        private enum SettingTabIndex
        {
            CommonSetting,
            TemplateSetting,
            LevelSetting,
            SummarySetting,
        }
        private ReminderCommonSetting CommonSetting { get; set; }
        private List<ReminderLevelSetting> LevelSettingList { get; set; }
        private ReminderLevelSetting LevelSetting { get; set; }
        private List<ReminderSummarySetting> SummarySettingList { get; set; }
        private List<ColumnNameSetting> ColumnNameSetting { get; set; }
        private int TemplateId { get; set; }
        private bool IsExistReminderData { get; set; }
        private bool CommonChangeFlg { get; set; }
        private bool TemplateChangeFlg { get; set; }
        private bool LevelChangeFlg { get; set; }
        private bool SummaryChangeFlg { get; set; }
        private string CellName(string value) => $"cel{value}";
        #endregion

        public PI0401()
        {
            InitializeComponent();
            grdSummarySetting.SetupShortcutKeys();
            Text = "督促設定";
            AddHandlers();
        }

        #region 画面の初期化
        private void PI0401_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                SetFunctionKey();

                var loadTask = new List<Task>();
                if (ApplicationControl == null)
                    loadTask.Add(LoadApplicationControlAsync());
                if (Company == null)
                    loadTask.Add(LoadCompanyAsync());

                loadTask.Add(LoadControlColorAsync());
                loadTask.Add(ReminderExist());
                loadTask.Add(GetCommonSettingAsync());
                loadTask.Add(GetLevelSettingsAsync());
                loadTask.Add(GetSummarySettingsAsync());
                loadTask.Add(GetColumnNameSettingAsync());
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                SetFormat();
                SetCommonSettingCombo();
                SetLevelCombo();
                InitializeGrid();
                InitializeTabs();
                CommonChangeFlg = false;
                SummaryChangeFlg = false;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void SetFormat()
        {
            txtTemplatePatternNo.PaddingChar = '0';
            txtLevelPatternNo.PaddingChar = '0';
        }

        private void SetCommonSettingCombo()
        {
            foreach(var s in ColumnNameSetting.Where(x => x.TableName == nameof(Billing)))
            {
                cmbOutputDetailItem.Items.Add(new GrapeCity.Win.Editors.ListItem(s.DisplayColumnName, s.ColumnName));
            }
            cmbOutputDetailItem.TextSubItemIndex = 0;
            cmbOutputDetailItem.ValueSubItemIndex = 1;

            cmbDepartmentSummaryMode.Items.Add(new GrapeCity.Win.Editors.ListItem("営業担当者の所属部門で集計", (int)DepartmentSummaryMode.StaffDepartment));
            cmbDepartmentSummaryMode.Items.Add(new GrapeCity.Win.Editors.ListItem("請求データの請求部門で集計", (int)DepartmentSummaryMode.BillDepartment));

            cmbCalculateBaseDate.Items.Add(new GrapeCity.Win.Editors.ListItem("当初予定日", (int)CalculateBaseDate.OriginalDueAt));
            cmbCalculateBaseDate.Items.Add(new GrapeCity.Win.Editors.ListItem("入金予定日", (int)CalculateBaseDate.DueAt));
            cmbCalculateBaseDate.Items.Add(new GrapeCity.Win.Editors.ListItem("請求日", (int)CalculateBaseDate.BilledAt));

            cmbIncludeOnTheDay.Items.Add(new GrapeCity.Win.Editors.ListItem("含めない", (int)IncludeOnTheDay.Exclude));
            cmbIncludeOnTheDay.Items.Add(new GrapeCity.Win.Editors.ListItem("含める", (int)IncludeOnTheDay.Include));
        }

        private void InitializeGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  50, nameof(ReminderSummarySetting.Available)    , dataField: nameof(ReminderSummarySetting.Available)    , cell: builder.GetCheckBoxCell(), caption: "集計" , readOnly: false),
                new CellSetting(height, 250, nameof(ReminderSummarySetting.ColumnNameJp) , dataField: nameof(ReminderSummarySetting.ColumnNameJp) , cell: builder.GetTextBoxCell(), caption: "項目名"),
                new CellSetting(height,   0, nameof(ReminderSummarySetting.ColumnName)   , dataField: nameof(ReminderSummarySetting.ColumnName)   , visible: false ),
                new CellSetting(height,   0, nameof(ReminderSummarySetting.DisplayOrder) , dataField: nameof(ReminderSummarySetting.DisplayOrder) , visible: false ),
            });

            grdSummarySetting.Template = builder.Build();
            grdSummarySetting.SetRowColor(ColorContext);
            grdSummarySetting.HideSelection = true;
            grdSummarySetting.CurrentCellBorderLine = new Line(LineStyle.None, Color.Empty);
            grdSummarySetting.CurrentRowBorderLine = new Line(LineStyle.None, Color.Empty);
        }

        private void InitializeTabs()
        {
            SetCommonSetting();

            rdoCustomer.Enabled = !IsExistReminderData;
            rdoReminder.Enabled = !IsExistReminderData;
            cmbDepartmentSummaryMode.Enabled = !IsExistReminderData;
            cmbCalculateBaseDate.Enabled = !IsExistReminderData;
            cmbIncludeOnTheDay.Enabled = !IsExistReminderData;

            ClearTabs(tbcReminderSetting.TabPages.IndexOf(tbpTemplateSetting));
            ClearTabs(tbcReminderSetting.TabPages.IndexOf(tbpLevelSetting));

            SetSummarySetting();
        }

        private void SetCommonSetting()
        {
            txtOwnDepartmentName.Text = CommonSetting != null ? CommonSetting.OwnDepartmentName : string.Empty;
            txtAccountingStaffName.Text = CommonSetting != null ? CommonSetting.AccountingStaffName : string.Empty;
            cbxOutputDetail.Checked = CommonSetting != null ? CommonSetting.OutputDetail == 1 : false;
            if (cbxOutputDetail.Checked)
            {
                cmbOutputDetailItem.SelectedValue = CommonSetting.OutputDetailItem;
            }
            else
            {
                cmbOutputDetailItem.SelectedIndex = -1;
                cmbOutputDetailItem.Enabled = false;
            }
            rdoReminder.Checked = CommonSetting != null ? CommonSetting.ReminderManagementMode == (int)ReminderManagementMode.ByReminder : true;
            rdoCustomer.Checked = CommonSetting != null ? CommonSetting.ReminderManagementMode == (int)ReminderManagementMode.ByCustomer : false;
            cmbDepartmentSummaryMode.SelectedIndex = CommonSetting != null ? CommonSetting.DepartmentSummaryMode : 0;
            cmbCalculateBaseDate.SelectedIndex = CommonSetting != null ? CommonSetting.CalculateBaseDate : 0;
            cmbIncludeOnTheDay.SelectedIndex = CommonSetting != null ? CommonSetting.IncludeOnTheDay : 0;
            cbxDisplayArrearsInterest.Checked = CommonSetting != null ? CommonSetting.DisplayArrearsInterest == 1 : false;
            nmbArrearsInterestRate.AllowDeleteToNull = !cbxDisplayArrearsInterest.Checked;
            nmbArrearsInterestRate.Enabled = cbxDisplayArrearsInterest.Checked;
            nmbArrearsInterestRate.Value = CommonSetting != null ? CommonSetting.ArrearsInterestRate : null;
        }

        private void SetSummarySetting()
        {
            string calculateBaseDate = cmbCalculateBaseDate.Text;

            foreach (var setting in SummarySettingList.Where(x => x.ColumnName == "CalculateBaseDate"))
                setting.ColumnNameJp = calculateBaseDate;

            grdSummarySetting.DataSource = null;
            grdSummarySetting.DataSource = new BindingSource(SummarySettingList, null);
            grdSummarySetting.Enabled = !IsExistReminderData;
        }

        private void SetLevelCombo()
        {
            cmbReminderLevel.Items.Clear();
            foreach (var level in LevelSettingList)
                cmbReminderLevel.Items.Add(level.ReminderLevel.ToString());
        }

        #endregion

        #region ファンクションキー初期化
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Save;

            BaseContext.SetFunction06Caption("PDF設定");
            BaseContext.SetFunction06Enabled(true);
            OnF06ClickHandler = CallPdfOutputSetting;

            BaseContext.SetFunction10Caption("終了");
            OnF10ClickHandler = Exit;
        }

        private void SetFunctionKey()
        {
            switch (tbcReminderSetting.SelectedIndex)
            {
                case (int)SettingTabIndex.CommonSetting:
                    BaseContext.SetFunction02Caption("再読込");
                    BaseContext.SetFunction02Enabled(true);
                    OnF02ClickHandler = ReloadCommonSetting;

                    BaseContext.SetFunction03Caption("");
                    BaseContext.SetFunction03Enabled(false);
                    break;
                case (int)SettingTabIndex.TemplateSetting:
                    BaseContext.SetFunction02Caption("クリア");
                    BaseContext.SetFunction02Enabled(true);
                    OnF02ClickHandler = ClearTemplate;

                    BaseContext.SetFunction03Caption("削除");
                    BaseContext.SetFunction03Enabled(false);
                    OnF03ClickHandler = DeleteTemplateSetting;
                    break;
                case (int)SettingTabIndex.LevelSetting:
                    BaseContext.SetFunction02Caption("クリア");
                    BaseContext.SetFunction02Enabled(true);
                    OnF02ClickHandler = ClearLevel;

                    BaseContext.SetFunction03Caption("削除");
                    BaseContext.SetFunction03Enabled(false);
                    OnF03ClickHandler = DeleteLevelSetting;
                    break;
                case (int)SettingTabIndex.SummarySetting:
                    BaseContext.SetFunction02Caption("再読込");
                    BaseContext.SetFunction02Enabled(!IsExistReminderData);
                    OnF02ClickHandler = ReloadSummarySetting;

                    BaseContext.SetFunction03Caption("");
                    BaseContext.SetFunction03Enabled(false);
                    break;
            }
        }

        #endregion
        #region F1 登録処理
        [OperationLog("登録")]
        private void Save()
        {
            try
            {
                ClearStatusMessage();
                switch (tbcReminderSetting.SelectedIndex)
                {
                    case (int)SettingTabIndex.CommonSetting:
                        SaveReminderCommonSettings();
                        break;
                    case (int)SettingTabIndex.TemplateSetting:
                        SaveReminderTemplateSettings();
                        break;
                    case (int)SettingTabIndex.LevelSetting:
                        SaveReminderLevelSettings();
                        break;
                    case (int)SettingTabIndex.SummarySetting:
                        if (!IsExistReminderData)
                            SaveReminderSummarySettings();
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private bool RequireFieldsChecking()
        {
            switch (tbcReminderSetting.SelectedIndex)
            {
                case (int)SettingTabIndex.TemplateSetting:
                    if (string.IsNullOrWhiteSpace(txtTemplatePatternNo.Text))
                    {
                        ShowWarningDialog(MsgWngInputRequired, lblTemplatePatternNo.Text);
                        txtTemplatePatternNo.Focus();
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(txtTemplatePatternName.Text))
                    {
                        ShowWarningDialog(MsgWngInputRequired, lblTemplatePatternName.Text);
                        txtTemplatePatternName.Focus();
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(txtTitle.Text))
                    {
                        ShowWarningDialog(MsgWngInputRequired, lblTitle.Text);
                        txtTitle.Focus();
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(txtMainText.Text))
                    {
                        ShowWarningDialog(MsgWngInputRequired, lblMainText.Text);
                        txtMainText.Focus();
                        return false;
                    }
                    break;
                case (int)SettingTabIndex.LevelSetting:
                    if ((!cbxAddReminderLevel.Checked) && (string.IsNullOrWhiteSpace(cmbReminderLevel.Text)))
                    {
                        ShowWarningDialog(MsgWngSelectionRequired, lblReminderLevel.Text);
                        cmbReminderLevel.Focus();
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(txtLevelPatternNo.Text))
                    {
                        ShowWarningDialog(MsgWngInputRequired, lblLevelPatternNo.Text);
                        txtLevelPatternNo.Focus();
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(nmbArrearDays.Text))
                    {
                        ShowWarningDialog(MsgWngInputRequired, lblArrearDays.Text);
                        nmbArrearDays.Focus();
                        return false;
                    }
                    break;
            }
            return true;
        }

        private void SaveReminderCommonSettings()
        {
            if (cbxDisplayArrearsInterest.Checked)
            {
                if (!nmbArrearsInterestRate.Value.HasValue || nmbArrearsInterestRate.Value == 0)
                {
                    DispStatusMessage(MsgWngInputRequired, lblArrearsInterestRate.Text);
                    nmbArrearsInterestRate.Focus();
                    return;
                }
            }

            if (!ShowConfirmDialog(MsgQstConfirmSave))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }

            ReminderCommonSetting commonSetting = PrepareCommonSetting();
            Task<ReminderCommonSettingResult> task = SaveCommonSettingAsync(commonSetting);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (task.Result.ProcessResult.Result)
            {
                DispStatusMessage(MsgInfSaveSuccess);
                CommonChangeFlg = false;
            }
            else
                ShowWarningDialog(MsgErrSaveError);
        }

        private void SaveReminderTemplateSettings()
        {
            if (!RequireFieldsChecking()) return;

            ZeroLeftPaddingWithoutValidated();

            if (!ShowConfirmDialog(MsgQstConfirmSave))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }

            ReminderTemplateSetting templateSetting = PrepareTemplateSetting();
            Task<ReminderTemplateSettingResult> task = SaveTemplateSettingAsync(templateSetting);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (task.Result.ProcessResult.Result)
            {
                ClearTabs(tbcReminderSetting.TabPages.IndexOf(tbpTemplateSetting));
                DispStatusMessage(MsgInfSaveSuccess);
            }
            else
                ShowWarningDialog(MsgErrSaveError);
        }

        private void SaveReminderLevelSettings()
        {
            if (!RequireFieldsChecking()) return;

            ZeroLeftPaddingWithoutValidated();

            if (!ShowConfirmDialog(MsgQstConfirmSave))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }

            var result = false;
            var isTemplateExist = false;
            var isRegistTemplate = false;
            Task task = ExistTemplateSettingAsync(TemplateId)
                .ContinueWith( async t =>
                {
                    isTemplateExist = t.Result;
                    if (isTemplateExist)
                    {
                        if (cbxAddReminderLevel.Checked)
                            isRegistTemplate = await ExistTemplateAtLevelAsync(TemplateId);

                        if (!isRegistTemplate)
                        {
                            ReminderLevelSetting levelSetting = PrepareLevelSetting();
                            var levelResult = await SaveLevelSettingAsync(levelSetting);
                            result = levelResult.ProcessResult.Result;
                            await GetLevelSettingsAsync();
                            SetLevelCombo();
                        }
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext())
                .Unwrap();
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (isRegistTemplate)
            {
                ShowWarningDialog(MsgWngRegistedOtherLevelPatternNo);
                return;
            }

            if (!isTemplateExist)
            {
                ShowWarningDialog(MsgWngNotRegistPatternNo, txtLevelPatternNo.Text);
                return;
            }

            if (result)
            {
                ClearTabs(tbcReminderSetting.TabPages.IndexOf(tbpLevelSetting));
                DispStatusMessage(MsgInfSaveSuccess);
            }
            else
                ShowWarningDialog(MsgErrSaveError);
        }

        private void SaveReminderSummarySettings()
        {
            if (!ShowConfirmDialog(MsgQstConfirmSave))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }

            List<ReminderSummarySetting> summarySettings = PrepareSummarySetting();
            Task<ReminderSummarySettingsResult> task = SaveSummarySettingAsync(summarySettings);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (task.Result.ProcessResult.Result)
            {
                DispStatusMessage(MsgInfSaveSuccess);
                SummaryChangeFlg = false;
            }
            else
                ShowWarningDialog(MsgErrSaveError);
        }

        #endregion

        #region PreparingData
        private ReminderCommonSetting PrepareCommonSetting()
        {
            var commonSetting = new ReminderCommonSetting();
            commonSetting.CompanyId = CompanyId;
            commonSetting.OwnDepartmentName = txtOwnDepartmentName.Text.Trim();
            commonSetting.AccountingStaffName = txtAccountingStaffName.Text.Trim();
            commonSetting.OutputDetail = cbxOutputDetail.Checked ? 1 : 0;
            commonSetting.OutputDetailItem = cbxOutputDetail.Checked ? (string)cmbOutputDetailItem.SelectedValue : "";
            commonSetting.ReminderManagementMode = rdoReminder.Checked ? (int)ReminderManagementMode.ByReminder : (int)ReminderManagementMode.ByCustomer;
            commonSetting.DepartmentSummaryMode = Convert.ToInt32(cmbDepartmentSummaryMode.SelectedItem.SubItems[1].Value);
            commonSetting.CalculateBaseDate = Convert.ToInt32(cmbCalculateBaseDate.SelectedItem.SubItems[1].Value);
            commonSetting.IncludeOnTheDay = Convert.ToInt32(cmbIncludeOnTheDay.SelectedItem.SubItems[1].Value);
            commonSetting.DisplayArrearsInterest = cbxDisplayArrearsInterest.Checked ? 1 : 0;
            commonSetting.ArrearsInterestRate = nmbArrearsInterestRate.Value;
            commonSetting.CreateBy = Login.UserId;
            commonSetting.UpdateBy = Login.UserId;
            return commonSetting;
        }

        private ReminderTemplateSetting PrepareTemplateSetting()
        {
            var templateSetting = new ReminderTemplateSetting();
            templateSetting.CompanyId = CompanyId;
            templateSetting.Code = txtTemplatePatternNo.Text;
            templateSetting.Name = txtTemplatePatternName.Text;
            templateSetting.Title = txtTitle.Text.Trim();
            templateSetting.Greeting = txtGreeting.Text.Trim();
            templateSetting.MainText = txtMainText.Text.Trim();
            templateSetting.SubText = txtSubText.Text.Trim();
            templateSetting.Conclusion = txtConclusion.Text.Trim();
            templateSetting.CreateBy = Login.UserId;
            templateSetting.UpdateBy = Login.UserId;
            return templateSetting;
        }

        private ReminderLevelSetting PrepareLevelSetting()
        {
            int level = 0;

            if (cbxAddReminderLevel.Checked)
            {
                Task<int> task = GetMaxLevelAsync();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                level = task.Result;
            }
            else
            {
                level = Convert.ToInt32(cmbReminderLevel.SelectedItem.Text);
            }

            decimal arrearDays = nmbArrearDays.Value ?? 0M;
            var levelSetting = new ReminderLevelSetting();
            levelSetting.CompanyId = CompanyId;
            levelSetting.ReminderLevel = level;
            levelSetting.ReminderTemplateId = TemplateId;
            levelSetting.ArrearDays = Decimal.ToInt32(arrearDays);
            levelSetting.CreateBy = Login.UserId;
            levelSetting.UpdateBy = Login.UserId;
            return levelSetting;
        }

        private List<ReminderSummarySetting> PrepareSummarySetting()
        {
            var summarySettings = new List<ReminderSummarySetting>();

            foreach (Row row in grdSummarySetting.Rows)
            {
                var summarySetting = new ReminderSummarySetting();
                summarySetting.CompanyId = CompanyId;
                summarySetting.ColumnName = row.Cells[CellName(nameof(ReminderSummarySetting.ColumnName))].Value.ToString();
                summarySetting.ColumnNameJp = row.Cells[CellName(nameof(ReminderSummarySetting.ColumnNameJp))].Value.ToString();
                summarySetting.DisplayOrder = int.Parse(row.Cells[CellName(nameof(ReminderSummarySetting.DisplayOrder))].Value.ToString());
                summarySetting.Available = int.Parse(row.Cells[CellName(nameof(ReminderSummarySetting.Available))].Value.ToString());
                summarySetting.CreateBy = Login.UserId;
                summarySetting.UpdateBy = Login.UserId;

                summarySettings.Add(summarySetting);
            }
            return summarySettings;
        }
        #endregion

        #region F2 クリア処理
        [OperationLog("クリア")]
        private void ClearTemplate()
        {
            if (TemplateChangeFlg && !ShowConfirmDialog(MsgQstConfirmClear))
                return;
            ClearTabs(tbcReminderSetting.SelectedIndex);
        }

        [OperationLog("クリア")]
        private void ClearLevel()
        {
            if (IsLevelSettingEdit() && !ShowConfirmDialog(MsgQstConfirmClear))
                return;
            ClearTabs(tbcReminderSetting.SelectedIndex);
        }

        private void ClearTabs(int tabIndex)
        {
            ClearStatusMessage();
            switch (tabIndex)
            {
                case (int)SettingTabIndex.TemplateSetting:
                    txtTemplatePatternNo.Clear();
                    txtTemplatePatternName.Clear();
                    txtTitle.Clear();
                    txtGreeting.Clear();
                    txtMainText.Clear();
                    txtSubText.Clear();
                    txtConclusion.Clear();
                    txtTemplatePatternNo.Enabled = true;
                    btnTemplatePatternSearch.Enabled = true;
                    BaseContext.SetFunction03Enabled(false);
                    txtTemplatePatternNo.Focus();
                    TemplateChangeFlg = false;
                    break;
                case (int)SettingTabIndex.LevelSetting:
                    cmbReminderLevel.SelectedIndex = -1;
                    cbxAddReminderLevel.Checked = false;
                    txtLevelPatternNo.Clear();
                    lblLevelPatternName.Clear();
                    nmbArrearDays.Clear();
                    BaseContext.SetFunction03Enabled(false);
                    cmbReminderLevel.Focus();
                    LevelSetting = null;
                    LevelChangeFlg = false;
                    break;
            }
        }

        #endregion

        #region F2 再読込
        [OperationLog("再読込")]
        private void ReloadCommonSetting()
        {
            if (CommonChangeFlg && !ShowConfirmDialog(MsgQstConfirmUpdateData))
                return;

            try
            {
                CommonSetting = null;
                ProgressDialog.Start(ParentForm, GetCommonSettingAsync(), false, SessionKey);
                SetCommonSetting();
                CommonChangeFlg = false;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("再読込")]
        private void ReloadSummarySetting()
        {
            if (SummaryChangeFlg && !ShowConfirmDialog(MsgQstConfirmUpdateData))
                return;

            ClearStatusMessage();

            try
            {
                SummarySettingList = null;
                ProgressDialog.Start(ParentForm, GetSummarySettingsAsync(), false, SessionKey);
                SetSummarySetting();
                SummaryChangeFlg = false;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        #endregion

        #region F3 削除処理
        [OperationLog("削除 文面")]
        private void DeleteTemplateSetting()
        {
            if (string.IsNullOrWhiteSpace(txtTemplatePatternNo.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblTemplatePatternNo.Text);
                txtTemplatePatternNo.Focus();
                return;
            }
            if (!ShowConfirmDialog(MsgQstConfirmDelete))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }

            try
            {
                CountResult result = null;
                var exist = false;
                Task task = ExistTemplateAtLevelAsync(TemplateId)
                    .ContinueWith(async t =>
                    {
                        exist = t.Result;
                        if (!exist) result = await DeleteTemplateSettingAsync(TemplateId);
                    }, TaskScheduler.FromCurrentSynchronizationContext())
                    .Unwrap();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (exist)
                {
                    ShowWarningDialog(MsgWngDeleteConstraint, new string[] { "レベル設定", "パターンNo" });
                    return;
                }
                if (result.Count > 0)
                {
                    ClearTabs(tbcReminderSetting.TabPages.IndexOf(tbpTemplateSetting));
                    DispStatusMessage(MsgInfDeleteSuccess);
                }
                else
                    ShowWarningDialog(MsgErrDeleteError);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("削除 レベル")]
        private void DeleteLevelSetting()
        {
            if (string.IsNullOrWhiteSpace(cmbReminderLevel.Text))
            {
                ShowWarningDialog(MsgWngSelectionRequired, lblReminderLevel.Text);
                cmbReminderLevel.Focus();
                return;
            }
            if (!ShowConfirmDialog(MsgQstConfirmDelete))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }

            try
            {
                Task task = DeleteLevelSettingAsync(int.Parse(cmbReminderLevel.Text))
                    .ContinueWith(async t =>
                    {
                        if (t.Result.Count > 0)
                        {
                            await GetLevelSettingsAsync();
                            SetLevelCombo();
                            ClearTabs(tbcReminderSetting.TabPages.IndexOf(tbpLevelSetting));
                            DispStatusMessage(MsgInfDeleteSuccess);
                            LevelSetting = null;
                        }
                        else
                            ShowWarningDialog(MsgErrDeleteError);

                    }, TaskScheduler.FromCurrentSynchronizationContext())
                    .Unwrap();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        #endregion
        #region F6 PDF出力設定
        [OperationLog("PDF設定")]
        private void CallPdfOutputSetting()
        {
            ClearStatusMessage();
            var form = ApplicationContext.Create(nameof(PC1503));
            var screen = form.GetAll<PC1503>().FirstOrDefault();
            screen.lblNumber.Text = "発行番号";
            screen.lblDate.Text = "発行日";
            screen.ReturnScreen = this;
            
            var result = ApplicationContext.ShowDialog(BaseForm, form);
        }
        #endregion

        #region F10 終了処理
        [OperationLog("終了")]
        private void Exit()
        {
            int index = tbcReminderSetting.SelectedIndex;
            bool changeFlg = (index == (int)SettingTabIndex.CommonSetting && CommonChangeFlg) ||
                             (index == (int)SettingTabIndex.TemplateSetting && TemplateChangeFlg) ||
                             (index == (int)SettingTabIndex.LevelSetting && IsLevelSettingEdit()) ||
                             (index == (int)SettingTabIndex.SummarySetting && SummaryChangeFlg);
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
            }
        }
        private void OnContentChanged(object sender, EventArgs e)
        {
            switch (tbcReminderSetting.SelectedIndex)
            {
                case (int)SettingTabIndex.CommonSetting:
                    CommonChangeFlg = true;
                    break;
                case (int)SettingTabIndex.TemplateSetting:
                    TemplateChangeFlg = true;
                    break;
                case (int)SettingTabIndex.LevelSetting:
                    LevelChangeFlg = true;
                    break;
                case (int)SettingTabIndex.SummarySetting:
                    SummaryChangeFlg = true;
                    break;
            }
        }

        private bool IsLevelSettingEdit()
        {
            if (!LevelChangeFlg && LevelSetting == null)
                return false;

            if (LevelChangeFlg && LevelSetting == null)
                return true;

            if (LevelSetting.ReminderTemplateCode != txtLevelPatternNo.Text)
                return true;

            if (LevelSetting.ArrearDays != nmbArrearDays.Value)
                return true;

            return false;
        }

        #endregion

        #region Validatedイベント
        private void ZeroLeftPaddingWithoutValidated()
        {
            if (tbcReminderSetting.SelectedIndex == 1 && IsNeedValidate(0, txtTemplatePatternNo.TextLength, txtTemplatePatternNo.MaxLength))
            {
                txtTemplatePatternNo.Text = ZeroLeftPadding(txtTemplatePatternNo);
                txtTemplatePatternNo_Validated(null, null);
            }
            if (tbcReminderSetting.SelectedIndex == 2 && IsNeedValidate(0, txtLevelPatternNo.TextLength, txtLevelPatternNo.MaxLength))
            {
                txtLevelPatternNo.Text = ZeroLeftPadding(txtLevelPatternNo);
                txtLevelPatternNo_Validated(null, null);
            }
        }

        private void txtTemplatePatternNo_Validated(object sender, EventArgs e)
        {
            string code = txtTemplatePatternNo.Text;
            if (string.IsNullOrWhiteSpace(code))
                return;

            ClearStatusMessage();

            try
            {
                Task<ReminderTemplateSetting> task = GetTemplateSettingByCodeAsync(code);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                ReminderTemplateSetting templateSetting = task.Result;
                if (templateSetting == null)
                {
                    DispStatusMessage(MsgInfNewPatternNo, lblTemplatePatternNo.Text);
                    TemplateId = 0;
                    txtTemplatePatternName.Focus();
                    return;
                }

                TemplateId = templateSetting.Id;
                txtTemplatePatternName.Text = templateSetting.Name;
                txtTitle.Text = templateSetting.Title;
                txtGreeting.Text = templateSetting.Greeting;
                txtMainText.Text = templateSetting.MainText;
                txtSubText.Text = templateSetting.SubText;
                txtConclusion.Text = templateSetting.Conclusion;
                BaseContext.SetFunction03Enabled(true);
                txtTemplatePatternNo.Enabled = false;
                btnTemplatePatternSearch.Enabled = false;
                TemplateChangeFlg = false;

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtLevelPatternNo_Validated(object sender, EventArgs e)
        {
            string code = txtLevelPatternNo.Text;
            if (string.IsNullOrWhiteSpace(code))
                return;

            ClearStatusMessage();

            try
            {
                Task<ReminderTemplateSetting> task = GetTemplateSettingByCodeAsync(code);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                ReminderTemplateSetting templateSetting = task.Result;
                if (templateSetting == null)
                {
                    ShowWarningDialog(MsgWngNotRegistPatternNo, txtLevelPatternNo.Text);
                    txtLevelPatternNo.Focus();
                    return;
                }

                lblLevelPatternName.Text = templateSetting.Name;
                TemplateId = templateSetting.Id;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region CheckBox CheckedChangedイベント
        private void cbxOutputDetail_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxOutputDetail.Checked)
            {
                cmbOutputDetailItem.SelectedIndex = 0;
                cmbOutputDetailItem.Enabled = true;
            }
            else
            {
                cmbOutputDetailItem.SelectedIndex = -1;
                cmbOutputDetailItem.Enabled = false;
            }
        }
        private void cbxAddReminderLevel_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxAddReminderLevel.Checked)
            {
                LevelSetting = null;
                cmbReminderLevel.SelectedIndex = -1;
                cmbReminderLevel.Enabled = false;
            }
            else
                cmbReminderLevel.Enabled = true;

        }

        private void cbxDisplayArrearsInterest_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxDisplayArrearsInterest.Checked)
            {
                nmbArrearsInterestRate.Enabled = true;
                nmbArrearsInterestRate.Value = 0m;
                nmbArrearsInterestRate.AllowDeleteToNull = false;
            }
            else
            {
                nmbArrearsInterestRate.Enabled = false;
                nmbArrearsInterestRate.AllowDeleteToNull = true;
                nmbArrearsInterestRate.Value = null;
            }
        }
        #endregion

        #region タブ変更イベント
        private void tbcReminderSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearStatusMessage();
            SetFunctionKey();
        }
        #endregion

        #region ComboBox SelectedIndexChangedイベント
        private void cmbCalculateBaseDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSummarySetting();
        }

        private void cmbReminderLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbReminderLevel.SelectedItem == null) return;
            try
            {
                Task<ReminderLevelSettingResult> task = GetLevelSettingByLevelAsync(int.Parse(cmbReminderLevel.SelectedItem.Text));
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                if (task.Result.ProcessResult.Result)
                {
                    LevelSetting = task.Result.ReminderLevelSetting;
                    TemplateId = LevelSetting.ReminderTemplateId;
                    txtLevelPatternNo.Text = LevelSetting.ReminderTemplateCode;
                    lblLevelPatternName.Text = LevelSetting.ReminderTemplateName;
                    nmbArrearDays.Value = Convert.ToInt32(LevelSetting.ArrearDays);

                    BaseContext.SetFunction03Enabled(true);
                    LevelChangeFlg = false;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region 検索ボタンClickイベント処理
        private void btnTemplatePatternSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var templateSetting = this.ShowReminderTemplateSettingSearchDialog();
                if (templateSetting != null)
                {
                    TemplateId = templateSetting.Id;
                    txtTemplatePatternNo.Text = templateSetting.Code;
                    txtTemplatePatternName.Text = templateSetting.Name;
                    txtTitle.Text = templateSetting.Title;
                    txtGreeting.Text = templateSetting.Greeting;
                    txtMainText.Text = templateSetting.MainText;
                    txtSubText.Text = templateSetting.SubText;
                    txtConclusion.Text = templateSetting.Conclusion;
                    BaseContext.SetFunction03Enabled(true);
                    txtTemplatePatternNo.Enabled = false;
                    btnTemplatePatternSearch.Enabled = false;
                    TemplateChangeFlg = false;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnLevelPatternSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var templateSetting = this.ShowReminderTemplateSettingSearchDialog();
                if (templateSetting != null)
                {
                    txtLevelPatternNo.Text = templateSetting.Code;
                    lblLevelPatternName.Text = templateSetting.Name;
                    TemplateId = templateSetting.Id;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region 集計グリッドイベント
        private void grdSummarySetting_CellClick(object sender, CellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (IsExistReminderData ||
                e.CellName == CellName(nameof(ReminderSummarySetting.ColumnNameJp))) return;

            Row row = grdSummarySetting.Rows[e.RowIndex];
            CheckRequiredSummaryKey(row);
        }

        private void grdSummarySetting_CellDoubleClick(object sender, CellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (IsExistReminderData ||
                e.CellName == CellName(nameof(ReminderSummarySetting.ColumnNameJp))) return;

            Row row = grdSummarySetting.Rows[e.RowIndex];
            CheckRequiredSummaryKey(row);
        }

        private void grdSummarySetting_CellValueChanged(object sender, CellEventArgs e)
        {
            if (IsExistReminderData ||
                e.CellName == CellName(nameof(ReminderSummarySetting.ColumnNameJp))) return;

            Row row = grdSummarySetting.Rows[e.RowIndex];
            CheckRequiredSummaryKey(row);
        }

        private void CheckRequiredSummaryKey(Row row)
        {
            ClearStatusMessage();
            grdSummarySetting.EndEdit();

            if (row[CellName(nameof(ReminderSummarySetting.ColumnName))].Value.ToString() == "CalculateBaseDate" ||
                row[CellName(nameof(ReminderSummarySetting.ColumnName))].Value.ToString() == "CustomerCode" ||
                row[CellName(nameof(ReminderSummarySetting.ColumnName))].Value.ToString() == "CurrencyCode")
            {
                string contentName = row[CellName(nameof(ReminderSummarySetting.ColumnNameJp))].Value.ToString();
                row[CellName(nameof(ReminderSummarySetting.Available))].Value = 1;
                DispStatusMessage(MsgInfNeedForSummarized, contentName);
                return;
            }
            SummaryChangeFlg = true;
        }

        #endregion

        #region WebService
        private async Task ReminderExist()
            => await ServiceProxyFactory.DoAsync(async (ReminderServiceClient client) =>
            {
                var result = await client.ExistAsync(SessionKey, CompanyId);
                IsExistReminderData = result.Exist;
            });

        #region CommonSetting
        private async Task GetCommonSettingAsync()
            => await ServiceProxyFactory.DoAsync(async (ReminderSettingServiceClient client) =>
            {
                var result = await client.GetReminderCommonSettingAsync(SessionKey, CompanyId);
                CommonSetting = result.ReminderCommonSetting ?? new ReminderCommonSetting();
            });

        private async Task<ReminderCommonSettingResult> SaveCommonSettingAsync(ReminderCommonSetting commonSetting)
            => await ServiceProxyFactory.DoAsync(async (ReminderSettingServiceClient client) =>
            {
                var result = await client.SaveReminderCommonSettingAsync(SessionKey, commonSetting);
                return result;
            });
        #endregion

        #region TemplateSetting
        private async Task<bool> ExistTemplateSettingAsync(int templateId)
            => await ServiceProxyFactory.DoAsync(async (ReminderSettingServiceClient client) =>
            {
                var result = await client.ExistReminderTemplateSettingAsync(SessionKey, templateId);
                return result.Exist;
            });

        private async Task<ReminderTemplateSetting> GetTemplateSettingByCodeAsync(string Code)
            => await ServiceProxyFactory.DoAsync(async (ReminderSettingServiceClient client) =>
            {
                var result = await client.GetReminderTemplateSettingByCodeAsync(SessionKey, CompanyId, Code);
                return result.ReminderTemplateSetting;
            });

        private async Task<ReminderTemplateSettingResult> SaveTemplateSettingAsync(ReminderTemplateSetting templateSetting)
            => await ServiceProxyFactory.DoAsync(async (ReminderSettingServiceClient client) =>
            {
                var result = await client.SaveReminderTemplateSettingAsync(SessionKey, templateSetting);
                return result;
            });

        private async Task<CountResult> DeleteTemplateSettingAsync(int templateId)
            => await ServiceProxyFactory.DoAsync(async (ReminderSettingServiceClient client) =>
            {
                var result = await client.DeleteReminderTemplateSettingAsync(SessionKey, templateId);
                return result;
            });
        #endregion

        #region LevelSetting
        private async Task<bool> ExistTemplateAtLevelAsync(int templateId)
            => await ServiceProxyFactory.DoAsync(async (ReminderSettingServiceClient client) =>
            {
                var result = await client.ExistTemplateAtReminderLevelAsync(SessionKey, templateId);
                return result.Exist;
            });

        private async Task GetLevelSettingsAsync()
            => await ServiceProxyFactory.DoAsync(async (ReminderSettingServiceClient client) =>
            {
                var results = await client.GetReminderLevelSettingsAsync(SessionKey, CompanyId);
                LevelSettingList =  results.ReminderLevelSettings;
            });

        private async Task<ReminderLevelSettingResult> GetLevelSettingByLevelAsync(int level)
            => await ServiceProxyFactory.DoAsync(async (ReminderSettingServiceClient client) =>
            {
                var result = await client.GetReminderLevelSettingByLevelAsync(SessionKey, CompanyId, level);
                return result;
            });

        private async Task<int> GetMaxLevelAsync()
            => await ServiceProxyFactory.DoAsync(async (ReminderSettingServiceClient client) =>
            {
                var result = await client.GetMaxReminderLevelAsync(SessionKey, CompanyId);
                return result.MaxReminderLevel;
            });

        private async Task<ReminderLevelSettingResult> SaveLevelSettingAsync(ReminderLevelSetting levelSetting)
            => await ServiceProxyFactory.DoAsync(async (ReminderSettingServiceClient client) =>
            {
                var result = await client.SaveReminderLevelSettingAsync(SessionKey, levelSetting);
                return result;
            });

        private async Task<CountResult> DeleteLevelSettingAsync(int level)
            => await ServiceProxyFactory.DoAsync(async (ReminderSettingServiceClient client) =>
            {
                var result = await client.DeleteReminderLevelSettingAsync(SessionKey, CompanyId, level);
                return result;
            });
        #endregion

        #region SummarySetting
        private async Task GetSummarySettingsAsync()
            => await ServiceProxyFactory.DoAsync(async (ReminderSettingServiceClient client) =>
            {
                var results = await client.GetReminderSummarySettingsAsync(SessionKey, CompanyId);
                SummarySettingList = results.ReminderSummarySettings;
            });

        private async Task GetColumnNameSettingAsync()
            => await ServiceProxyFactory.DoAsync(async (ColumnNameSettingMasterService.ColumnNameSettingMasterClient client) =>
            {
                var results = await client.GetItemsAsync(SessionKey, CompanyId);
                ColumnNameSetting = results.ColumnNames;
            });

        private async Task<ReminderSummarySettingsResult> SaveSummarySettingAsync(List<ReminderSummarySetting> summarySettings)
            => await ServiceProxyFactory.DoAsync(async (ReminderSettingServiceClient client) =>
            {
                var result = await client.SaveReminderSummarySettingAsync(SessionKey, summarySettings.ToArray());
                return result;
            });
        #endregion

        #endregion
    }
}
