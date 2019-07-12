using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.ImporterSettingService;
using Rac.VOne.Client.Screen.StaffMasterService;
using Rac.VOne.Client.Screen.TaskScheduleMasterService;
using Rac.VOne.Common;
using Rac.VOne.Common.Importer.Customer;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Common.Constants;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>得意先マスター取込設定</summary>
    public partial class PB0503 : VOneScreenBase
    {
        private int FormatId { get { return (int)FreeImporterFormatType.Customer; } }
        private DateTime ImporterSettingUpdateAt { get; set; }
        private List<Staff> StaffList { get; set; }
        private List<Category> CollectCategoryList { get; set; }
        private List<ImporterSettingDetail> ImpSettingDetailsList { get; set; }
        private ImporterSetting ImpSetting { get; set; }
        private bool SearchFlg { get; set; } = false;
        private int ImporterSettingId { get; set; }

        private List<Setting> ComboBoxSource { get; set; }
        private string CellName(string value) => $"cel{value}";
        private string CellNameSequence { get { return CellName(nameof(ImporterSettingDetail.Sequence)); } }
        private string CellNameFixedValue { get { return CellName(nameof(ImporterSettingDetail.FixedValue)); } }
        private string CellNameFieldName  { get { return CellName(nameof(ImporterSettingDetail.FieldName)); } }
        private string CellNameFieldIndex { get { return CellName(nameof(ImporterSettingDetail.FieldIndex)); } }
        private string CellNameImportDivision { get { return CellName(nameof(ImporterSettingDetail.ImportDivision)); } }
        private string CellNameUpdateKey { get { return CellName(nameof(ImporterSettingDetail.UpdateKey)); } }
        private string CellNameAttributeDivision { get { return CellName(nameof(ImporterSettingDetail.AttributeDivision)); } }
        private string CellNameUpdateAt { get { return CellName(nameof(ImporterSettingDetail.UpdateAt)); } }
        private bool IsRowField(Row row, Fields field)
        {
            var value = row.Cells[CellNameSequence].Value?.ToString();
            var sequence = Fields.CompanyCode;
            return Enum.TryParse(value, out sequence) && field == sequence;
        }

        public PB0503()
        {
            InitializeComponent();
            grdSetting.SetupShortcutKeys();
            Text = "得意先マスター 取込設定";
            AddHandlers();

            //grdSetting.DataError += (sender, e) =>
            //{
            //    Console.WriteLine(e);
            //};
        }

        #region フォームロード
        private void PB0503_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                InitializeGridTemplate();
                ProgressDialog.Start(ParentForm, LoadDataAsync(), false, SessionKey);
                txtInitialDirectory.Enabled = !LimitAccessFolder;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task LoadDataAsync()
        {
            var tasks = new List<Task>();
            if (ApplicationControl == null)
                tasks.Add(LoadApplicationControlAsync());
            if (Company == null)
                tasks.Add(LoadCompanyAsync());
            tasks.Add(LoadControlColorAsync());
            tasks.Add(LoadStaffInfoAsync());
            tasks.Add(LoadCollectCategoryInfoAsync());
            tasks.Add(LoadComboboxSourceAsync());
            await Task.WhenAll(tasks);
            Clear();
        }
        #endregion

        #region 画面の初期化
        private void InitializeGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var cell = builder.GetNumberCellFreeInput(1, 999, 3);
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, nameof(ImporterSettingDetail.Sequence)         , caption: "No"        , cell: builder.GetRowHeaderCell()),
                new CellSetting(height,  40, nameof(ImporterSettingDetail.UpdateKey)        , caption: "更新"      , cell: builder.GetCheckBoxCell(), readOnly: false),
                new CellSetting(height, 150, nameof(ImporterSettingDetail.FieldName)        , caption: "VONE項目名", cell: builder.GetTextBoxCell()),
                new CellSetting(height, 230, nameof(ImporterSettingDetail.ImportDivision)   , caption: "取込有無"  , cell: builder.GetComboBoxCell(), readOnly: false),
                new CellSetting(height,  80, nameof(ImporterSettingDetail.FixedValue)       , caption: "固定値"    , cell: builder.GetTextBoxCell(), readOnly: false),
                new CellSetting(height,  80, nameof(ImporterSettingDetail.FieldIndex)       , caption: "項目番号"  , cell: cell, readOnly: false),
                new CellSetting(height,  80, nameof(ImporterSettingDetail.AttributeDivision), caption: "属性情報"  , cell: builder.GetComboBoxCell(), readOnly: false),
                new CellSetting(height,   0, nameof(ImporterSettingDetail.UpdateAt)         , visible: false)
            });
            var template = builder.Build();
            grdSetting.Template = template;
            grdSetting.HideSelection = true;
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Save;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = ConfirmClear;

            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = Delete;

            BaseContext.SetFunction08Caption("参照新規");
            BaseContext.SetFunction08Enabled(false);
            OnF08ClickHandler = SearchPattern;

            BaseContext.SetFunction09Caption("検索");
            BaseContext.SetFunction09Enabled(false);
            OnF09ClickHandler = SearchCode;

            BaseContext.SetFunction10Caption("戻る");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Exit;
        }
        #endregion

        #region Set Data in Grid
        /// <summary> DBから取得したデータをGridに設定する </summary>
        private void SetGridData()
        {
            var comboDatas = ComboBoxSource.GroupBy(g => g.ItemId).ToDictionary(
                x => x.Key,
                x => x.Select(y => new { y.ItemKey, y.ItemValue }).ToArray());

            grdSetting.RowCount = ImpSettingDetailsList.Count;
            for (int i = 0; i < grdSetting.RowCount; i++)
            {
                var detail = ImpSettingDetailsList[i];
                grdSetting.SetValue(i, CellNameSequence, detail.Sequence);
                grdSetting.SetValue(i, CellNameUpdateKey, detail.UpdateKey);
                grdSetting.SetValue(i, CellNameFieldName, detail.FieldName);
                grdSetting.SetValue(i, CellNameFixedValue, detail.FixedValue);
                if (detail.FieldIndex > 0)
                    grdSetting.SetValue(i, CellNameFieldIndex, detail.FieldIndex);
                grdSetting.SetValue(i, CellNameUpdateAt, detail.UpdateAt);
                ImporterSettingUpdateAt = detail.ImporterSettingUpdateAt;
                {
                    var key = $"TRKM{detail.BaseImportDivision}";
                    if (comboDatas.ContainsKey(key))
                    {
                        var combo = grdSetting.Rows[i].Cells[CellNameImportDivision] as ComboBoxCell;
                        if (combo != null)
                        {
                            combo.DataSource = comboDatas[key];
                            combo.DisplayMember = nameof(Setting.ItemValue);
                            combo.ValueMember = nameof(Setting.ItemKey);
                        }
                        combo.Value
                            = detail.BaseImportDivision == 2 ? 0 /* 会社コード 取込なし */
                            : detail.BaseImportDivision == 0 ? 1 /* 得意先コードなどの取込必須項目 */
                            : detail.ImportDivision;
                        if (detail.ImporterSettingId == 0 && detail.BaseImportDivision == 0)
                            detail.ImportDivision = (int)combo.Value;
                    }
                }
                grdSetting.Rows[i].Cells[CellNameImportDivision].Enabled = IsImportDivisionEnabled(detail);
                grdSetting.Rows[i].Cells[CellNameUpdateKey].Enabled = IsUpdateKeyEnabled(detail);
                grdSetting.Rows[i].Cells[CellNameFixedValue].Enabled = IsFixedValueEnabled(detail);
                grdSetting.Rows[i].Cells[CellNameFieldIndex].Enabled = IsFieldIndexEnabled(detail);
                var isAttributeEnabled = IsAttribudeEnabled(detail);
                grdSetting.Rows[i].Cells[CellNameAttributeDivision].Enabled = isAttributeEnabled;
                {
                    var key = $"ATTR{detail.BaseAttributeDivision}";
                    if (detail.BaseAttributeDivision > 0 && comboDatas.ContainsKey(key))
                    {
                        var combo = grdSetting.Rows[i].Cells[CellNameAttributeDivision] as ComboBoxCell;
                        if (combo != null)
                        {
                            combo.DataSource = comboDatas[key];
                            combo.DisplayMember = nameof(Setting.ItemValue);
                            combo.ValueMember = nameof(Setting.ItemKey);
                            if (isAttributeEnabled)
                                combo.Value = detail.AttributeDivision;
                        }
                    }
                }
            }
            Modified = false;
            ActiveControl = txtPatternName;
            txtPatternName.Focus();
        }
        #endregion

        #region 登録処理
        [OperationLog("登録")]
        private void Save()
        {
            try
            {
                if (!ValidateForSave())
                    return;

                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var succeeded = false;
                Task task = SaveImporterSetting()
                    .ContinueWith(async t =>
                    {
                        succeeded = t.Result;
                        if (succeeded) await LoadDataAsync();
                    }, TaskScheduler.FromCurrentSynchronizationContext())
                    .Unwrap();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                if (succeeded)
                {
                    Clear();
                    DispStatusMessage(MsgInfSaveSuccess);
                }
                else
                {
                    ShowWarningDialog(MsgErrSaveError);
                    DispStatusMessage(MsgErrSaveError);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private bool ValidateForSave()
        {
            ClearStatusMessage();

            grdSetting.EndEdit();

            if (string.IsNullOrWhiteSpace(txtPatternNumber.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "パターンNo.");
                txtPatternNumber.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPatternName.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "パターン名");
                txtPatternName.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtInitialDirectory.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "取込フォルダ");
                txtInitialDirectory.Focus();
                return false;
            }
            if (!Directory.Exists(txtInitialDirectory.Text))
            {
                txtInitialDirectory.Focus();
                ShowWarningDialog(MsgWngNotExistFolder);
                return false;
            }
            if (!nmbStartLineCount.Value.HasValue)
            {
                ShowWarningDialog(MsgWngInputRequired, "取込開始行");
                nmbStartLineCount.Focus();
                return false;
            }

            bool? normalClosingAt = null;
            int? shareTransferFee = null;

            Action<Cell> setCurrentCell = cell =>
            {
                grdSetting.CurrentCell = cell;
                grdSetting.BeginEdit(true);
                cell.Selected = true;
            };

            for (var i = 0; i < grdSetting.RowCount; i++)
            {
                var row = grdSetting.Rows[i];
                var itemName = row.Cells[CellNameFieldName].Value.ToString();

                if (row.Cells[CellNameFieldIndex].Enabled
                    && string.IsNullOrEmpty(row.Cells[CellNameFieldIndex].DisplayText))
                {
                    setCurrentCell(row.Cells[CellNameFieldIndex]);
                    ShowWarningDialog(MsgWngInputRequired, "項目番号");
                    return false;
                }
                else if (row.Cells[CellNameFixedValue].Enabled
                    && string.IsNullOrEmpty(row.Cells[CellNameFixedValue].DisplayText))
                {
                    setCurrentCell(row.Cells[CellNameFixedValue]);
                    ShowWarningDialog(MsgWngNoInputFixedValue, itemName);
                    return false;
                }
                else if (row.Cells[CellNameAttributeDivision].Enabled
                        && (Convert.ToInt32(row.Cells[CellNameAttributeDivision].Value) == 0))
                {
                    setCurrentCell(row.Cells[CellNameAttributeDivision]);
                    ShowWarningDialog(MsgWngSelectionRequired, "属性情報");
                    return false;
                }

                if (row.Cells[CellNameFixedValue].Enabled)
                {
                    var fixedValue = Convert.ToString(row.Cells[CellNameFixedValue].Value);

                    Func<bool> IsValidLetter = delegate()
                    {
                        if (!Regex.IsMatch(fixedValue, "^[01]$"))
                        {
                            setCurrentCell(row.Cells[CellNameFixedValue]);
                            ShowWarningDialog(MsgWngInputInvalidLetter, itemName);
                            return false;
                        }
                        return true;
                    };

                    if (IsRowField(row, Fields.ShareTransferFee))
                    {
                        if (!IsValidLetter()) return false;
                        shareTransferFee = Convert.ToInt16(fixedValue);
                    }
                    else if (IsRowField(row, Fields.UseFeeLearning))
                    {
                        if (!IsValidLetter()) return false;
                        if (shareTransferFee == 0 && Convert.ToInt16(fixedValue) != 0)
                        {
                            setCurrentCell(row.Cells[CellNameFixedValue]);
                            ShowWarningDialog(MsgWngShareTransferFeeMisMatch, itemName);
                            return false;
                        }
                    }
                    else if (IsRowField(row, Fields.UseFeeTolerance))
                    {
                        if (!IsValidLetter()) return false;
                        if (shareTransferFee == 0 && Convert.ToInt16(fixedValue) != 0)
                        {
                            setCurrentCell(row.Cells[CellNameFixedValue]);
                            ShowWarningDialog(MsgWngShareTransferFeeMisMatch, itemName);
                            return false;
                        }
                    }
                    else if (IsRowField(row, Fields.IsParent)
                          || IsRowField(row, Fields.UseKanaLearning)
                          || IsRowField(row, Fields.PrioritizeMatchingIndividually)
                          || IsRowField(row, Fields.ExcludeInvoicePublish)
                          || IsRowField(row, Fields.ExcludeReminderPublish)
                          )
                    {
                        if (!IsValidLetter()) return false;
                    }
                    else if (IsRowField(row, Fields.CollectCategoryId))
                    {
                        var code = fixedValue;
                        var exist = CollectCategoryList.Exists(x => x.Code == code);
                        if (!exist)
                        {
                            setCurrentCell(row.Cells[CellNameFixedValue]);
                            ShowWarningDialog(MsgWngMasterNotExist, "区分", code);
                            return false;
                        }
                    }
                    else if (IsRowField(row, Fields.StaffCode))
                    {
                        var code = fixedValue;
                        var exist = StaffList.Exists(x => x.Code == code);
                        if (!exist)
                        {
                            setCurrentCell(row.Cells[CellNameFixedValue]);
                            ShowWarningDialog(MsgWngMasterNotExist, itemName, code);
                            return false;
                        }
                    }
                    else if (IsRowField(row, Fields.ClosingDay))
                    {
                        var day = 0;
                        if (!Regex.IsMatch(fixedValue, "^[0-9]{2}$")
                            || !int.TryParse(fixedValue, out day)
                            || !((0 <= day && day <= 27) || day == 99))
                        {
                            setCurrentCell(row.Cells[CellNameFixedValue]);
                            ShowWarningDialog(MsgWngNumberValueValid0To27or99, itemName);
                            return false;
                        }
                        normalClosingAt = day != 0;
                    }
                    else if (IsRowField(row, Fields.CollectOffsetMonth))
                    {
                        if (!Regex.IsMatch(fixedValue, "^[0-9]$"))
                        {
                            setCurrentCell(row.Cells[CellNameFixedValue]);
                            ShowWarningDialog(MsgWngInputableRangeIs0to9, itemName);
                            return false;
                        }
                    }
                    else if (IsRowField(row, Fields.CollectOffsetDay))
                    {
                        var day = 0;
                        if (!Regex.IsMatch(fixedValue, "^[0-9]{2}$")
                            || !int.TryParse(fixedValue, out day)
                            || !((1 <= day && day <= 27) || day == 99))
                        {
                            setCurrentCell(row.Cells[CellNameFixedValue]);
                            ShowWarningDialog(MsgWngNumberValueValid1To27or99, itemName);
                            return false;
                        }
                    }
                    else if (IsRowField(row, Fields.CollectOffsetDayPerBilling))
                    {
                        var day = 0;
                        if (!Regex.IsMatch(fixedValue, @"^[0-9]{2}$")
                            || !int.TryParse(fixedValue, out day)
                            || !(0 <= day && day <= 99))
                        {
                            setCurrentCell(row.Cells[CellNameFixedValue]);
                            ShowWarningDialog(MsgWngNumberValueValid0To99, itemName);
                            return false;

                        }
                    }
                    else if (IsRowField(row, Fields.HolidayFlag))
                    {
                        if (!Regex.IsMatch(fixedValue, "^[012]$"))
                        {
                            setCurrentCell(row.Cells[CellNameFixedValue]);
                            ShowWarningDialog(MsgWngInputInvalidLetter, itemName);
                            return false;
                        }
                    }
                    else if (IsRowField(row, Fields.Honorific))
                    {
                        if (!string.IsNullOrEmpty(fixedValue) && fixedValue.Length > 6)
                        {
                            setCurrentCell(row.Cells[CellNameFixedValue]);
                            ShowWarningDialog(MsgWngPara1InputPara2CharCount, itemName, "6");
                            return false;
                        }
                    }
                }
            }

            var dueAtNormalRows = grdSetting.Rows.Where(x =>
                IsRowField(x, Fields.CollectOffsetMonth)
             || IsRowField(x, Fields.CollectOffsetDay)).ToArray();
            var dueAtPerBillingRows = grdSetting.Rows.Where(x =>
                IsRowField(x, Fields.CollectOffsetDayPerBilling)).ToArray();
            Func<Row, bool> IsNoImport = row => int.Parse(row.Cells[CellNameImportDivision].Value.ToString()) == 0;

            if (!normalClosingAt.HasValue) /* 取込 */
            {
                // 月/日セット or 都度 いずれか
                var isNormalNoImport = dueAtNormalRows.Any(x => IsNoImport(x));
                var isPerBillingNoImport = dueAtPerBillingRows.All(x => IsNoImport(x));
                if (isNormalNoImport && isPerBillingNoImport)
                {
                    var row = isNormalNoImport ? dueAtNormalRows.FirstOrDefault(x => IsNoImport(x))
                        : dueAtPerBillingRows.FirstOrDefault();
                    if (row != null)
                    {
                        setCurrentCell(row.Cells[CellNameImportDivision]);
                        ShowWarningDialog(MsgWngSelectionRequired,
                            "締日が取込の場合は、回収予定 月/日 または 都度請求 のいずれかの 取込有無");
                        return false;
                    }
                }
            }
            else if (normalClosingAt.Value) /* 固定値 通常締め・入金予定日算出 */
            {
                var row = dueAtNormalRows.FirstOrDefault(x => IsNoImport(x));
                if (row != null)
                {
                    setCurrentCell(row.Cells[CellNameImportDivision]);
                    ShowWarningDialog(MsgWngSelectionRequired, "取込有無");
                    return false;
                }

            }
            else /* 固定値 都度請求 */
            {
                var row = dueAtPerBillingRows.FirstOrDefault(x => IsNoImport(x));
                if (row != null)
                {
                    setCurrentCell(row.Cells[CellNameImportDivision]);
                    ShowWarningDialog(MsgWngSelectionRequired, "取込有無");
                    return false;
                }
            }
            return true;

        }

        private async Task<bool> SaveImporterSetting()
        {
            var succeeded = false;
            var impSetting = new ImporterSetting();

            impSetting.FormatId = FormatId;
            impSetting.CompanyId = CompanyId;
            impSetting.Code = txtPatternNumber.Text;
            impSetting.Name = txtPatternName.Text.Trim();
            impSetting.InitialDirectory = txtInitialDirectory.Text;
            impSetting.EncodingCodePage = 932;
            impSetting.StartLineCount = Convert.ToInt32(nmbStartLineCount.Value);
            impSetting.IgnoreLastLine = cbxIgnoreLastLine.Checked ? 1 : 0;
            impSetting.AutoCreationCustomer = 0;
            impSetting.PostAction = 0;
            impSetting.CreateBy = Login.UserId;
            impSetting.UpdateBy = Login.UserId;
            impSetting.UpdateAt = ImporterSettingUpdateAt;

            var impSettingDetails = PrepareImporterSettingDatailsData();
            ImporterSettingAndDetailResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ImporterSettingServiceClient>();
                result = await service.SaveAsync(SessionKey, impSetting, impSettingDetails.ToArray());
            });

            if (result.ProcessResult.Result)
            {
                succeeded = (result.ImporterSettingDetail != null && result.ImporterSetting != null);
            }

            return succeeded;
        }

        /// <summary> GridのデータからListを生成する　</summary>
        /// <returns>生成されたList</returns>
        private List<ImporterSettingDetail> PrepareImporterSettingDatailsData()
        {
            var saveList = new List<ImporterSettingDetail>();
            var nowDateTime = DateTime.Now;
            for (int i = 0; i < grdSetting.RowCount; i++)
            {
                var detail = new ImporterSettingDetail();
                detail.Sequence = int.Parse(grdSetting.Rows[i].Cells[CellNameSequence].DisplayText);
                detail.ImportDivision = int.Parse(grdSetting.Rows[i].Cells[CellNameImportDivision].Value.ToString());
                if (grdSetting.Rows[i].Cells[CellNameFieldIndex].DisplayText != "")
                    detail.FieldIndex = int.Parse(grdSetting.Rows[i].Cells[CellNameFieldIndex].DisplayText);
                detail.Caption = "";
                detail.AttributeDivision = (grdSetting.Rows[i].Cells[CellNameAttributeDivision].Value != null) ? 
                                int.Parse(grdSetting.Rows[i].Cells[CellNameAttributeDivision].Value.ToString()) : 0;
                detail.ItemPriority = 0;
                detail.DoOverwrite = 0;
                detail.FixedValue = grdSetting.Rows[i].Cells[CellNameFixedValue].DisplayText;
                detail.IsUnique = 0;
                detail.UpdateKey = Convert.ToBoolean(grdSetting.Rows[i].Cells[CellNameUpdateKey].Value) ? 1 : 0;
                detail.CreateBy = Login.UserId;
                detail.CreateAt = nowDateTime;
                detail.UpdateBy = Login.UserId;
                detail.UpdateAt = nowDateTime;
                saveList.Add(detail);
            }
            return saveList;
        }
        #endregion

        #region クリア処理 
        [OperationLog("クリア")]
        private void ConfirmClear()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear)) return;
            Clear();
        }
        private void Clear()
        {
            ClearStatusMessage();
            txtPatternNumber.Clear();
            txtPatternName.Clear();
            txtInitialDirectory.Clear();
            nmbStartLineCount.Clear();
            cbxIgnoreLastLine.Checked = false;
            grdSetting.Rows.Clear();
            grdSetting.HideSelection = true;
            txtPatternNumber.Enabled = true;
            btnPatternNoSearch.Enabled = true;
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);
            txtPatternNumber.Focus();
            Modified = false;
        }
        #endregion

        #region 削除処理 
        [OperationLog("削除")]
        private void Delete()
        {
            ClearStatusMessage();

            try
            {
                Task task = ExistTaskSchedulerAsync()
                    .ContinueWith(async t =>
                    {
                        if (t.Result)
                        {
                            ShowWarningDialog(MsgWngDeleteConstraint, "タイムスケジューラー", " 取込パターン");
                            return;
                        }

                        if (!ShowConfirmDialog(MsgQstConfirmDelete))
                        {
                            DispStatusMessage(MsgInfProcessCanceled);
                            return;
                        }

                        var deleteResult = await DeleteImportSettingAsync();
                        if (deleteResult)
                        {
                            Clear();
                            DispStatusMessage(MsgInfDeleteSuccess);
                        }
                        else
                        {
                            ShowWarningDialog(MsgErrDeleteError);
                            DispStatusMessage(MsgErrDeleteError);
                        }
                    }, TaskScheduler.FromCurrentSynchronizationContext()).Unwrap();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        #endregion

        #region 参照新規処理
        [OperationLog("参照新規")]
        private void SearchPattern()
        {
            try
            {
                var import = this.ShowCustomerImporterSettingSearchDialog();
                if (import == null) return;
                SearchFlg = true;
                var loadTask = new List<Task>();
                var searchCode = txtPatternNumber.Text;
                txtPatternNumber.Text = import.Code;
                loadTask.Add(LoadPatternData(txtPatternNumber.Text, true));
                loadTask.Add(LoadDetailByCodeAsync(txtPatternNumber.Text));
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
                SetGridData();
                txtPatternNumber.Text = searchCode;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        #endregion

        #region 検索処理
        /// <summary>　Gridにあるデータによって検索ダイアログを設定</summary>
        [OperationLog("検索")]
        private void SearchCode()
        {
            var row = grdSetting.CurrentRow;
            if (row == null) return;
            var fixedValue = string.Empty;
            if (IsRowField(row, Fields.ShareTransferFee))
            {
                var shareTransferFee = this.ShowShareTransferFeeSearchDialog();
                if (shareTransferFee == null) return;
                fixedValue = shareTransferFee.Key;
            }
            else if (IsRowField(row, Fields.CollectCategoryId))
            {
                var category = this.ShowCollectCategroySearchDialog();
                if (category == null) return;
                fixedValue = category.Code;
            }
            else if (IsRowField(row, Fields.StaffCode))
            {
                var staff = this.ShowStaffSearchDialog();
                if (staff == null) return;
                fixedValue = staff.Code;
            }
            else if (IsRowField(row, Fields.HolidayFlag))
            {
                var holiday = this.ShowHolidaySettingSearchDialog();
                if (holiday == null) return;
                fixedValue = holiday.Key;
            }
            else if (IsRowField(row, Fields.IsParent))
            {
                var value = this.ShowIsParentSettingSearchDialog();
                if (value == null) return;
                fixedValue = value.Key;
            }
            else if (IsRowField(row, Fields.UseFeeLearning)
                  || IsRowField(row, Fields.UseKanaLearning)
                  || IsRowField(row, Fields.UseFeeTolerance)
                  || IsRowField(row, Fields.PrioritizeMatchingIndividually)
                  || IsRowField(row, Fields.ExcludeInvoicePublish)
                  || IsRowField(row, Fields.ExcludeReminderPublish))
            {
                var title = row.Cells[CellNameFieldName].DisplayText;
                var value = this.ShowYesOrNoSettingSearchDialog(title);
                if (value == null) return;
                fixedValue = value.Key;
            }
            row.Cells[CellNameFixedValue].Value = fixedValue;
        }
        #endregion

        # region 終了処理
        [OperationLog("終了")]
        private void Exit()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;
            ParentForm.Close();
        }
        #endregion

        #region Webサービス呼び出し
        private async Task LoadDetailByCodeAsync(string code)
        {
            var result = await Util.GetImporterSettingDetailByCodeAsync(Login, FormatId, code);
            if (result != null)
            {
                ImpSettingDetailsList = new List<ImporterSettingDetail>(result
                    .Where(x =>
                    {
                        if (x.Sequence == (int)Fields.ExcludeInvoicePublish) return UsePublishInvoice;
                        if (x.Sequence == (int)Fields.ExcludeReminderPublish) return UseReminder;
                        return true;
                    }));
            }
        }

        private async Task LoadHeaderByCodeAsync(string code)
            => ImpSetting = await Util.GetImporterSettingAsync(Login, FormatId, code);

        private async Task LoadComboboxSourceAsync()
            => ComboBoxSource = await Util.GetSettingAsync(Login);

        private async Task LoadStaffInfoAsync()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<StaffMasterClient>();
                StaffsResult result = await service.GetItemsAsync(SessionKey, new StaffSearch { CompanyId = CompanyId });

                if (result.ProcessResult.Result)
                {
                    StaffList = result.Staffs;
                }
            });
        }

        private async Task LoadCollectCategoryInfoAsync()
        {
            var option = new CategorySearch();
            option.CompanyId = CompanyId;
            option.CategoryType = CategoryType.Collect;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CategoryMasterClient>();
                CategoriesResult result = await service.GetItemsAsync(SessionKey, option);

                if (result.ProcessResult.Result)
                {
                    CollectCategoryList = result.Categories;
                }
            });
        }

        private async Task<bool> ExistTaskSchedulerAsync()
            => await ServiceProxyFactory.DoAsync(async (TaskScheduleMasterClient client) =>
            {
                var result = await client.ExistsAsync(SessionKey, CompanyId, FormatId, ImporterSettingId);
                return result.Exist;
            });

        private async Task<bool> DeleteImportSettingAsync()
            => await ServiceProxyFactory.DoAsync(async (ImporterSettingServiceClient client) =>
            {
                var result = await client.DeleteAsync(SessionKey, ImporterSettingId);
                return result.Count > 0;
            });

        #endregion

        #region event handlers

        private void AddHandlers()
        {
            foreach (var control in this.GetAll<Control>())
            {
                if (control is Common.Controls.VOneTextControl)
                    ((Common.Controls.VOneTextControl)control).TextChanged += OnContentChanged;
                if (control is CheckBox)
                    ((CheckBox)control).CheckedChanged += OnContentChanged;
                if (control is Common.Controls.VOneNumberControl)
                    ((Common.Controls.VOneNumberControl)control).ValueChanged += OnContentChanged;
            }
        }

        private void OnContentChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        private void grdSetting_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            Modified = true;

            if (grdSetting.CurrentRow == null
                || e.CellName != CellNameUpdateKey
                || !(IsRowField(grdSetting.CurrentRow, Fields.ThresholdValue)
                    || IsRowField(grdSetting.CurrentRow, Fields.ClosingDay))) return;
            grdSetting.EndEdit();
            var flag = Convert.ToBoolean(grdSetting.CurrentCell.Value);
            var rowIndex = grdSetting.CurrentRow.Index;
            if (IsRowField(grdSetting.CurrentRow, Fields.ThresholdValue))
            {
                var maxOffset = GetThresholdRelatedFieldMaxOffset();
                foreach (var offset in Enumerable.Range(1, maxOffset))
                    grdSetting.SetValue(rowIndex + offset, CellNameUpdateKey, flag);
            }
            if (IsRowField(grdSetting.CurrentRow, Fields.ClosingDay))
            {
                var start = (int)Fields.CollectOffsetMonth - (int)Fields.ClosingDay;
                var count = (int)Fields.CollectOffsetDayPerBilling - (int)Fields.CollectOffsetMonth + 1;
                foreach (var offset in Enumerable.Range(start, count))
                {
                    if (int.Parse(grdSetting.Rows[rowIndex + offset].Cells[CellNameImportDivision].Value.ToString()) == 0) continue;
                    grdSetting.SetValue(rowIndex + offset, CellNameUpdateKey, flag);
                }
            }
        }

        private void btnInitialDirectory_Click(object sender, EventArgs e)
        {
            var path = txtInitialDirectory.Text.Trim();
            var selectedPath = string.Empty;
            var rootBrowserPath = new List<string>();
            if (!LimitAccessFolder ?
                !ShowFolderBrowserDialog(path, out selectedPath) :
                !ShowRootFolderBrowserDialog(ApplicationControl.RootPath, out rootBrowserPath, FolderBrowserType.SelectFolder)) return;

            txtInitialDirectory.Text = (!LimitAccessFolder) ? selectedPath : rootBrowserPath.FirstOrDefault();
        }

        private void grdSetting_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
        {
            var combo = e.Control as ComboBox;
            if (combo == null) return;
            combo.SelectedIndexChanged -= cb_SelectedIndexChanged;
            combo.SelectedIndexChanged += cb_SelectedIndexChanged;
        }

        private void cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grdSetting.CurrentCell.Name != CellNameImportDivision) return;
            int? seq = (int?)grdSetting.CurrentRow.Cells[CellNameSequence].Value;
            if (!seq.HasValue) return;

            var combo = (ComboBox)sender;
            var detail = ImpSettingDetailsList.FirstOrDefault(x => x.Sequence == seq.Value);
            if (detail == null) return;
            if (combo.SelectedValue == null) return;
            var div = 0;
            if (!int.TryParse(Convert.ToString(combo.SelectedValue), out div)) return;
            detail.ImportDivision = div;
            var isUpdateKeyEnabled = IsUpdateKeyEnabled(detail);
            grdSetting.CurrentRow.Cells[CellNameUpdateKey].Enabled = isUpdateKeyEnabled;
            var isFixedValueEnabled = IsFixedValueEnabled(detail);
            grdSetting.CurrentRow.Cells[CellNameFixedValue].Enabled = isFixedValueEnabled;
            if (!isFixedValueEnabled)
                grdSetting.CurrentRow.Cells[CellNameFixedValue].Value = null;

            var isUpdateKeyChecked = isUpdateKeyEnabled && !isFixedValueEnabled;
            if (IsDoOrNotWithFixedImportDivision(detail) && IsCollectOffsetRelatedField(detail)) /* 入金予定日 関連項目 */
            {
                // 0 : 取込無 は強制 false それ以外は、締日 のチェック状態を参照
                isUpdateKeyChecked = div != 0 & GetClosingDayUpdateKeyChecked();
            }
            grdSetting.CurrentRow.Cells[CellNameUpdateKey].Value = isUpdateKeyChecked;


            var isFieldIndexEnabled = IsFieldIndexEnabled(detail);
            grdSetting.CurrentRow.Cells[CellNameFieldIndex].Enabled = isFieldIndexEnabled;
            if (!isFieldIndexEnabled)
                grdSetting.CurrentRow.Cells[CellNameFieldIndex].Value = null;
            var isAttributeEnabled = IsAttribudeEnabled(detail);
            grdSetting.CurrentRow.Cells[CellNameAttributeDivision].Enabled = isAttributeEnabled;
            if (!isAttributeEnabled)
                grdSetting.CurrentRow.Cells[CellNameAttributeDivision].Value = null;

            var rowIndex = grdSetting.CurrentCell.RowIndex;

            if (IsRowField(grdSetting.CurrentRow, Fields.ThresholdValue))
            {
                var maxOffset = GetThresholdRelatedFieldMaxOffset();
                foreach (var offset in Enumerable.Range(1, maxOffset))
                {
                    grdSetting.Rows[rowIndex + offset].Cells[CellNameFieldIndex].Enabled = isFieldIndexEnabled;
                    if (!isFieldIndexEnabled)
                        grdSetting.Rows[rowIndex + offset].Cells[CellNameFieldIndex].Value = null;
                    grdSetting.Rows[rowIndex + offset].Cells[CellNameImportDivision].Value = combo.SelectedIndex;
                    grdSetting.Rows[rowIndex + offset].Cells[CellNameUpdateKey].Value = isUpdateKeyEnabled;
                    var detailSub = ImpSettingDetailsList.First(x => x.Sequence == seq.Value + offset);
                    detailSub.ImportDivision = div;
                    isAttributeEnabled = IsAttribudeEnabled(detailSub);
                    grdSetting.Rows[rowIndex + offset].Cells[CellNameAttributeDivision].Enabled = isAttributeEnabled;
                    if (!isAttributeEnabled)
                        grdSetting.Rows[rowIndex + offset].Cells[CellNameAttributeDivision].Value = null;
                }
            }
            if (IsRowField(grdSetting.CurrentRow, Fields.ClosingDay))
            {
                var start = (int)Fields.CollectOffsetMonth - (int)Fields.ClosingDay;
                var count = (int)Fields.CollectOffsetDayPerBilling - (int)Fields.CollectOffsetMonth + 1;
                foreach (var offset in Enumerable.Range(start, count))
                    grdSetting.SetValue(rowIndex + offset, CellNameUpdateKey, isUpdateKeyChecked);
            }
        }

        private bool GetClosingDayUpdateKeyChecked()
            => Convert.ToBoolean(grdSetting.Rows.FirstOrDefault(x => IsRowField(x, Fields.ClosingDay))?.Cells[CellNameUpdateKey].Value);

        private void grdSetting_CellEnter(object sender, CellEventArgs e)
        {
            grdSetting.BeginEdit(true);
            var row = grdSetting.CurrentRow;
            var enabled = false;
            if (e.CellName == CellNameFixedValue
                && (IsRowField(row, Fields.ShareTransferFee)
                 || IsRowField(row, Fields.CollectCategoryId)
                 || IsRowField(row, Fields.StaffCode)
                 || IsRowField(row, Fields.HolidayFlag)
                 || IsRowField(row, Fields.UseFeeLearning)
                 || IsRowField(row, Fields.UseKanaLearning)
                 || IsRowField(row, Fields.UseFeeTolerance)
                 || IsRowField(row, Fields.PrioritizeMatchingIndividually)
                 || IsRowField(row, Fields.IsParent)
                 || IsRowField(row, Fields.ExcludeInvoicePublish)
                 || IsRowField(row, Fields.ExcludeReminderPublish))
                 )
            {
                enabled = grdSetting.Rows[e.RowIndex].Cells[CellNameFixedValue].Enabled;
                grdSetting.CurrentCell = grdSetting.Rows[e.RowIndex].Cells[CellNameFixedValue];
            }
            BaseContext.SetFunction09Enabled(enabled);
        }

        #endregion

        #region 各field helper メソッド

        /// <summary>
        /// 得意先コード、名称、カナ等 必須項目 ImporterSettingDetail.BaseImportDivision == 0
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        private bool IsCustomerRequiredImportDivision(ImporterSettingDetail detail)
            => detail.BaseImportDivision == 0;

        /// <summary>
        /// 通常の 取込有/無 の取込区分 ImporterSettingDetail.BaseImportDivision == 1
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        private bool IsDoOrNotImportDivision(ImporterSettingDetail detail)
            => detail.BaseImportDivision == 1;

        /// <summary>
        /// 固定値/取込 の取込区分 detail.BaseImportDivision == 12
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        private bool IsFixedImportDivision(ImporterSettingDetail detail)
            => detail.BaseImportDivision == 12;

        /// <summary>
        /// 0 : 取込無, 1 : 取込有, 2 : 固定値 の3項目
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        private bool IsDoOrNotWithFixedImportDivision(ImporterSettingDetail detail)
            => detail.BaseImportDivision == 11;

        /// <summary>
        /// 約定関連項目 か 否か
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        private bool IsThresholdRelatedField(ImporterSettingDetail detail)
            => IsThresholdRelatedField((Fields)detail.Sequence);

        /// <summary>
        /// 約定関連項目 か 否か
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        private bool IsThresholdRelatedField(Fields field)
            => field == Fields.LessThanCollectCategoryId
            || field == Fields.GreaterThanCollectCategoryId1
            || field == Fields.GreaterThanRate1
            || field == Fields.GreaterThanRoundingMode1
            || field == Fields.GreaterThanSightOfBill1
            || field == Fields.GreaterThanCollectCategoryId2
            || field == Fields.GreaterThanRate2
            || field == Fields.GreaterThanRoundingMode2
            || field == Fields.GreaterThanSightOfBill2
            || field == Fields.GreaterThanCollectCategoryId3
            || field == Fields.GreaterThanRate3
            || field == Fields.GreaterThanRoundingMode3
            || field == Fields.GreaterThanSightOfBill3
            ;

        private bool IsCollectOffsetRelatedField(ImporterSettingDetail detail)
            => IsCollectOffsetRelatedField((Fields)detail.Sequence);

        /// <summary>
        /// 回収予定関連（締日）
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool IsCollectOffsetRelatedField(Fields field)
            => field == Fields.CollectOffsetMonth
            || field == Fields.CollectOffsetDay
            || field == Fields.CollectOffsetDayPerBilling
            ;

        /// <summary>
        /// 上書 チェックボックス 有効無効
        /// 約定関連項目ではない
        /// かつ
        ///     得意先 名称、カナ
        ///     通常の 取込有/無 で 1 : 取込有 のもの
        ///     固定値/取込 で 債権代表者フラグ 以外
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        private bool IsUpdateKeyEnabled(ImporterSettingDetail detail)
            => !IsThresholdRelatedField(detail)
            && (
                IsCustomerRequiredImportDivision(detail) && (Fields)detail.Sequence != Fields.CustomerCode
             || IsDoOrNotImportDivision(detail) && detail.ImportDivision == 1
             || IsFixedImportDivision(detail) && (Fields)detail.Sequence != Fields.IsParent
             || IsDoOrNotWithFixedImportDivision(detail) && detail.ImportDivision != 0 && (Fields)detail.Sequence == Fields.Honorific
                );

        /// <summary>
        /// 取込区分 有効無効
        /// 約定関連項目ではない
        /// かつ
        ///     通常の 取込有/無
        ///     固定値/取込
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        private bool IsImportDivisionEnabled(ImporterSettingDetail detail)
            => !IsThresholdRelatedField(detail)
            && (IsDoOrNotImportDivision(detail)
             || IsFixedImportDivision(detail)
             || IsDoOrNotWithFixedImportDivision(detail));

        /// <summary>
        /// 固定値 有効無効
        /// 固定値/取込    で 0 : 固定値 のもの
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        private bool IsFixedValueEnabled(ImporterSettingDetail detail)
            => IsFixedImportDivision(detail) && detail.ImportDivision == 0
            || IsDoOrNotWithFixedImportDivision(detail) && detail.ImportDivision == 2;

        /// <summary>
        /// 属性区分 有効無効
        /// 属性区分 が 0, 2 以外
        /// かつ
        ///    取込区分 1 : 取込有 のもの
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        private bool IsAttribudeEnabled(ImporterSettingDetail detail)
            => detail.BaseAttributeDivision != 0
            && detail.BaseAttributeDivision != 2
            && detail.ImportDivision == 1;

        /// <summary>
        /// 項目番号 有効無効
        /// 得意先 コード、名称、カナ
        /// 通常 取込有/無 で 1 : 取込有 のもの
        /// 固定値/取込    で 1 : 取込有 のもの
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        private bool IsFieldIndexEnabled(ImporterSettingDetail detail)
            => IsCustomerRequiredImportDivision(detail)
            || IsDoOrNotImportDivision(detail) && detail.ImportDivision == 1
            || IsFixedImportDivision(detail) && detail.ImportDivision == 1
            || IsDoOrNotWithFixedImportDivision(detail) && detail.ImportDivision == 1;


        /// <summary>
        /// 約定関連項目 max offset 取得
        /// </summary>
        /// <returns></returns>
        private int GetThresholdRelatedFieldMaxOffset()
            => (int)Fields.GreaterThanSightOfBill3 - (int)Fields.ThresholdValue;

        #endregion

        private void txtPatternNumber_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPatternNumber.Text)) return;

            try
            {
                txtPatternNumber.Text = txtPatternNumber.Text.PadLeft(2, '0');
                var loadTask = new List<Task>();
                loadTask.Add(LoadPatternData(txtPatternNumber.Text));
                loadTask.Add(LoadDetailByCodeAsync(txtPatternNumber.Text));
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
                SetGridData();

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task LoadPatternData(string code, bool loadRefData = false)
        {
            await LoadHeaderByCodeAsync(code);

            if (ImpSetting != null)
            {
                if (!SearchFlg)
                {
                    txtPatternNumber.Text = ImpSetting.Code;
                    txtPatternName.Text = ImpSetting.Name;
                    txtPatternNumber.Enabled = false;
                    btnPatternNoSearch.Enabled = false;
                }

                SearchFlg = false;//for original state
                Modified = false;
                txtInitialDirectory.Text = ImpSetting.InitialDirectory;
                nmbStartLineCount.Value = ImpSetting.StartLineCount;
                ImporterSettingUpdateAt = ImpSetting.UpdateAt;
                ImporterSettingId = ImpSetting.Id;
                cbxIgnoreLastLine.Checked = (ImpSetting.IgnoreLastLine == 1) ? true : false;

                if (!loadRefData)
                {
                    BaseContext.SetFunction03Enabled(true);
                    BaseContext.SetFunction08Enabled(false);
                    ClearStatusMessage();
                }
            }
            else
            {
                BaseContext.SetFunction08Enabled(true);
                DispStatusMessage(MsgInfNewData, "パターンNo.");
                cbxIgnoreLastLine.Checked = false;
                txtPatternName.Clear();
                txtInitialDirectory.Clear();
                nmbStartLineCount.Clear();
            }
        }

        private void btnPatternNoSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var import = this.ShowCustomerImporterSettingSearchDialog();
                if (import == null) return;
                txtPatternNumber.Text = import.Code;
                txtPatternName.Text = import.Name;
                txtPatternNumber.Enabled = false;
                btnPatternNoSearch.Enabled = false;
                var loadTask = new List<Task>();
                loadTask.Add(LoadPatternData(txtPatternNumber.Text));
                loadTask.Add(LoadDetailByCodeAsync(txtPatternNumber.Text));
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
                SetGridData();
                ClearStatusMessage();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
    }
}
