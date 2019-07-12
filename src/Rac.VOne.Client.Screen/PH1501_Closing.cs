using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Screen.ClosingService;
using Rac.VOne.Client.Screen.ClosingSettingMasterService;
using Rac.VOne.Client.Screen.ReceiptService;
using GrapeCity.Win.MultiRow;
using static Rac.VOne.Web.Models.ClosingSetting;
using static Rac.VOne.Message.Constants;
using static Rac.VOne.Client.Screen.UtilClosing;

namespace Rac.VOne.Client.Screen
{
    /// <summary>
    /// 締め処理
    /// </summary>
    public partial class PH1501 : VOneScreenBase
    {
        #region メンバー
        private byte[] ClientKey { get; set; }
        private string CellName(string value) => $"cel{value}";
        private static class FKeyNames
        {
            internal const string F01 = "更新";
            internal const string F02 = "クリア";
            internal const string F03 = "取消";
            internal const string F10 = "終了";
        }
        #endregion

        #region 初期化
        public PH1501()
        {
            InitializeComponent();
            InitializeHandlers();
            Text = "締め処理";
        }
        private void InitializeHandlers()
        {
            Load += PH1501_Load;
            grid.CellValueChanged += grid_CellValueChanged;
            grid.DataError += grid_DataError;
            grid.CellEditedFormattedValueChanged += grid_CellEditedFormattedValueChanged;
            grid.DataBindingComplete += (sender, e) => ClearControlValues();
            grid.CellDoubleClick += grid_CellDoubleClick;
        }
        private void PH1501_Load(object sender, EventArgs e)
        {
            try
            {
                var tasks = new List<Task>();
                if (ApplicationControl == null)
                    tasks.Add(LoadApplicationControlAsync());
                if (Company == null)
                    tasks.Add(LoadCompanyAsync());
                tasks.Add(LoadControlColorAsync());
                tasks.Add(LoadClientKeyAsync());
               
                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks.ToArray()), false, SessionKey);

                SetScreenName();
                grid.SetupShortcutKeys();
                InitializeGridTemplate();
                datLastClosingMonth.Enabled = false;
                ProgressDialog.Start(ParentForm, BindingGridDataSource(), false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private async Task BindingGridDataSource()
        {
            grid.DataSource = await GetClosingHistorysAsync();
        }
        private void SetClosingSetting(ClosingSetting setting)
        {
            if (setting == null) setting = new ClosingSetting();
            rdoBilledAt.Checked = setting.BaseDate == BaseDateValues.BilledAt;
            rdoSalesAt.Checked = setting.BaseDate == BaseDateValues.SalesAt;
            rdoAllowJournalPending.Checked = setting.AllowReceptJournalPending;
            rdoNoAllowJournalPending.Checked = !setting.AllowReceptJournalPending;
            rdoAllowMutchingPending.Checked = setting.AllowMutchingPending;
            rdoNoAllowMutching.Checked = !setting.AllowMutchingPending;
        }
        private void InitializeGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            builder.AutoLocationSet = false;
            var height = builder.DefaultRowHeight;

            const int w1 = 50;
            const int w2 = 100;
            const int w3 = 50;
            const int w4 = 147;
            const int w5 = 146;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height, w1     , "a", caption: string.Empty, location:new Point(0           , 0)),
                new CellSetting(height, w2     , "b", caption: string.Empty, location:new Point(w1          , 0)),
                new CellSetting(height, w3     , "c", caption: string.Empty, location:new Point(w1 + w2     , 0)),
                new CellSetting(height, w4 + w5, "d", caption: "発生件数"  , location:new Point(w1 + w2 + w3, 0)),

