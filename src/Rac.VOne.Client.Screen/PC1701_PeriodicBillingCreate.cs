using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Screen.PeriodicBillingService;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using static Rac.VOne.Common.Constants;
using static Rac.VOne.Message.Constants;
using Header = Rac.VOne.Web.Models.PeriodicBillingSetting;

namespace Rac.VOne.Client.Screen
{
    /// <summary> 定期請求データ登録 /// </summary>
    public partial class PC1701 : VOneScreenBase
    {
        private DateTime BaseDate { get; set; }
        private bool IsChecked(Row row) => (row.DataBoundItem as Header)?.Selected ?? false;
        private bool IsGridModifiedConfirm { get { return grid.Rows.Any(x => !IsChecked(x)); } }
        private bool IsGridModifiedReConfirm { get { return grid.Rows.Any(x => IsChecked(x)); } }
        private string CellName(string value) => $"cel{value}";

        #region 画面の初期化

        public PC1701()
        {
            InitializeComponent();
            grid.SetupShortcutKeys();
            Text = "定期請求データ登録";
        }

        protected override void InitLayout()
        {
            base.InitLayout();
            if (ParentForm == null) return;
            ParentForm.Shown += (sender, e) => {
                var today = DateTime.Today;
                BaseDate = new DateTime(today.Year, today.Month, 1);
                datBaseDate.Value = BaseDate;
            };
        }

        private void PC1701_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                ProgressDialog.Start(ParentForm, InitializeLoadDataAsync(), false, SessionKey);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }


        private async Task InitializeLoadDataAsync()
        {
            await Task.WhenAll(
                LoadApplicationControlAsync(),
                LoadCompanyAsync(),
                LoadControlColorAsync());
        }

        private void InitializeGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var wdh = cbxReCreate.Checked ? 100 : 0;
            var center = MultiRowContentAlignment.MiddleCenter;

            builder.Items.AddRange(new[]
            {
                new CellSetting(height,  40, "Header"                                                                           , cell: builder.GetRowHeaderCell()),
                new CellSetting(height,  40, nameof(Header.Selected)            , dataField: nameof(Header.Selected)            , cell: builder.GetCheckBoxCell(isBoolType: true), caption: "選択", readOnly : false),
                new CellSetting(height, wdh, nameof(Header.LastUpdateAt)        , dataField: nameof(Header.LastUpdateAt)        , cell: builder.GetDateCell_yyyyMMdd()           , caption: "登録日", sortable: true),
                new CellSetting(height, 120, nameof(Header.Name)                , dataField: nameof(Header.Name)                , cell: builder.GetTextBoxCell()                 , caption: "パターン名", sortable: true),
                new CellSetting(height,  80, nameof(Header.CustomerCode)        , dataField: nameof(Header.CustomerCode)        , cell: builder.GetTextBoxCell(center)           , caption: "得意先コード", sortable: true),
                new CellSetting(height, 120, nameof(Header.CustomerName)        , dataField: nameof(Header.CustomerName)        , cell: builder.GetTextBoxCell()                 , caption: "得意先名", sortable: true),
                new CellSetting(height,  80, nameof(Header.DestinationCode)     , dataField: nameof(Header.DestinationCode)     , cell: builder.GetTextBoxCell(center)           , caption: "送付先コード", sortable: true),
                new CellSetting(height, 120, nameof(Header.Addressee)           , dataField: nameof(Header.Addressee)           , cell: builder.GetTextBoxCell()                 , caption: "宛名", sortable: true),
                new CellSetting(height, 100, nameof(Header.BillingAmount)       , dataField: nameof(Header.BillingAmount)       , cell: builder.GetTextBoxCurrencyCell()         , caption: "請求金額", sortable: true),
                new CellSetting(height,  80, nameof(Header.BilledAt)            , dataField: nameof(Header.BilledAt)            , cell: builder.GetDateCell_yyyyMMdd()           , caption: "請求日", sortable: true),
                new CellSetting(height, 120, nameof(Header.CollectCategoryName) , dataField: nameof(Header.CollectCategoryName) , cell: builder.GetTextBoxCell()                 , caption: "回収区分", sortable: true),
                new CellSetting(height,  80, nameof(Header.DepartmentCode)      , dataField: nameof(Header.DepartmentCode)      , cell: builder.GetTextBoxCell(center)           , caption: "部門コード", sortable: true),
                new CellSetting(height, 120, nameof(Header.DepartmentName)      , dataField: nameof(Header.DepartmentName)      , cell: builder.GetTextBoxCell()                 , caption: "請求部門", sortable: true),
                new CellSetting(height,  80, nameof(Header.StaffCode)           , dataField: nameof(Header.StaffCode)           , cell: builder.GetTextBoxCell(center)           , caption: "担当者コード", sortable: true),
                new CellSetting(height, 120, nameof(Header.StaffName)           , dataField: nameof(Header.StaffName)           , cell: builder.GetTextBoxCell()                 , caption: "担当者名", sortable: true),
                new CellSetting(height, wdh, nameof(Header.LastUpdatedBy)       , dataField: nameof(Header.LastUpdatedBy)       , cell: builder.GetTextBoxCell()                 , caption: "作成者", sortable: true),
            });

