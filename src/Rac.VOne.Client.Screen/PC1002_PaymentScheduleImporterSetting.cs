using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.ImporterSettingService;
using Rac.VOne.Client.Screen.SettingMasterService;
using Rac.VOne.Client.Screen.TaskScheduleMasterService;
using Rac.VOne.Common.Importer.PaymentSchedule;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Common.Constants;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>入金予定フリーインポーター・取込設定</summary>
    public partial class PC1002 : VOneScreenBase
    {
        #region 変数宣言
        private DateTime updateAt { get; set; }
        private int ImporterSettingId { get; set; }
        private string code { get; set; } = "";

        /// <summary>
        /// 入金予定フリーインポーター FormatId : 3
        /// </summary>
        private int FormatId { get { return (int)FreeImporterFormatType.PaymentSchedule; } }
        /// <summary>
        /// タスクスケジューラ ImportType 5 : 入金予定フリーインポーター
        /// </summary>
        private int ImportType { get { return (int)TaskScheduleImportType.PaymentSchedule; } }

        /// <summary>0 : 行番号</summary>
        private int CellIndexRowHeader { get { return 0; } }
        /// <summary>1 : 項目名</summary>
        private int CellIndexFieldName { get { return 1; } }
        /// <summary>2 : キー</summary>
        private int CellIndexImportDivision { get { return 2; } }
        /// <summary>3 : 項目番号</summary>
        private int CellIndexFieldIndex { get { return 3; } }
        /// <summary>4 : 項目名（注釈）</summary>
        private int CellIndexCaption { get { return 4; } }
        /// <summary>5 : 引当優先順位・項目間</summary>
        private int CellIndexPriority { get { return 5; } }
        /// <summary>6 : 引当優先順位・項目内</summary>
        private int CellIndexAssendingOrder { get { return 6; } }
        /// <summary>7 : 属性情報</summary>
        private int CellIndexAttribute { get { return 7; } }
        /// <summary>8 : Sequence</summary>
        private int CellIndexSequence { get { return 8; } }
        #endregion

        #region initialization

        public PC1002()
        {
            InitializeComponent();
            grid.SetupShortcutKeys();
            Text = "入金予定フリーインポーター・取込設定 ";

            AddHandlers();
        }

        private void PC1002_Load(object sender, EventArgs e)
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

                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);
                InitializeGridTemplate();
                Clear();
                txtInitialDirectory.Enabled = !LimitAccessFolder;
                Modified = false;
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

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Save;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = ConfirmToClear;

            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction03Enabled(true);
            OnF03ClickHandler = Delete;

            BaseContext.SetFunction08Caption("参照新規");
            BaseContext.SetFunction08Enabled(true);
            OnF08ClickHandler = SearchCode;

            BaseContext.SetFunction10Caption("終了");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Exit;

            SuspendLayout();
            ResumeLayout();
        }

        private void InitializeGridTemplate()
        {
            var template = new Template();
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext, autoLocationSet: false);
            var height = builder.DefaultRowHeight;

            //0, 40, 155,           435,     595,
            //       155, 235, 335, 435, 515
            var posX = new int[]
            {
                0, 40, 155,           435,     595,
                       155, 235, 335, 435, 515
            };

            var posY = new int[] { 0, height, height * 2 };

            builder.Items.Clear();

            builder.Items.AddRange(new CellSetting[]
            {
                #region ヘッダー1
                new CellSetting(height * 2,  40, "Header"        , location: new Point(posX[0], posY[0]), caption: "No."          ),
                new CellSetting(height * 2, 115, "FieldName"     , location: new Point(posX[1], posY[0]), caption: "項目名"       ),
                new CellSetting(height    , 280, "Caption1"      , location: new Point(posX[2], posY[0]), caption: "取込"         ),
                new CellSetting(height    , 160, "PItem1"        , location: new Point(posX[3], posY[0]), caption: "引当優先順位" ),
                new CellSetting(height * 2, 100, "AttributeInfo" , location: new Point(posX[4], posY[0]), caption: "属性情報"     ),
                #endregion
                #region ヘッダー2
                new CellSetting(height    ,  80, "ImportDivision", location: new Point(posX[5], posY[1]), caption: "キー"          ),
                new CellSetting(height    , 100, "FieldIndex"    , location: new Point(posX[6], posY[1]), caption: "項目番号"      ),
                new CellSetting(height    , 100, "Caption"       , location: new Point(posX[7], posY[1]), caption: "項目名（注釈）"),
                new CellSetting(height    ,  80, "PItem"         , location: new Point(posX[8], posY[1]), caption: "項目間"        ),
                new CellSetting(height    ,  80, "Item"          , location: new Point(posX[9], posY[1]), caption: "項目内"        ),
                #endregion
            });
            builder.BuildHeaderOnly(template);
            builder.Items.Clear();

            var assendingOrder = builder.GetComboBoxCell();
            assendingOrder.DataSource = new[]
            {
                new { Value = 0, Text = "昇順" },
                new { Value = 1, Text = "降順" },
            };
            assendingOrder.DisplayMember = "Text";
            assendingOrder.ValueMember = "Value";

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "Header"         , location: new Point(posX[0], posY[0]), cell: builder.GetRowHeaderCell() ),
                new CellSetting(height, 115, "FieldName"      , location: new Point(posX[1], posY[0])                 , dataField: nameof(ImporterSetting.FieldName)         ),
                new CellSetting(height,  80, "ImportDivision" , location: new Point(posX[2], posY[0]), readOnly: false, dataField: nameof(ImporterSetting.ImportDivisions)  , cell: builder.GetCheckBoxCell() ),
                new CellSetting(height, 100, "FieldIndex"     , location: new Point(posX[6], posY[0]), readOnly: false, dataField: nameof(ImporterSetting.FieldIndex)       , cell: builder.GetNumberCellFreeInput(1,999,3)),
                new CellSetting(height, 100, "Caption"        , location: new Point(posX[7], posY[0]), readOnly: false, dataField: nameof(ImporterSetting.Caption)          , cell: builder.GetTextBoxCell(maxLength: 50)),
                new CellSetting(height,  80, "PItem"          , location: new Point(posX[8], posY[0]), readOnly: false, dataField: nameof(ImporterSetting.FieldIndex)       , cell: builder.GetNumberCellFreeInput(1,999,3) ),
                new CellSetting(height,  80, "Item"           , location: new Point(posX[9], posY[0]), readOnly: false, cell: assendingOrder ),
                new CellSetting(height, 100, "AttributeInfo"  , location: new Point(posX[4], posY[0]), readOnly: false, cell: builder.GetComboBoxCell() ),
                new CellSetting(     0,   0, "Sequence"       , visible: false )
            });

            builder.BuildRowOnly(template);
            grid.Template = template;
            grid.HideSelection = true;
            Modified = false;
        }

        #endregion

        [OperationLog("登録")]
        private void Save()
        {
            try
            {
                grid.EndEdit();
                if (!ValidateForSave()) return;
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
        private void ConfirmToClear()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear))
                return;

            Clear();
            txtPatternNumber.Focus();
        }

        [OperationLog("参照新規")]
        private void SearchCode()
        {
            var import = this.ShowScheduledPaymentImporterSettingSearchDialog();
            if (import == null) return;
            try
            {
                var task = ServiceProxyFactory.DoAsync<ImporterSettingServiceClient>(async client =>
                {
                    var result = await client.GetHeaderByCodeAsync(SessionKey, CompanyId, FormatId, import.Code);
                    var header = result.ProcessResult.Result ? result.ImporterSetting : null;
                    if (header == null) return;
                    code = header.Code;
                    txtInitialDirectory.Text = header.InitialDirectory;
                    nmbStartLineCount.Value = header.StartLineCount;
                    cbxIgnoreLastLine.Checked = (header.IgnoreLastLine == 1);
                    if (header.PostAction == 0)
                        rdoNoAction.Checked = true;
                    else if (header.PostAction == 1)
                        rdoDelete.Checked = true;
                    else
                        rdoAddDate.Checked = true;

                    await SetDataGridViewAsync();
                });

                ProgressDialog.Start(ParentForm, task, false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }

        }

        [OperationLog("終了")]
        private void Exit()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;
            ParentForm.Close();
        }

        private bool ValidateForSave()
        {
            if (!txtPatternNumber.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblPatternNumber.Text))) return false;
            if (!txtPatternName.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblPatternName.Text))) return false;
            if (!txtInitialDirectory.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblInitialDirectory.Text))) return false;

            if (!Directory.Exists(txtInitialDirectory.Text))
            {
                ShowWarningDialog(MsgWngNotExistFolder);
                this.ActiveControl = txtInitialDirectory;
                txtInitialDirectory.Focus();
                return false;
            }
            if (!nmbStartLineCount.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblStartLineCount.Text))) return false;

            // 有効な取込キーチェックボックスが全てOFF状態の場合
            var importDivisionCells = grid.Rows.Select(r => r.Cells[CellIndexImportDivision]);
            if (importDivisionCells.Where(c => c.Enabled).All(c => (int)c.Value == 0))
            {
                ShowWarningDialog(MsgWngSelectionRequired, "取込キー");

                // 有効な取込キーチェックボックスの中で一番上のものにフォーカス
                var idx = importDivisionCells.ToList().FindIndex(c => c.Enabled);
                idx = (0 <= idx) ? idx : 0;
                grid.Rows[idx].Cells[CellIndexImportDivision].Selected = true;
                grid.CurrentCell = grid.Rows[idx].Cells[CellIndexImportDivision];
                grid.HideSelection = false;
                this.ActiveControl = grid;
                return false;
            }

            // その他のグリッド入力値
            for (var i = 0; i < grid.RowCount; i++)
            {
                var importDivision = Convert.ToInt32(grid.Rows[i].Cells[CellIndexImportDivision].Value);
                var fieldIndex = grid.Rows[i].Cells[CellIndexFieldIndex].Value;
                var caption = grid.Rows[i].Cells[CellIndexCaption].Value;
                var itemPriority = Convert.ToInt32(grid.Rows[i].Cells[CellIndexAssendingOrder].Value);
                var attributeDivision = Convert.ToInt32(grid.Rows[i].Cells[CellIndexAttribute].Value);
                var nyukinyote = grid.Rows[i].Cells[CellIndexFieldIndex].Value;
                var seq = Convert.ToInt32(grid.Rows[i].Cells[CellIndexSequence].Value);
                var pIndex = grid.Rows[i].Cells[CellIndexPriority].Value;

                if (nyukinyote == null && seq == 1)
                {
                    grid.Rows[i].Cells[CellIndexFieldIndex].Selected = true;
                    grid.CurrentCell = grid.Rows[i].Cells[CellIndexFieldIndex];
                    grid.HideSelection = false;
                    this.ActiveControl = grid;
                    ShowWarningDialog(MsgWngInputRequired, "項目番号");
                    return false;
                }

                if (importDivision == 1 && fieldIndex == null)
                {
                    grid.Rows[i].Cells[CellIndexFieldIndex].Selected = true;
                    grid.CurrentCell = grid.Rows[i].Cells[CellIndexFieldIndex];
                    grid.HideSelection = false;
                    this.ActiveControl = grid;
                    ShowWarningDialog(MsgWngInputRequired, "項目番号");
                    return false;
                }
                else if (pIndex != null && grid.Rows[i].Cells[CellIndexAssendingOrder].DisplayText == "")
                {
                    grid.Rows[i].Cells[CellIndexAssendingOrder].Selected = true;
                    grid.CurrentCell = grid.Rows[i].Cells[CellIndexAssendingOrder];
                    grid.HideSelection = false;
                    this.ActiveControl = grid;
                    ShowWarningDialog(MsgWngSelectionRequired, "項目内");
                    return false;
                }
                else if (fieldIndex != null && attributeDivision == 0 && grid.Rows[i].Cells[CellIndexAttribute].Enabled)
                {
                    grid.Rows[i].Cells[CellIndexAttribute].Selected = true;
                    grid.CurrentCell = grid.Rows[i].Cells[CellIndexAttribute];
                    grid.HideSelection = false;
                    this.ActiveControl = grid;
                    ShowWarningDialog(MsgWngSelectionRequired, "属性情報");
                    return false;
                }
            }
            ClearStatusMessage();
            return true;

        }

        private async Task SaveImporterSettingAsync()
        {
            var header = new ImporterSetting();
            header.CompanyId = CompanyId;
            header.Code = txtPatternNumber.Text;
            header.Name = txtPatternName.Text.Trim();
            header.FormatId = FormatId;
            header.InitialDirectory = txtInitialDirectory.Text.Trim();
            header.StartLineCount = Convert.ToInt32(nmbStartLineCount.Value);

            if (rdoNoAction.Checked)
                header.PostAction = 0;
            else if (rdoDelete.Checked)
                header.PostAction = 1;
            else if (rdoAddDate.Checked)
                header.PostAction = 2;

            header.IgnoreLastLine = cbxIgnoreLastLine.Checked ? 1 : 0;
            header.UpdateBy = Login.UserId;
            header.UpdateAt = updateAt;
            header.CreateBy = Login.UserId;

            var details = PrepareSaveImporterSettingDetail();
            ImporterSettingAndDetailResult result = null;
            await ServiceProxyFactory.DoAsync<ImporterSettingServiceClient>(async client
                => result = await client.SaveAsync(SessionKey, header, details.ToArray()));
            if (result == null
                || result.ImporterSetting == null
                || result.ImporterSettingDetail == null)
            {
                ShowWarningDialog(MsgErrSaveError);
                return;
            }
            Clear();
            DispStatusMessage(MsgInfSaveSuccess);
        }

        private List<ImporterSettingDetail> PrepareSaveImporterSettingDetail()
        {
            var nowDateTime = DateTime.Now;
            return grid.Rows.Select(row =>
            {
                var detail = new ImporterSettingDetail();
                detail.IsUnique = 0;
                detail.FixedValue = "";
                detail.ImporterSettingId = ImporterSettingId;
                detail.UpdateBy = Login.UserId;
                detail.UpdateAt = nowDateTime;
                detail.Sequence = Convert.ToInt32(row.Cells[CellIndexSequence].Value);
                detail.CreateBy = Login.UserId;
                detail.CreateAt = nowDateTime;
                detail.ImportDivision = Convert.ToInt32(row.Cells[CellIndexImportDivision].Value);
                var field = (Fields)detail.Sequence;
                if (detail.ImportDivision == 1)
                {
                    detail.FieldIndex = Convert.ToInt32(row.Cells[CellIndexFieldIndex].Value);
                }
                else if (detail.ImportDivision == 0
                    && (field == Fields.ReceiptAmount
                     || field == Fields.ScheduledPaymentKey))
                {
                    detail.FieldIndex = Convert.ToInt32(row.Cells[CellIndexFieldIndex].Value);
                }
                else
                {
                    detail.FieldIndex = Convert.ToInt32(row.Cells[CellIndexPriority].Value);
                }
                detail.Caption = row.Cells[CellIndexCaption].Value?.ToString() ?? "";
                detail.ItemPriority = Convert.ToInt32(row.Cells[CellIndexAssendingOrder].Value);
                detail.AttributeDivision = Convert.ToInt32(row.Cells[CellIndexAttribute].Value);
                return detail;
            }).ToList();
        }

        public void Clear()
        {
            ClearStatusMessage();
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            cbxIgnoreLastLine.Checked = false;
            this.ActiveControl = txtPatternNumber;
            txtPatternNumber.Focus();
            txtPatternNumber.Clear();
            txtPatternName.Clear();
            nmbStartLineCount.Clear();
            txtInitialDirectory.Clear();
            txtPatternNumber.Enabled = true;
            grid.Rows.Clear();
            grid.HideSelection = true;
            btnPatternNoSearch.Enabled = true;
            rdoNoAction.Checked = true;
            Modified = false;
        }

        private async Task ShowAllDataAsync()
        {
            if (txtPatternNumber.Text != "")
            {
                var patternNo = txtPatternNumber.Text;
                txtPatternNumber.Text = patternNo.PadLeft(2, '0');
                txtPatternNumber.Enabled = false;
                ImporterSettingResult result = null;
                await ServiceProxyFactory.DoAsync<ImporterSettingServiceClient>(async client
                    => result = await client.GetHeaderByCodeAsync(SessionKey, CompanyId, FormatId, txtPatternNumber.Text));

                if (result.ImporterSetting != null)
                {
                    txtPatternName.Text = result.ImporterSetting.Name;
                    txtInitialDirectory.Text = result.ImporterSetting.InitialDirectory;
                    nmbStartLineCount.Text = result.ImporterSetting.StartLineCount.ToString();
                    updateAt = result.ImporterSetting.UpdateAt;
                    ImporterSettingId = result.ImporterSetting.Id;
                    btnPatternNoSearch.Enabled = false;
                    if (result.ImporterSetting.PostAction == 0)
                    {
                        rdoNoAction.Checked = true;
                    }
                    else if (result.ImporterSetting.PostAction == 1)
                    {
                        rdoDelete.Checked = true;
                    }
                    else if (result.ImporterSetting.PostAction == 2)
                    {
                        rdoAddDate.Checked = true;
                    }
                    if (result.ImporterSetting.IgnoreLastLine == 1)
                    {
                        cbxIgnoreLastLine.Checked = true;
                    }
                    else if (result.ImporterSetting.IgnoreLastLine == 0)
                    {
                        cbxIgnoreLastLine.Checked = false;
                    }
                    await SetDataGridViewAsync();
                    BaseContext.SetFunction03Enabled(true);
                    Modified = false;
                    ClearStatusMessage();
                }
                else
                {
                    BaseContext.SetFunction08Enabled(true);
                    await SetDataGridViewAsync();
                    btnPatternNoSearch.Enabled = false;
                    ClearStatusMessage();
                    DispStatusMessage(MsgInfNewData, "パターンNo.");
                    Modified = true;
                }
                this.ActiveControl = txtPatternName;
                txtPatternName.Focus();
            }
        }

        private async Task SetDataGridViewAsync()
        {
            if (string.IsNullOrEmpty(txtPatternNumber.Text)) return;
            if (code == "")
            {
                code = txtPatternNumber.Text;
            }
            var comboYear = new List<Setting>();
            var comboToroku = new List<Setting>();
            var comboExternalCode = new List<Setting>();

            ImporterSettingDetailsResult result = null;
            await ServiceProxyFactory.DoAsync<ImporterSettingServiceClient>(async client
                => result = await client.GetDetailByCodeAsync(SessionKey, CompanyId, FormatId, code));
            Action<ComboBoxCell, List<Setting>> comboboxCellSetter = (cmb, items) =>
            {
                if (items.Count > 0)
                {
                    cmb.DataSource = items.Select(x => new { x.ItemKey, x.ItemValue }).ToArray();
                    cmb.DisplayMember = nameof(Setting.ItemValue);
                    cmb.ValueMember = nameof(Setting.ItemKey);
                }
            };
            await ServiceProxyFactory.DoAsync<SettingMasterClient>(async client =>
            {
                var settingListResult = await client.GetItemsAsync(SessionKey, new string[] { "ATTR1", "ATTR3", "ATTR6" });
                comboYear = settingListResult.Settings.Where(x => x.ItemId == "ATTR1").ToList();
                comboToroku = settingListResult.Settings.Where(x => x.ItemId == "ATTR3").ToList();
                comboExternalCode = settingListResult.Settings.Where(x => x.ItemId == "ATTR6").ToList();
            });
            code = "";
            if (result.ImporterSettingDetails.Count == 0) return;
            var detailCurrencyCode = result.ImporterSettingDetails.FirstOrDefault(d => d.Sequence == (int)Fields.CurrencyCode);

            grid.CellEditedFormattedValueChanged -= grid_CellEditedFormattedValueChanged;
            grid.CellValueChanged -= grid_CellValueChanged;
            Invoke(new System.Action(() =>
            {
                ComboBoxCell comboAttribute = null;
                grid.RowCount = result.ImporterSettingDetails.Count;
                for (int i = 0; i < result.ImporterSettingDetails.Count; i++)
                {
                    var detail = result.ImporterSettingDetails[i];
                    var field = (Fields)detail.Sequence;
                    var row = grid.Rows[i];
                    row.Cells[CellIndexRowHeader].Value = i + 1;
                    row.Cells[CellIndexFieldName].Value = detail.FieldName;
                    row.Cells[CellIndexImportDivision].Value = detail.ImportDivision;
                    row.Cells[CellIndexCaption].Value = detail.Caption;
                    row.Cells[CellIndexSequence].Value = detail.Sequence;

                    row.Cells[CellIndexImportDivision].Enabled = IsImportDivisionEnabled(field);
                    comboAttribute = (ComboBoxCell)row.Cells[CellIndexAttribute];
                    var fieldIndexEnabled = IsFieldIndexEnabled(detail);
                    row.Cells[CellIndexFieldIndex].Enabled = fieldIndexEnabled;
                    row.Cells[CellIndexCaption].Enabled = fieldIndexEnabled;
                    if (fieldIndexEnabled && detail.FieldIndex > 0)
                    {
                        row.Cells[CellIndexFieldIndex].Value = detail.FieldIndex;
                    }

                    var priorityEnabled = IsItemPriorityEnabled(detail);
                    row.Cells[CellIndexPriority].Enabled = priorityEnabled;
                    row.Cells[CellIndexAssendingOrder].Enabled = priorityEnabled;
                    if (priorityEnabled && detail.FieldIndex > 0)
                    {
                        row.Cells[CellIndexPriority].Value = detail.FieldIndex;
                        row.Cells[CellIndexAssendingOrder].Value = detail.ItemPriority;
                    }
                    row.Cells[CellIndexAttribute].Enabled = IsAttributeEnabled(detail);
                    if (IsDateAttribute(field))
                    {
                        comboboxCellSetter(comboAttribute, comboYear);
                        comboAttribute.Value = detail.AttributeDivision;
                    }
                    if (IsStringAttribute(field))
                    {
                        comboboxCellSetter(comboAttribute, comboToroku);
                        comboAttribute.Value = detail.AttributeDivision;
                    }
                    if(IsExternalCodeAttribute(field))
                    {
                        comboboxCellSetter(comboAttribute, comboExternalCode);
                        comboAttribute.Value = detail.AttributeDivision;
                    }
                }

                if (UseForeignCurrency)
                {
                    var rowIndex = GetGridRowIndex(Fields.CurrencyCode);
                    if (rowIndex != -1)
                    {
                        grid.Rows[rowIndex].Cells[CellIndexImportDivision].Value = true;
                        grid.Rows[rowIndex].Cells[CellIndexImportDivision].Enabled = false;
                        grid.Rows[rowIndex].Cells[CellIndexFieldIndex].Enabled = true;
                        if (detailCurrencyCode.FieldIndex == 0)
                        {
                            grid.Rows[rowIndex].Cells[CellIndexFieldIndex].Value = null;
                        }
                        else
                        {
                            grid.Rows[rowIndex].Cells[CellIndexFieldIndex].Value = detailCurrencyCode.FieldIndex;
                        }
                        grid.Rows[rowIndex].Cells[CellIndexCaption].Value = detailCurrencyCode.Caption;
                        grid.Rows[rowIndex].Cells[CellIndexCaption].Enabled = true;
                        grid.Rows[rowIndex].Cells[CellIndexPriority].Enabled = false;
                        grid.Rows[rowIndex].Cells[CellIndexAssendingOrder].Enabled = false;
                        grid.Rows[rowIndex].Cells[CellIndexAttribute].Enabled = false;
                    }
                }
            }));
            grid.CellEditedFormattedValueChanged += grid_CellEditedFormattedValueChanged;
            grid.CellValueChanged += grid_CellValueChanged;

            ColumnNameSettingsResult colResult = null;
            await ServiceProxyFactory.DoAsync<ColumnNameSettingMasterClient>(async client
                => colResult = await client.GetItemsAsync(Login.SessionKey, Login.CompanyId));
            if (colResult.ProcessResult.Result)
            {
                var names = colResult.ColumnNames.Where(x => x.TableName == nameof(Billing)).ToList();
                if (names.Count > 0)
                {
                    foreach (var setting in names)
                    {
                        var rowIndex = -1;
                        switch (setting.ColumnName)
                        {
                            case nameof(Billing.Note1): rowIndex = GetGridRowIndex(Fields.Note1); break;
                            case nameof(Billing.Note2): rowIndex = GetGridRowIndex(Fields.Note2); break;
                            case nameof(Billing.Note3): rowIndex = GetGridRowIndex(Fields.Note3); break;
                            case nameof(Billing.Note4): rowIndex = GetGridRowIndex(Fields.Note4); break;
                            case nameof(Billing.Note5): rowIndex = GetGridRowIndex(Fields.Note5); break;
                            case nameof(Billing.Note6): rowIndex = GetGridRowIndex(Fields.Note6); break;
                            case nameof(Billing.Note7): rowIndex = GetGridRowIndex(Fields.Note7); break;
                            case nameof(Billing.Note8): rowIndex = GetGridRowIndex(Fields.Note8); break;
                            default: continue;
                        }
                        if (rowIndex != -1)
                            grid.Rows[rowIndex].Cells[CellIndexFieldName].Value
                                = setting.DisplayColumnName;

                    }
                }
            }
        }


        [OperationLog("削除")]
        private void Delete()
        {
            try
            {
                if (!ValidateForDelete()) return;
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

        private bool ValidateForDelete()
        {
            if (!txtPatternNumber.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblPatternNumber.Text))) return false;
            var task = ServiceProxyFactory.DoAsync(async (TaskScheduleMasterClient client)
                => await client.ExistsAsync(SessionKey, CompanyId, ImportType, ImporterSettingId));
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            var checkResult = task.Result;
            if (checkResult.Exist)
            {
                ShowWarningDialog(MsgWngDeleteConstraint, "タイムスケジューラー", "取込パターン");
                return false;
            }
            return true;
        }

        private async Task DeleteImportSettingAsync()
        {

            var result = await ServiceProxyFactory.DoAsync(async (ImporterSettingServiceClient client)
                => await client.DeleteAsync(SessionKey, ImporterSettingId));

            Clear();
            if (result.Count > 0)
            {
                this.ActiveControl = txtPatternNumber;
                txtPatternNumber.Focus();
                DispStatusMessage(MsgInfDeleteSuccess);
            }
            else
            {
                ShowWarningDialog(MsgErrDeleteError);
            }
        }

        #region event handlers

        private void txtPatternNo_Validated(object sender, EventArgs e)
        {
            try
            {
                ProgressDialog.Start(ParentForm, ShowAllDataAsync(), false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var import = this.ShowScheduledPaymentImporterSettingSearchDialog();
            try
            {
                if (import != null)
                {
                    txtPatternNumber.Text = import.Code;
                    txtPatternName.Text = import.Name;
                    txtPatternNumber.Enabled = false;
                    ProgressDialog.Start(ParentForm, ShowAllDataAsync(), false, SessionKey);
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

        #region 入力項目変更イベント処理
        private void AddHandlers()
        {
            txtPatternNumber.TextChanged += new EventHandler(OnContentChanged);
            txtPatternName.TextChanged += new EventHandler(OnContentChanged);

            foreach (Control control in gbxImportFileConfig.Controls)
            {
                if (control is CheckBox)
                {
                    ((CheckBox)control).CheckedChanged +=
                        new EventHandler(OnContentChanged);
                }
                if (control is Common.Controls.VOneTextControl)
                {
                    control.TextChanged += new EventHandler(OnContentChanged);
                }
                if (control is Common.Controls.VOneNumberControl)
                {
                    control.TextChanged += new EventHandler(OnContentChanged);
                }
            }
            foreach (Control control in gbxActiion.Controls)
            {
                if (control is RadioButton)
                {
                    ((RadioButton)control).CheckedChanged +=
                        new EventHandler(OnContentChanged);
                }
            }
        }

        private void OnContentChanged(object sender, EventArgs e)
        {
            if (!Modified)
            {
                Modified = true;
            }
        }

        #endregion

        #region grid event handlers

        private void grid_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            Modified = true;
            if (e.RowIndex < 0 || e.CellIndex != CellIndexImportDivision) return;
            var checkBoxStatus = Convert.ToBoolean(grid.CurrentCell.EditedFormattedValue);
            var row = grid.Rows[e.RowIndex];
            var field = ConvertRowToFields(row);
            if (!IsImportDivisionEnabled(field)) return;
            if (checkBoxStatus)
            {
                row.Cells[CellIndexFieldIndex].Enabled = true;
                row.Cells[CellIndexCaption].Enabled = true;
                row.Cells[CellIndexAttribute].Enabled = IsAttributeEnabled(field);
                row.Cells[CellIndexPriority].Enabled = false;
                row.Cells[CellIndexPriority].Value = null;
                row.Cells[CellIndexAssendingOrder].Enabled = false;
                row.Cells[CellIndexAssendingOrder].Value = null;
            }
            else
            {
                row.Cells[CellIndexFieldIndex].Enabled = false;
                row.Cells[CellIndexCaption].Enabled = false;
                row.Cells[CellIndexFieldIndex].Value = null;
                row.Cells[CellIndexCaption].Value = "";
                row.Cells[CellIndexAttribute].Enabled = false;
                row.Cells[CellIndexAttribute].Value = null;
                row.Cells[CellIndexPriority].Enabled = true;
                row.Cells[CellIndexAssendingOrder].Enabled = true;
            }
        }

        private void grid_CellValueChanged(object sender, CellEventArgs e)
        {
            if (e.RowIndex < 0 || e.CellIndex != CellIndexImportDivision) return;
            var row = grid.Rows[e.RowIndex];
            var field = ConvertRowToFields(row);
            if (!IsImportDivisionEnabled(field)) return;
            var value = row.GcMultiRow.GetValue(e.RowIndex, CellIndexImportDivision);
            if (value.ToString() == "1")
            {
                row.Cells[CellIndexFieldIndex].Enabled = true;
                row.Cells[CellIndexCaption].Enabled = true;
                row.Cells[CellIndexAttribute].Enabled = IsAttributeEnabled(field);
                row.Cells[CellIndexPriority].Enabled = false;
                row.Cells[CellIndexPriority].Value = null;
                row.Cells[CellIndexAssendingOrder].Enabled = false;
                row.Cells[CellIndexAssendingOrder].Value = null;
            }
            else
            {
                row.Cells[CellIndexFieldIndex].Enabled = false;
                row.Cells[CellIndexFieldIndex].Value = null;
                row.Cells[CellIndexCaption].Enabled = false;
                row.Cells[CellIndexCaption].Value = "";
                row.Cells[CellIndexPriority].Enabled = true;
                row.Cells[CellIndexAssendingOrder].Enabled = true;
                row.Cells[CellIndexAttribute].Enabled = false;
                row.Cells[CellIndexAttribute].Value = null;
            }
        }

        #endregion

        #endregion

        #region grid helper
        private int GetGridRowIndex(Fields field)
            => grid.Rows.FirstOrDefault(x => ConvertRowToFields(x) == field)?.Index ?? -1;

        private Fields ConvertRowToFields(Row row)
            => (Fields)int.Parse(row.Cells[CellIndexSequence].Value?.ToString());


        /// <summary>
        /// 取込必須項目
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool IsImportRequired(Fields field)
            => field == Fields.ReceiptAmount
            || field == Fields.ScheduledPaymentKey
            ;

        /// <summary>
        /// 取込 無視項目
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool IsImportIgnored(Fields field)
            => field == Fields.CompanyCode;

        /// <summary>
        /// 照合対象 除外項目
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool IsCollationIgnored(Fields field)
            => field == Fields.TaxAmount
            || field == Fields.StaffCode
            ;

        /// <summary>
        /// キー チェックボックスの有効無効
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool IsImportDivisionEnabled(Fields field)
            => !(IsImportRequired(field)
              || IsImportIgnored(field)
              || IsCollationIgnored(field)
            );

        /// <summary>
        /// 項目番号 有効無効
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool IsFieldIndexEnabled(Fields field)
            => !(IsImportIgnored(field)
              || IsCollationIgnored(field)
            );

        /// <summary>
        /// 項目番号 有効無効 項目名（注釈）も連動
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        private bool IsFieldIndexEnabled(ImporterSettingDetail detail)
            => IsFieldIndexEnabled((Fields)detail.Sequence)
            && (IsImportRequired((Fields)detail.Sequence)
                || detail.ImportDivision == 1 )
            ;

        /// <summary>
        /// 優先順位 項目間 有効無効 項目内 も連動
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        private bool IsItemPriorityEnabled(ImporterSettingDetail detail)
            => !IsImportRequired((Fields)detail.Sequence)
            && !IsImportIgnored((Fields)detail.Sequence)
            && detail.ImportDivision == 0
            ;

        private bool IsDateAttribute(Fields field)
            => field == Fields.BilledAt
            || field == Fields.DueAt
            || field == Fields.SalesAt
            || field == Fields.ClosingAt
            ;

        private bool IsStringAttribute(Fields field)
            => field == Fields.InvoiceCode
            || field == Fields.Note1
            || field == Fields.Note2
            || field == Fields.Note3
            || field == Fields.Note4
            || field == Fields.Note5
            || field == Fields.Note6
            || field == Fields.Note7
            || field == Fields.Note8
            || field == Fields.ScheduledPaymentKey
            ;

        private bool IsExternalCodeAttribute(Fields field)
            => field == Fields.BillingCategoryCode
            ;

        /// <summary>
        /// 属性区分 有効項目
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool IsAttributeEnabled(Fields field)
            => IsDateAttribute(field)
            || IsStringAttribute(field)
            || IsExternalCodeAttribute(field)
            ;

        private bool IsAttributeEnabled(ImporterSettingDetail detail)
            => IsAttributeEnabled((Fields)detail.Sequence)
            && (IsImportRequired((Fields)detail.Sequence)
             || detail.ImportDivision == 1)
            ;

        #endregion

    }
}