                new CellSetting(height, w1, nameof(ClosingHistory.Selected)       , caption: "取消"    , location: new Point(0                , height)),
                new CellSetting(height, w2, nameof(ClosingHistory.ClosingMonth)   , caption: "処理年月", location: new Point(w1               , height)),
                new CellSetting(height, w3, nameof(ClosingHistory.IsClosedDisplay), caption: "締め"    , location: new Point(w1 + w2          , height)),
                new CellSetting(height, w4, nameof(ClosingHistory.BillingCount)   , caption: "請求"    , location: new Point(w1 + w2 + w3     , height)),
                new CellSetting(height, w5, nameof(ClosingHistory.ReceiptCount)   , caption: "入金"    , location: new Point(w1 + w2 + w3 + w4, height)),
            });

            var template = new Template();

            builder.BuildHeaderOnly(template);
            builder.Items.Clear();

            builder.Items.AddRange(new CellSetting[]
           {
               new CellSetting(height, w1, nameof(ClosingHistory.Selected)       , dataField: nameof(ClosingHistory.Selected)       , caption: "取消"    , location: new Point(0                , 0), cell: builder.GetCheckBoxCell(isBoolType:true), readOnly: false, enabled: false),
               new CellSetting(height, w2, nameof(ClosingHistory.ClosingMonth)   , dataField: nameof(ClosingHistory.ClosingMonth)   , caption: "処理年月", location: new Point(w1               , 0), cell: builder.GetDateCell_yyyyMM() ),
               new CellSetting(height, w3, nameof(ClosingHistory.IsClosedDisplay), dataField: nameof(ClosingHistory.IsClosedDisplay), caption: "締め"    , location: new Point(w1 + w2          , 0), cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter) ),
               new CellSetting(height, w4, nameof(ClosingHistory.BillingCount)   , dataField: nameof(ClosingHistory.BillingCount)   , caption: "請求"    , location: new Point(w1 + w2 + w3     , 0), cell: builder.GetNumberCell()  ),
               new CellSetting(height, w5, nameof(ClosingHistory.ReceiptCount)   , dataField: nameof(ClosingHistory.ReceiptCount)   , caption: "入金"    , location: new Point(w1 + w2 + w3 + w4, 0), cell: builder.GetNumberCell()  ),
           });

            builder.BuildRowOnly(template);

            grid.Template = template;
            grid.HideSelection = true;
            grid.AllowUserToResize = true;
            grid.SetupShortcutKeys();
            grid.AllowAutoExtend = false;
            grid.HorizontalScrollBarMode = ScrollBarMode.Automatic;
        }
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            OnF01ClickHandler = Save;
            OnF02ClickHandler = Clear;

            BaseContext.SetFunction03Caption(FKeyNames.F03);
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = Cancel;

            OnF10ClickHandler = Close;
        }
        #endregion

        #region ファンクションキー押下処理
        #region F01/更新

        [OperationLog(FKeyNames.F01)]
        private void Save()
        {
            try
            {
                ClearStatusMessage();
                if (!CheckForSave()) return;

                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                if (!datLastClosingMonth.Value.HasValue)
                {
                    var task = SaveClosingSetting();
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                    if (task == null)
                    {
                        ShowWarningDialog(MsgErrSomethingError, FKeyNames.F01);
                        return;
                    }
                    SetClosingSetting(task.Result);
                }

                var taskSave = SaveClosing(datClosingMonth.Value.Value);
                ProgressDialog.Start(ParentForm, taskSave, false, SessionKey);
                if (!taskSave.Result)
                {
                    ShowWarningDialog(MsgErrSomethingError, FKeyNames.F01);
                    return;
                }

                ProgressDialog.Start(ParentForm, BindingGridDataSource(), false, SessionKey);
                DispStatusMessage(MsgInfUpdateSuccess);
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrSomethingError, FKeyNames.F01);
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private bool CheckForSave()
        {
            if (!datClosingMonth.Value.HasValue)
            {
                ShowWarningDialog(MsgWngInputRequired, lblClosingMonth.Text);
                datClosingMonth.Focus();
                return false;
            }

            var items = grid.Rows.Select(x => x.DataBoundItem as ClosingHistory);
            var closing = datClosingMonth.Value.Value;

            var min = items.Min(x => x.ClosingMonth);
            if (IsOlderMonth(closing, min))
            {
                ShowWarningDialog(MsgWngClosingMonthIsOld, min.ToString("yyyy/MM"));
                datClosingMonth.Focus();
                return false;
            }


            if (items.Any(x => x.IsClosed && IsEqualingMonth(closing, x.ClosingMonth)))
            {
                ShowWarningDialog(MsgWngIsClosed, closing.ToString("yyyy/MM"));
                datClosingMonth.Focus();
                return false;
            }

            if (!items.Any(x => IsEqualingMonth(x.ClosingMonth, closing)))
            {
                ShowWarningDialog(MsgWngSelectedDataIsNotExists, $"{lblClosingMonth.Text}のデータ" );
                datClosingMonth.Focus();
                return false;
            }

            DateTime closingFrom = new DateTime();
            DateTime closingTo = new DateTime();
            GetClosingFromTo(ref closingFrom, ref closingTo);

            if (IsExistNonApportionedReceipt(closingFrom, closingTo).Exist)
            {
                ShowWarningDialog(MsgWngCommonExists, "未振分の入金データ");
                return false;
            }

            if (rdoNoAllowJournalPending.Checked
                && IsExistNonOutputedReceipt(closingFrom, closingTo).Exist)
            {
                ShowWarningDialog(MsgWngCommonExists, "入金仕訳が未出力の入金データ");
                return false;
            }

            if (rdoNoAllowMutching.Checked
                && IsExistNonAssignmentReceipt(closingFrom, closingTo).Exist)
            {
                ShowWarningDialog(MsgWngCommonExists, "未消込の入金データ");
                return false;
            }
            return true;
        }
        private bool IsEqualingMonth(DateTime value1, DateTime value2)
            => (value1.Year == value2.Year && value1.Month == value2.Month);
        private bool IsOlderMonth(DateTime older, DateTime later)
        {
            if (older.Year < later.Year) return true;
            if (older.Year > later.Year) return false;
            if (older.Month < later.Month) return true;
            if (older.Month > later.Month) return false;
            return false;
        }
        private void GetClosingFromTo(ref DateTime closingFrom, ref DateTime closingTo)
        {
            var closing = datClosingMonth.Value.Value;
            if (Company.ClosingDay == 99)
                closingTo = GetEndOfMonth(closing.Year, closing.Month);
            else
                closingTo = new DateTime(closing.Year, closing.Month, Company.ClosingDay);

            var min = grid.Rows.Select(x => x.DataBoundItem as ClosingHistory)
                .Where(x => !x.IsClosed)
                .Min(x => x.ClosingMonth);

            if (Company.ClosingDay == 99)
            {
                closingFrom = new DateTime(min.Year, min.Month, 1);
            }
            else
            {
                var tempDay = new DateTime(min.Year, min.Month, 1).AddMonths(-1);
                closingFrom = new DateTime(tempDay.Year, tempDay.Month, Company.ClosingDay).AddDays(1);
            }
        }

        private DateTime GetEndOfMonth(int year, int month)
            => new DateTime(year, month, 1).AddMonths(1).AddDays(-1.0);
        #endregion

        #region F02/クリア
        [OperationLog(FKeyNames.F02)]
        private void Clear()
        {
            ClearStatusMessage();
            ProgressDialog.Start(ParentForm, BindingGridDataSource(), false, SessionKey);
        }
        private void ClearControlValues()
        {
            var closing = GetClosingInformation(Login.SessionKey, Login.CompanyId).Closing;
            datLastClosingMonth.Value = closing?.ClosingMonth;
            datClosingMonth.Clear();

            var taskSetting = GetClosingSettingAsync();
            ProgressDialog.Start(ParentForm, taskSetting, false, SessionKey);
            SetClosingSetting(taskSetting.Result);
            gbxJournalizingPattern.Enabled = !datLastClosingMonth.Value.HasValue;
            SetClosing();

            if (!datLastClosingMonth.Value.HasValue) return;

            var lastClosing = datLastClosingMonth.Value.Value;
            foreach (var row in grid.Rows)
            {
                var closingMonth = (row.DataBoundItem as ClosingHistory).ClosingMonth;
                if (IsEqualingMonth(closingMonth, lastClosing)
                    || IsOlderMonth(closingMonth, lastClosing))
                {
                    row[CellName(nameof(ClosingHistory.Selected))].Enabled = true;
                }
            }
            grid.EndEdit();
            datClosingMonth.Focus();
        }
        #endregion

        #region F03/取消
        [OperationLog(FKeyNames.F03)]
        private void Cancel()
        {
            try
            {
                ClearStatusMessage();
                if (!ShowConfirmDialog(MsgQstConfirmForCancel))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var closingMonth = GetClosingMonthForCansel();
                Task<bool> task = null;
                if (closingMonth == null)
                    task = DeleteClosing();
                else
                    task = SaveClosing(closingMonth.Value);

                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                if (!task.Result)
                {
                    ShowWarningDialog(MsgErrSomethingError, FKeyNames.F03);
                    return;
                }
                ProgressDialog.Start(ParentForm, BindingGridDataSource(), false, SessionKey);

                DispStatusMessage(MsgInfUpdateSuccess);
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrSomethingError, FKeyNames.F03);
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private DateTime? GetClosingMonthForCansel()
        {
            var nonSelected = grid.Rows.Where(x => x[CellName(nameof(ClosingHistory.Selected))].Enabled == true)
                .Select(x => x.DataBoundItem as ClosingHistory)
                .Where(x => !x.Selected);
            if (nonSelected.Count() < 1) return null;
            return nonSelected.Select(x => x.ClosingMonth).Max();
        }
        #endregion

        #region F10/終了
        [OperationLog(FKeyNames.F10)]
        private void Close()
        {
            BaseForm.Close();
            return;
        }
        #endregion
        #endregion

        #region WebService
        private async Task LoadClientKeyAsync()
            => ClientKey = await Util.CreateClientKey(Login, nameof(PH1501));

        private async Task<List<ClosingHistory>> GetClosingHistorysAsync() =>
            await ServiceProxyFactory.DoAsync(async ( ClosingServiceClient client) =>
            {
                var result = await client.GetClosingHistoryAsync(Login.SessionKey, Login.CompanyId);
                if (result != null && result.ProcessResult.Result)
                    return result.ClosingHistorys;
                return null;
            });

        private async Task<ClosingSetting> GetClosingSettingAsync() =>
            await ServiceProxyFactory.DoAsync(async (ClosingSettingMasterClient client) =>
            {
                var result = await client.GetAsync(Login.SessionKey, Login.CompanyId);
                if (result != null && result.ProcessResult.Result)
                    return result.ClosingSetting;
                return null;
            });
         private async Task<bool> SaveClosing(DateTime closingMonth) =>
             await ServiceProxyFactory.DoAsync(async (ClosingServiceClient client) =>
             {
                 var closing = new Closing
                 {
                     CompanyId = Login.CompanyId,
                     ClosingMonth = closingMonth,
                     UpdateBy = Login.UserId,
                 };
                 var result = await client.SaveAsync(Login.SessionKey, closing);
                 if (result != null && result.ProcessResult.Result)
                     return true;
                 return false;
             });
        private async Task<bool> DeleteClosing() =>
            await ServiceProxyFactory.DoAsync(async (ClosingServiceClient client) =>
            {
                var result = await client.DeleteAsync(Login.SessionKey, Login.CompanyId);
                if (result != null 
                && result.ProcessResult.Result
                && result.Count > 0)
                    return true;
                return false;
            });
        private async Task<ClosingSetting> SaveClosingSetting() =>
             await ServiceProxyFactory.DoAsync(async (ClosingSettingMasterClient client) =>
             {
                 var setting = new ClosingSetting
                 {
                     CompanyId = Login.CompanyId,
                     BaseDate = rdoBilledAt.Checked ? BaseDateValues.BilledAt : BaseDateValues.SalesAt,
                     AllowReceptJournalPending = rdoAllowJournalPending.Checked,
                     AllowMutchingPending = rdoAllowMutchingPending.Checked,
                     UpdateBy = Login.UserId,
                 };
                 var result = await client.SaveAsync(Login.SessionKey, setting);
                 if (result != null && result.ProcessResult.Result)
                     return result.ClosingSetting;
                 return null;
             });

        private ExistResult IsExistNonApportionedReceipt(DateTime closingFrom, DateTime closingTo) =>
            ServiceProxyFactory.Do((ReceiptServiceClient client) =>
            {
                var result = client.ExistNonApportionedReceipt(Login.SessionKey,
                    Login.CompanyId,
                    closingFrom,
                    closingTo);
                return result;
            });

        private ExistResult IsExistNonOutputedReceipt(DateTime closingFrom, DateTime closingTo) =>
           ServiceProxyFactory.Do((ReceiptServiceClient client) =>
           {
               var result = client.ExistNonOutputedReceipt(Login.SessionKey,
                   Login.CompanyId,
                   closingFrom,
                   closingTo);
               return result;
           });

        private ExistResult IsExistNonAssignmentReceipt(DateTime closingFrom, DateTime closingTo) =>
           ServiceProxyFactory.Do((ReceiptServiceClient client) =>
           {
               var result = client.ExistNonAssignmentReceipt(Login.SessionKey,
                   Login.CompanyId,
                   closingFrom,
                   closingTo);
               return result;
           });
        #endregion

        #region イベントハンドラー
        private void grid_DataError(object sender, DataErrorEventArgs e)
        {
            Debug.Fail(e.Exception.Message);
        }
        private void grid_CellValueChanged(object sender, CellEventArgs e)
        {
            if (e.CellName != CellName(nameof(ClosingHistory.Selected))) return;

            var selectedItem = grid.Rows[e.RowIndex].DataBoundItem as ClosingHistory;

            foreach (var row in grid.Rows)
            {
                if (row[CellName(nameof(ClosingHistory.Selected))].Enabled == false)
                    continue;

                var item = row.DataBoundItem as ClosingHistory;
                if ((item.ClosingMonth > selectedItem.ClosingMonth)
                    == selectedItem.Selected)
                    item.Selected = selectedItem.Selected;

            }
            grid.EndEdit();

            if(grid.Rows.Select(x => x.DataBoundItem as ClosingHistory)
                .Any(x => x.Selected))
                BaseContext.SetFunction03Enabled(true);
            else
                BaseContext.SetFunction03Enabled(false);

        }
        private void grid_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            if (e.CellName != CellName(nameof(ClosingHistory.Selected))) return;
            grid.EndEdit();
        }
        private void grid_CellDoubleClick(object sender, CellEventArgs e)
        {
            ClearStatusMessage();
            if (e.RowIndex < 0) return;
            var item = grid.Rows[e.RowIndex].DataBoundItem as ClosingHistory;
            if (item.IsClosed)
            {
                datClosingMonth.Clear();
                return;
            }
            datClosingMonth.Value = item.ClosingMonth;
        }
        #endregion
    }
}
