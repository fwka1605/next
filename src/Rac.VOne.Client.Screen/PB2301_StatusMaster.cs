using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Screen.StatusMasterService;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>ステータスマスター</summary>
    public partial class PB2301 : VOneScreenBase
    {
        #region 変数宣言
        private int StatusId { get; set; }
        private string CellName(string value) => $"cel{value}";

        private const int StatusType = (int)Rac.VOne.Common.Constants.StatusType.Reminder;

        #endregion
        public PB2301()
        {
            InitializeComponent();
            grdStatus.SetupShortcutKeys();
            Text = "ステータスマスター";
            AddHandlers();
        }

        #region 画面の初期化
        private void PB2301_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();

                var loadTask = new List<Task>();
                if (ApplicationControl == null)
                    loadTask.Add(LoadApplicationControlAsync());
                if (Company == null)
                    loadTask.Add(LoadCompanyAsync());

                loadTask.Add(LoadControlColorAsync());

                Task<List<Status>> statusListTask = GetStatusListAsync();
                loadTask.Add(statusListTask);

                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                grdStatus.DataSource = new BindingSource(statusListTask.Result, null);

                SetFormat();
                InitializeGrid();
                Clear();
            }
            catch(Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void SetFormat()
        {
            txtStatusCode.PaddingChar = '0';
        }

        private void InitializeGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "Header"                                                           , cell: builder.GetRowHeaderCell()),
                new CellSetting(height,  90, nameof(Status.Code)        , dataField: nameof(Status.Code)        , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), caption: "ステータスコード"),
                new CellSetting(height, 380, nameof(Status.Name)        , dataField: nameof(Status.Name)        , cell: builder.GetTextBoxCell() , caption: "ステータス名称"),
                new CellSetting(height,  50, nameof(Status.DisplayOrder), dataField: nameof(Status.DisplayOrder), cell: builder.GetNumberCell() , caption: "表示順番"),
                new CellSetting(height,   0, nameof(Status.Id)          , dataField: nameof(Status.Id)          , visible: false )
            });

            grdStatus.Template = builder.Build();
            grdStatus.SetRowColor(ColorContext);
            grdStatus.HideSelection = true;
            grdStatus.CurrentCellBorderLine = new Line(LineStyle.None, Color.Empty);
            grdStatus.CurrentRowBorderLine = new Line(LineStyle.None, Color.Empty);

        }
        #endregion

        #region ファンクションキー初期化
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
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = Delete;

            BaseContext.SetFunction10Caption("終了");
            OnF10ClickHandler = Exit;
        }
        #endregion

        #region F1 登録処理
        [OperationLog("登録")]
        private void Save()
        {
            ClearStatusMessage();
            if (!RequireFieldsChecking()) return;

            ZeroLeftPaddingWithoutValidated();

            if (!ShowConfirmDialog(MsgQstConfirmSave))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }

            try
            {
                Task<bool> task = SaveStatus();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                if (task.Result)
                {
                    DispStatusMessage(MsgInfSaveSuccess);
                    Modified = false;
                    txtStatusCode.Focus();
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

        private bool RequireFieldsChecking()
        {
            if (string.IsNullOrWhiteSpace(txtStatusCode.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "ステータスコード");
                txtStatusCode.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtStatusName.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "ステータス名称");
                txtStatusName.Focus();
                return false;
            }
            return true;
        }

        private void ZeroLeftPaddingWithoutValidated()
        {
            if (IsNeedValidate(0, txtStatusCode.TextLength, txtStatusCode.MaxLength))
            {
                txtStatusCode.Text = ZeroLeftPadding(txtStatusCode);
                txtStatusCode_Validated(null, null);
            }
        }

        private async Task<bool> SaveStatus()
        {
            var result = false;
            try
            {
                var status = PrepareStatusData();
                var webResult = await SaveStatusAsync(status);
                if (webResult.ProcessResult.Result)
                {
                    var statuses = await GetStatusListAsync();
                    grdStatus.DataSource = new BindingSource(statuses, null);
                    Clear();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            return result;
        }

        private Status PrepareStatusData()
        {
            decimal displayOrder = nmbDisplayOrder.Value ?? 0M;
            var status = new Status();
            status.CompanyId = Login.CompanyId;
            status.StatusType = StatusType;
            status.Code = txtStatusCode.Text;
            status.Name = txtStatusName.Text;
            status.Completed = 0;
            status.DisplayOrder = Decimal.ToInt32(displayOrder);
            status.CreateBy = Login.UserId;
            status.UpdateBy = Login.UserId;

            return status;
        }

        #endregion

        #region F2 クリア処理
        [OperationLog("クリア")]
        private void ConfirmToClear()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear))
                return;

            Clear();
            ClearStatusMessage();
        }

        private void Clear()
        {
            txtStatusCode.Clear();
            txtStatusName.Clear();
            nmbDisplayOrder.Clear();
            txtStatusCode.Enabled = true;
            BaseContext.SetFunction03Enabled(false);
            Modified = false;
            StatusId = 0;
            txtStatusCode.Focus();
        }
        #endregion

        #region F3 削除処理
        [OperationLog("削除")]
        private void Delete()
        {
            if (!ValidateForDelete()) return;

            if (!ShowConfirmDialog(MsgQstConfirmDelete))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            try
            {
                ProgressDialog.Start(ParentForm, DeleteStatus(), false, SessionKey);
                txtStatusCode.Focus();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private bool ValidateForDelete()
        {
            if (txtStatusCode.Text == "00" || txtStatusCode.Text == "99")
            {
                ShowWarningDialog(MsgWngDefaultStatusAndCannotDelete);
                return false;
            }

            var valid = true;
            System.Action messaging = null;

            var task = Task.Run(async () =>
            {
                var reminderExist = await ExistReminderAsync(StatusId);
                if (reminderExist.Exist)
                {
                    messaging = () => ShowWarningDialog(MsgWngDeleteConstraint, "督促データ", lblStatusCode.Text);
                    valid = false;
                    return;
                }
                var reminderHistoryExist = await ExistReminderHistoryAsync(StatusId);
                if (reminderHistoryExist.Exist)
                {
                    messaging = () => ShowWarningDialog(MsgWngDeleteConstraint, "督促履歴", lblStatusCode.Text);
                    valid = false;
                    return;
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (!valid)
                messaging?.Invoke();

            return valid;
        }

        private async Task DeleteStatus()
        {
            CountResult result = await DeleteStatusAsync(StatusId);

            if (result.Count > 0)
            {
                var statuses = await GetStatusListAsync();
                grdStatus.DataSource = new BindingSource(statuses, null);
                Clear();
                DispStatusMessage(MsgInfDeleteSuccess);
                Modified = false;
            }
            else
            {
                ShowWarningDialog(MsgErrDeleteError);
            }
        }
        #endregion

        #region F10 終了処理
        [OperationLog("終了")]
        private void Exit()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose))
                return;
            ParentForm.Close();
        }
        #endregion

        #region  入力項目変更イベント処理
        private void AddHandlers()
        {
            foreach (Control control in gbxStatusInput.Controls)
            {
                if (control is Common.Controls.VOneTextControl)
                    control.TextChanged += new EventHandler(OnContentChanged);
                if (control is Common.Controls.VOneNumberControl)
                    control.TextChanged += new EventHandler(OnContentChanged);
            }
        }

        private void OnContentChanged(object sender, EventArgs e)
        {
            if (!Modified)
                Modified = true;
        }
        #endregion

        #region grdStatus_CellDoubleClick
        private void grdStatus_CellDoubleClick(object sender, CellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (Modified && !ShowConfirmDialog(MsgQstConfirmUpdateData))
                    return;

                BaseContext.SetFunction03Enabled(true);
                txtStatusCode.Enabled = false;
                txtStatusCode.Text = grdStatus.Rows[e.RowIndex].Cells[CellName(nameof(Status.Code))].DisplayText;
                txtStatusName.Text = grdStatus.Rows[e.RowIndex].Cells[CellName(nameof(Status.Name))].DisplayText;
                nmbDisplayOrder.Value = Convert.ToInt32(grdStatus.Rows[e.RowIndex].Cells[CellName(nameof(Status.DisplayOrder))].DisplayText);
                StatusId = Convert.ToInt32(grdStatus.Rows[e.RowIndex].Cells[CellName(nameof(Status.Id))].Value);

                ClearStatusMessage();
                Modified = false;
            }
        }
        #endregion

        #region Validated処理
        private void txtStatusCode_Validated(object sender, EventArgs e)
        {
            string code = txtStatusCode.Text;
            if (string.IsNullOrWhiteSpace(code))
                return;

            try
            {
                ProgressDialog.Start(ParentForm, LoadStatusAsync(code), false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region 共通
        private async Task LoadStatusAsync(string code)
        {
            ClearStatusMessage();
            var status = await GetStatusAsync(code);

            if (status == null)
            {
                StatusId = 0;
                DispStatusMessage(MsgInfSaveNewData, "ステータス");
                Modified = true;
                return;
            }

            BaseContext.SetFunction03Enabled(true);
            txtStatusCode.Enabled = false;
            StatusId = status.Id;
            txtStatusName.Text = status.Name;
            nmbDisplayOrder.Value = status.DisplayOrder;
            Modified = false;
        }
        #endregion

        #region WebService
        private async Task<Status> GetStatusAsync(string code)
            => await ServiceProxyFactory.DoAsync(async (StatusMasterClient client) =>
            {
                StatusResult result = null;
                result = await client.GetStatusByCodeAsync(SessionKey, CompanyId, StatusType, code);
                return result.Status;
            });

        private async Task<List<Status>> GetStatusListAsync()
            => await ServiceProxyFactory.DoAsync(async (StatusMasterClient client) =>
            {
                StatusesResult results = null;
                results = await client.GetStatusesByStatusTypeAsync(SessionKey, CompanyId, StatusType);
                return results.Statuses;
            });

        private async Task<StatusResult> SaveStatusAsync(Status status)
            => await ServiceProxyFactory.DoAsync(async (StatusMasterClient client) =>
            {
                StatusResult result = null;
                result = await client.SaveAsync(SessionKey, status);
                return result;
            });

        private async Task<CountResult> DeleteStatusAsync(int Id)
            => await ServiceProxyFactory.DoAsync(async (StatusMasterClient client) =>
            {
                CountResult result = null;
                result = await client.DeleteAsync(SessionKey, Id);
                return result;
            });

        private async Task<ExistResult> ExistReminderAsync(int Id)
            => await ServiceProxyFactory.DoAsync(async (StatusMasterClient client) =>
            {
                ExistResult result = null;
                result = await client.ExistReminderAsync(SessionKey, Id);
                return result;
            });

        private async Task<ExistResult> ExistReminderHistoryAsync(int Id)
            => await ServiceProxyFactory.DoAsync(async (StatusMasterClient client) =>
            {
                ExistResult result = null;
                result = await client.ExistReminderHistoryAsync(SessionKey, Id);
                return result;
            });
        #endregion
    }
}
