using GrapeCity.Win.MultiRow;
using GrapeCity.Win.MultiRow.InputMan;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.ReportSettingMasterService;
using Rac.VOne.Client.Screen.SettingMasterService;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;
using static Rac.VOne.Client.Reports.Settings.PF0501;

namespace Rac.VOne.Client.Screen
{
    /// <summary>
    /// 帳票設定（出力設定）
    /// </summary>
    public partial class PH9905 : VOneScreenBase
    {
        #region 変数宣言

        public string FormName { get; set; }
        public bool SettingSaved { get; set; }
        private bool FromCustomerLedger { get; set; }
        private bool FromCollectionSchedule { get; set; }
        private List<ReportSetting> ReportSettingList { get; set; }
        private List<Setting> SettingList { get; set; }

        private const string ReportDueCommentName = "入金期日コメント";
        private const string ReportDueCommentValue = "期日は[YMD]です。";
        private static readonly Color ColorInformation = Color.FromArgb(-4128769);
        private static readonly Color ColorError = Color.FromArgb(-128);
        #endregion

        public PH9905()
        {
            InitializeComponent();
            InitializeUserComponent();
        }

        private void InitializeUserComponent()
        {
            grid.SetupShortcutKeys();
            Text = "帳票設定";
            ReportSettingList = new List<ReportSetting>();
            SettingList = new List<Setting>();

            FormWidth = 400;
            FormHeight = 460;
            FunctionKeysSetter = buttons =>
            {
                foreach (var button in buttons)
                {
                    if (button.Name == "btnF01"
                    || button.Name == "btnF02")
                        button.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                    else if (button.Name == "btnF10")
                        button.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    else
                        button.Visible = false;
                }
            };
        }

        protected override void InitLayout()
        {
            base.InitLayout();
            if (ParentForm == null) return;
            ParentForm.Load += PH9905_Load;
        }

        #region ファンクションキー設定

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Save;

            BaseContext.SetFunction02Caption("再表示");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = Reload;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = null;

            BaseContext.SetFunction10Caption("戻る");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Exit;

        }
        #endregion

