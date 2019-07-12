using Rac.VOne.Client.Common;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.Importer;
using Rac.VOne.Client.Screen.ImporterSettingService;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>入金フリーインポーター</summary>
    public partial class PD0201 : VOneScreenBase
    {

        private ImporterSetting ImporterSetting { get; set; } = new ImporterSetting();
        private ReceiptImporter Importer { get; set; }

        private string Note1 { get; set; } = "";
        private string Note2 { get; set; } = "";
        private string Note3 { get; set; } = "";
        private string Note4 { get; set; } = "";

        #region initialize

        public PD0201()
        {
            InitializeComponent();
            Text = "入金フリーインポーター";

            AddHandlers();
        }

        private void PD0201_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                txtCode.Select();
                var loadTask = new List<Task>();

                if (Company == null)
                    loadTask.Add(LoadCompanyAsync());
                if (ApplicationControl == null)
                    loadTask.Add(LoadApplicationControlAsync());
                loadTask.Add(LoadColumnNameSettingAsync());
                loadTask.Add(LoadControlColorAsync());

                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
                Importer = new ReceiptImporter(Login, ApplicationControl);
                Clear();
                Modified = false;
                txtCode.PaddingChar = '0';
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

            BaseContext.SetFunction01Caption("読込");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Import;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = ConfirmToClear;

            BaseContext.SetFunction03Caption("印刷");
            BaseContext.SetFunction03Enabled(true);
            OnF03ClickHandler = DoPrint;

            BaseContext.SetFunction04Caption("登録");
            BaseContext.SetFunction04Enabled(true);
            OnF04ClickHandler = Save;

            BaseContext.SetFunction05Caption("取込設定");
            BaseContext.SetFunction05Enabled(true);
            OnF05ClickHandler = OpenSetting;

            BaseContext.SetFunction06Caption("出力");
            BaseContext.SetFunction06Enabled(true);
            OnF06ClickHandler = Output;

            BaseContext.SetFunction10Caption("終了");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Exit;
        }

        #endregion

        #region F1 event
        [OperationLog("読込")]
        private void Import()
        {
            ClearStatusMessage();
            try
            {
                if (!ValidateChildren()) return;
                if (!ValidateForRead()) return;

                ReadCsv();
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrReadingError);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private bool ValidateForRead()
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                txtCode.Focus();
                ShowWarningDialog(MsgWngInputRequired, lblCode.Text);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFilePath.Text))
            {
                txtFilePath.Focus();
                ShowWarningDialog(MsgWngInputRequired, lblInitialDirectory.Text);
                return false;
            }

            if (!File.Exists(txtFilePath.Text))
            {
                txtFilePath.Focus();
                ShowWarningDialog(MsgWngOpenFileNotFound);
                return false;
            }
            return true;
        }

        private void ReadCsv()
        {

            Importer.ImporterSettingId = ImporterSetting.Id;
            Importer.FilePath = txtFilePath.Text;

            Exception exOuter = null;
            var task = Task.Run(async () =>
            {
                try
                {
                    await Importer.ReadCsvAsync();
                }
                catch (Exception ex)
                {
                    exOuter = ex;
                }
            });

            NLogHandler.WriteDebug(this, "入金フリーインポーター 読込開始");
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            NLogHandler.WriteDebug(this, "入金フリーインポーター 読込終了");

            if (exOuter != null)
            {
                NLogHandler.WriteErrorLog(this, exOuter, SessionKey);
                ShowWarningDialog(MsgErrReadingError);
                return;
            }

            lblReadNo.Text = Importer.ReadCount.ToString("#,##0");
            lblPossibleNo.Text = Importer.ValidCount.ToString("#,##0");
            lblImpossibleNo.Text = Importer.InvalidCount.ToString("#,##0");

            //すべて取込可能
            if (Importer.ValidCount > 0 && Importer.InvalidCount == 0)
            {
                BaseContext.SetFunction01Enabled(false);
                txtFilePath.Enabled = false;
                BaseContext.SetFunction03Enabled(true);
                BaseContext.SetFunction04Enabled(true);
                BaseContext.SetFunction06Enabled(false);
                rdoPossible.Checked = true;
                gbxPrint.Enabled = false;
                btnPath.Enabled = false;

            }
            //すべて取込不可能
            else if (Importer.ValidCount == 0 && Importer.InvalidCount > 0)
            {
                BaseContext.SetFunction03Enabled(true);
                BaseContext.SetFunction06Enabled(true);
                BaseContext.SetFunction04Enabled(false);
                rdoImpossible.Checked = true;
                gbxPrint.Enabled = false;
            }
            //取込可能、取込不可能が混在
            else if (Importer.ValidCount > 0 && Importer.InvalidCount > 0)
            {
                gbxPrint.Enabled = true;
                BaseContext.SetFunction03Enabled(true);
                BaseContext.SetFunction06Enabled(false);
                BaseContext.SetFunction04Enabled(false);
                rdoPossible.Checked = true;
            }
            else
            {
                ShowWarningDialog(MsgWngNoImportData);
                gbxPrint.Enabled = false;
            }
        }

        #endregion

        #region F2 event

        private void Clear()
        {
            ClearStatusMessage();
            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction05Enabled(true);
            txtCode.Clear();
            txtFilePath.Clear();
            lblName.Clear();
            txtCode.Enabled = true;
            txtCode.Select();
            txtFilePath.Enabled = false;
            lblReadNo.Clear();
            lblPossibleNo.Clear();
            lblImpossibleNo.Clear();
            lblNumber.Clear();
            lblMoney.Clear();
            Modified = false;
            btnCode.Enabled = true;
            //money = 0M;
            rdoPossible.Checked = true;
            gbxPrint.Enabled = false;
            btnPath.Enabled = false;
            //外貨対応ONの場合、非表示
            if (ApplicationControl.UseForeignCurrency == 1)
            {
                lblMoney.Visible = false;
                lblTorikomiAmount.Visible = false;
            }
            else
            {
                lblMoney.Visible = true;
                lblTorikomiAmount.Visible = true;
            }
        }

        [OperationLog("クリア")]
        private void ConfirmToClear()
        {
            ClearStatusMessage();
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear)) return;
            Clear();
        }

        #endregion

        #region F3 event
        [OperationLog("印刷")]
        private void DoPrint()
        {
            try
            {
                var serverPath = "";
                ReceiptImporterSectionReport report = null;

                ProgressDialog.Start(ParentForm, Task.Run(() =>
                {
                    serverPath = Util.GetGeneralSettingServerPathAsync(Login).Result;
                    serverPath = Util.GetDirectoryName(serverPath);
                    var isPossible = rdoPossible.Checked;
                    var reportSource = Importer.GetReportSource(isPossible);
                    report = new ReceiptImporterSectionReport();
                    report.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                    report.Name = $"入金フリーインポーター　取込{(isPossible ? "可能" : "不可能")}データ一覧{DateTime.Today:yyyyMMdd}";
                    report.SetData(reportSource, isPossible, ApplicationControl.UseForeignCurrency, Note1);

                    report.Run(false);
                }), false, SessionKey);
                ShowDialogPreview(ParentForm, report, serverPath);
            }
            catch(Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }
        #endregion

        #region F4 event
        [OperationLog("登録")]
        private void Save()
        {
            try
            {
                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }
                ProgressDialog.Start(ParentForm, SaveReceipt(), false, SessionKey);
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrImportErrorWithoutLog);
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task SaveReceipt()
        {
            NLogHandler.WriteDebug(this, "入金フリーインポーター 取込開始");
            var importResult = await Importer.ImportAsync();
            NLogHandler.WriteDebug(this, "入金フリーインポーター 取込終了");

            if (!importResult)
            {
                ShowWarningDialog(MsgErrImportErrorWithoutLog);
                return;
            }

            lblNumber.Text = Importer.SaveCount.ToString("#,##0");
            lblMoney.Text = Importer.SaveAmount.ToString("#,##0");

            var moveResult = Importer.MoveFile();

            DispStatusMessage(MsgInfFinishImport);
            BaseContext.SetFunction04Enabled(false);

            if (importResult & moveResult)
                Modified = false;
        }
        #endregion

        #region F5 event
        [OperationLog("取込設定")]
        private void OpenSetting()
        {
            var form = ApplicationContext.Create(nameof(PD0202));
            var result = ApplicationContext.ShowDialog(ParentForm, form);
        }

        #endregion

        #region F6 event
        [OperationLog("出力")]
        private void Output()
        {
            try
            {
                var dir = Path.GetDirectoryName(txtFilePath.Text);
                var fileName = $"入金フリーインポーター{DateTime.Today:yyyyMMdd}.log";
                var filePath = string.Empty;
                var filter = "すべてのファイル (*.*)|*.*|LOGファイル (*.log)|*.log";
                if (!ShowSaveFileDialog(dir, fileName, out filePath, filter))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }
                var result = Importer.WriteErrorLog(filePath);

                if (result)
                {
                    DispStatusMessage(MsgInfOutputUnreadData, "取込不可能ログデータ");
                }
                else
                {
                    ShowWarningDialog(MsgErrSomethingError, "取込不可能ログデータ出力");
                }
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrSomethingError, "取込不可能ログデータ出力");
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region F10 event
        [OperationLog("終了")]
        private void Exit()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;
            ParentForm.Close();
        }
        #endregion

        #region web service
        private async Task LoadColumnNameSettingAsync()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ColumnNameSettingMasterClient>();

                var result = await service.GetItemsAsync(Login.SessionKey, Login.CompanyId);

                if (result.ProcessResult.Result)
                {
                    var ColumnSettingInfo = result.ColumnNames.Where(x => x.TableName == "Receipt").ToList();

                    foreach (ColumnNameSetting cs in ColumnSettingInfo)
                    {
                        switch (cs.ColumnName)
                        {
                            case "Note1": Note1 = cs.Alias; break;
                            case "Note2": Note2 = cs.Alias; break;
                            case "Note3": Note3 = cs.Alias; break;
                            case "Note4": Note4 = cs.Alias; break;
                        }
                    }
                }
            });
        }

        private async Task LoadImporterSettingAsync(string code)
            => await ServiceProxyFactory.DoAsync<ImporterSettingServiceClient>(async client =>
            {
                var result = await client.GetHeaderByCodeAsync(SessionKey, CompanyId, (int)Rac.VOne.Common.Constants.FreeImporterFormatType.Receipt, code);
                if (result?.ImporterSetting != null)
                {
                    ImporterSetting = result.ImporterSetting;
                    txtFilePath.Text = result.ImporterSetting.InitialDirectory;
                    lblName.Text = result.ImporterSetting.Name;
                    txtFilePath.Enabled = !LimitAccessFolder;
                    txtFilePath.ReadOnly = LimitAccessFolder;

                    txtCode.Enabled = false;
                    btnCode.Enabled = false;
                    if (!LimitAccessFolder) { ActiveControl = txtFilePath; txtFilePath.Focus(); }
                    ClearStatusMessage();
                    btnPath.Enabled = true;
                    BaseContext.SetFunction05Enabled(false);
                    Modified = false;
                }
                else
                {
                    ShowWarningDialog(MsgWngNotRegistPatternNo, txtCode.Text);
                    txtCode.Clear();
                    lblName.Clear();
                    txtFilePath.Clear();
                    Modified = true;
                    BaseContext.SetFunction05Enabled(true);
                }
            });
        #endregion

        #region event handlers

        private void AddHandlers()
        {
            foreach (Control control in this.GetAll<Common.Controls.VOneTextControl>())
            {
                control.TextChanged += new EventHandler(OnContentChanged);
            }
        }

        private void OnContentChanged(object sender, EventArgs e)
        {
            if (!Modified)
            {
                Modified = true;
            }
        }

        private void txtCode_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            if (string.IsNullOrEmpty(txtCode.Text)) return;

            try
            {
                ProgressDialog.Start(ParentForm, LoadImporterSettingAsync(txtCode.Text), false, SessionKey);
                if (string.IsNullOrEmpty(txtCode.Text))
                    this.ActiveControl = txtCode;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnCode_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedImporterSetting = this.ShowReceiptImporterSettingSearchDialog();
                if (selectedImporterSetting != null)
                {
                    txtCode.Text = selectedImporterSetting.Code;
                    lblName.Text = selectedImporterSetting.Name;
                    ProgressDialog.Start(ParentForm, LoadImporterSettingAsync(txtCode.Text), false, SessionKey);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            var path = txtFilePath.Text.Trim();
            var fileNames = new List<string>();
            if (!LimitAccessFolder ?
                !ShowOpenFileDialog(path, out fileNames) :
                !ShowRootFolderBrowserDialog(ApplicationControl.RootPath, out fileNames, FolderBrowserType.SelectFile)) return;

            txtFilePath.Text = fileNames?.FirstOrDefault() ?? string.Empty;
        }


        private void rdoPossible_Click(object sender, EventArgs e)
        {
            BaseContext.SetFunction06Enabled(false);
        }

        private void rdoImpossible_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(lblImpossibleNo.Text))
            {
                BaseContext.SetFunction06Enabled(true);
            }
        }

        #endregion

    }
}