            grid.Template = builder.Build();
            grid.SetRowColor(ColorContext);
            grid.HideSelection = true;
            grid.CurrentCellBorderLine = new Line(LineStyle.None, Color.Empty);
            grid.CurrentRowBorderLine = new Line(LineStyle.None, Color.Empty);
            grid.AllowAutoExtend = false;
        }

        private bool LoadPeriodicBillingSettings()
        {
            ClearStatusMessage();
            try
            {
                grid.DataSource = null;
                var recreate = cbxReCreate.Checked;

                var task = GetSettingsAsync(new PeriodicBillingSettingSearch
                {
                    CompanyId = CompanyId,
                    ReCreate = recreate,
                    BaseDate = BaseDate,
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                var settings = task.Result.PeriodicBillingSettings;

                if (!task.Result.ProcessResult.Result || !settings.Any())
                    return false;

                grid.DataSource = new BindingSource(settings, null);

                BaseContext.SetFunction01Enabled(!recreate);
                BaseContext.SetFunction02Enabled(recreate);
                BaseContext.SetFunction08Enabled(true);
                BaseContext.SetFunction09Enabled(true);

                SelectFlagOnOff(!recreate); // 仕組み自体どうにかした方がいいんじゃないの？

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                return false;
            }
            return true;
        }

        #endregion

        #region ファンクションキー初期化
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("請求登録");
            BaseContext.SetFunction02Caption("再登録");
            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction08Caption("全選択");
            BaseContext.SetFunction09Caption("全解除");
            BaseContext.SetFunction10Caption("終了");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);

            OnF01ClickHandler = Create;
            OnF02ClickHandler = ReCreate;
            OnF08ClickHandler = CheckAll;
            OnF09ClickHandler = UnCheckAll;
            OnF10ClickHandler = Exit;
        }
        #endregion

        #region F1 請求登録処理
        [OperationLog("請求登録")]
        private void Create()
        {
            if (!ValidateInputValues()) return;
            ClearStatusMessage();
            try
            {
                var settings = GetSelectedSettings();
                if (!settings.Any())
                {
                    ShowWarningDialog(MsgWngNotExistUpdateData, "登録するデータ");
                    return;
                }

                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var task = CreateBillingsAsync(settings);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (task.Exception != null)
                {
                    NLogHandler.WriteErrorLog(this, task.Exception, SessionKey);
                    DispStatusMessage(MsgErrSaveError);
                    return;
                }

                if (!task.Result)
                {
                    DispStatusMessage(MsgErrSaveError);
                    return;
                }

                LoadPeriodicBillingSettings();
                datBaseDate.Focus();

                DispStatusMessage(MsgInfSaveSuccess);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private bool ValidateInputValues()
        {
            if (!datBaseDate.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblPeriodicDate.Text))) return false;
            return true;
        }

        private List<Header> GetSelectedSettings()
            => grid.Rows
            .Select(x => x.DataBoundItem as Header)
            .Where(x => x.Selected)
            .Select(x => {
                x.UpdateBy = Login.UserId;
                return x;
            })
            .ToList();

        #endregion

        #region F2 再登録処理
        [OperationLog("再登録")]
        private void ReCreate()
        {
            Create();
        }
        #endregion

        #region F8 全選択処理
        [OperationLog("全選択")]
        private void CheckAll()
            => SelectFlagOnOff(isChecked: true);

        #endregion

        #region F9 全解除処理
        [OperationLog("全解除")]
        private void UnCheckAll()
            => SelectFlagOnOff(isChecked: false);

        #endregion

        #region F10 終了処理
        [OperationLog("終了")]
        private void Exit()
        {
            var isEdit = (!cbxReCreate.Checked && IsGridModifiedConfirm) ||
                         (cbxReCreate.Checked && IsGridModifiedReConfirm);
            if (isEdit && !ShowConfirmDialog(MsgQstConfirmClose))
                return;
            ParentForm.Close();
        }
        #endregion

        #region 再登録のCheckedChangedイベント
        private void cbxReConfirm_CheckedChanged(object sender, EventArgs e)
        {
            if (!ValidateInputValues()) return;
            InitializeGrid();
            if (!LoadPeriodicBillingSettings())
            {
                BaseContext.SetFunction01Enabled(false);
                BaseContext.SetFunction02Enabled(false);
                ShowWarningDialog(MsgWngNotExistSearchData);
            }
        }
        #endregion

        #region 処理年月のValueChangedイベント
        private void datBaseDate_ValueChanged(object sender, EventArgs e)
        {
            if (!datBaseDate.Value.HasValue) return;

            BaseDate = datBaseDate.Value.Value;

            InitializeGrid();
            if (!LoadPeriodicBillingSettings())
            {
                BaseContext.SetFunction01Enabled(false);
                BaseContext.SetFunction02Enabled(false);
                ShowWarningDialog(MsgWngNotExistSearchData);
            }
        }
        #endregion

        #region グリッドイベント
        private void grid_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            grid.CommitEdit();
        }

        private void grid_CellValueChanged(object sender, CellEventArgs e)
        {
        }

        #endregion

        #region 共通

        private void SelectFlagOnOff(bool isChecked)
        {
            ClearStatusMessage();
            grid.EndEdit();
            grid.SuspendLayout();
            foreach (var header in grid.Rows.Select(x => x.DataBoundItem as Header))
                header.Selected = isChecked;
            grid.ResumeLayout();
            grid.Focus();
        }

        #endregion

        #region WebService
        private async Task<PeriodicBillingSettingsResult> GetSettingsAsync(PeriodicBillingSettingSearch option)
            => await ServiceProxyFactory.DoAsync(async (PeriodicBillingSettingMasterService.PeriodicBillingSettingMasterClient client) =>
            {
                var result = await client.GetItemsAsync(SessionKey, option);
                if (result.ProcessResult.Result && result.PeriodicBillingSettings.Any())
                    foreach (var setting in result.PeriodicBillingSettings)
                        setting.InitializeForGetBilling(BaseDate);

                return result;
            });

        private async Task<bool> CreateBillingsAsync(IEnumerable<Header> settings)
            => await ServiceProxyFactory.DoAsync(async (PeriodicBillingServiceClient client)
                => (await client.CreateAsync(SessionKey, settings.ToArray()))?.ProcessResult.Result ?? false);

        #endregion
    }
}
