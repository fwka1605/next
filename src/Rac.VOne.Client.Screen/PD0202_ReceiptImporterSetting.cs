using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.ImporterSettingService;
using Rac.VOne.Client.Screen.SectionMasterService;
using Rac.VOne.Client.Screen.SettingMasterService;
using Rac.VOne.Client.Screen.TaskScheduleMasterService;
using Rac.VOne.Common;
using Rac.VOne.Common.Importer.Receipt;
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
using GcTextBoxCell = GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell;

namespace Rac.VOne.Client.Screen
{
    /// <summary>入金フリーインポーター・取込設定</summary>
    public partial class PD0202 : VOneScreenBase
    {
        private DateTime updateAt;
        private int importerSettingId { get; set; }
        /// <summary>フォーマットID 2:入金フリーインポーター </summary>
        private int FormatId { get { return (int)FreeImporterFormatType.Receipt; } }
        /// <summary>タスクスケジューラ ImportType 4 : 入金フリーインポーター</summary>
        private int ImportType { get { return (int)TaskScheduleImportType.Receipt; } }

        private Dictionary<string, List<Setting>> SettingsByKeys { get; set; }

        private Dictionary<string, string> ColumnNames { get; set; }

        /// <summary>0 : 行番号</summary>
        private int CellIndexRowHeader { get { return 0; } }
        /// <summary>1 : 重複</summary>
        private int CellIndexIsUnique { get { return 1; } }
        /// <summary>2 : 項目名</summary>
        private int CellIndexFieldName { get { return 2; } }
        /// <summary>3 : 取込区分</summary>
        private int CellIndexImportDivision { get { return 3; } }
        /// <summary>4 : 固定値</summary>
        private int CellIndexFixedValue { get { return 4; } }
        /// <summary>5 : 項目番号</summary>
        private int CellIndexFieldIndex { get { return 5; } }
        /// <summary>6 : 属性区分</summary>
        private int CellIndexAttribute { get { return 6; } }
        /// <summary>7 : Sequence</summary>
        private int CellIndexSequence { get { return 7; } }

        #region initialize

        public PD0202()
        {
            InitializeComponent();
            InitializeUserComponent();
            InitializeHandlers();
        }

        private void InitializeUserComponent()
        {
            grid.SetupShortcutKeys();
            Text = "入金フリーインポーター　取込設定 ";
        }

        private void InitializeHandlers()
        {
            foreach (Control control in this.GetAll<Common.Controls.VOneTextControl>())
            {
                control.TextChanged += new EventHandler(OnContentChanged);
            }
            foreach (Control control in gbxFile.Controls)
            {
                if (control is Common.Controls.VOneTextControl)
                {
                    control.TextChanged += new EventHandler(OnContentChanged);
                }
                if (control is Common.Controls.VOneNumberControl)
                {
                    control.TextChanged += new EventHandler(OnContentChanged);
                }
                if (control is CheckBox)
                {
                    ((CheckBox)control).CheckedChanged +=
                        new EventHandler(OnContentChanged);
                }
            }
            foreach (Control control in gbxPostAction.Controls)
            {
                if (control is RadioButton)
                {
                    ((RadioButton)control).CheckedChanged +=
                        new EventHandler(OnContentChanged);
                }
            }

            grid.EditingControlShowing  += grid_EditingControlShowing;
        }