        #region フォームロード
        private void PH9905_Load(object sender, EventArgs e)
        {
            try
            {
                FromCustomerLedger = FormName == nameof(PF0501);
                FromCollectionSchedule = FormName == nameof(PF0601);

                ProgressDialog.Start(ParentForm, InitializeLoadDataAsync(), false, SessionKey);

                SetOrderedList();
                InitializeGridTemplate();
                SetGridTemplate();
                ClearStatusMessage();
                Modified = false;

                grid.CellValueChanged += grdSearchSettingData_CellValueChanged;
                grid.CurrentCellDirtyStateChanged += grdSearchSettingData_CurrentCellDirtyStateChanged;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private async Task InitializeLoadDataAsync()
        {
            var tasks = new List<Task> {
                LoadCompanyAsync(),
                LoadApplicationControlAsync(),
                LoadControlColorAsync()
            };
            await Task.WhenAll(tasks);

            var result = await LoadListAsync();
            ReportSettingList.AddRange(result);

            var itemIds = ReportSettingList
                .GroupBy(r => r.ItemId).Select(g => g.Key)
                .ToArray();
            SettingList = await LoadSettingListAsync(itemIds);
        }

        #endregion

        #region 項目変更処理
        private void grdSearchSettingData_CellValueChanged(object sender, CellEventArgs e)
        {
            Modified = true;
        }

        private void grdSearchSettingData_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            grid.CommitEdit(DataErrorContexts.Commit);
        }
        #endregion

        #region 帳票設定リストの取得処理
        private async Task<List<ReportSetting>> LoadListAsync()
        {
            List<ReportSetting> list = null;
            await ServiceProxyFactory.DoAsync(async (ReportSettingMasterClient client) =>
            {
                var result = await client.GetItemsAsync(SessionKey, CompanyId, FormName);
                if (result.ProcessResult.Result)
                    ReportSettingList = result.ReportSettings;
            });

            return list ?? new List<ReportSetting>();
        }
        #endregion

        #region 設定リストの取得処理
        private async Task<List<Setting>> LoadSettingListAsync(string[] itemIds)
        {
            List<Setting> list = null;
            await ServiceProxyFactory.DoAsync< SettingMasterClient>(async client =>
            {
                var result = await client.GetItemsAsync(SessionKey, itemIds);
                if (result.ProcessResult.Result)
                    list = result.Settings;
            });

            return list ?? new List<Setting>();
        }
        #endregion

        #region リストの整理
        private void SetOrderedList()
        {
            var groupSetting = SettingList
                .GroupBy(s => s.ItemId)
                .ToDictionary(s => s.Key, s => s.ToList());

            for (var i = 0; i < ReportSettingList.Count; i++)
            {
                var key = ReportSettingList[i].ItemId;

                if (!groupSetting.ContainsKey(key))
                    continue;

                var setting = groupSetting.First(gs => gs.Key == key).Value;
                ReportSettingList[i].SettingList = setting;
            }
        }
        #endregion

        #region グリッド初期設定
        public void InitializeGridTemplate()
        {
            var widthSettingName = FormName == nameof(PC0401) ? 180 : 150;
            var widthSetting = FormName == nameof(PC0401) ? 100 : 189;

            var template = new Template();

            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height, widthSettingName, "Caption"  , caption: "設定名" ),
                new CellSetting(height, widthSetting, "ItemValue", caption: "設定値" )
            });

            builder.BuildHeaderOnly(template);
            builder.Items.Clear();

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height, widthSettingName, "Caption"       , cell: builder.GetTextBoxCell()),
                new CellSetting(height, widthSetting, "ItemValueCombo", cell: builder.GetComboBoxCell(), readOnly: false )
            });

            builder.BuildRowOnly(template);
            var newCell = builder.GetTextBoxCell(maxLength: 50, ime: ImeMode.Hiragana);
            newCell.Size = new Size(widthSetting, height);
            newCell.Name = "ItemValueText";
            newCell.Location = new Point(widthSettingName, 0);
            newCell.Style.Border = new Border(new GrapeCity.Win.MultiRow.Line(LineStyle.Thin, Color.Black));
            template.Row.Cells.Add(newCell);
            grid.Template = template;
            //grid.RestoreValue = true;
        }
        #endregion

        #region グリッドデータ設定
        public void SetGridTemplate()
        {
            if (UseForeignCurrency)
                ReportSettingList = ReportSettingList.Where(x => x.ItemId != "ReportUnitPrice").ToList();

            if (FromCustomerLedger && !UseSection)
                ReportSettingList = ReportSettingList.Where(x => x.DisplayOrder != DisplaySectionCode).ToList();

            grid.RowCount = ReportSettingList.Count;

            for (var i = 0; i < ReportSettingList.Count; i++)
            {
                var reportSetting = new ReportSetting();
                reportSetting = ReportSettingList[i];

                var settingList = new List<Setting>();
                settingList = reportSetting.SettingList;

                var textCellName = (GcTextBoxCell)grid.Rows[i].Cells[0];
                textCellName.Value = reportSetting.Caption;

                if (reportSetting.IsText != 1)
                {
                    var comboCellValue = (ComboBoxCell)grid.Rows[i].Cells[1];
                    comboCellValue.Items.Clear();

                    comboCellValue.DataSource = settingList;
                    comboCellValue.ValueMember = "ItemKey";
                    comboCellValue.DisplayMember = "ItemValue";
                    comboCellValue.Value = reportSetting.ItemKey;
                    grid.Rows[i].Cells[2].Visible = false;
                }
                else
                {
                    var textCellValue = (GcTextBoxCell)grid.Rows[i].Cells[2];

                    if (reportSetting.Caption == ReportDueCommentName)
                        textCellValue.Value = !string.IsNullOrEmpty(reportSetting.ItemKey)
                            ? reportSetting.ItemKey : ReportDueCommentValue;
                    else
                        textCellValue.Value = reportSetting.ItemKey;

                    grid.Rows[i].Cells[1].Visible = false;
                }
            }
        }
        #endregion

        #region 帳票設定リスト設定（保存用）
        public void SetReportSettingInfo()
        {
            for (var i = 0; i < ReportSettingList.Count; i++)
            {
                var reportSetting = new ReportSetting();
                reportSetting = ReportSettingList[i];

                if (reportSetting.IsText != 1)
                {
                    var comboCell = (ComboBoxCell)grid.Rows[i].Cells[1];
                    reportSetting.ItemKey = comboCell.Value.ToString();
                    reportSetting.ItemValue = comboCell.DisplayText.ToString();
                }
                else
                {
                    var textCell = (GcTextBoxCell)grid.Rows[i].Cells[2];

                    if (textCell.Value == null || textCell.Value == DBNull.Value
                    || string.IsNullOrEmpty(textCell.Value.ToString()))
                        reportSetting.ItemKey = string.Empty;
                    else
                        reportSetting.ItemKey = textCell.Value.ToString();
                }
                ReportSettingList[i] = reportSetting;
            }
        }
        #endregion

        #region 入力チェック
        public bool ValidateInput()
        {
            SetReportSettingInfo();

            var totalByDayNone = ReportSettingList.Exists(
                x => x.Caption == "請求データ集計"
                && x.ItemKey == "2"
                && x.ItemValue == "しない");

            var totalOnly = ReportSettingList.Exists(
                x => x.Caption == "伝票集計方法"
                && x.ItemKey == "0"
                && x.ItemValue == "合計");

            if (FromCustomerLedger && totalByDayNone && totalOnly)
            {
                ShowWarningDialog(MsgWngNotAllowedSheetSummaryWhenBillingSummary);
                return false;
            }

            var doPageBreak = ReportSettingList.Exists(
                x => x.Caption == "請求部門ごと改ページ"
                && x.ItemKey == "1"
                && x.ItemValue == "する");

            var hideDepartment = ReportSettingList.Exists(
                x => x.Caption == "請求部門ごと表示"
                && x.ItemKey == "0"
                && x.ItemValue == "しない");

            if (FromCollectionSchedule && doPageBreak && hideDepartment)
            {
                ShowWarningDialog(MsgWngNotAllowedDeptPageBreakeWhenNotDisplay);
                return false;
            }

            return true;
        }
        #endregion

        #region 保存処理
        [OperationLog("登録")]
        public void Save()
        {
            if (!ShowConfirmDialog(MsgQstConfirmSave))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }

            if (ReportSettingList.Count <= 0 || !ValidateInput())
                return;

            try
            {
                ReportSettingsResult result = null;
                var reportSetting = ReportSettingList.ToArray();
                ProgressDialog.Start(ParentForm, Task.Run(async () =>
                {
                    result = await ServiceProxyFactory.DoAsync(async (ReportSettingMasterClient client)
                        => await client.SaveAsync(SessionKey, reportSetting));
                }), false, SessionKey);

                if (result.ProcessResult.Result)
                {
                    DispStatusMessage(MsgInfSaveSuccess);
                    Modified = false;
                    SettingSaved = true;
                }
                else
                {
                    ShowWarningDialog(MsgErrSaveError);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region 再表示処理
        [OperationLog("再表示")]
        public void Reload()
        {
            ClearStatusMessage();

            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear)) return;

            ReloadAll();
            Modified = false;
        }
        #endregion

        #region 全項目初期化処理
        public void ReloadAll()
        {
            for (var i = 0; i < ReportSettingList.Count; i++)
            {
                var reportSetting = new ReportSetting();
                reportSetting = ReportSettingList[i];

                if (reportSetting.IsText != 1)
                {
                    var comboCell = (ComboBoxCell)grid.Rows[i].Cells[1];
                    comboCell.Value = reportSetting.ItemKey;
                }
                else
                {
                    var textCell = (GcTextBoxCell)grid.Rows[i].Cells[2];
                    textCell.Value = reportSetting.ItemKey;
                }
            }
        }
        #endregion

        #region 戻る処理
        [OperationLog("戻る")]
        public void Exit()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;

            ParentForm.DialogResult = DialogResult.Cancel;
        }

        #endregion
    }
}
