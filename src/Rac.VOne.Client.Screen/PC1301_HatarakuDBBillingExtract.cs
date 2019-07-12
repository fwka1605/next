using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rac.VOne.Common;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.HttpClients;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.Importer;
using Rac.VOne.Client.Screen.ImporterSettingService;
using Rac.VOne.Client.Screen.WebApiSettingMasterService;
using Rac.VOne.Client.Reports;
using Rac.VOne.Common.Extensions;
using Rac.VOne.Web.Models;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>働くDB 請求データ抽出</summary>
    public partial class PC1301 : VOneScreenBase
    {
        private string Note1Caption { get; set; }

        private HatarakuDBClient WebApiClient { get; set; }
        private WebApiSetting WebApiSetting { get; set; }
        private ImporterSetting ImporterSetting { get; set; }

        private BillingImporter Importer { get; set; }

        /// <summary>働くDB から取得したcsv ファイルを保存する一時的なファイルパス
        /// AppData などの場所を利用する
        /// </summary>
        private string TemporaryFilePath { get; set; }

        #region initialize

        public PC1301()
        {
            InitializeComponent();
            InitializeUserComponent();
            InitializeHandlers();
        }

        private void InitializeUserComponent()
        {
            Text = "働くDB 請求データ抽出";
            WebApiClient = new HatarakuDBClient();
        }

        private void InitializeHandlers()
        {
            Load += PC1301_Load;
        }

        private void PC1301_Load(object sender, EventArgs e)
        {
            SetScreenName();
            ProgressDialog.Start(ParentForm, InitializeDataAsync(), false, SessionKey);
            if (ImporterSetting == null)
            {
                ShowWarningDialog(MsgWngNotSettingMaster, "働くDB WebAPI 連携設定");
            }
        }

        private async Task InitializeDataAsync()
        {
            await Task.WhenAll(
                LoadCompanyAsync(),
                LoadApplicationControlAsync(),
                LoadControlColorAsync(),
                LoadColumnNameSattingsAsync());
            await LoadWebApiSettingAsync();
            Importer = new BillingImporter(Login, ApplicationControl);
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("抽出");
            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction03Caption("印刷");
            BaseContext.SetFunction04Caption("登録");
            BaseContext.SetFunction05Caption("取込設定");
            BaseContext.SetFunction06Caption("出力");
            BaseContext.SetFunction07Caption("連携設定");

            BaseContext.SetFunction01Enabled(false);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Enabled(true);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(true);
            BaseContext.SetFunction10Enabled(true);

            OnF01ClickHandler = Extract;
            OnF02ClickHandler = Clear;
            OnF03ClickHandler = OutputReport;
            OnF04ClickHandler = Register;
            OnF05ClickHandler = ShowImporterSetting;
            OnF06ClickHandler = OutputErrorLog;
            OnF07ClickHandler = ShowWebApiSetting;
            OnF10ClickHandler = Close;
        }

        #endregion

        #region function keys

        [OperationLog("抽出")]
        private void Extract()
        {
            ClearStatusMessage();
            Modified = false;
            var csv = string.Empty;
            var writeResult = false;
            var readResult = false;
            var task = Task.Run(async () =>
            {
                csv = await ExtractInnerAsync();
                if (string.IsNullOrEmpty(csv)) return;
                writeResult = WriteTemporaryFile(csv);
                if (!writeResult) return;
                readResult = await ReadInnerAsync();
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (string.IsNullOrEmpty(csv))
            {
                ShowWarningDialog(MsgWngNotExistSearchData);
                return;
            }

            if (!writeResult || !readResult)
            {
                ShowWarningDialog(MsgErrSomethingError, "請求データ読込");
                return;
            }
            var format = "#,##0";
            lblDispExtractCount.Text = Importer.ReadCount.ToString(format);
            lblDispValidCount.Text = Importer.ValidCount.ToString(format);
            lblDispInvalidCount.Text = Importer.InvalidCount.ToString(format);
            lblDispNewCustomerCount.Text = Importer.NewCustomerCreationCount.ToString(format);

            if (Importer.InvalidCount > 0)
            {
                BaseContext.SetFunction03Enabled(true);
                BaseContext.SetFunction06Enabled(true);
                if (Importer.ValidCount == 0)
                {
                    rdoPrintImpossible.Checked = true;
                    gbxPrint.Enabled = false;
                }
                else
                    gbxPrint.Enabled = true;
                return;
            }

            if (Importer.ReadCount == 0
                || Importer.ValidCount == 0)
            {
                ShowWarningDialog(MsgWngNoImportData);
                gbxPrint.Enabled = false;
                return;
            }

            BaseContext.SetFunction01Enabled(false);
            BaseContext.SetFunction03Enabled(true);
            BaseContext.SetFunction04Enabled(true);
            rdoPrintPossible.Checked = true;
            gbxPrint.Enabled = false;
            Modified = true;
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            ClearStatusMessage();
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear)) return;
            ClearControlValues();
        }

        [OperationLog("印刷")]
        private void OutputReport()
        {
            ClearStatusMessage();

            var pathTask = Util.GetGeneralSettingServerPathAsync(Login);
            var createTask = CreateReportAsync();
            var tasks = new List<Task> { pathTask, createTask };
            ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);
            if (pathTask.Exception != null)
            {
                NLogHandler.WriteErrorLog(this, pathTask.Exception, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
                return;
            }
            if (createTask.Exception != null)
            {
                NLogHandler.WriteErrorLog(this, createTask.Exception, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
                return;
            }
            var path = pathTask.Result;
            var report = createTask.Result;
            if (report == null)
            {
                ShowWarningDialog(MsgErrCreateReportError);
                return;
            }
            ShowDialogPreview(ParentForm, report);

        }

        [OperationLog("登録")]
        private void Register()
        {
            ClearStatusMessage();
            ProgressDialog.Start(ParentForm, ImportInnerAsync(), false, SessionKey);
        }

        [OperationLog("取込設定")]
        private void ShowImporterSetting()
        {
            ClearStatusMessage();
            using (var form = ApplicationContext.Create(nameof(PC0102)))
                ApplicationContext.ShowDialog(ParentForm, form);
        }

        [OperationLog("出力")]
        private void OutputErrorLog()
        {
            ClearStatusMessage();
            try
            {
                var task = Util.GetGeneralSettingServerPathAsync(Login);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                var serverPath = task.Result;

                var dir = Util.GetDirectoryName(serverPath);
                var fileName = $"働くDB 請求データ抽出{DateTime.Today:yyyyMMdd}.log";
                var filePath = string.Empty;
                var filter = "すべてのファイル (*.*)|*.*|LOGファイル (*.log)|*.log";
                if (!ShowSaveFileDialog(dir, fileName, out filePath, filter))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }
                var result = Importer.WriteErrorLog(filePath);

                if (!result)
                {
                    ShowWarningDialog(MsgErrSomethingError, "取込不可能ログデータ出力");
                    return;
                }
                DispStatusMessage(MsgInfOutputUnreadData, "取込不可能ログデータ");
            }
            catch (Exception)
            {
                DispStatusMessage(MsgErrSomethingError, "取込不可能ログデータの出力");
            }

        }

        [OperationLog("連携設定")]
        private void ShowWebApiSetting()
        {
            ClearStatusMessage();
            var settingSaveResult = DialogResult.Cancel;
            using (var form = ApplicationContext.Create(nameof(PH1101)))
            {
                var screen = form.GetAll<PH1101>().First();
                screen.ParentScreen = this;
                settingSaveResult = ApplicationContext.ShowDialog(ParentForm, form);
            }
            if (settingSaveResult != DialogResult.OK) return;
            ProgressDialog.Start(ParentForm, LoadWebApiSettingAsync(), false, SessionKey);
        }

        [OperationLog("終了")]
        private void Close()
        {
            ClearStatusMessage();
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;
            ParentForm.Close();
        }

        #endregion

        #region clear

        private void ClearControlValues()
        {

            gbxPrint.Enabled = false;
            rdoPrintPossible.Checked = true;

            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction01Enabled(!string.IsNullOrEmpty(lblPatternName.Text));
            BaseContext.SetFunction05Enabled(true);

            lblDispExtractCount.Clear();
            lblDispValidCount.Clear();
            lblDispInvalidCount.Clear();
            lblDispNewCustomerCount.Clear();
            lblDispImportCount.Clear();
            lblDispImportAmount.Clear();

            Modified = false;
        }

        #endregion

        #region extract data


        private async Task<string> ExtractInnerAsync()
        {
            WebApiClient.WebApiSetting = WebApiSetting;
            var csv = string.Empty;
            try
            {
                csv = await WebApiClient.ExportAsync();
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            return csv;
        }

        private bool WriteTemporaryFile(string csv)
        {
            var path = (TemporaryFilePath = GetTemporaryFilePath());
            var result = false;
            try
            {
                using (var writer = new StreamWriter(path, false, Encoding.GetEncoding(932)))
                    writer.Write(csv);
                result = true;
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            return result;
        }

        private async Task<bool> ReadInnerAsync()
        {
            var result = false;
            Importer.FilePath = TemporaryFilePath;
            Importer.ImporterSettingId = ImporterSetting.Id;
            try
            {
                await Importer.ReadCsvAsync();
                result = true;
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            finally
            {
                DeleteTemporaryFile();
            }
            return result;
        }


        private string GetTemporaryFilePath()
            => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                $"{Guid.NewGuid()}.csv");

        private void DeleteTemporaryFile()
        {
            try
            {
                if (string.IsNullOrEmpty(TemporaryFilePath)
                    || !File.Exists(TemporaryFilePath)) return;
                File.Delete(TemporaryFilePath);
            }
            catch { }
        }

        #endregion

        #region output report

        private async Task<BillingImporterSectionReport> CreateReportAsync()
        {
            BillingImporterSectionReport report = null;
            await Task.Run(() => {
                report = new BillingImporterSectionReport();
                var source = Importer.GetReportSource(rdoPrintPossible.Checked);
                report = new BillingImporterSectionReport();
                report.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);

                var title = $"働くDB 請求データ　取込{(rdoPrintPossible.Checked ? "可能" : "不可能")}データ一覧";
                report.Name = title + DateTime.Now.ToString("yyyyMMdd");
                report.SetData(source, title, UseForeignCurrency, Note1Caption);
                report.Run(false);
            });
            return report;
        }

        #endregion

        #region import

        private async Task ImportInnerAsync()
        {
            var result = await Importer.ImportAsync();
            if (result)
            {
                lblDispImportAmount.Text = Importer.SaveAmount.ToString("#,##0");
                lblDispImportCount.Text = Importer.SaveCount.ToString("#,##0");
                Modified = false;
                BaseContext.SetFunction04Enabled(false);
                DispStatusMessage(MsgInfProcessFinish);
            }
            else
            {
                ShowWarningDialog(MsgErrImportErrorWithoutLog);
            }
        }


        #endregion

        #region call web service

        private async Task<bool> LoadWebApiSettingAsync()
        {
            var settiing = (WebApiSetting = await GetWebApiSettingAsync());
            if (settiing == null) return false;
            var extractSetting = settiing.ExtractSetting.ConvertToModel<WebApiHatarakuDBExtractSetting>(ignoreNull: false);
            if (extractSetting == null || string.IsNullOrEmpty(extractSetting.PatternCode)) return false;
            var pattern = (ImporterSetting = await GetImporterSettingAsync(extractSetting.PatternCode));
            if (pattern == null) return false;
            txtPatternCode.Text = pattern.Code;
            lblPatternName.Text = pattern.Name;
            BaseContext.SetFunction01Enabled(true);
            return true;
        }

        private async Task<WebApiSetting> GetWebApiSettingAsync()
            => await ServiceProxyFactory.DoAsync(async (WebApiSettingMasterClient client) => {
                var result = await client.GetByIdAsync(SessionKey, CompanyId, WebApiType.HatarakuDb);
                if (result.ProcessResult.Result)
                    return result.WebApiSetting;
                return null;
            });

        private async Task<ImporterSetting> GetImporterSettingAsync(string code)
            => await ServiceProxyFactory.DoAsync(async (ImporterSettingServiceClient client) => {
                var result = await client.GetHeaderByCodeAsync(SessionKey, CompanyId,
                    (int)Rac.VOne.Common.Constants.FreeImporterFormatType.Billing, code);
                if (result.ProcessResult.Result)
                    return result.ImporterSetting;
                return null;
            });

        private async Task LoadColumnNameSattingsAsync()
            => await ServiceProxyFactory.DoAsync(async (ColumnNameSettingMasterService.ColumnNameSettingMasterClient client) =>
            {
                var result = await client.GetItemsAsync(SessionKey, CompanyId);
                if (!(result?.ProcessResult.Result ?? false)) return;
                Note1Caption = result.ColumnNames
                    .First(x => x.TableName  == nameof(Billing)
                             && x.ColumnName == nameof(Billing.Note1)).DisplayColumnName;
            });

        #endregion

    }
}
