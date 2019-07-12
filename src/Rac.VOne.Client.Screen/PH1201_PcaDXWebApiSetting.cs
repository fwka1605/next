using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.HttpClients;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.WebApiSettingMasterService;
using Rac.VOne.Web.Models;
using Rac.VOne.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>PCA会計DX Web API 連携設定</summary>
    public partial class PH1201 : VOneScreenBase
    {
        internal VOneScreenBase ParentScreen { private get; set; }

        private readonly PcaWebApiClient client;
        private WebApiSetting Setting {
            get { return client.WebApiSetting; }
            set { client.WebApiSetting = value; }
        }
        #region initialize

        public PH1201()
        {
            InitializeComponent();
            client = new PcaWebApiClient();
            InitializeUserComponent();
            InitializeHandlers();
        }

        private void InitializeUserComponent()
        {
            Text = "PCA会計DX Web API 連携設定";
            client.ErrorListener = SetResponseMessage;
        }

        private void InitializeHandlers()
        {
            Load += PH1201_Load;
            txtClientId     .TextChanged += OnTextChanged;
            txtClientSecret .TextChanged += OnTextChanged;
            txtBaseUri      .TextChanged += OnTextChanged;
            txtApiVersion   .TextChanged += OnTextChanged;
            txtAccessToken  .TextChanged += OnTextChanged;
            txtRefreshToken .TextChanged += OnTextChanged;

        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();
            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction02Caption("再表示");
            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction04Caption("再認証");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(true);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction10Enabled(true);

            OnF01ClickHandler = Save;
            OnF02ClickHandler = Reload;
            OnF03ClickHandler = Delete;
            OnF04ClickHandler = Authorize;
            OnF10ClickHandler = Close;
        }

        private void PH1201_Load(object sender, EventArgs e)
        {
            SetScreenName();
            ProgressDialog.Start(ParentForm, InitializeLoadAsync(), false, SessionKey);
        }

        private async Task InitializeLoadAsync()
        {
            var tasks = new List<Task> {
                LoadCompanyAsync(),
                LoadApplicationControlAsync(),
                LoadControlColorAsync(),
                LoadWebApiSettingsAsync()
            };
            await Task.WhenAll(tasks);
        }

        #endregion


        #region function keys

        #region save
        [OperationLog("登録")]
        private void Save()
        {
            ClearStatusMessage();
            if (!ValidateInputValues()) return;
            if (!AuthorizeInner())
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            if (!LoadPcaCompanyInfo())
            {
                DispStatusMessage(MsgErrSaveError);
                return;
            }
            if (!ShowConfirmDialog(MsgQstConfirmSave))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            var saveTask = SaveAsync(Setting);
            ProgressDialog.Start(ParentForm, saveTask, false, SessionKey);
            if (saveTask.Exception != null)
            {
                NLogHandler.WriteErrorLog(this, saveTask.Exception, SessionKey);
                ShowWarningDialog(MsgErrSaveError);
                return;
            }
            if (!saveTask.Result)
            {
                ShowWarningDialog(MsgErrSaveError);
                return;
            }
            Modified = false;
            client.TokenRefreshed = false;
            if (ParentScreen is PE0901)
            {
                ParentForm.DialogResult = DialogResult.OK;
            }
            DispStatusMessage(MsgInfSaveSuccess);
        }

        #endregion

        #region reload

        [OperationLog("再表示")]
        private void Reload()
        {
            ClearStatusMessage();
            if (Modified && !ShowConfirmDialog(MsgQstConfirmUpdateData)) return;
            ProgressDialog.Start(ParentForm, LoadWebApiSettingsAsync(), false, SessionKey);
        }

        #endregion

        #region delete

        [OperationLog("削除")]
        private void Delete()
        {
            ClearStatusMessage();
            if (!ShowConfirmDialog(MsgQstConfirmDelete))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            var deleteTask = DeleteAsync();
            ProgressDialog.Start(ParentForm, deleteTask, false, SessionKey);
            if (deleteTask.Exception != null
                || !deleteTask.Result)
            {
                if (deleteTask.Exception != null)
                    NLogHandler.WriteErrorLog(this, deleteTask.Exception, SessionKey);
                ShowWarningDialog(MsgErrDeleteError);
                return;
            }
            ClearControlValues();
            DispStatusMessage(MsgInfDeleteSuccess);
        }

        #endregion

        #region authorie

        [OperationLog("再認証")]
        private void Authorize()
        {
            ClearStatusMessage();
            if (!ShowConfirmDialog(MsgQstConfirmStartXXX, "再認証"))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            if (!AuthorizeInner(force: true))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            if (!LoadPcaCompanyInfo())
            {
                DispStatusMessage(MsgErrSomethingError, "再認証");
                return;
            }
            var saveTask = SaveAsync(Setting);
            ProgressDialog.Start(ParentForm, saveTask, false, SessionKey);
            if (saveTask.Exception != null
                || !saveTask.Result)
            {
                if (saveTask.Exception != null)
                    NLogHandler.WriteErrorLog(this, saveTask.Exception, SessionKey);
                ShowWarningDialog(MsgErrSaveError);
                return;
            }
            if (ParentScreen is PE0901)
            {
                ParentForm.DialogResult = DialogResult.OK;
            }
            Modified = false;
            client.TokenRefreshed = false;
        }

        #endregion

        #region close

        [OperationLog("終了")]
        private void Close()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;
            ClearStatusMessage();
            ParentForm.Close();
        }

        #endregion

        #endregion

        #region validation

        private bool ValidateInputValues()
        {
            if (!txtClientId    .ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblClientId      .Text))) return false;
            if (!txtClientSecret.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblClientSecret  .Text))) return false;
            if (!txtBaseUri     .ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblBaseUri       .Text))) return false;
            if (!txtApiVersion  .ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblApiVersion    .Text))) return false;
            return true;
        }

        #endregion

        #region clear control values

        private void ClearControlValues()
        {
            txtClientId.Clear();
            txtClientSecret.Clear();
            txtBaseUri.Clear();
            txtApiVersion.Text = "v1";
            txtAccessToken.Clear();
            txtRefreshToken.Clear();
            txtResponse.Clear();
            BaseContext.SetFunction04Enabled(false);
            Modified = false;
        }

        #endregion

        #region other methods

        private void SetSetting()
        {
            if (Setting == null) Setting = new WebApiSetting {
                CompanyId           = CompanyId,
                ApiTypeId           = WebApiType.PcaDX,
                CreateBy            = Login.UserId,
            };
            Setting.UpdateBy        = Login.UserId;
            Setting.ClientId        = txtClientId       .Text;
            Setting.ClientSecret    = txtClientSecret   .Text;
            Setting.BaseUri         = txtBaseUri        .Text;
            Setting.ApiVersion      = txtApiVersion     .Text;
            Setting.AccessToken     = txtAccessToken    .Text;
            Setting.RefreshToken    = txtRefreshToken   .Text;
        }

        private void SetResponseMessage(string message)
        {
            Action textSetter = () => txtResponse.Text = message;
            if (txtResponse.InvokeRequired)
                txtResponse.BeginInvoke(textSetter);
            else
                textSetter();
        }



        #endregion

        #region call web services

        private async Task LoadWebApiSettingsAsync()
        {
            try
            {
                ClearControlValues();
                var result = await ServiceProxyFactory.DoAsync(async (WebApiSettingMasterClient client)
                    => await client.GetByIdAsync(SessionKey, CompanyId, WebApiType.PcaDX));
                if (!(result?.ProcessResult.Result ?? false)
                    || result.WebApiSetting == null)
                {
                    BaseContext.SetFunction04Enabled(false);
                    return;
                }
                BaseContext.SetFunction04Enabled(true);
                Setting = result.WebApiSetting;
                txtClientId.Text = Setting.ClientId;
                txtClientSecret.Text = Setting.ClientSecret;
                txtBaseUri.Text = Setting.BaseUri;
                txtApiVersion.Text = Setting.ApiVersion;
                SetToken();
            }
            finally
            {
                Modified = false;
            }
        }

        private void SetToken()
        {
            txtAccessToken.Text = Setting.AccessToken;
            txtRefreshToken.Text = Setting.RefreshToken;
        }

        private async Task<bool> SaveAsync(WebApiSetting setting)
            => ((await ServiceProxyFactory.DoAsync(async (WebApiSettingMasterClient client)
                => await client.SaveAsync(SessionKey, setting)))
            ?.ProcessResult.Result ?? false);

        private async Task<bool> DeleteAsync()
            => ((await ServiceProxyFactory.DoAsync(async (WebApiSettingMasterClient client)
                => await client.DeleteAsync(SessionKey, CompanyId, WebApiType.PcaDX)))
            ?.ProcessResult.Result ?? false);

        #endregion

        #region pca web api calles

        private bool AuthorizeInner(bool force = false)
        {
            SetSetting();
            if (!force && !IsAuthorizationRequired) return true;
            var task = client.AuthorizeAsync();
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            if (task.Result)
            {
                SetToken();
            }
            return task.Result;
        }

        private bool IsAuthorizationRequired
            => string.IsNullOrEmpty(Setting?.AccessToken)
            && string.IsNullOrEmpty(Setting?.RefreshToken);

        private bool LoadPcaCompanyInfo()
        {
            var companyInfo = string.Empty;
            var task = Task.Run(async () => {
                var selected = await client.SelectDataAreaAsync(SelectAreaForm.SelectByGUI);
                if (!selected) return;
                var comp = await client.GetCompAsync();
                companyInfo = comp.GetCompanyInfo();
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            if (task.Exception != null)
            {
                NLogHandler.WriteErrorLog(this, task.Exception, SessionKey);
                return false;
            }
            if (string.IsNullOrEmpty(companyInfo)) return false;
            if (client.TokenRefreshed) SetToken();
            SetResponseMessage(companyInfo);
            return true;
        }


        #endregion

        #region event handlers

        private void OnTextChanged(object sender, EventArgs e) => Modified = true;

        #endregion
    }
}
