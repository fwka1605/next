using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.BillingBalanceService;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Screen.Dialogs;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary> 残高メンテナンス処理 </summary>
    public partial class PH0301 : VOneScreenBase
    {
        public PH0301()
        {
            InitializeComponent();
            Text = "残高メンテナンス処理";
        }

        #region 初期表示

        private void PH0301_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();

                var loadTask = new List<Task>();

                loadTask.Add(LoadControlColorAsync());

                if (Company == null)
                    loadTask.Add(LoadCompanyAsync());

                if (ApplicationControl == null)
                    loadTask.Add(LoadApplicationControlAsync());

                loadTask.Add(LoadLastCarryOverAt());

                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
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

            BaseContext.SetFunction01Caption("更新");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = UpdateManageBalance;

            BaseContext.SetFunction02Caption("戻し");
            BaseContext.SetFunction02Enabled(false);
            OnF02ClickHandler = RestoreManageBalance;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Caption("");
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Caption("");
            BaseContext.SetFunction05Enabled(false);
            BaseContext.SetFunction06Caption("");
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Caption("");
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Caption("");
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Caption("");
            BaseContext.SetFunction09Enabled(false);

            BaseContext.SetFunction10Caption("終了");
            OnF10ClickHandler = ExitManageBalance;
        }

        #endregion

        #region ファンクションキーイベント

        [OperationLog("更新")]
        private void UpdateManageBalance()
        {
            try
            {
                ClearStatusMessage();

                if (!RequireCheck() || !ShowConfirmDialog(MsgQstConfirmBalanceCarryForward))
                    return;

                SaveManageBalance();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("戻し")]
        private void RestoreManageBalance()
        {
            try
            {
                ClearStatusMessage();

                if (!ShowConfirmDialog(MsgQstConfirmBalanceCarryBackward))
                    return;

                CancelManageBalance();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private bool RequireCheck()
        {
            if (!datCarryOverAt.Value.HasValue)
            {
                ShowWarningDialog(MsgWngInputRequired, lblCarryOverAt.Text);
                datCarryOverAt.Focus();
                return false;
            }

            if(datLastCarryOverAt.Value.HasValue && datLastCarryOverAt.Value.Value.CompareTo(datCarryOverAt.Value.Value) >= 0)
            {
                ShowWarningDialog(MsgWngNotExistUpdateData, "繰越年月日は前回繰越日よりも後の日付");
                datCarryOverAt.Focus();
                return false;
            }

            return true;
        }

        [OperationLog("終了")]
        private void ExitManageBalance()
        {
            ParentForm.Close();
        }

        #endregion

        #region Web Service

        private async Task LoadLastCarryOverAt()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BillingBalanceServiceClient>();
                var result = await service.GetLastCarryOverAtAsync(SessionKey, CompanyId);

                if (result.LastCarryOverAt.HasValue)
                {
                    datLastCarryOverAt.Value = result.LastCarryOverAt;
                    BaseContext.SetFunction02Enabled(true);
                }
                else
                {
                    BaseContext.SetFunction02Enabled(false);
                    datLastCarryOverAt.Clear();
                }
                datCarryOverAt.Clear();
            });
        }

        private void SaveManageBalance()
        {
            try
            {
                BillingBalancesResult result = null;
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<BillingBalanceServiceClient>();
                    result = await service.SaveAsync(SessionKey, CompanyId, datCarryOverAt.Value.Value.Date, Login.UserId);
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (result.ProcessResult.Result && result.BillingBalances.Count > 0)
                {
                    datLastCarryOverAt.Value = datCarryOverAt.Value;
                    BaseContext.SetFunction02Enabled(true);
                    datCarryOverAt.Clear();
                    datCarryOverAt.Focus();
                    DispStatusMessage(MsgInfFinishBalanceCarryForward);
                }
                else if (result.ProcessResult.Result && result.BillingBalances.Count == 0)
                {
                    ShowWarningDialog(MsgWngNoData, "更新する");
                }
                else
                {
                    ShowWarningDialog(MsgErrBalanceCarryForwardError);
                }
            }
            catch
            {
                ShowWarningDialog(MsgErrBalanceCarryForwardError);
                throw;
            }
        }

        private void CancelManageBalance()
        {
            try
            {
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<BillingBalanceServiceClient>();
                    var result = await service.CancelAsync(SessionKey, CompanyId);

                    if (result.ProcessResult.Result)
                    {
                        await LoadLastCarryOverAt();
                        DispStatusMessage(MsgInfReturnCarryForwardBalance);
                    }
                    else
                    {
                        ShowWarningDialog(MsgErrBalanceCarryBackwardError);
                    }
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                datCarryOverAt.Focus();
            }
            catch
            {
                ShowWarningDialog(MsgErrBalanceCarryBackwardError);
                throw;
            }
        }
        #endregion
    }
}
