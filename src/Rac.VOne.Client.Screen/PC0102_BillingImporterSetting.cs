using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.AccountTitleMasterService;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.DepartmentMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.ImporterSettingService;
using Rac.VOne.Client.Screen.JuridicalPersonalityMasterService;
using Rac.VOne.Client.Screen.SettingMasterService;
using Rac.VOne.Client.Screen.StaffMasterService;
using Rac.VOne.Client.Screen.TaskScheduleMasterService;
using Rac.VOne.Common;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Common.Importer.Billing;
using Rac.VOne.Web.Models;
using System;
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
    /// <summary>請求フリーインポーター・取込設定</summary>
    public partial class PC0102 : VOneScreenBase
    {
        /// <summary>請求フリーインポーター フォーマットID</summary>
        private int FormatId { get { return (int)FreeImporterFormatType.Billing; } }
        /// <summary>タスクスケジューラ ImportType 請求フリーインポーター</summary>
        private int ImportType { get { return (int)TaskScheduleImportType.Billing; } }
        private DataExpression DataExpression { get; set; }
        private DateTime UpdateAt { get; set; }
        private bool SearchFlg { get; set; } = false;
        private List<ImporterSettingDetail> ImpSettingDetailsList { get; set; }
        private int ImporterSettingId { get; set; }
        private List<Setting> TaxAmountSetting { get; set; }
        private List<Setting> CustomerSetting { get; set; }
        private List<Setting> BillingAmountSetting { get; set; }
        private IEnumerable<string> LegalPersonalities { get; set; }
        private bool changeSelected { get; set; } = false;

        /// <summary>0 : 行番号</summary>
        private int CellIndexRowHeader { get { return 0; } }
        /// <summary>1 : 重複</summary>
        private int CellIndexIsUnique { get { return 1; } }
        /// <summary>2 : 上書</summary>
        private int CellIndexOverWrite { get { return 2; } }
        /// <summary>3 : 項目名</summary>
        private int CellIndexFieldName { get { return 3; } }
        /// <summary>4 : 取込区分</summary>
        private int CellIndexImportDivision { get { return 4; } }
        /// <summary>5 : 固定値</summary>
        private int CellIndexFixedValue { get { return 5; } }
        /// <summary>6 : 項目番号</summary>
        private int CellIndexFieldIndex { get { return 6; } }
        /// <summary>7 : 属性区分</summary>
        private int CellIndexAttribute { get { return 7; } }
        /// <summary>8 : Sequence</summary>
        private int CellIndexSequence { get { return 8; } }

        public PC0102()
        {
            InitializeComponent();
            grid.SetupShortcutKeys();
            Text = "請求フリーインポーター・取込設定";
            AddHandlers();

        }

        #region　画面の初期化
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = SavePattern;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = ClearPattern;

            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction03Enabled(true);
            OnF03ClickHandler = DeletePattern;

            BaseContext.SetFunction08Caption("参照新規");
            BaseContext.SetFunction08Enabled(true);
            OnF08ClickHandler = SearchPattern;

            BaseContext.SetFunction09Caption("検索");
            BaseContext.SetFunction09Enabled(false);
            OnF09ClickHandler = SearchCode;

            BaseContext.SetFunction10Caption("終了");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = ExitForm;

            SuspendLayout();
            ResumeLayout();
        }

        private void PC0102_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                var tasks = new List<Task>();
                if (Company == null)
                    tasks.Add(LoadCompanyAsync());
                if (ApplicationControl == null)
                    tasks.Add(LoadApplicationControlAsync());
                tasks.Add(LoadControlColorAsync());
                tasks.Add(LoadLegalPersonalitiesAsync());
                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);
                DataExpression = new DataExpression(ApplicationControl);
                InitializeGridTemplate();
                ClearPattern();
                txtInitialDirectory.Enabled = !LimitAccessFolder;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void InitializeGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var template = new Template();

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "Header"              , caption: "No."),
                new CellSetting(height,  40, "cbxOverLap"          , caption: "重複"),
                new CellSetting(height,  40, "cbxOverWrite"        , caption: "上書"),
                new CellSetting(height, 115, "txtFieldName"        , caption: "VONE項目名"),
                new CellSetting(height, 270, "cmbImportDivision"   , caption: "取込有無"),
                new CellSetting(height,  80, "txtFixedValue"       , caption: "固定値"),
                new CellSetting(height,  60, "nmbFieldIndex"       , caption: "項目番号"),
                new CellSetting(height,  80, "cmbAttributeDivision", caption: "属性情報")
            });

            builder.BuildHeaderOnly(template);
            builder.Items.Clear();

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "Header"              , cell: builder.GetRowHeaderCell()),
                new CellSetting(height,  40, "cbxOverLap"          , readOnly: false, cell: builder.GetCheckBoxCell()),
                new CellSetting(height,  40, "cbxOverWrite"        , readOnly: false, cell: builder.GetCheckBoxCell()),
                new CellSetting(height, 115, "txtFieldName"        , dataField: nameof(ImporterSettingDetail.FieldName)),
                new CellSetting(height, 270, "cmbImportDivision"   , readOnly: false, cell: builder.GetComboBoxCell()),
                new CellSetting(height,  80, "txtFixedValue"       , readOnly: false, dataField: nameof(ImporterSettingDetail.FixedValue), cell: builder.GetTextBoxCell(maxLength: 50)),
                new CellSetting(height,  60, "nmbFieldIndex"       , readOnly: false, cell: builder.GetNumberCellFreeInput(1, 999, 3)),
                new CellSetting(height,  80, "cmbAttributeDivision", readOnly: false, cell: builder.GetComboBoxCell()),
                new CellSetting(height,   0, "Sequence"            , dataField: nameof(ImporterSettingDetail.Sequence), visible: false)
            });

            builder.BuildRowOnly(template);
            grid.Template = template;
            grid.HideSelection = true;
        }
        #endregion

        #region Function Key Event
        [OperationLog("登録")]
        private void SavePattern()
        {
            try
            {
                ClearStatusMessage();
                if (!ValidateRequireData()) return;

                if (!ValidateGridData()) return;

                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }
                ProgressDialog.Start(ParentForm, SaveImporterSettingAsync(), false, SessionKey);
                this.ActiveControl = txtPatternNumber;
                txtPatternNumber.Focus();
                Modified = false;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("クリア")]
        private void ClearPattern()
        {
            ClearStatusMessage();
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear)) return;

            txtPatternNumber.Clear();
            txtPatternName.Clear();
            txtInitialDirectory.Clear();
            nmbStartLineCount.Clear();
            rdoNoAction.Checked = true;
            cbxIgnoreLastLine.Checked = false;
            cbxAutoCreationCustomer.Checked = false;
            grid.DataSource = null;
            grid.HideSelection = true;
            this.ActiveControl = txtPatternNumber;
            txtPatternNumber.Enabled = true;
            btnPatternNoSearch.Enabled = true;
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);
            txtPatternNumber.Focus();
            Modified = false;
            changeSelected = false;
        }

        [OperationLog("削除")]
        private void DeletePattern()
        {
            ClearStatusMessage();
            var checkResult = new ExistResult();
            try
            {
                var task = ServiceProxyFactory.DoAsync<TaskScheduleMasterClient>(async client =>
                {
                    checkResult = await client.ExistsAsync(SessionKey, CompanyId, ImportType, ImporterSettingId);
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                if (checkResult.Exist)
                {
                    ShowWarningDialog(MsgWngDeleteConstraint, "タイムスケジューラー", " 取込パターン");
                    return;
                }
                if (!ShowConfirmDialog(MsgQstConfirmDelete))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }
                ProgressDialog.Start(ParentForm, DeleteImportSettingAsync(), false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("参照新規")]
        private void SearchPattern()
        {
            try
            {
                var import = this.ShowBillingImporterSettingSearchDialog();
                if (import == null) return;
                SearchFlg = true;
                var loadTask = new List<Task>();
                var searchCode = txtPatternNumber.Text;
                txtPatternNumber.Text = import.Code;
                loadTask.Add(LoadPatternData(txtPatternNumber.Text, true));
                loadTask.Add(LoadGridDataAsync());
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
                SettingGridData();
                txtPatternNumber.Text = searchCode;
                txtPatternNumber.Enabled = true;
                btnPatternNoSearch.Enabled = true;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("検索")]
        private void SearchCode()
        {
            var fieldName = grid.CurrentRow.Cells[CellIndexFieldName].DisplayText;

            if (fieldName == "債権科目")
            {
                var accountTitle = this.ShowAccountTitleSearchDialog();
                if (accountTitle != null)
                {
                    grid.CurrentRow.Cells[CellIndexFixedValue].Value = accountTitle.Code;
                    grid.CurrentRow.Cells[CellIndexFieldIndex].Selected = true;
                    BaseContext.SetFunction09Enabled(false);
                }
            }

            if (fieldName == "担当者")
            {
                var staff = this.ShowStaffSearchDialog();
                if (staff != null)
                {
                    grid.CurrentRow.Cells[CellIndexFixedValue].Value = staff.Code;
                    grid.CurrentRow.Cells[CellIndexFieldIndex].Selected = true;
                    BaseContext.SetFunction09Enabled(false);
                }
            }

            if (fieldName == "請求部門")
            {
                var department = this.ShowDepartmentSearchDialog();
                if (department != null)
                {
                    grid.CurrentRow.Cells[CellIndexFixedValue].Value = department.Code;
                    grid.CurrentRow.Cells[CellIndexFieldIndex].Selected = true;
                    BaseContext.SetFunction09Enabled(false);
                }
            }

            if (fieldName == "請求区分")
            {
                var category = this.ShowBillingCategorySearchDialog();
                if (category != null)
                {
                    grid.CurrentRow.Cells[CellIndexFixedValue].Value = category.Code;
                    grid.CurrentRow.Cells[CellIndexFieldIndex].Selected = true;
                    BaseContext.SetFunction09Enabled(false);
                }
            }

            if (fieldName == "回収区分")
            {
                var category = this.ShowCollectCategroySearchDialog();
                if (category != null)
                {
                    grid.CurrentRow.Cells[CellIndexFixedValue].Value = category.Code;
                    grid.CurrentRow.Cells[CellIndexFieldIndex].Selected = true;
                    BaseContext.SetFunction09Enabled(false);
                }
            }

            if (fieldName == "歩引利用")
            {
                var yesOrNo = this.ShowYesOrNoSettingSearchDialog(fieldName);
                if (yesOrNo != null)
                {
                    grid.CurrentRow.Cells[CellIndexFixedValue].Value = yesOrNo.Key;
                    grid.CurrentRow.Cells[CellIndexFieldIndex].Selected = true;
                    BaseContext.SetFunction09Enabled(false);
                }
            }

            if (fieldName == "通貨コード")
            {
                var currency = this.ShowCurrencySearchDialog();
                if (currency != null)
                {
                    grid.CurrentRow.Cells[CellIndexFixedValue].Value = currency.Code;
                    grid.CurrentRow.Cells[CellIndexFieldIndex].Selected = true;
                    BaseContext.SetFunction09Enabled(false);
                }
            }
        }

        [OperationLog("終了")]
        private void ExitForm()
        {
            ClearStatusMessage();
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;
            ParentForm.Close();
        }
        #endregion

        #region Webサービス呼び出し
        private async Task LoadPatternData(string code, bool loadRefData = false)
        {
            await ServiceProxyFactory.DoAsync<ImporterSettingServiceClient>(async client =>
            {
                var result = await client.GetHeaderByCodeAsync(SessionKey, CompanyId, FormatId, code);
                if (result.ImporterSetting != null)
                {
                    if (!SearchFlg)
                    {
                        txtPatternNumber.Text = result.ImporterSetting.Code;
                        txtPatternName.Text = result.ImporterSetting.Name;
                        txtPatternNumber.Enabled = false;
                        btnPatternNoSearch.Enabled = false;
                    }

                    SearchFlg = false;//for original state
                    Modified = false;
                    txtInitialDirectory.Text = result.ImporterSetting.InitialDirectory;
                    nmbStartLineCount.Value = result.ImporterSetting.StartLineCount;
                    UpdateAt = result.ImporterSetting.UpdateAt;
                    ImporterSettingId = result.ImporterSetting.Id;
                    if (result.ImporterSetting.PostAction == 0) rdoNoAction.Checked = true;
                    else if (result.ImporterSetting.PostAction == 1) rdoDelete.Checked = true;
                    else if (result.ImporterSetting.PostAction == 2) rdoAddDate.Checked = true;
                    cbxIgnoreLastLine.Checked = (result.ImporterSetting.IgnoreLastLine == 1) ? true : false;
                    cbxAutoCreationCustomer.Checked = (result.ImporterSetting.AutoCreationCustomer == 1) ? true : false;
                    changeSelected = false;

                    this.ActiveControl = txtInitialDirectory;
                    if (!loadRefData)
                    {
                        BaseContext.SetFunction03Enabled(true);
                        BaseContext.SetFunction08Enabled(false);
                        ClearStatusMessage();
                    }
                }
                else
                {
                    this.ActiveControl = txtPatternName;
                    BaseContext.SetFunction08Enabled(true);
                    DispStatusMessage(MsgInfNewData, "パターンNo.");
                    cbxIgnoreLastLine.Checked = false;
                    cbxAutoCreationCustomer.Checked = false;
                    rdoNoAction.Checked = true;
                    txtPatternName.Clear();
                    txtInitialDirectory.Clear();
                    nmbStartLineCount.Clear();
                    changeSelected = true;
                }
            });
        }

        private async Task LoadGridDataAsync()
        {
            await ServiceProxyFactory.DoAsync<ImporterSettingServiceClient>(async client =>
            {
                var result = await client.GetDetailByCodeAsync(SessionKey, CompanyId, FormatId, txtPatternNumber.Text);
                if (result.ImporterSettingDetails != null)
                {
                    ImpSettingDetailsList = new List<ImporterSettingDetail>(result.ImporterSettingDetails
                        .Where(x =>
                        {
                            if (x.Sequence == (int)Fields.CurrencyCode) return UseForeignCurrency;
                            if (x.Sequence == (int)Fields.UseDiscount) return UseDiscount;
                            if (x.Sequence == (int)Fields.ContractNumber) return UseLongTermAdvanceReceived;
                            return true;
                        }));
                    grid.DataSource = null;
                    grid.Refresh();
                    grid.DataSource = ImpSettingDetailsList;
                }
            });
        }

        private List<Setting> GetSetting(string[] itemId)
        {
            var comboSetting = new List<Setting>();
            var task = ServiceProxyFactory.DoAsync<SettingMasterClient>(async client =>
            {
                var result = await client.GetItemsAsync(SessionKey, itemId);
                if (result.Settings != null)
                {
                    comboSetting = result.Settings;
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return comboSetting;
        }

        private async Task SaveImporterSettingAsync()
        {
            var header = PrepareImporterSetting();
            var details = PrepareImporterSettingDetail();
            await ServiceProxyFactory.DoAsync<ImporterSettingServiceClient>(async client =>
            {
                var result = await client.SaveAsync(SessionKey, header, details.ToArray());
                if (result.ImporterSetting != null && result.ImporterSettingDetail != null)
                {
                    Modified = false;
                    ClearPattern();
                    DispStatusMessage(MsgInfSaveSuccess);
                }
                else
                {
                    ShowWarningDialog(MsgErrSaveError);
                    DispStatusMessage(MsgErrSaveError);
                }
            });
        }

        private async Task DeleteImportSettingAsync()
        {
            await ServiceProxyFactory.DoAsync<ImporterSettingServiceClient>(async client =>
            {
                var result = await client.DeleteAsync(SessionKey, ImporterSettingId);
                if (result.Count > 0)
                {
                    Modified = false;
                    ClearPattern();
                    DispStatusMessage(MsgInfDeleteSuccess);
                }
                else
                {
                    ShowWarningDialog(MsgErrDeleteError);
                    DispStatusMessage(MsgErrDeleteError);
                }
            });
        }

        private async Task LoadLegalPersonalitiesAsync()
        {
            if (LegalPersonalities == null)
            {
                await ServiceProxyFactory.DoAsync<JuridicalPersonalityMasterClient>(async client =>
                {
                    var result = await client.GetItemsAsync(SessionKey, CompanyId);
                    if (result.ProcessResult.Result)
                    {
                        LegalPersonalities = result.JuridicalPersonalities.Select(x => x.Kana);
                    }
                });
            }
        }

        private bool ExistCategoryMaster(CategorySearch option)
        {
            var result = new CategoriesResult();
            var task = ServiceProxyFactory.DoAsync<CategoryMasterClient>(async client
                => result = await client.GetItemsAsync(SessionKey, option));
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return (result.Categories.Count > 0);
        }

        private bool ExistStaffMaster(string code)
        {
            var result = new StaffsResult();
            var task = ServiceProxyFactory.DoAsync<StaffMasterClient>(async client =>
            {
                result = await client.GetByCodeAsync(SessionKey, CompanyId, new[] { code });
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return (result.Staffs.Count > 0);
        }

        private bool ExistDepartmentMaster(string code)
        {
            var result = new DepartmentsResult();
            var task = ServiceProxyFactory.DoAsync<DepartmentMasterClient>(async client =>
            {
                result = await client.GetByCodeAsync(SessionKey,
                CompanyId, new string[] { code });
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return (result.Departments.Count > 0);
        }

        private bool ExistAccountTitleMaster(string code)
        {
            var result = new AccountTitlesResult();
            var task = ServiceProxyFactory.DoAsync<AccountTitleMasterClient>(async client =>
            {
                result = await client.GetByCodeAsync(SessionKey, CompanyId, new[] { code });
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return (result.AccountTitles.Count > 0);
        }

        private bool ExistCurrencyMaster(string code)
        {
            var result = new CurrenciesResult();
            var task = ServiceProxyFactory.DoAsync<CurrencyMasterClient>(async client =>
            {
                result = await client.GetByCodeAsync(SessionKey, CompanyId, new[] { code });
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return (result.Currencies.Count > 0);
        }

        private List<ColumnNameSetting> LoadCulumnNameSetting()
        {
            var columnResult = new ColumnNameSettingsResult();
            var columnSettingList = new List<ColumnNameSetting>();

            var task = ServiceProxyFactory.DoAsync<ColumnNameSettingMasterClient>(async client =>
            {
                columnResult = await client.GetItemsAsync(SessionKey, CompanyId);
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            if (columnResult.ProcessResult.Result)
            {
                columnSettingList = columnResult.ColumnNames.Where(x => x.TableName == nameof(Billing)).ToList();
            }
            return columnSettingList;
        }
        #endregion

        #region グリットの設定
        private void SettingGridData()
        {
            var names = LoadCulumnNameSetting();
            if (ImpSettingDetailsList == null || ImpSettingDetailsList.Count < 1) return;
            var division = new List<string>();
            division.AddRange(ImpSettingDetailsList.Select(x => x.BaseImportDivision).Distinct().Select(x => $"TRKM{x}"));
            division.AddRange(ImpSettingDetailsList.Select(x => x.BaseAttributeDivision).Distinct().Select(x => $"ATTR{x}"));

            var comboSetting = GetSetting(division.Distinct().ToArray());
            TaxAmountSetting = comboSetting.Where(x => x.ItemId == "TRKM3").Select(c => c).ToList();
            CustomerSetting = comboSetting.Where(x => x.ItemId == "TRKM14").Select(c => c).ToList();
            BillingAmountSetting = comboSetting.Where(x => x.ItemId == "TRKM17").Select(c => c).ToList();
            if (grid.RowCount > 0)
            {
                foreach (var row in grid.Rows)
                {
                    var sequence = int.Parse(row.Cells[CellIndexSequence].Value.ToString());
                    var detail = ImpSettingDetailsList.FirstOrDefault(x => x.Sequence == sequence);
                    var field = (Fields)detail.Sequence;

                    row.Cells[CellIndexIsUnique].Value = (detail.IsUnique == 1);
                    row.Cells[CellIndexOverWrite].Value = (detail.DoOverwrite == 1);

                    //取込有無のグリットコンボー設定
                    var import = $"TRKM{detail.BaseImportDivision}";
                    var comboCell = (ComboBoxCell)row.Cells[CellIndexImportDivision];
                    if (cbxAutoCreationCustomer.Checked
                        && (field == Fields.CustomerName
                         || field == Fields.CustomerKana))
                    {
                        comboCell.DataSource = comboSetting.Where(x => x.ItemId == import && x.ItemKey != "0")
                            .Select(c => new { c.ItemKey, c.ItemValue }).ToArray();
                    }
                    else
                    {
                        comboCell.DataSource = comboSetting.Where(x => x.ItemId == import)
                            .Select(c => new { c.ItemKey, c.ItemValue }).ToArray();
                    }

                    comboCell.DisplayMember = "ItemValue";
                    comboCell.ValueMember = "ItemKey";
                    comboCell.Value = (field == Fields.CustomerCode || field == Fields.BilledAt) ? 1 : detail.ImportDivision;

                    //属性情報のグリットコンボー設定
                    if (detail.BaseAttributeDivision != 0)
                    {
                        var attr = $"ATTR{detail.BaseAttributeDivision}";
                        var attributeSource = comboSetting.Where(x => x.ItemId == attr)
                            .Select(c => new { c.ItemKey, c.ItemValue }).ToArray();

                        comboCell = (ComboBoxCell)row.Cells[CellIndexAttribute];
                        comboCell.DataSource = attributeSource;
                        comboCell.DisplayMember = "ItemValue";
                        comboCell.ValueMember = "ItemKey";
                        comboCell.Value = detail.AttributeDivision;
                    }

                    if (field == Fields.CustomerCode
                     || field == Fields.BillingAmount)
                        row.Cells[CellIndexAttribute].Enabled = false;

                    // 項目番号列の設定
                    if (detail.FieldIndex != 0)
                    {
                        row.Cells[CellIndexFieldIndex].Value = detail.FieldIndex;
                    }

                    //会社コード、数量、単位、単価、契約番号、消費税率 の取込無固定設定
                    if (field == Fields.CompanyCode
                     || field == Fields.Quantity
                     || field == Fields.UnitSymbol
                     || field == Fields.UnitPrice
                     || field == Fields.ContractNumber
                     || field == Fields.TaxRate)
                    {
                        row.Cells[CellIndexImportDivision].Enabled = false;
                    }

                    //得意先コード、請求日の取込有を設定
                    if (field == Fields.CustomerCode
                     || field == Fields.BilledAt)
                    {
                        row.Cells[CellIndexImportDivision].Enabled = false;
                    }

                    // 数量、単位、単価のVOne項目名無効設定
                    if (field == Fields.Quantity
                     || field == Fields.UnitSymbol
                     || field == Fields.UnitPrice)
                    {
                        row.Cells[CellIndexFieldName].Enabled = false;
                    }

                    // 契約番号の取込有無設定
                    if (field == Fields.ContractNumber)
                    {
                        row.Cells[CellIndexImportDivision].Value = (ApplicationControl.RegisterContractInAdvance == 1) ? 1 : 0;
                    }

                    //取込有無列の値による有効・無効設定

                    var value = row.Cells[CellIndexImportDivision].DisplayText;

                    if (value.Contains("取込有"))
                    {
                        EnableImportSetting(row.Index);
                    }

                    if (value.Contains("固定値"))
                    {
                        EnableFixedValueSetting(row.Index);
                    }

                    if (value.Contains("取込無"))
                    {
                        DisableImportSetting(row.Index);
                    }

                    //入金予定日のマスター空欄の場合の有効・無効設定
                    if (field == Fields.DueAt && value.Contains("空欄"))
                    {
                        row.Cells[CellIndexFieldIndex].Enabled = true;
                        row.Cells[CellIndexAttribute].Enabled = true;
                        row.Cells[CellIndexOverWrite].Enabled = false;
                        if (cbxAutoCreationCustomer.Checked == false)
                        {
                            row.Cells[CellIndexFixedValue].Enabled = false;
                        }
                    }

                    if (value.Contains("得意先コード")
                        && (field == Fields.CustomerName
                         || field == Fields.CustomerKana))
                    {
                        row.Cells[CellIndexFieldIndex].Enabled = false;
                        row.Cells[CellIndexAttribute].Enabled = false;
                        row.Cells[CellIndexFixedValue].Enabled = false;
                        row.Cells[CellIndexOverWrite].Enabled = false;
                        row.Cells[CellIndexIsUnique].Enabled = false;
                    }

                    // 請求部門
                    if (field == Fields.DepartmentCode)
                    {
                        var cell = (GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell)row.Cells[CellIndexFixedValue];
                        cell.Format = DataExpression.DepartmentCodeFormatString;
                        cell.MaxLength = DataExpression.DepartmentCodeLength;
                    }

                    // 債権科目
                    if (field == Fields.DebitAccountTitleCode)
                    {
                        var cell = (GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell)row.Cells[CellIndexFixedValue];
                        cell.Format = DataExpression.AccountTitleCodeFormatString;
                        cell.MaxLength = DataExpression.AccountTitleCodeLength;
                    }

                    // 入金予定日, 請求締日, 請求区分, 回収区分
                    if (field == Fields.DueAt
                     || field == Fields.ClosingAt
                     || field == Fields.BillingCategoryCode
                     || field == Fields.CollectCategoryCode)
                    {
                        var cell = (GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell)row.Cells[CellIndexFixedValue];
                        cell.Format = "9";
                        cell.MaxLength = field == Fields.DueAt ? 3 : 2;
                        cell.Style.ImeMode = ImeMode.Disable;
                    }

                    // 担当者
                    if (field == Fields.StaffCode)
                    {
                        var cell = (GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell)row.Cells[CellIndexFixedValue];
                        cell.Format = DataExpression.StaffCodeFormatString;
                        cell.MaxLength = DataExpression.StaffCodeLength;
                    }

                    // 得意先名称, 得意先カナ
                    if (field == Fields.CustomerName
                     || field == Fields.CustomerKana)
                    {
                        row.Cells[CellIndexFixedValue].Style.ImeMode = (field == Fields.CustomerName) ? ImeMode.Hiragana : ImeMode.KatakanaHalf;
                    }

                    // 歩引利用
                    if (field == Fields.UseDiscount)
                    {
                        var cell = (GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell)row.Cells[CellIndexFixedValue];
                        cell.Format = "9";
                        cell.MaxLength = 1;
                        cell.Style.ImeMode = ImeMode.Disable;
                    }

                    // 通貨コード
                    if (field == Fields.CurrencyCode)
                    {
                        var cell = (GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell)row.Cells[CellIndexFixedValue];
                        cell.Format = "A";
                        cell.MaxLength = 3;
                    }

                    // 専用銀行コード, 専用支店コード, 仮想支店コード
                    if (field == Fields.ExclusiveBankCode
                     || field == Fields.ExclusiveBranchCode
                     || field == Fields.ExclusiveVirtualBranchCode )
                    {
                        var cell = (GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell)row.Cells[CellIndexFixedValue];
                        cell.Format = "9";
                        cell.MaxLength = field == Fields.ExclusiveBankCode ? 4 : 3;
                        cell.Style.ImeMode = ImeMode.Disable;
                    }

                    if (names.Count > 0
                        && (field == Fields.Note1
                         || field == Fields.Note2
                         || field == Fields.Note3
                         || field == Fields.Note4
                         || field == Fields.Note5
                         || field == Fields.Note6
                         || field == Fields.Note7
                         || field == Fields.Note8
                         ))
                    {
                        var name = names.Where(x => x.ColumnName == detail.TargetColumn).FirstOrDefault();
                        if (name != null)
                            row.Cells[CellIndexFieldName].Value = name.DisplayColumnName;
                    }
                }
                Modified = false;
            }
        }

        private void grid_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            if (e.CellIndex == CellIndexImportDivision)
            {
                var value = grid.CurrentCell.EditedFormattedValue?.ToString() ?? "";

                if (value.Contains("取込有"))
                {
                    EnableImportSetting(e.RowIndex);
                }

                if (value.Contains("固定値"))
                {
                    EnableFixedValueSetting(e.RowIndex);
                }

                if (value.Contains("取込無"))
                {
                    changeSelected = true;
                    DisableImportSetting(e.RowIndex);
                }

                if (value.Contains("空欄")
                    && ConvertRowToField(grid.CurrentRow) == Fields.DueAt)
                {
                    grid.CurrentRow.Cells[CellIndexIsUnique].Enabled = true;
                    grid.CurrentRow.Cells[CellIndexFieldIndex].Enabled = true;
                    grid.CurrentRow.Cells[CellIndexAttribute].Enabled = true;
                }
                if (value.Contains("得意先コード"))
                {
                    grid.CurrentRow.Cells[CellIndexFixedValue].Value = null;
                    grid.CurrentRow.Cells[CellIndexFixedValue].Enabled = false;

                    grid.CurrentRow.Cells[CellIndexFieldIndex].Value = null;
                    grid.CurrentRow.Cells[CellIndexFieldIndex].Enabled = false;

                    grid.CurrentRow.Cells[CellIndexAttribute].Value = null;
                    grid.CurrentRow.Cells[CellIndexAttribute].Enabled = false;
                }
            }
        }

        private void EnableImportSetting(int rowIndex)
        {
            var row = grid.Rows[rowIndex];
            var field = ConvertRowToField(row);

            row.Cells[CellIndexIsUnique].Enabled = true;
            row.Cells[CellIndexFieldIndex].Enabled = true;

            //得意先マスター自動作成の時固定値の有効設定
            if (IsFixedValueEnabledWhenImportDivisionImportOrDisabled(field)
                && cbxAutoCreationCustomer.Checked)
            {
                row.Cells[CellIndexFixedValue].Enabled = true;
            }
            else
            {
                row.Cells[CellIndexFixedValue].Value = null;
                row.Cells[CellIndexFixedValue].Enabled = false;
            }

            if (IsAttributeEnabledWhenImportDivisionImport(field))
            {
                row.Cells[CellIndexAttribute].Enabled = true;
            }
            else
                row.Cells[CellIndexAttribute].Enabled = false;

            if (IsUniqueEnabledWhenImportDivisionImport(field))
            {
                row.Cells[CellIndexIsUnique].Enabled = false;
                row.Cells[CellIndexIsUnique].Selected = false;
            }

            if (IsOverwirteEnabledWhenImportDivisionImport(field))
            {
                row.Cells[CellIndexOverWrite].Enabled = true;
            }
            else
            {
                row.Cells[CellIndexOverWrite].Enabled = false;
            }

            if (field == Fields.Price)
            {
                //金額（抜）：取込無　時　請求金額の取込無も表示
                var index = GetGridRowIndex(Fields.BillingAmount);
                var combocell = (ComboBoxCell)grid.Rows[index].Cells[CellIndexImportDivision];
                combocell.DataSource = BillingAmountSetting.Select(c => new { c.ItemKey, c.ItemValue }).ToArray();

                grid.Rows[index].Cells[CellIndexImportDivision].Value = 0;
                grid.Rows[index].Cells[CellIndexImportDivision].Enabled = false;
                grid.Rows[index].Cells[CellIndexFieldIndex].Enabled = false;
                grid.Rows[index].Cells[CellIndexFieldIndex].Value = null;
                grid.Rows[index].Cells[CellIndexIsUnique].Enabled = false;
                grid.Rows[index].Cells[CellIndexIsUnique].Value = false;

                index = GetGridRowIndex(Fields.TaxRate);
                var taxRateImportDivision = Convert.ToInt32(grid.Rows[index].Cells[CellIndexImportDivision].Value);

                index = GetGridRowIndex(Fields.TaxAmount);
                var combocellTaxAmt = (ComboBoxCell)grid.Rows[index].Cells[CellIndexImportDivision];
                combocellTaxAmt.DataSource = TaxAmountSetting.Where(x => x.ItemKey != "0")
                    .Select(c => new { c.ItemKey, c.ItemValue }).ToArray();

                grid.Rows[index].Cells[CellIndexImportDivision].Value = taxRateImportDivision == 0 ? 2 : 1;
                grid.Rows[index].Cells[CellIndexImportDivision].Enabled = true;
                grid.Rows[index].Cells[CellIndexFieldIndex].Enabled = taxRateImportDivision == 0;
                grid.Rows[index].Cells[CellIndexIsUnique].Enabled = true;
            }

            if (field == Fields.BillingAmount)
            {
                grid.CommitEdit();
                var index = GetGridRowIndex(Fields.BillingAmount);
                var billingAmountImportDivision = Convert.ToInt32(grid.Rows[index].Cells[CellIndexImportDivision].Value);
                var isBillingAmountPlusTax = billingAmountImportDivision == 3 || billingAmountImportDivision == 4;

                index = GetGridRowIndex(Fields.TaxAmount);
                grid.Rows[index].Cells[CellIndexImportDivision].Value = isBillingAmountPlusTax ? 2 : 0;
                grid.Rows[index].Cells[CellIndexImportDivision].Enabled = false;
                grid.Rows[index].Cells[CellIndexFieldIndex].Enabled = isBillingAmountPlusTax;
                grid.Rows[index].Cells[CellIndexIsUnique].Enabled = isBillingAmountPlusTax;

                if (!isBillingAmountPlusTax)
                {
                    grid.Rows[index].Cells[CellIndexFieldIndex].Value = null;
                    grid.Rows[index].Cells[CellIndexIsUnique].Value = false;
                }
            }

            if (field == Fields.TaxAmount)
            {
                grid.CommitEdit();
                var index = GetGridRowIndex(Fields.TaxAmount);
                var indexTaxRate = GetGridRowIndex(Fields.TaxRate);
                var taxAmountImportDivision = Convert.ToInt32(grid.Rows[index].Cells[CellIndexImportDivision].Value);

                var isAvailable = taxAmountImportDivision == 1;
                grid.Rows[indexTaxRate].Cells[CellIndexImportDivision].Value = isAvailable ? 1 : 0;
                grid.Rows[indexTaxRate].Cells[CellIndexFieldIndex].Value = null;
                grid.Rows[indexTaxRate].Cells[CellIndexFieldIndex].Enabled = isAvailable;
                grid.Rows[indexTaxRate].Cells[CellIndexAttribute].Value = null;
                grid.Rows[indexTaxRate].Cells[CellIndexAttribute].Enabled = isAvailable;
            }

        }

        private void EnableFixedValueSetting(int rowIndex)
        {
            grid.Rows[rowIndex].Cells[CellIndexFixedValue].Enabled = true;

            grid.Rows[rowIndex].Cells[CellIndexFieldIndex].Value = null;
            grid.Rows[rowIndex].Cells[CellIndexFieldIndex].Enabled = false;

            grid.Rows[rowIndex].Cells[CellIndexAttribute].Value = null;
            grid.Rows[rowIndex].Cells[CellIndexAttribute].Enabled = false;

            grid.Rows[rowIndex].Cells[CellIndexIsUnique].Value = false;
            grid.Rows[rowIndex].Cells[CellIndexIsUnique].Enabled = false;

            grid.Rows[rowIndex].Cells[CellIndexOverWrite].Value = false;
            grid.Rows[rowIndex].Cells[CellIndexOverWrite].Enabled = false;
        }

        //bool changeSelected = false;
        private void DisableImportSetting(int rowIndex)
        {
            var row = grid.Rows[rowIndex];
            var field = ConvertRowToField(row);

            row.Cells[CellIndexIsUnique].Enabled = false;
            row.Cells[CellIndexIsUnique].Value = false;

            row.Cells[CellIndexOverWrite].Enabled = false;
            row.Cells[CellIndexOverWrite].Value = false;

            //得意先マスター自動作成の時固定値の有効設定
            if (IsFixedValueEnabledWhenImportDivisionImportOrDisabled(field)
                && cbxAutoCreationCustomer.Checked)
            {
                row.Cells[CellIndexFixedValue].Enabled = true;
            }
            else
            {
                row.Cells[CellIndexFixedValue].Value = null;
                row.Cells[CellIndexFixedValue].Enabled = false;
            }

            //得意先名称と得意先カナの取込有無列無効設定
            if (field == Fields.CustomerName
             || field == Fields.CustomerKana)
            {
                row.Cells[CellIndexImportDivision].Enabled = false;
            }

            if (field == Fields.CollationKey && !cbxAutoCreationCustomer.Checked)
            {
                row.Cells[CellIndexImportDivision].Enabled = false;
            }

            row.Cells[CellIndexFieldIndex].Value = null;
            row.Cells[CellIndexFieldIndex].Enabled = false;

            row.Cells[CellIndexAttribute].Value = null;
            row.Cells[CellIndexAttribute].Enabled = false;


            if (field == Fields.Price)
            {   // importdivision 2 value display in initial load 
                var taxAmountImportDivision = ImpSettingDetailsList.FirstOrDefault(k => k.Sequence == (int)Fields.TaxAmount).ImportDivision;

                grid.CommitEdit();

                //金額（抜）：取込無　時　請求金額の取込無を非表示
                var indexBillingAmt = GetGridRowIndex(Fields.BillingAmount);
                var combocell = (ComboBoxCell)grid.Rows[indexBillingAmt].Cells[CellIndexImportDivision];
                combocell.DataSource = BillingAmountSetting.Where(x => x.ItemKey != "0")
                    .Select(c => new { c.ItemKey, c.ItemValue }).ToArray();

                var billingAmountImportDivision = Convert.ToInt32(grid.Rows[indexBillingAmt].Cells[CellIndexImportDivision].Value);
                var isBillingAmountPlusTax = billingAmountImportDivision == 3 || billingAmountImportDivision == 4;

                if (changeSelected == false)
                {
                    grid.Rows[indexBillingAmt].Cells[CellIndexImportDivision].Value = billingAmountImportDivision;
                }
                else
                {
                    taxAmountImportDivision = 0;
                    grid.Rows[indexBillingAmt].Cells[CellIndexImportDivision].Value = 1;

                    //消費税率
                    var indexTaxRate = GetGridRowIndex(Fields.TaxRate);
                    grid.Rows[indexTaxRate].Cells[CellIndexImportDivision].Value = 0;
                    grid.Rows[indexTaxRate].Cells[CellIndexFieldIndex].Value = null;
                    grid.Rows[indexTaxRate].Cells[CellIndexFieldIndex].Enabled = false;
                    grid.Rows[indexTaxRate].Cells[CellIndexAttribute].Value = null;
                    grid.Rows[indexTaxRate].Cells[CellIndexAttribute].Enabled = false;
                }

                grid.Rows[indexBillingAmt].Cells[CellIndexImportDivision].Enabled = true;
                grid.Rows[indexBillingAmt].Cells[CellIndexFieldIndex].Enabled = true;
                grid.Rows[indexBillingAmt].Cells[CellIndexIsUnique].Enabled = true;

                //消費税
                var indexTaxAmount = GetGridRowIndex(Fields.TaxAmount);
                var combocellTaxAmt = (ComboBoxCell)grid.Rows[indexTaxAmount].Cells[CellIndexImportDivision];
                combocellTaxAmt.DataSource = TaxAmountSetting.Select(c => new { c.ItemKey, c.ItemValue }).ToArray();

                grid.Rows[indexTaxAmount].Cells[CellIndexImportDivision].Value = taxAmountImportDivision;
                grid.Rows[indexTaxAmount].Cells[CellIndexImportDivision].Enabled = false;
                grid.Rows[indexTaxAmount].Cells[CellIndexFieldIndex].Enabled = isBillingAmountPlusTax;
                grid.Rows[indexTaxAmount].Cells[CellIndexIsUnique].Enabled = isBillingAmountPlusTax;

                if (!isBillingAmountPlusTax)
                {
                    grid.Rows[indexTaxAmount].Cells[CellIndexFieldIndex].Value = null;
                    grid.Rows[indexTaxAmount].Cells[CellIndexIsUnique].Value = false;
                }
            }

            if (field == Fields.TaxAmount)
            {
                grid.CommitEdit();
                var index = GetGridRowIndex(Fields.TaxAmount);
                var indexTaxRate = GetGridRowIndex(Fields.TaxRate);
                var taxAmountImportDivision = Convert.ToInt32(grid.Rows[index].Cells[CellIndexImportDivision].Value);

                var isAvailable = taxAmountImportDivision == 1;
                grid.Rows[indexTaxRate].Cells[CellIndexImportDivision].Value = isAvailable ? 1 : 0;
                grid.Rows[indexTaxRate].Cells[CellIndexFieldIndex].Enabled = isAvailable;
                grid.Rows[indexTaxRate].Cells[CellIndexAttribute].Enabled = isAvailable;
            }

        }

        private void grid_CellEnter(object sender, CellEventArgs e)
        {
            if (e.CellIndex == CellIndexFixedValue && grid.CurrentRow != null)
            {
                var field = ConvertRowToField(grid.CurrentRow);
                if (IsCodeSearchDialogEnabled(field))
                {
                    BaseContext.SetFunction09Enabled(true);
                    return;
                }
            }
            BaseContext.SetFunction09Enabled(false);
        }

        private void grid_CellLeave(object sender, CellEventArgs e)
        {
            if (e.CellIndex != CellIndexFixedValue
                || e.RowIndex < 0) return;
            var field = ConvertRowToField(grid.Rows[e.RowIndex]);
            if (IsCodeSearchDialogEnabled(field))
            {
                grid.CommitEdit();
            }
        }
        #endregion

        private bool ValidateRequireData()
        {
            if (!txtPatternNumber.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblPatternNumber.Text))) return false;
            if (!txtPatternName.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblPatternName.Text))) return false;
            if (!txtInitialDirectory.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblInitialDirectory.Text))) return false;

            if (!Directory.Exists(txtInitialDirectory.Text))
            {
                txtInitialDirectory.Focus();
                ShowWarningDialog(MsgWngNotExistFolder);
                return false;
            }

            if (!nmbStartLineCount.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblStartLineCount.Text))) return false;

            return true;
        }

        private bool ValidateGridData()
        {
            int? collectOffsetMonth = null;
            int? closingDay = null;

            Func<string, bool> DueAtCheck = (dueDate)
                => dueDate.Length == 3
                && (int.Parse(dueDate.Substring(0, 1)) >= 0 && int.Parse(dueDate.Substring(0, 1)) < 10)
                && (int.Parse(dueDate.Substring(1)) > 0 && (int.Parse(dueDate.Substring(1)) < 28 ||
                    int.Parse(dueDate.Substring(1)) == 99));

            Func<string, bool> ClosingAtCheck = (closingAt)
                => (int.Parse(closingAt) >= 0 && (int.Parse(closingAt) < 28 ||
                    int.Parse(closingAt) == 99));

            Func<bool> InvalidCollectOffsetMonth = ()
                => collectOffsetMonth != null
                && closingDay != null
                && closingDay == 0
                && collectOffsetMonth != 0;

            Action<Cell> setCellFocus = cell =>
            {
                this.ActiveControl = grid;
                grid.CurrentCell = cell;
                grid.CurrentCell.Selected = true;
            };

            foreach (var row in grid.Rows)
            {
                var field = ConvertRowToField(row);
                var fieldName = row.Cells[CellIndexFieldName].Value.ToString();
                var importDivision = row.Cells[CellIndexImportDivision].FormattedValue.ToString();
                var fieldIndexCell = row.Cells[CellIndexFieldIndex];
                var attributeDivisionCell = row.Cells[CellIndexAttribute];
                var fixedValueCell = row.Cells[CellIndexFixedValue];
                var fixedValue = fixedValueCell.Value?.ToString();

                if (fieldIndexCell.Enabled && string.IsNullOrWhiteSpace(fieldIndexCell.Value?.ToString()))
                {
                    setCellFocus(fieldIndexCell);
                    ShowWarningDialog(MsgWngInputRequired, "項目番号");
                    return false;
                }

                if (fixedValueCell.Enabled && string.IsNullOrWhiteSpace(fixedValue))
                {
                    setCellFocus(fixedValueCell);
                    ShowWarningDialog(MsgWngInputRequired, "固定値");
                    return false;
                }

                if (fixedValueCell.Enabled && !string.IsNullOrWhiteSpace(fixedValue))
                {
                    // 入金予定日の値チェック
                    if (field == Fields.DueAt)
                    {
                        if (!DueAtCheck(fixedValue))
                        {
                            setCellFocus(fixedValueCell);
                            ShowWarningDialog(MsgWngInvalidValueOfReceiptDueDateFormat);
                            return false;
                        }
                        collectOffsetMonth = int.Parse(fixedValue.Substring(0,1));
                    }

                    // 請求部門のマスター存在チェック
                    if (field == Fields.DepartmentCode && !ExistDepartmentMaster(fixedValue))
                    {
                        fixedValueCell.Value = null;
                        setCellFocus(fixedValueCell);
                        ShowWarningDialog(MsgWngMasterNotExist, fieldName, fixedValue);
                        return false;
                    }

                    // 債権科目のマスター存在チェック
                    if (field == Fields.DebitAccountTitleCode && !ExistAccountTitleMaster(fixedValue))
                    {
                        fixedValueCell.Value = null;
                        setCellFocus(fixedValueCell);
                        ShowWarningDialog(MsgWngMasterNotExist, fieldName, fixedValue);
                        return false;
                    }

                    // 請求締日のチェック
                    if (field == Fields.ClosingAt)
                    {
                        if (!ClosingAtCheck(fixedValue))
                        {
                            setCellFocus(fixedValueCell);
                            ShowWarningDialog(MsgWngNumberValueValid0To27or99, fieldName);
                            return false;
                        }
                        closingDay = int.Parse(fixedValue);
                    }

                    // 担当者のマスター存在チェック
                    if (field == Fields.StaffCode && !ExistStaffMaster(fixedValue))
                    {
                        fixedValueCell.Value = null;
                        setCellFocus(fixedValueCell);
                        ShowWarningDialog(MsgWngMasterNotExist, fieldName, fixedValue);
                        return false;
                    }

                    // 請求区分, 回収区分のマスター存在チェック
                    if (field == Fields.BillingCategoryCode || field == Fields.CollectCategoryCode)
                    {
                        var option = new CategorySearch();
                        option.CompanyId = CompanyId;
                        option.Codes = new[] { fixedValue };
                        option.CategoryType = (field == Fields.BillingCategoryCode) ? CategoryType.Billing : CategoryType.Collect;
                        if (!ExistCategoryMaster(option))
                        {
                            fixedValueCell.Value = null;
                            setCellFocus(fixedValueCell);
                            ShowWarningDialog(MsgWngMasterNotExist, fieldName, fixedValue);
                            return false;
                        }
                    }

                    // 得意先名称, 得意先カナの桁数チェック
                    if (field == Fields.CustomerName || field == Fields.CustomerKana)
                    {
                        // ImporterSettingDetail.FixedValue が NVarChar(50) なので、
                        // 140の検証処理にかかることは無いが、
                        // 後々 DataBase の 変更が発生したときのため、ロジックだけ残す
                        if (!string.IsNullOrEmpty(fixedValue) && fixedValue.Length > 140)
                        {
                            setCellFocus(fixedValueCell);
                            ShowWarningDialog(MsgWngPara1InputPara2CharCount, fieldName, "140");
                            return false;
                        }
                    }

                    // 歩引利用の値チェック（0または1以外の場合エラー）
                    if (field == Fields.UseDiscount
                        && (fixedValue != "0" && fixedValue != "1"))
                    {
                        setCellFocus(fixedValueCell);
                        ShowWarningDialog(MsgWngInputInvalidLetter, fieldName);
                        return false;
                    }

                    //通貨コードのマスター存在チェック
                    if (field == Fields.CurrencyCode && !ExistCurrencyMaster(fixedValue))
                    {
                        fixedValueCell.Value = null;
                        setCellFocus(fixedValueCell);
                        ShowWarningDialog(MsgWngMasterNotExist, "通貨", fixedValue);
                        return false;
                    }

                }

                // 属性情報が未選択チェック
                if (attributeDivisionCell.Enabled
                    && string.IsNullOrWhiteSpace(attributeDivisionCell.EditedFormattedValue?.ToString()))
                {
                    setCellFocus(attributeDivisionCell);
                    ShowWarningDialog(MsgWngSelectionRequired, "属性情報");
                    return false;
                }
            }

            //都度請求の場合、入金予定日の前1桁は0かの確認
            if (InvalidCollectOffsetMonth())
            {
                var rowIndex = GetGridRowIndex(Fields.DueAt);
                setCellFocus(grid.Rows[rowIndex].Cells[CellIndexFixedValue]);
                ShowWarningDialog(MsgWngNumberValueValid0ForFirstAnd00To99ForRest);
                return false;
            }

            {
                var indexExclusiveBankCode = GetGridRowIndex(Fields.ExclusiveBankCode);
                var indexExclusiveBranchCode = GetGridRowIndex(Fields.ExclusiveBranchCode);
                var indexExclusiveVirtualBranchCode = GetGridRowIndex(Fields.ExclusiveVirtualBranchCode);
                var indexExclusiveAccountNumber = GetGridRowIndex(Fields.ExclusiveAccountNumber);

                //銀行コード、支店コード、仮想支店コード、仮想口座番号　のいずれかが「取込無」のチェック
                var impDivBankCode = grid.Rows[indexExclusiveBankCode].Cells[CellIndexImportDivision].DisplayText;
                var impDivBranchCode = grid.Rows[indexExclusiveBranchCode].Cells[CellIndexImportDivision].DisplayText;
                var impDivVirtualBranchCode = grid.Rows[indexExclusiveVirtualBranchCode].Cells[CellIndexImportDivision].DisplayText;
                var imptDivAccountNumber = grid.Rows[indexExclusiveAccountNumber].Cells[CellIndexImportDivision].DisplayText;
                var allDisable = (impDivBankCode.Contains("取込無")
                    && impDivBranchCode.Contains("取込無")
                    && impDivVirtualBranchCode.Contains("取込無")
                    && imptDivAccountNumber.Contains("取込無"));
                var allEnable = (!impDivBankCode.Contains("取込無")
                    && !impDivBranchCode.Contains("取込無")
                    && !impDivVirtualBranchCode.Contains("取込無")
                    && !imptDivAccountNumber.Contains("取込無"));

                if (!allDisable && (!allDisable && !allEnable))
                {
                    var rowIndex = 0;
                    if (impDivBankCode.Contains("取込無")) rowIndex = indexExclusiveBankCode;
                    else if (impDivBranchCode.Contains("取込無")) rowIndex = indexExclusiveBranchCode;
                    else if (impDivVirtualBranchCode.Contains("取込無")) rowIndex = indexExclusiveVirtualBranchCode;
                    else if (imptDivAccountNumber.Contains("取込無")) rowIndex = indexExclusiveAccountNumber;

                    setCellFocus(grid.Rows[rowIndex].Cells[CellIndexImportDivision]);
                    ShowWarningDialog(MsgWngBankInfoIncomplete);
                    return false;
                }
            }
            return true;
        }

        private ImporterSetting PrepareImporterSetting()
        {
            var header = new ImporterSetting();
            header.CompanyId = CompanyId;
            header.Code = txtPatternNumber.Text;
            header.Name = txtPatternName.Text;
            header.FormatId = FormatId;
            header.InitialDirectory = txtInitialDirectory.Text;
            header.StartLineCount = Convert.ToInt32(nmbStartLineCount.Value);
            if (rdoNoAction.Checked) header.PostAction = 0;
            else if (rdoDelete.Checked) header.PostAction = 1;
            else if (rdoAddDate.Checked) header.PostAction = 2;
            header.IgnoreLastLine = cbxIgnoreLastLine.Checked ? 1 : 0;
            header.AutoCreationCustomer = cbxAutoCreationCustomer.Checked ? 1 : 0;
            header.UpdateBy = Login.UserId;
            header.UpdateAt = UpdateAt;
            header.CreateBy = Login.UserId;
            header.CreateAt = DateTime.Now;
            return header;
        }

        private List<ImporterSettingDetail> PrepareImporterSettingDetail()
        {
            var result = new List<ImporterSettingDetail>();
            var nowDateTime = DateTime.Now;

            foreach (var row in grid.Rows)
            {
                var detail = new ImporterSettingDetail();
                detail.Sequence = Convert.ToInt32(row.Cells[CellIndexSequence].Value);
                detail.FixedValue = Convert.ToString(row.Cells[CellIndexFixedValue].DisplayText);
                detail.IsUnique = (Convert.ToBoolean(row.Cells[CellIndexIsUnique].EditedFormattedValue)) ? 1 : 0;
                detail.DoOverwrite = (Convert.ToBoolean(row.Cells[CellIndexOverWrite].EditedFormattedValue)) ? 1 : 0;
                detail.ImportDivision = Convert.ToInt32(row.Cells[CellIndexImportDivision].Value);
                detail.Caption = "";
                detail.FieldIndex = Convert.ToInt32(row.Cells[CellIndexFieldIndex].Value);
                detail.AttributeDivision = Convert.ToInt32(row.Cells[CellIndexAttribute].Value);
                detail.ImporterSettingId = ImporterSettingId;
                detail.UpdateBy = Login.UserId;
                detail.UpdateAt = nowDateTime;
                detail.CreateBy = Login.UserId;
                detail.CreateAt = nowDateTime;
                result.Add(detail);
            }
            return result;
        }

        private void AddHandlers()
        {
            foreach (var control in this.GetAll<Control>())
            {
                if (control is Common.Controls.VOneTextControl)
                    ((Common.Controls.VOneTextControl)control).TextChanged += OnContentChanged;
                if (control is Common.Controls.VOneComboControl)
                    ((Common.Controls.VOneComboControl)control).SelectedIndexChanged += OnContentChanged;
                if (control is CheckBox)
                    ((CheckBox)control).CheckedChanged += OnContentChanged;
                if (control is RadioButton)
                    ((RadioButton)control).CheckedChanged += OnContentChanged;
                if (control is Common.Controls.VOneNumberControl)
                    ((Common.Controls.VOneNumberControl)control).ValueChanged += OnContentChanged;
            }
        }

        private void OnContentChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        private void grid_CellValueChanged(object sender, CellEventArgs e)
        {
            Modified = true;
            if (e.CellIndex != CellIndexFixedValue
                || e.RowIndex < 0
                || grid.Rows[e.RowIndex].Cells[CellIndexFixedValue].Value == null) return;
            var row = grid.Rows[e.RowIndex];
            var sequence = int.Parse(grid.CurrentRow.Cells[CellIndexSequence].Value.ToString());
            var field = ConvertRowToField(row);

            var value = row.Cells[CellIndexFixedValue].Value?.ToString() ?? "";
            if (field == Fields.CustomerKana)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                    value = EbDataHelper.ConvertToPayerName(value, LegalPersonalities);
                }
                row.Cells[CellIndexFixedValue].Value = value;
            }

            var length = 0;
            char? paddingChar = null;
            if (field == Fields.DepartmentCode)
            {
                length = DataExpression.DepartmentCodeLength;
                paddingChar = DataExpression.DepartmentCodePaddingChar;
            }

            if (field == Fields.DebitAccountTitleCode)
            {
                length = DataExpression.AccountTitleCodeLength;
                paddingChar = DataExpression.AccountTitleCodePaddingChar;
            }

            if (field == Fields.StaffCode)
            {
                length = DataExpression.StaffCodeLength;
                paddingChar = DataExpression.StaffCodePaddingChar;
            }

            if (field == Fields.ClosingAt
             || field == Fields.BillingCategoryCode
             || field == Fields.CollectCategoryCode)
            {
                length = 2;
                paddingChar = '0';
            }

            if (field == Fields.ExclusiveBankCode)
            {
                length = 4;
                paddingChar = '0';
            }

            if (field == Fields.ExclusiveBranchCode
             || field == Fields.ExclusiveVirtualBranchCode)
            {
                length = 3;
                paddingChar = '0';
            }

            if (paddingChar.HasValue)
            {
                row.Cells[CellIndexFixedValue].Value = value.PadLeft(length, paddingChar.Value);
            }

        }

        private void cbxAutoCreationCustomer_CheckedChanged(object sender, EventArgs e)
        {
            if (grid.Rows.Count < 1) return;
            var autoCreation = cbxAutoCreationCustomer.Checked;
            foreach (var row in grid.Rows)
            {
                var field = ConvertRowToField(row);
                if (IsCustomerMasterFixedValueEnabled(field)
                    && !row.Cells[CellIndexImportDivision].DisplayText.Contains("固定値"))
                {
                    row.Cells[CellIndexFixedValue].Enabled = autoCreation;
                    if (!autoCreation) row.Cells[CellIndexFixedValue].Value = null;
                }

                // 得意先名称, 得意先カナ, 照合番号
                if (!(field == Fields.CustomerName || field == Fields.CustomerKana || field == Fields.CollationKey)) continue;
                if (field == Fields.CustomerName
                 || field == Fields.CustomerKana)
                {
                    var combocell = (ComboBoxCell)row.Cells[CellIndexImportDivision];
                    if (autoCreation)
                    {
                        combocell.DataSource = CustomerSetting.Where(x => x.ItemKey != "0")
                            .Select(c => new { c.ItemKey, c.ItemValue }).ToArray();
                        row.Cells[CellIndexImportDivision].Value = 1;
                    }
                    else
                    {
                        combocell.DataSource = CustomerSetting.Select(c => new { c.ItemKey, c.ItemValue }).ToArray();
                    }
                }
                row.Cells[CellIndexImportDivision].Enabled = autoCreation;

                if (!autoCreation)
                {
                    row.Cells[CellIndexImportDivision].Value = 0;
                    row.Cells[CellIndexFieldIndex].Enabled = false;
                    row.Cells[CellIndexFieldIndex].Value = null;
                    row.Cells[CellIndexFixedValue].Enabled = false;
                    row.Cells[CellIndexFixedValue].Value = null;
                    row.Cells[CellIndexAttribute].Enabled = false;
                    row.Cells[CellIndexAttribute].Value = null;
                }

            }
        }

        private void txtCode_Validated(object sender, EventArgs e)
        {
            try
            {
                if (txtPatternNumber.Text != "")
                {
                    txtPatternNumber.Text = txtPatternNumber.Text.PadLeft(2, '0');
                    var loadTask = new List<Task>();
                    loadTask.Add(LoadPatternData(txtPatternNumber.Text));
                    loadTask.Add(LoadGridDataAsync());
                    ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
                    SettingGridData();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnCode_Click(object sender, EventArgs e)
        {
            try
            {
                var importerSetting = this.ShowBillingImporterSettingSearchDialog();
                if (importerSetting != null)
                {
                    txtPatternNumber.Text = importerSetting.Code;
                    txtPatternName.Text = importerSetting.Name;
                    txtPatternNumber.Enabled = false;
                    btnPatternNoSearch.Enabled = false;
                    var loadTask = new List<Task>();
                    loadTask.Add(LoadPatternData(txtPatternNumber.Text));
                    loadTask.Add(LoadGridDataAsync());
                    ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
                    SettingGridData();
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnInitialDirectory_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnInitialDirectory);

            var path = txtInitialDirectory.Text.Trim();
            var selectedPath = string.Empty;
            var rootBrowserPath = new List<string>();
            if (!LimitAccessFolder ?
                !ShowFolderBrowserDialog(path, out selectedPath) :
                !ShowRootFolderBrowserDialog(ApplicationControl.RootPath, out rootBrowserPath, FolderBrowserType.SelectFolder)) return;

            txtInitialDirectory.Text = (!LimitAccessFolder) ? selectedPath : rootBrowserPath.FirstOrDefault();
        }

        #region importer setting helper

        private int GetGridRowIndex(Fields field)
            => grid.Rows.FirstOrDefault(x => ConvertRowToField(x) == field)?.Index ?? -1;

        private Fields ConvertRowToField(Row row)
            => (Fields)int.Parse(row.Cells[CellIndexSequence].Value?.ToString());

        /// <summary>
        /// 固定値で F9/検索 を有効化する Fields
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool IsCodeSearchDialogEnabled(Fields field)
            => field == Fields.DepartmentCode
            || field == Fields.DebitAccountTitleCode
            || field == Fields.StaffCode
            || field == Fields.BillingCategoryCode
            || field == Fields.CollectCategoryCode
            || field == Fields.UseDiscount
            || field == Fields.CurrencyCode
            ;

        /// <summary>
        /// 得意先 固定値を有効化する Fields 一覧
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool IsCustomerMasterFixedValueEnabled(Fields field)
            => field == Fields.DueAt
            || field == Fields.ClosingAt
            || field == Fields.CollectCategoryCode
            || field == Fields.StaffCode
            || field == Fields.CustomerName
            || field == Fields.CustomerKana
            ;

        /// <summary>
        /// 得意先 自動作成時 取込区分が 取込/取込なし の場合に 固定値を有効化する Fields 一覧
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool IsFixedValueEnabledWhenImportDivisionImportOrDisabled(Fields field)
            => field == Fields.DueAt
            || field == Fields.ClosingAt
            || field == Fields.StaffCode
            || field == Fields.CollectCategoryCode
            ;

        /// <summary>
        /// 取込区分 取込 の場合に、属性区分を有効にする Fields 一覧
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool IsAttributeEnabledWhenImportDivisionImport(Fields field)
            => field == Fields.BilledAt
            || field == Fields.DueAt
            || field == Fields.SalesAt
            || field == Fields.InvoiceCode
            || field == Fields.ClosingAt
            || field == Fields.Note1
            || field == Fields.BillingCategoryCode
            || field == Fields.CollectCategoryCode
            || field == Fields.ContractNumber
            || field == Fields.Note2
            || field == Fields.Note3
            || field == Fields.Note4
            || field == Fields.Note5
            || field == Fields.Note6
            || field == Fields.Note7
            || field == Fields.Note8
            || field == Fields.CustomerName
            || field == Fields.CustomerKana
            || field == Fields.CollationKey
            || field == Fields.ExclusiveBankCode
            || field == Fields.ExclusiveBranchCode
            || field == Fields.ExclusiveVirtualBranchCode
            || field == Fields.ExclusiveAccountNumber
            || field == Fields.TaxRate
            ;

        /// <summary>
        /// 取込区分 取込 の場合に 重複 を有効にする Fields 一覧
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool IsUniqueEnabledWhenImportDivisionImport(Fields field)
            => field == Fields.CompanyCode
            || field == Fields.ContractNumber
            || field == Fields.CustomerName
            || field == Fields.CustomerKana
            || field == Fields.UseDiscount
            || field == Fields.ExclusiveBankCode
            || field == Fields.ExclusiveBranchCode
            || field == Fields.ExclusiveVirtualBranchCode
            || field == Fields.ExclusiveAccountNumber
            || field == Fields.TaxRate
            ;

        /// <summary>
        /// 取込区分 取込 の場合に 上書 を有効にする Fields 一覧
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool IsOverwirteEnabledWhenImportDivisionImport(Fields field)
            => field == Fields.CustomerCode
            || field == Fields.BilledAt
            || field == Fields.InvoiceCode
            || field == Fields.Note1
            || field == Fields.Note2
            || field == Fields.Note3
            || field == Fields.Note4
            || field == Fields.Note5
            || field == Fields.Note6
            || field == Fields.Note7
            || field == Fields.Note8
            || field == Fields.CurrencyCode
            || field == Fields.TaxRate
            ;

        #endregion

    }
}
