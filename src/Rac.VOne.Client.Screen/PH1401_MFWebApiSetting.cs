using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.WebApiSettingMasterService;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.HttpClients;
using Rac.VOne.Common;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary> MFクラウド請求書 Web API 連携設定 </summary>
    public partial class PH1401 : VOneScreenBase
    {
        private readonly MFWebApiClient client;
        private WebApiSetting Setting
        {
            get { return client.WebApiSetting; }
            set { client.WebApiSetting = value; }
        }
        private string AccessToken { get; set; }
        private string RefreshToken { get; set; }

        private const string ApiVersion = "v1";

        #region initialize

        public PH1401()
        {
            InitializeComponent();
            client = new MFWebApiClient();
            InitializeUserComponent();
            InitializeHandlers();
        }

        private void InitializeUserComponent()
        {
            Text = "MFクラウド請求書 Web API 連携設定";
            client.ErrorListener = SetResponseMessage;
            lblScopeDescription.Text = client.Scope;
            lblCallbackURLDescription.Text = client.RedirectUri;
        }

        private void InitializeHandlers()
        {
            Load += PH1401_Load;
            txtClientId         .TextChanged += OnTextChanged;
            txtClientSecret     .TextChanged += OnTextChanged;
            txtAuthorizationCode.TextChanged += OnTextChanged;
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();
            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction02Caption("");
            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction04Caption("");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(false);
            BaseContext.SetFunction03Enabled(true);
            BaseContext.SetFunction04Enabled(false);

            OnF01ClickHandler = Save;
            OnF03ClickHandler = Delete;
            OnF10ClickHandler = Close;
        }

        private void PH1401_Load(object sender, EventArgs e)
        {
            SetScreenName();
            ProgressDialog.Start(ParentForm, InitializeLoadAsync(), false, SessionKey);
            DisplayConnectionStatus();
        }

        private async Task InitializeLoadAsync()
        {
            var tasks = new List<Task>
            {
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
            if (!AuthorizeInner(true))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            if (!LoadMFOfficeInfo())
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

            DisplayConnectionStatus();

            Modified = false;
            client.TokenRefreshed = false;

            DispStatusMessage(MsgInfSaveSuccess);
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

            Task task = DeleteAsync()
                .ContinueWith(async t =>
                {
                    if (t.Exception != null || !t.Result)
                    {
                        if (t.Exception != null)
                            NLogHandler.WriteErrorLog(this, t.Exception, SessionKey);
                        ShowWarningDialog(MsgErrDeleteError);
                        return;
                    }

                    UpdateSettingData();
                    await SaveAsync(Setting);

                }, TaskScheduler.FromCurrentSynchronizationContext())
                .Unwrap();
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            ClearControlValues();
            DisplayConnectionStatus();

            DispStatusMessage(MsgInfDeleteSuccess);
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
            if (!txtClientId.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblClientId.Text))) return false;
            if (!txtClientSecret.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblClientSecret.Text))) return false;
            if (!txtAuthorizationCode.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblAuthorizationCode.Text))) return false;
            return true;
        }
        #endregion

        #region clear control values

        private void ClearControlValues()
        {
            txtClientId.Clear();
            txtClientSecret.Clear();
            txtAuthorizationCode.Clear();
            lblConnectionStatusDescription.Clear();
            txtResponse.Clear();
            Setting = null;
            Modified = false;
        }

        #endregion

        #region other methods
        private void SetSetting()
        {
            if (Setting == null) Setting = new WebApiSetting
            {
                CompanyId = CompanyId,
                ApiTypeId = WebApiType.MoneyForward,
                CreateBy = Login.UserId,
            };
            Setting.UpdateBy = Login.UserId;
            Setting.ClientId = txtClientId.Text;
            Setting.ClientSecret = txtClientSecret.Text;
            Setting.BaseUri = client.BaseUri;
            Setting.ApiVersion = ApiVersion;
            client.AuthorizationCode = GetAuthorizationCode();
        }

        private void UpdateSettingData()
        {
            Setting.AccessToken = string.Empty;
            Setting.RefreshToken = string.Empty;
            Setting.ClientId = string.Empty;
            Setting.ClientSecret = string.Empty;
        }

        private string GetAuthorizationCode()
        {
            var search = "code=";
            var authorizationCode = txtAuthorizationCode.Text.Trim();
            var index = authorizationCode.IndexOf(search, StringComparison.OrdinalIgnoreCase) + search.Length;
            return authorizationCode.Substring(index);
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

        #region call web service

        private async Task LoadWebApiSettingsAsync()
        {
            try
            {
                ClearControlValues();
                var result = await ServiceProxyFactory.DoAsync(async (WebApiSettingMasterClient client)
                    => await client.GetByIdAsync(SessionKey, CompanyId, WebApiType.MoneyForward));
                if (!(result?.ProcessResult.Result ?? false) || result.WebApiSetting == null)
                    return;

                Setting = result.WebApiSetting;
                txtClientId.Text = Setting.ClientId;
                txtClientSecret.Text = Setting.ClientSecret;
            }
            finally
            {
                Modified = false;
            }
        }

        private void SetToken()
        {
            AccessToken = Setting.AccessToken;
            RefreshToken = Setting.RefreshToken;
        }

        private async Task<bool> SaveAsync(WebApiSetting setting)
            => ((await ServiceProxyFactory.DoAsync(async (WebApiSettingMasterClient client)
                => await client.SaveAsync(SessionKey, setting)))
            ?.ProcessResult.Result ?? false);

        private async Task<bool> DeleteAsync()
            => ((await ServiceProxyFactory.DoAsync(async (WebApiSettingMasterClient client)
                => await client.DeleteAsync(SessionKey, CompanyId, WebApiType.MoneyForward)))
            ?.ProcessResult.Result ?? false);

        #endregion

        #region mf web api call

        private void DisplayConnectionStatus()
        {
            if (Setting == null || string.IsNullOrWhiteSpace(Setting.AccessToken))
            {
                lblConnectionStatusDescription.Text = "未連携";
                return;
            }

            var task = client.ValidateToken();
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            lblConnectionStatusDescription.Text = task.Result ? "連携中": "トークン有効期限切れ";

        }


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

        private bool LoadMFOfficeInfo()
        {
            var prefix = "\r\n連携に成功しました。\r\n";
            var officeInfo = string.Empty;
            var task = Task.Run(async () => {
                var comp = await client.GetOfficeAsync();
                officeInfo = comp.GetOfficeInfo();
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            if (task.Exception != null)
            {
                NLogHandler.WriteErrorLog(this, task.Exception, SessionKey);
                return false;
            }
            if (string.IsNullOrEmpty(officeInfo)) return false;
            if (client.TokenRefreshed) SetToken();
            SetResponseMessage(prefix + officeInfo);
            return true;
        }

        #endregion

        #region event handlers
        private void OnTextChanged(object sender, EventArgs e) => Modified = true;
        #endregion

    }
}
