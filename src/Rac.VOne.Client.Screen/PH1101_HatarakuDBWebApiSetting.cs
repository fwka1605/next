using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.HttpClients;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.ImporterSettingService;
using Rac.VOne.Client.Screen.WebApiSettingMasterService;
using Rac.VOne.Common.Extensions;
using Rac.VOne.Web.Models;
using Rac.VOne.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>働くDB Web API 連携設定</summary>
    public partial class PH1101 : VOneScreenBase
    {
        private HatarakuDBClient WebApiClient { get; set; }
        /// <summary>呼出し元 VOneScreenBase</summary>
        internal VOneScreenBase ParentScreen { private get; set; }

        #region initialize
        public PH1101()
        {
            InitializeComponent();
            InitializeUserComponent();
            InitializeHandlers();
        }
        private void InitializeUserComponent()
        {
            Text = "働くDB Web API 連携設定";
            WebApiClient = new HatarakuDBClient();
        }
        private void InitializeHandlers()
        {
            Load += PC2101_Load;

            txtBaseUri              .TextChanged += TextBox_TextChanged;
            txtAccessToken          .TextChanged += TextBox_TextChanged;
            txtExtractDbSchemaId    .TextChanged += TextBox_TextChanged;
            txtExtractSearchId      .TextChanged += TextBox_TextChanged;
            txtExtractListId        .TextChanged += TextBox_TextChanged;
            txtImporterPatternCode  .TextChanged += TextBox_TextChanged;
            txtOutputDbSchemaId     .TextChanged += TextBox_TextChanged;
            txtOutputImportId       .TextChanged += TextBox_TextChanged;

            txtImporterPatternCode.Validated += txtImporterPatternCode_Validated;
            btnImporterPattern.Click += btnImporterPattern_Click;
        }

        private void PC2101_Load(object sender, EventArgs e)
        {
            SetScreenName();
            ProgressDialog.Start(ParentForm, InitializeDataAsync(), false, SessionKey);
        }

        private async Task InitializeDataAsync()
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

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();
            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction02Caption("再表示");
            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction04Caption("テスト接続");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(true);
            BaseContext.SetFunction04Enabled(true);
            BaseContext.SetFunction10Enabled(true);

            OnF01ClickHandler = Save;
            OnF02ClickHandler = Reload;
            OnF03ClickHandler = Delete;
            OnF04ClickHandler = ConnectWebApi;
            OnF10ClickHandler = Close;
        }

        #endregion

        #region function keys

        [OperationLog("登録")]
        private void Save()
        {
            ClearStatusMessage();
            if (!ValidateInputValuesForRegister()) return;
            if (!ShowConfirmDialog(MsgQstConfirmSave))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            var setting = GetWebApiSetting();
            var saveTask = SaveAsync(setting);
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
            if (ParentScreen is PC1301)
            {
                ParentForm.DialogResult = DialogResult.OK;
            }
            DispStatusMessage(MsgInfSaveSuccess);
        }

        [OperationLog("再表示")]
        private void Reload()
        {
            ClearStatusMessage();
            if (Modified && !ShowConfirmDialog(MsgQstConfirmUpdateData)) return;
            ProgressDialog.Start(ParentForm, LoadWebApiSettingsAsync(), false, SessionKey);
        }

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

        [OperationLog("テスト接続")]
        private void ConnectWebApi()
        {
            ClearStatusMessage();
            txtResponse.Clear();
            if (!ValidateInputValuesForConnectionTest()) return;
            WebApiClient.WebApiSetting = GetWebApiSetting();

            var task = WebApiClient.ConnectAsnyc();
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            var response = task.Result;
            if (task.Exception != null)
            {
                NLogHandler.WriteErrorLog(this, task.Exception, SessionKey);
                ShowWarningDialog(MsgErrSomethingError, "テスト接続");
                return;
            }
            if (response == null)
            {
                ShowWarningDialog(MsgErrSomethingError, "テスト接続");
                return;
            }
            txtResponse.Text = response.ToString();
            DispStatusMessage(MsgInfSuccessTestConnection);
        }

        [OperationLog("終了")]
        private void Close()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;
            ClearStatusMessage();
            ParentForm.Close();
        }

        #endregion

        #region validation

        private bool ValidateInputValuesForRegister()
        {
            if (!ValidateInputValuesForConnectionTest()) return false;
            if (!txtImporterPatternCode.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblImporterPattern.Text))) return false;
            if (!txtOutputDbSchemaId.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblOutputDbSchemaId.Text))) return false;
            if (!txtOutputImportId.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblOutputImportId.Text))) return false;
            return true;
        }

        private bool ValidateInputValuesForConnectionTest()
        {
            if (!txtAccessToken.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblAccessToken.Text))) return false;
            if (!txtBaseUri.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblBaseUri.Text))) return false;
            if (!txtApiVersion.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblApiVersion.Text))) return false;
            if (!txtExtractDbSchemaId.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblExtractDbSchemaId.Text))) return false;
            return true;
        }

        #endregion

        #region clear control values

        private void ClearControlValues()
        {
            txtBaseUri.Clear();
            txtApiVersion.Text = "v1";
            txtAccessToken.Clear();
            txtExtractDbSchemaId.Clear();
            txtExtractListId.Clear();
            txtExtractSearchId.Clear();
            txtImporterPatternCode.Clear();
            lblImporterPatternName.Clear();
            txtOutputDbSchemaId.Clear();
            txtOutputImportId.Clear();
            txtResponse.Clear();
            Modified = false;
        }

        #endregion

        #region convert control values to models (WebApiSetting)

        private WebApiSetting GetWebApiSetting()
        {
            var setting = new WebApiSetting
            {
                CompanyId = CompanyId,
                ApiTypeId = WebApiType.HatarakuDb,
                BaseUri = txtBaseUri.Text,
                AccessToken = txtAccessToken.Text,
                ApiVersion = txtApiVersion.Text,
            };
            setting.ExtractSetting = GetWebApiExtractParameters().ConvertToJson(ignoreNull: false);
            setting.OutputSetting = GetWebApiOutputParameters().ConvertToJson(ignoreNull: false);
            setting.CreateBy = Login.UserId;
            setting.UpdateBy = Login.UserId;
            return setting;
        }

        private WebApiHatarakuDBExtractSetting GetWebApiExtractParameters()
        {
            var setting = new WebApiHatarakuDBExtractSetting();
            setting.dbSchemaId = txtExtractDbSchemaId.Text;
            if (!string.IsNullOrEmpty(txtExtractSearchId.Text)) setting.searchId = txtExtractSearchId.Text;
            if (!string.IsNullOrEmpty(txtExtractListId.Text  )) setting.listId   = txtExtractListId.Text;
            setting.PatternCode = txtImporterPatternCode.Text;
            return setting;
        }

        private WebApiHatarakuDBOutputSetting GetWebApiOutputParameters() => new WebApiHatarakuDBOutputSetting
        {
            dbSchemaId = txtOutputDbSchemaId.Text,
            importId   = txtOutputImportId.Text
        };

        #endregion

        #region call web services

        private async Task LoadWebApiSettingsAsync()
        {
            try
            {
                ClearControlValues();
                var result = await ServiceProxyFactory.DoAsync(async (WebApiSettingMasterClient client)
                    => await client.GetByIdAsync(SessionKey, CompanyId, WebApiType.HatarakuDb));
                if (!(result?.ProcessResult.Result ?? false)) return;
                var setting = result.WebApiSetting;
                if (setting == null) return;
                txtBaseUri.Text = setting.BaseUri;
                txtApiVersion.Text = setting.ApiVersion;
                txtAccessToken.Text = setting.AccessToken;

                var extractSetting = setting.ExtractSetting.ConvertToModel<WebApiHatarakuDBExtractSetting>(ignoreNull: false);
                txtExtractDbSchemaId.Text = extractSetting.dbSchemaId.ToString();
                txtExtractSearchId.Text = extractSetting.searchId?.ToString();
                txtExtractListId.Text = extractSetting.listId?.ToString();
                txtImporterPatternCode.Text = extractSetting.PatternCode;

                var outputSetting = setting.OutputSetting.ConvertToModel<WebApiHatarakuDBOutputSetting>(ignoreNull: false);
                txtOutputDbSchemaId.Text = outputSetting.dbSchemaId.ToString();
                txtOutputImportId.Text = outputSetting.importId.ToString();

                var header = await LoadImporterSettingAsync(extractSetting.PatternCode);
                if (header == null) return;
                lblImporterPatternName.Text = header.Name;
            }
            finally
            {
                Modified = false;
            }
        }

        private async Task<ImporterSetting> LoadImporterSettingAsync(string code)
        {
            var result = await ServiceProxyFactory.DoAsync(async (ImporterSettingServiceClient client)
                => await client.GetHeaderByCodeAsync(SessionKey, CompanyId, (int)VOne.Common.Constants.FreeImporterFormatType.Billing, code));
            if (result.ProcessResult.Result)
                return result.ImporterSetting;
            return null;
        }

        private async Task<bool> SaveAsync(WebApiSetting setting)
            => ((await ServiceProxyFactory.DoAsync(async (WebApiSettingMasterClient client)
                => await client.SaveAsync(SessionKey, setting)))
            ?.ProcessResult.Result ?? false);

        private async Task<bool> DeleteAsync()
            => ((await ServiceProxyFactory.DoAsync(async (WebApiSettingMasterClient client)
                => await client.DeleteAsync(SessionKey, CompanyId, WebApiType.HatarakuDb)))
            ?.ProcessResult.Result ?? false);

        #endregion

        #region event handlers

        private void TextBox_TextChanged(object sender, EventArgs e) => Modified = true;

        private void txtImporterPatternCode_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            if (string.IsNullOrEmpty(txtImporterPatternCode.Text))
            {
                lblImporterPatternName.Clear();
                return;
            }
            var task = LoadImporterSettingAsync(txtImporterPatternCode.Text);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            if (task.Exception != null)
            {
                NLogHandler.WriteErrorLog(this, task.Exception, SessionKey);
                ShowWarningDialog(MsgErrDataSearch);
                return;
            }
            var pattern = task.Result;
            if (pattern == null)
            {
                ShowWarningDialog(MsgWngNotRegistPatternNo, txtImporterPatternCode.Text);
                txtImporterPatternCode.Clear();
                lblImporterPatternName.Clear();
                return;
            }
            lblImporterPatternName.Text = pattern.Name;
        }

        private void btnImporterPattern_Click(object sender, EventArgs e)
        {
            var pattern = this.ShowBillingImporterSettingSearchDialog();
            if (pattern == null) return;
            txtImporterPatternCode.Text = pattern.Code;
            lblImporterPatternName.Text = pattern.Name;
        }

        #endregion
    }
}