        private void PD0202_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                ProgressDialog.Start(ParentForm, InitializeLoadDataAsync(), false, SessionKey);
                InitializeGridTemplate();
                ClearControlValues();
                txtInitialDirectory.Enabled = !LimitAccessFolder;
                Modified = false;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }

        }

        private async Task InitializeLoadDataAsync()
            => await Task.WhenAll(
                LoadCompanyAsync(),
                LoadApplicationControlAsync(),
                LoadControlColorAsync(),
                LoadSettingsAsync(),
                LoadColumnNamesAsync());

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction08Caption("参照新規");
            BaseContext.SetFunction09Caption("検索");
            BaseContext.SetFunction10Caption("戻る");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(true);
            BaseContext.SetFunction08Enabled(true);
            BaseContext.SetFunction09Enabled(true);
            BaseContext.SetFunction10Enabled(true);

            OnF01ClickHandler = SaveData;
            OnF02ClickHandler = Clear;
            OnF03ClickHandler = DeleteData;
            OnF08ClickHandler = SearchPattern;
            OnF09ClickHandler = SearchCode;
            OnF10ClickHandler = Close;
        }

        private void InitializeGridTemplate()
        {
            var template = new Template();

            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "Header"           , caption: "No."       ),
                new CellSetting(height,  40, "IsUnique"         , caption: "重複"      ),
                new CellSetting(height, 115, "FieldName"        , caption: "VONE項目名"),
                new CellSetting(height, 270, "ImportDivision"   , caption: "取込有無"  ),
                new CellSetting(height,  80, "FixedValue"       , caption: "固定値"    ),
                new CellSetting(height,  60, "FieldIndex"       , caption: "項目番号"  ),
                new CellSetting(height, 120, "AttributeDivision", caption: "属性情報"  )
            });
            builder.BuildHeaderOnly(template);
            builder.Items.Clear();

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "Header"                                                                           , cell: builder.GetRowHeaderCell()),
                new CellSetting(height,  40, "IsUnique"         , readOnly: false, dataField: nameof(ImporterSetting.IsUnique)  , cell: builder.GetCheckBoxCell()),
                new CellSetting(height, 115, "FieldName"                         , dataField: nameof(ImporterSetting.FieldName)),
                new CellSetting(height, 270, "ImportDivision"   , readOnly: false                                               , cell: builder.GetComboBoxCell()),
                new CellSetting(height,  80, "FixedValue"       , readOnly: false, dataField: nameof(ImporterSetting.FixedValue), cell: builder.GetTextBoxCell(maxLength: 50) ),
                new CellSetting(height,  60, "FieldIndex"       , readOnly: false, dataField: nameof(ImporterSetting.FieldIndex), cell: builder.GetNumberCellFreeInput(1,999,3) ),
                new CellSetting(height, 120, "AttributeDivision", readOnly: false                                               , cell: builder.GetComboBoxCell()),
                new CellSetting(height,   0, "Sequence"         , visible: false)
            });

            builder.BuildRowOnly(template);
            grid.Template = template;
            grid.HideSelection = true;
            Modified = false;
        }

        #endregion


        #region F1 save

        [OperationLog("登録")]
        private void SaveData()
        {
            ClearStatusMessage();
            grid.EndEdit();
            if (!ValidateInputValues()) return;
            if (!ShowConfirmDialog(MsgQstConfirmSave))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            try
            {
                ProgressDialog.Start(ParentForm, SaveImporterSetting(), false, SessionKey);
                this.ActiveControl = txtSettingCode;
                txtSettingCode.Focus();
                Modified = false;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        #endregion

        #region F2 clear
        [OperationLog("クリア")]
        private void Clear()
        {
            ClearStatusMessage();
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear)) return;
            ClearControlValues();
        }


        #endregion

        #region F3 delete
        [OperationLog("削除")]
        private void DeleteData()
        {
            ClearStatusMessage();
            if (!ValidateForDelete()) return;
            if (!ShowConfirmDialog(MsgQstConfirmDelete))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            try
            {
                var task = DeleteImporterSettingAsync(importerSettingId);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                if (task.Exception != null)
                {
                    NLogHandler.WriteErrorLog(this, task.Exception, SessionKey);
                    ShowWarningDialog(MsgErrDeleteError);
                    return;
                }
                if (!task.Result)
                {
                    ShowWarningDialog(MsgErrDeleteError);
                    return;
                }

                ClearControlValues();
                DispStatusMessage(MsgInfDeleteSuccess);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        #endregion

        #region F8 load other setting
        [OperationLog("参照新規")]
        private void SearchPattern()
        {
            var setting = this.ShowReceiptImporterSettingSearchDialog();
            if (setting == null) return;
            SetHeaderFileInformation(setting);
            ProgressDialog.Start(ParentForm, LoadDetailsAsync(setting.Code), false, SessionKey);
        }
        #endregion

        #region F9 search (in grid show search code)
        [OperationLog("検索")]
        private void SearchCode()
        {
            var row = grid.CurrentRow;
            var field = ConvertRowToField(row);

            if (field == Fields.ReceiptCategoryCode)
            {
                var category = this.ShowReceiptCategorySearchDialog(search: new CategorySearch {
                    CompanyId = CompanyId,
                    CategoryType = CategoryType.Receipt,
                    SearchPredicate = x => string.CompareOrdinal(x.Code, "98") < 0
                });
                if (category == null) return;
                row.Cells["celFixedValue"].Value = category.Code;
                row.Cells["celFieldIndex"].Selected = true;
                BaseContext.SetFunction09Enabled(false);
            }

            if (field == Fields.SectionCode)
            {
                var section = this.ShowSectionSearchDialog();
                if (section == null) return;
                row.Cells["celFixedValue"].Value = section.Code;
                row.Cells["celFieldIndex"].Selected = true;
                BaseContext.SetFunction09Enabled(false);
            }

            if (field == Fields.CurrencyCode)
            {
                var currency = this.ShowCurrencySearchDialog();
                if (currency == null) return;
                row.Cells["celFixedValue"].Value = currency.Code;
                row.Cells["celFieldIndex"].Selected = true;
                BaseContext.SetFunction09Enabled(false);
            }
            if (field == Fields.AccountTypeId)
            {
                var accountType = this.ShowBankAccountTypeSearchDialog();
                if (accountType == null) return;
                row.Cells[CellIndexFixedValue].Value = accountType.Id.ToString();
                row.Cells[CellIndexFieldIndex].Selected = true;
                BaseContext.SetFunction09Enabled(false);
            }
        }

        #endregion

        #region F10 close
        [OperationLog("戻る")]
        private void Close()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;
            ParentForm.Close();
        }
        #endregion

        #region set importer setting values to controls

        private async Task LoadImporterSettingAsync(string code)
        {
            if (string.IsNullOrEmpty(code)) return;
            txtSettingCode.Enabled = false;
            btnSearchSetting.Enabled = false;
            var setting = await GetImporterSettingAsync(code);
            if (setting == null)
            {
                BaseContext.SetFunction08Enabled(true);
                await LoadDetailsAsync(code);
                DispStatusMessage(MsgInfNewData, "パターンNo.");
                Modified = true;
                return;
            }
            SetHeaderName(setting);
            SetHeaderFileInformation(setting);
            await LoadDetailsAsync(code);

            BaseContext.SetFunction03Enabled(true);
            Modified = false;

            txtSettingName.Focus();
        }

        private void SetHeaderName(ImporterSetting setting)
        {
            txtSettingCode.Text = setting.Code;
            txtSettingName.Text = setting.Name;
        }

        private void SetHeaderFileInformation(ImporterSetting setting)
        {
            txtInitialDirectory.Text = setting.InitialDirectory;
            nmbStartLineCount.Value = setting.StartLineCount;
            updateAt = setting.UpdateAt;
            importerSettingId = setting.Id;
            if (setting.PostAction == 0)
                rdoDoNothing.Checked = true;
            else if (setting.PostAction == 1)
                rdoDelete.Checked = true;
            else if (setting.PostAction == 2)
                rdoRename.Checked = true;
            cbxIgnoreLastLine.Checked = setting.IgnoreLastLine == 1;
        }

        private async Task LoadDetailsAsync(string code)
        {
            if (string.IsNullOrEmpty(code)) return;

            grid.CellEnter          -= grid_CellEnter;
            grid.CellValueChanged   -= grid_CellValueChanged;

            var details = await GetImporterSettingDetailAsync(code);

            if (!(details?.Any() ?? false)) return;
            ComboBoxCell comboCell;
            CheckBoxCell chkValue;

            grid.RowCount = details.Count;
            for (var i = 0; i < details.Count; i++)
            {
                var detail = details[i];
                var field = (Fields)detail.Sequence;
                var row = grid.Rows[i];

                row.Cells[CellIndexRowHeader].Value = i + 1;
                row.Cells[CellIndexIsUnique].Value = detail.IsUnique;
                row.Cells[CellIndexFieldName].Value = detail.FieldName;
                row.Cells[CellIndexFixedValue].Value = detail.FixedValue;
                row.Cells[CellIndexSequence].Value = detail.Sequence;

                chkValue = (CheckBoxCell)row.Cells[CellIndexIsUnique];

                if (detail.BaseAttributeDivision > 0)
                {
                    var key = $"ATTR{detail.BaseAttributeDivision}";
                    comboCell = (ComboBoxCell)row.Cells[CellIndexAttribute];
                    SetDataSourceToComboBoxCell(comboCell, GetSettingsByKey(key));
                    comboCell.Value = detail.AttributeDivision;
                    row.Cells[CellIndexAttribute].Visible = true;
                }

                if (field == Fields.RecordedAt)
                {
                    comboCell = (ComboBoxCell)row.Cells[CellIndexImportDivision];
                    comboCell.DataSource = new[] { new { Value = 0, Text = "1：取込有（必須）" } };
                    comboCell.DisplayMember = "Text";
                    comboCell.ValueMember = "Value";
                    comboCell.Value = 0;
                    row.Cells[CellIndexImportDivision].Visible = true;
                    row.Cells[CellIndexImportDivision].Enabled = false;
                }
                else if (field == Fields.ReceiptAmount)
                {
                    comboCell = (ComboBoxCell)row.Cells[CellIndexImportDivision];
                    var comboToriumuNyukingaku = GetSettingsByKey("TRKM21").Where(c => c.ItemKey != "0").ToList();
                    if (comboToriumuNyukingaku.Count > 0)
                        SetDataSourceToComboBoxCell(comboCell, comboToriumuNyukingaku);

                    comboCell.Value = (detail.ImportDivision == 0 ? 1 : detail.ImportDivision);
                    row.Cells[CellIndexImportDivision].Visible = true;
                }
                else
                {
                    var key = $"TRKM{detail.BaseImportDivision}";
                    comboCell = (ComboBoxCell)row.Cells[CellIndexImportDivision];
                    SetDataSourceToComboBoxCell(comboCell, GetSettingsByKey(key));
                    comboCell.Value = detail.ImportDivision;
                    row.Cells[CellIndexImportDivision].Visible = true;
                }

                if (row.Cells[CellIndexImportDivision].DisplayText.Contains("取込有"))
                {
                    chkValue.Enabled = true;
                    row.Cells[CellIndexFixedValue].Enabled = false;
                    row.Cells[CellIndexFixedValue].Value = null;
                    row.Cells[CellIndexFieldIndex].Enabled = true;
                    if (field == Fields.CustomerCode
                     || field == Fields.ReceiptAmount
                     || field == Fields.SectionCode
                     || field == Fields.CurrencyCode
                     || field == Fields.BillBankCode
                     || field == Fields.BillBranchCode
                     || field == Fields.BankCode
                     || field == Fields.BranchCode
                     || field == Fields.AccountTypeId
                     || field == Fields.AccountNumber
                     || field == Fields.BillNumber
                        )
                    {
                        row.Cells[CellIndexAttribute].Enabled = false;
                    }
                    else
                    {
                        row.Cells[CellIndexAttribute].Enabled = true;
                    }
                }
                else if (row.Cells[CellIndexImportDivision].DisplayText.Contains("固定値"))
                {
                    chkValue.Enabled = false;
                    chkValue.Value = 0;
                    row.Cells[CellIndexFixedValue].Enabled = true;
                    row.Cells[CellIndexFieldIndex].Enabled = false;
                    row.Cells[CellIndexFieldIndex].Value = null;
                    row.Cells[CellIndexAttribute].Enabled = false;
                }
                else if (row.Cells[CellIndexImportDivision].DisplayText.Contains("取込無"))
                {
                    chkValue.Enabled = false;
                    chkValue.Value = 0;
                    row.Cells[CellIndexFixedValue].Enabled = false;
                    row.Cells[CellIndexFixedValue].Value = null;
                    row.Cells[CellIndexFieldIndex].Enabled = false;
                    row.Cells[CellIndexFieldIndex].Value = null;
                    row.Cells[CellIndexAttribute].Enabled = false;
                }

                if (detail.FieldIndex != 0)
                    row.Cells[CellIndexFieldIndex].Value = detail.FieldIndex;
            }
            var expression = new DataExpression(ApplicationControl);
            // set textbox input control
            foreach (var item in new[] {
                    new { field = Fields.ReceiptCategoryCode, format = "9", length =  2, ime = ImeMode.Disable  },
                    new { field = Fields.CurrencyCode       , format = "A", length =  3, ime = ImeMode.Disable  },
                    new { field = Fields.BankCode           , format = "9", length =  4, ime = ImeMode.Disable  },
                    new { field = Fields.BankName           , format =  "", length = 30, ime = ImeMode.Hiragana },
                    new { field = Fields.BranchCode         , format = "9", length =  3, ime = ImeMode.Disable  },
                    new { field = Fields.BranchName         , format =  "", length = 30, ime = ImeMode.Hiragana },
                    new { field = Fields.AccountTypeId      , format = "9", length =  1, ime = ImeMode.Disable  },
                    new { field = Fields.AccountNumber      , format = "9", length =  7, ime = ImeMode.Disable  },
                    new { field = Fields.AccountName        , format =  "", length = 50, ime = ImeMode.Hiragana }, /* column は 140 FixedValue が 50 なので やむなし */
                    new {
                        field   = Fields.SectionCode,
                        format  = expression.SectionCodeFormatString,
                        length  = expression.SectionCodeLength,
                        ime     = ImeMode.Disable
                    },
                })
            {
                var index = GetGridRowIndex(item.field);
                if (index < 0) continue;
                var cell = (GcTextBoxCell)grid.Rows[index].Cells[CellIndexFixedValue];
                cell.MaxLength      = item.length;
                if (!string.IsNullOrEmpty(item.format)) {
                    cell.Format         = item.format;
                    cell.AllowSpace     = GrapeCity.Win.Editors.AllowSpace.None;
                }
                if (item.ime != ImeMode.Inherit)        cell.Style.ImeMode  = item.ime;
            }

            foreach (var pair in new[] {
                new { key = nameof(Receipt.Note1), field = Fields.Note1 },
                new { key = nameof(Receipt.Note2), field = Fields.Note2 },
                new { key = nameof(Receipt.Note3), field = Fields.Note3 },
                new { key = nameof(Receipt.Note4), field = Fields.Note4 },
            })
            {
                if (!ColumnNames.ContainsKey(pair.key)) continue;
                var alias = ColumnNames[pair.key];
                var index = GetGridRowIndex(pair.field);
                if (index == -1) continue;
                grid.Rows[index].Cells[CellIndexFieldName].Value = alias;
            }

            var detailCustomerCode = details.FirstOrDefault(x => x.Sequence == (int)Fields.CustomerCode);
            if (detailCustomerCode.ImportDivision == 1)
            {
                foreach (var index in GetCustomerRelatedIndexes())
                {
                    var row = grid.Rows[index];
                    var field = ConvertRowToField(row);
                    if (field == Fields.SourceBankName
                     || field == Fields.SourceBranchName)
                    {
                        row.Cells[CellIndexImportDivision].Enabled = false;
                        row.Cells[CellIndexFixedValue].Enabled = false;
                        row.Cells[CellIndexFieldIndex].Enabled = false;
                        row.Cells[CellIndexFieldIndex].Value = null;
                        row.Cells[CellIndexAttribute].Enabled = false;
                        row.Cells[CellIndexAttribute].Value = null;
                    }
                    else if (field != Fields.BillBranchCode)
                    {
                        row.Cells[CellIndexImportDivision].Enabled = true;
                    }
                }
            }
            else if (detailCustomerCode.ImportDivision == 0)
            {
                foreach (var index in GetCustomerRelatedIndexes())
                {
                    var row = grid.Rows[index];
                    var field = ConvertRowToField(row);
                    if (field == Fields.PayerName)
                    {
                        row.Cells[CellIndexIsUnique].Enabled = true;
                        row.Cells[CellIndexImportDivision].Value = 1;
                        row.Cells[CellIndexImportDivision].Enabled = false;
                        row.Cells[CellIndexFixedValue].Enabled = false;
                        row.Cells[CellIndexFieldIndex].Enabled = true;
                        row.Cells[CellIndexAttribute].Enabled = true;
                    }
                    else if (field == Fields.SourceBankName
                          || field == Fields.SourceBranchName)
                    {
                        row.Cells[CellIndexImportDivision].Enabled = true;
                    }
                    else
                    {
                        row.Cells[CellIndexImportDivision].Value = 0;
                        row.Cells[CellIndexImportDivision].Enabled = false;
                        row.Cells[CellIndexFixedValue].Value = null;
                        row.Cells[CellIndexFixedValue].Enabled = false;
                        row.Cells[CellIndexFieldIndex].Value = null;
                        row.Cells[CellIndexFieldIndex].Enabled = false;
                        row.Cells[CellIndexAttribute].Value = null;
                        row.Cells[CellIndexAttribute].Enabled = false;
                    }
                }
            }

            grid.CellEnter          += grid_CellEnter;
            grid.CellValueChanged   += grid_CellValueChanged;

        }

        private IEnumerable<int> GetCustomerRelatedIndexes()
        {
            var startIndex  = GetGridRowIndex(Fields.PayerName);
            var endIndex    = GetGridRowIndex(Fields.BillDrawer);
            var count = endIndex - startIndex + 1;
            var ignoreStartIndex    = GetGridRowIndex(Fields.BankCode);
            var ignoreEndIndex      = GetGridRowIndex(Fields.AccountName);
            return Enumerable.Range(startIndex, count).Where(x => !(ignoreStartIndex <= x && x <= ignoreEndIndex)).ToArray();
        }

        private void SetDataSourceToComboBoxCell(ComboBoxCell cell, List<Setting> items)
        {
            if (!items?.Any() ?? false) return;
            var source = items.Select(x => new { x.ItemKey, x.ItemValue }).ToArray();
            cell.DataSource = source;
            cell.DisplayMember = nameof(Setting.ItemValue);
            cell.ValueMember = nameof(Setting.ItemKey);
        }
        private List<Setting> GetSettingsByKey(string itemId)
        {
            List<Setting> list;
            return SettingsByKeys.TryGetValue(itemId, out list) ? list : new List<Setting>();
        }

        #endregion

        #region save importer setting

        #region validation

        private bool ValidateInputValues()
        {
            if (!txtSettingCode.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblSettingCode.Text))) return false;
            if (!txtSettingName.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblSettingName.Text))) return false;
            if (!txtInitialDirectory.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblInitialDirectory.Text))) return false;
            if (!Directory.Exists(txtInitialDirectory.Text))
            {
                ShowWarningDialog(MsgWngNotExistFolder);
                this.ActiveControl = txtInitialDirectory;
                txtInitialDirectory.Focus();
                return false;
            }
            if (!nmbStartLineCount.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblStartLineCount.Text))) return false;

            for (var i = 0; i < grid.RowCount; i++)
            {
                var row = grid.Rows[i];
                var isUnique            = Convert.ToInt32(row.Cells[CellIndexIsUnique].Value);
                var importDivision      = Convert.ToInt32(row.Cells[CellIndexImportDivision].Value);
                var fieldIndex          = Convert.ToInt32(row.Cells[CellIndexFieldIndex].Value);
                var attributeDivision   = Convert.ToInt32(row.Cells[CellIndexAttribute].Value);
                var fixedValue          = row.Cells[CellIndexFixedValue].Value;
                var field = ConvertRowToField(row);

                if ((field == Fields.RecordedAt || field == Fields.ReceiptAmount) && fieldIndex == 0)
                {
                    SetCurrentCell(row.Cells[CellIndexFieldIndex], () => ShowWarningDialog(MsgWngInputRequired, "項目番号"));
                    return false;
                }
                if (importDivision == 1 && fieldIndex == 0)
                {
                    SetCurrentCell(row.Cells[CellIndexFieldIndex], () => ShowWarningDialog(MsgWngInputRequired, "項目番号"));
                    return false;
                }
                else if (row.Cells[CellIndexFixedValue].Enabled && fixedValue == null)
                {
                    SetCurrentCell(row.Cells[CellIndexFixedValue], () => ShowWarningDialog(MsgWngInputRequired, "固定値"));
                    return false;
                }
                else if (fixedValue != null && row.Cells[CellIndexFixedValue].Enabled)
                {
                    var code = Convert.ToString(fixedValue);
                    if (field == Fields.ReceiptCategoryCode)
                    {
                        if (!ValidateReceiptCategoryCode(code))
                        {
                            row.Cells[CellIndexFixedValue].Value = null;
                            SetCurrentCell(row.Cells[CellIndexFixedValue], () => ShowWarningDialog(MsgWngMasterNotExist, "区分マスター", code));
                            return false;
                        }
                        if ((code == "99" || code == "98"))
                        {
                            row.Cells[CellIndexFixedValue].Value = null;
                            SetCurrentCell(row.Cells[CellIndexFixedValue], () => ShowWarningDialog(MsgWngInvalidCategoryForUpdate, code));
                            return false;
                        }
                    }

                    if ((field == Fields.BankName
                      || field == Fields.BranchName
                      || field == Fields.AccountName)
                      && string.IsNullOrWhiteSpace(code))
                    {
                        SetCurrentCell(row.Cells[CellIndexFixedValue], () => ShowWarningDialog(MsgWngInputRequired, "固定値"));
                        return false;
                    }

                    if (field == Fields.SectionCode && !ValidateSectionCode(code))
                    {
                        row.Cells[CellIndexFixedValue].Value = null;
                        SetCurrentCell(row.Cells[CellIndexFixedValue], () => ShowWarningDialog(MsgWngMasterNotExist, "入金部門マスター", code));
                        return false;
                    }
                    if (field == Fields.CurrencyCode && !ValidateCurrencyCode(code))
                    {
                        row.Cells[CellIndexFixedValue].Value = null;
                        SetCurrentCell(row.Cells[CellIndexFixedValue], () => ShowWarningDialog(MsgWngInvalidCategoryForUpdate, "通貨マスター", code));
                        return false;
                    }
                    if (field == Fields.AccountTypeId && !ValidateAccountTypeId(code))
                    {
                        row.Cells[CellIndexFixedValue].Value = null;
                        SetCurrentCell(row.Cells[CellIndexFixedValue], () => ShowWarningDialog(MsgWngInvalidInputValue, "預金種別"));
                        return false;
                    }
                }
                else if (row.Cells[CellIndexAttribute].Enabled && attributeDivision == 0)
                {
                    SetCurrentCell(row.Cells[CellIndexAttribute], () => ShowWarningDialog(MsgWngSelectionRequired, "属性情報"));
                    return false;
                }
            }

            return true;
        }

        private void SetCurrentCell(Cell cell, System.Action messaging)
        {
            cell.Selected = true;
            grid.CurrentCell = cell;
            grid.HideSelection = false;
            this.ActiveControl = grid;
            messaging?.Invoke();
        }

        private bool ValidateReceiptCategoryCode(string code)
        {
            var task = CheckExistsCategoryMasterAsync(code);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return task.Result;
        }

        private bool ValidateSectionCode(string code)
        {
            var task = CheckExistsSectionMasterAsync(code);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return task.Result;
        }

        private bool ValidateCurrencyCode(string code)
        {
            var task = CheckExistsCurrencyMasterAsync(code);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return task.Result;
        }

        /// <summary>預金種別 1:普通, 2:当座, 4:貯蓄, 5:通知</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool ValidateAccountTypeId(string id)
            => (new[] { "1", "2", "4", "5" }).Contains(id);

        #endregion

        private async Task SaveImporterSetting()
        {
            var header = PrepareImporterSettingForSave();
            var details = PrepareImporterSettingDetailsForSave();
            var saveResult = await SaveImporterSettingAsync(header, details);
            if (!saveResult)
            {
                ShowWarningDialog(MsgErrSaveError);
                return;
            }
            ClearControlValues();
            DispStatusMessage(MsgInfSaveSuccess);
        }

        private ImporterSetting PrepareImporterSettingForSave()
            => new ImporterSetting {
                CompanyId           = CompanyId,
                Code                = txtSettingCode.Text,
                Name                = txtSettingName.Text.Trim(),
                FormatId            = FormatId,
                InitialDirectory    = txtInitialDirectory.Text.Trim(),
                StartLineCount      = Convert.ToInt32(nmbStartLineCount.Value),
                IgnoreLastLine      = cbxIgnoreLastLine.Checked ? 1 : 0,
                UpdateBy            = Login.UserId,
                UpdateAt            = updateAt,
                CreateBy            = Login.UserId,
                PostAction          = rdoDoNothing.Checked  ? 0
                                    : rdoDelete.Checked     ? 1
                                    :                         2,
            };

        private List<ImporterSettingDetail> PrepareImporterSettingDetailsForSave()
        {
            var details = new List<ImporterSettingDetail>();
            var nowDateTime = DateTime.Now;
            for (var i = 0; i < grid.RowCount; i++)
            {
                var detail = new ImporterSettingDetail();
                detail.Sequence = Convert.ToInt32(grid.Rows[i].Cells[CellIndexSequence].Value);
                detail.IsUnique = Convert.ToInt32(grid.Rows[i].Cells[CellIndexIsUnique].Value);
                if (grid.Rows[i].Cells[CellIndexFixedValue].DisplayText == "")
                {
                    detail.FixedValue = "";
                }
                else
                {
                    detail.FixedValue = grid.Rows[i].Cells[CellIndexFixedValue].Value.ToString();
                }
                detail.ImporterSettingId = importerSettingId;
                detail.UpdateBy = Login.UserId;
                detail.UpdateAt = nowDateTime;
                detail.CreateBy = Login.UserId;
                detail.CreateAt = nowDateTime;
                detail.ImportDivision = Convert.ToInt32(grid.Rows[i].Cells[CellIndexImportDivision].Value);
                detail.Caption = "";
                if (detail.ImportDivision == 1)
                {
                    detail.FieldIndex = Convert.ToInt32(grid.Rows[i].Cells[CellIndexFieldIndex].Value);
                }
                else if (detail.Sequence == (int)Fields.RecordedAt || detail.Sequence == (int)Fields.ReceiptAmount)
                {
                    detail.FieldIndex = Convert.ToInt32(grid.Rows[i].Cells[CellIndexFieldIndex].Value);
                }

                detail.AttributeDivision = Convert.ToInt32(grid.Rows[i].Cells[CellIndexAttribute].Value);
                details.Add(detail);
            }
            return details;
        }


        #endregion

        #region clear control values

        public void ClearControlValues()
        {
            ClearStatusMessage();
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);
            cbxIgnoreLastLine.Checked = false;
            txtSettingCode.Clear();
            txtSettingName.Clear();
            nmbStartLineCount.Clear();
            txtInitialDirectory.Clear();
            txtSettingCode.Enabled = true;
            grid.Rows.Clear();
            grid.HideSelection = true;
            Modified = false;
            btnSearchSetting.Enabled = true;
            rdoDoNothing.Checked = true;
            txtSettingCode.Focus();
        }


        #endregion

        #region delete pattern

        private bool ValidateForDelete()
        {
            var task = CheckTaskScheduleExistAsync(importerSettingId);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            if (task.Result)
            {
                ShowWarningDialog(MsgWngDeleteConstraint, "タイムスケジューラー", "取込パターン");
                return false;
            }
            return true;
        }

        #endregion

        #region event handlers

        private void OnContentChanged(object sender, EventArgs e)
        {
            if (!Modified) Modified = true;
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnPath);
            var path = txtInitialDirectory.Text.Trim();
            var selectedPath = string.Empty;
            var rootBrowserPath = new List<string>();
            if (!LimitAccessFolder ?
                !ShowFolderBrowserDialog(path, out selectedPath) :
                !ShowRootFolderBrowserDialog(ApplicationControl.RootPath, out rootBrowserPath, FolderBrowserType.SelectFolder)) return;

            txtInitialDirectory.Text = (!LimitAccessFolder) ? selectedPath : rootBrowserPath?.FirstOrDefault();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var setting = this.ShowReceiptImporterSettingSearchDialog();
                if (setting == null) return;

                ProgressDialog.Start(ParentForm, LoadImporterSettingAsync(setting.Code), false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtPatternNo_Validated(object sender, EventArgs e)
        {
            try
            {
                var code = txtSettingCode.Text;
                if (string.IsNullOrEmpty(code)) return;

                ProgressDialog.Start(ParentForm, LoadImporterSettingAsync(code), false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void grid_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
        {
            Modified = true;
            if (e.Control is ComboBox)
            {
                var combo = e.Control as ComboBox;
                if (combo == null) return;
                combo.SelectedIndexChanged -= combo_SelectedIndexChanged;
                combo.SelectedIndexChanged += combo_SelectedIndexChanged;
            }

        }
        private void grid_CellEnter(object sender, CellEventArgs e)
        {
            if (e.CellIndex == CellIndexFixedValue
                && grid.CurrentCell.Enabled)
            {
                var field = ConvertRowToField(grid.CurrentRow);
                if (field == Fields.ReceiptCategoryCode
                ||  field == Fields.SectionCode
                ||  field == Fields.CurrencyCode
                ||  field == Fields.AccountTypeId)
                {
                    BaseContext.SetFunction09Enabled(true);
                    return;
                }
            }
            BaseContext.SetFunction09Enabled(false);
        }

        private void grid_CellValueChanged(object sender, CellEventArgs e)
        {
            Modified = true;
            if (e.CellIndex != CellIndexFixedValue
                || e.RowIndex < 0
                || grid.Rows[e.RowIndex].Cells[e.CellIndex].Value == null) return;
            var row = grid.Rows[e.RowIndex];
            var field = ConvertRowToField(row);
            var value = row.Cells[CellIndexFixedValue].Value?.ToString() ?? string.Empty;
            var length = 0;
            var paddingChar = (char?)null;

            if (field == Fields.ReceiptCategoryCode) {
                length = 2; paddingChar = '0';
            }
            if (field == Fields.BankCode) {
                length = 4; paddingChar = '0';
            }
            if (field == Fields.BranchCode) {
                length = 3; paddingChar = '0';
            }
            if (field == Fields.AccountNumber) {
                length = 7; paddingChar = '0';
            }

            if (string.IsNullOrEmpty(value) || !paddingChar.HasValue) return;
            row.Cells[CellIndexFixedValue].Value = value.PadLeft(length, paddingChar.Value);

        }

        private void combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combo = sender as ComboBox;
            if (combo == null) return;

            var field = ConvertRowToField(grid.CurrentRow);
            if (combo.Text.Contains("取込有"))
            {
                grid.CurrentRow.Cells[CellIndexFieldIndex].Enabled = true;
                grid.CurrentRow.Cells[CellIndexFixedValue].Value = null;
                grid.CurrentRow.Cells[CellIndexFixedValue].Enabled = false;
                grid.CurrentRow.Cells[CellIndexIsUnique].Enabled = true;
                if (field == Fields.CustomerCode
                 || field == Fields.ReceiptAmount
                 || field == Fields.SectionCode
                 || field == Fields.CurrencyCode
                 || field == Fields.BillBankCode
                 || field == Fields.BillBranchCode
                 || field == Fields.BankCode
                 || field == Fields.BranchCode
                 || field == Fields.AccountTypeId
                 || field == Fields.AccountNumber
                 || field == Fields.BillNumber
                    )
                {
                    grid.CurrentRow.Cells[CellIndexAttribute].Enabled = false;
                    grid.CurrentRow.Cells[CellIndexAttribute].Value = null;
                }
                else
                {
                    grid.CurrentRow.Cells[CellIndexAttribute].Enabled = true;
                }

                if (field == Fields.BillBankCode)
                {
                    var index = GetGridRowIndex(Fields.BillBranchCode);
                    grid.Rows[index].Cells[CellIndexImportDivision].Value = 1;
                    grid.Rows[index].Cells[CellIndexFieldIndex].Enabled = true;
                    grid.Rows[index].Cells[CellIndexIsUnique].Enabled = true;
                }

                if (field == Fields.CustomerCode)
                {
                    foreach (var index in GetCustomerRelatedIndexes())
                    {
                        var row = grid.Rows[index];
                        field = ConvertRowToField(row);
                        if (field == Fields.SourceBankName
                         || field == Fields.SourceBranchName)
                        {
                            row.Cells[CellIndexIsUnique].Enabled = false;
                            row.Cells[CellIndexIsUnique].Value = 0;
                            row.Cells[CellIndexImportDivision].Value = 0;
                            row.Cells[CellIndexImportDivision].Enabled = false;
                            row.Cells[CellIndexFixedValue].Value = null;
                            row.Cells[CellIndexFixedValue].Enabled = false;
                            row.Cells[CellIndexFieldIndex].Enabled = false;
                            row.Cells[CellIndexFieldIndex].Value = null;
                            row.Cells[CellIndexAttribute].Enabled = false;
                            row.Cells[CellIndexAttribute].Value = null;
                        }
                        else if (field != Fields.BillBranchCode)
                        {
                            row.Cells[CellIndexImportDivision].Enabled = true;
                        }
                    }
                }
            }
            else if (combo.Text.Contains("固定値"))
            {
                grid.CurrentRow.Cells[CellIndexFixedValue].Enabled = true;
                grid.CurrentRow.Cells[CellIndexFieldIndex].Enabled = false;
                grid.CurrentRow.Cells[CellIndexFieldIndex].Value = null;
                grid.CurrentRow.Cells[CellIndexAttribute].Enabled = false;
                grid.CurrentRow.Cells[CellIndexAttribute].Value = null;
                grid.CurrentRow.Cells[CellIndexIsUnique].Enabled = false;
                grid.CurrentRow.Cells[CellIndexIsUnique].Value = 0;
            }
            else if (combo.Text.Contains("取込無"))
            {
                grid.CurrentRow.Cells[CellIndexFixedValue].Value = null;
                grid.CurrentRow.Cells[CellIndexFixedValue].Enabled = false;
                grid.CurrentRow.Cells[CellIndexFieldIndex].Enabled = false;
                grid.CurrentRow.Cells[CellIndexFieldIndex].Value = null;
                grid.CurrentRow.Cells[CellIndexAttribute].Enabled = false;
                grid.CurrentRow.Cells[CellIndexAttribute].Value = null;
                grid.CurrentRow.Cells[CellIndexIsUnique].Enabled = false;
                grid.CurrentRow.Cells[CellIndexIsUnique].Value = 0;
                if (field == Fields.BillBankCode)
                {
                    var index = GetGridRowIndex(Fields.BillBranchCode);
                    grid.Rows[index].Cells[CellIndexImportDivision].Value = 0;
                    grid.Rows[index].Cells[CellIndexFieldIndex].Value = null;
                    grid.Rows[index].Cells[CellIndexFieldIndex].Enabled = false;
                    grid.Rows[index].Cells[CellIndexIsUnique].Enabled = false;
                    grid.Rows[index].Cells[CellIndexIsUnique].Value = 0;
                }
                else if (field == Fields.CustomerCode)
                {
                    foreach (var index in GetCustomerRelatedIndexes())
                    {
                        var row = grid.Rows[index];
                        field = ConvertRowToField(row);
                        if (field == Fields.PayerName)
                        {
                            row.Cells[CellIndexIsUnique].Enabled = true;
                            row.Cells[CellIndexImportDivision].Value = 1;
                            row.Cells[CellIndexImportDivision].Enabled = false;
                            row.Cells[CellIndexFixedValue].Enabled = false;
                            row.Cells[CellIndexFieldIndex].Enabled = true;
                            row.Cells[CellIndexAttribute].Enabled = true;
                        }
                        else if (field == Fields.SourceBankName
                              || field == Fields.SourceBranchName)
                        {
                            row.Cells[CellIndexImportDivision].Enabled = true;
                        }
                        else
                        {
                            row.Cells[CellIndexIsUnique].Enabled = false;
                            row.Cells[CellIndexIsUnique].Value = 0;
                            row.Cells[CellIndexImportDivision].Value = 0;
                            row.Cells[CellIndexImportDivision].Enabled = false;
                            row.Cells[CellIndexFixedValue].Value = null;
                            row.Cells[CellIndexFixedValue].Enabled = false;
                            row.Cells[CellIndexFieldIndex].Value = null;
                            row.Cells[CellIndexFieldIndex].Enabled = false;
                            row.Cells[CellIndexAttribute].Value = null;
                            row.Cells[CellIndexAttribute].Enabled = false;
                        }
                    }
                }
            }
        }

        #endregion

        #region call web services

        private async Task LoadSettingsAsync()
            => await ServiceProxyFactory.DoAsync(async (SettingMasterClient client) => {
                var result = await client.GetItemsAsync(SessionKey, new string[] { });
                if (result.ProcessResult.Result)
                    SettingsByKeys = result.Settings.GroupBy(x => x.ItemId).ToDictionary(x => x.Key, x => x.ToList());
                else
                    SettingsByKeys = new Dictionary<string, List<Setting>>();
            });

        private async Task LoadColumnNamesAsync()
            => await ServiceProxyFactory.DoAsync(async (ColumnNameSettingMasterClient client) => {
                var result = await client.GetItemsAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    ColumnNames = result.ColumnNames.Where(x => x.TableName == nameof(Receipt)).ToDictionary(x => x.ColumnName, x => x.DisplayColumnName);
                else
                    ColumnNames = new Dictionary<string, string>();
            });

        private async Task<bool> CheckExistsCategoryMasterAsync(string code)
            => await ServiceProxyFactory.DoAsync(async (CategoryMasterClient client)
                => (await client.GetByCodeAsync(SessionKey, CompanyId, CategoryType.Receipt, new[] { code }))?.Categories.Any() ?? false);

        private async Task<bool> CheckExistsSectionMasterAsync(string code)
            => await ServiceProxyFactory.DoAsync(async (SectionMasterClient client)
                => (await client.GetByCodeAsync(SessionKey, CompanyId, new[] { code }))?.Sections.Any() ?? false);

        private async Task<bool> CheckExistsCurrencyMasterAsync(string code)
            => await ServiceProxyFactory.DoAsync(async (CurrencyMasterClient client)
                => (await client.GetByCodeAsync(SessionKey, CompanyId, new[] { code }))?.Currencies.Any() ?? false);

        private async Task<bool> SaveImporterSettingAsync(ImporterSetting header, List<ImporterSettingDetail> details)
            => await ServiceProxyFactory.DoAsync(async (ImporterSettingServiceClient client)
                => (await client.SaveAsync(SessionKey, header, details.ToArray()))?.ProcessResult.Result ?? false);

        private async Task<bool> CheckTaskScheduleExistAsync(int id)
            => await ServiceProxyFactory.DoAsync(async (TaskScheduleMasterClient client)
                => (await client.ExistsAsync(SessionKey, CompanyId, ImportType, id))?.Exist ?? false);

        private async Task<bool> DeleteImporterSettingAsync(int id)
            => await ServiceProxyFactory.DoAsync(async (ImporterSettingServiceClient client)
                => (await client.DeleteAsync(SessionKey, importerSettingId))?.ProcessResult.Result ?? false);

        private async Task<ImporterSetting> GetImporterSettingAsync(string code)
            => await ServiceProxyFactory.DoAsync(async (ImporterSettingServiceClient client)
                => (await client.GetHeaderByCodeAsync(SessionKey, CompanyId, FormatId, code))?.ImporterSetting);

        private async Task<List<ImporterSettingDetail>> GetImporterSettingDetailAsync(string code)
            => await ServiceProxyFactory.DoAsync(async (ImporterSettingServiceClient client)
                => (await client.GetDetailByCodeAsync(SessionKey, CompanyId, FormatId, code))
                    ?.ImporterSettingDetails.Where(x => x.IsUsableForReceipt(ApplicationControl)).ToList());

        #endregion

        #region importer setting helper

        private int GetGridRowIndex(Fields field)
            => grid.Rows.FirstOrDefault(x => ConvertRowToField(x) == field)?.Index ?? -1;

        private Fields ConvertRowToField(Row row)
            => (Fields)int.Parse(row.Cells[CellIndexSequence].Value?.ToString());

        #endregion


    }
}