using Rac.VOne.Client.Common;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.Importer;
using Rac.VOne.Client.Screen.ImporterSettingService;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;
using static Rac.VOne.Common.Constants;
namespace Rac.VOne.Client.Screen
{
    /// <summary>入金予定フリーインポーター</summary>
    public partial class PC1001 : VOneScreenBase
    {
        private int FormatId { get { return (int)FreeImporterFormatType.PaymentSchedule; } }
        private PaymentScheduleImporter Importer { get; set; }
        private ImporterSetting ImporterSetting { get; set; } = new ImporterSetting();
        private string Note1 { get; set; } = "";
        private string Note2 { get; set; } = "";
        private string Note3 { get; set; } = "";
        private string Note4 { get; set; } = "";

        #region initialize

        public PC1001()
        {
            InitializeComponent();
            Text = "入金予定フリーインポーター";

            AddHandlers();
        }

        private void PC1001_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                this.ActiveControl = txtPatternNo;
                txtPatternNo.Focus();
                var tasks = new List<Task>();
                if (Company == null)
                    tasks.Add(LoadCompanyAsync());
                if (ApplicationControl == null)
                    tasks.Add(LoadApplicationControlAsync());
                tasks.Add(LoadColumnNameSetting());
                tasks.Add(LoadControlColorAsync());

                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);
                Importer = new PaymentScheduleImporter(Login, ApplicationControl);

                lblTitleMoney.Visible = !UseForeignCurrency;
                lblMoney.Visible = !UseForeignCurrency;
                ClearInner();
                Modified = false;
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
            OnF01ClickHandler = Read;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = Clear;

            BaseContext.SetFunction03Caption("印刷");
            BaseContext.SetFunction03Enabled(true);
            OnF03ClickHandler = DoPrint;

            BaseContext.SetFunction04Caption("登録");
            BaseContext.SetFunction04Enabled(true);
            OnF04ClickHandler = Import;

            BaseContext.SetFunction05Caption("取込設定");
            BaseContext.SetFunction05Enabled(true);
            OnF05ClickHandler = ShowImporterSetting;

            BaseContext.SetFunction06Caption("出力");
            BaseContext.SetFunction06Enabled(true);
            OnF06ClickHandler = OutputErrorLog;

            BaseContext.SetFunction10Caption("終了");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Exit;
        }

        #endregion

        #region function keys

        [OperationLog("読込")]
        private void Read()
        {
            try
            {
                if (!ValidateChildren()) return;
                if (!ValidateForRead()) return;
                ProgressDialog.Start(ParentForm, ReadInnerAsync(), false, SessionKey);
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrReadingError);
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private bool ValidateForRead()
        {
            if (string.IsNullOrWhiteSpace(txtPatternNo.Text))
            {
                this.ActiveControl = txtPatternNo;
                txtPatternNo.Focus();
                ShowWarningDialog(MsgWngInputRequired, "取込パターン");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFilePath.Text))
            {
                this.ActiveControl = txtFilePath;
                txtFilePath.Focus();
                ShowWarningDialog(MsgWngInputRequired, "取込ファイル名");
                return false;
            }

            if (!File.Exists(txtFilePath.Text))
            {
                this.ActiveControl = txtFilePath;
                txtFilePath.Focus();
                ShowWarningDialog(MsgWngOpenFileNotFound);
                return false;
            }
            else
            {
                ClearStatusMessage();
                return true;
            }
        }

        private async Task ReadInnerAsync()
        {
            Importer.FilePath = txtFilePath.Text;
            Importer.ImporterSettingId = ImporterSetting.Id;
            Importer.DoReplaceAmount = rdoUpdate.Checked;
            Importer.DoTargetNotMatchedData = rdoClear.Checked;
            Importer.DoIgnoreSameCustomerGroup = rdoIgnore.Checked;

            await Importer.ReadCsvAsync();
            lblReadNo.Text = Importer.ReadCount.ToString("#,##0");
            lblPossibleNo.Text = Importer.ValidCount.ToString("#,##0");
            lblImpossibleNo.Text = Importer.InvalidCount.ToString("#,##0");

            if (Importer.ValidCount > 0 && Importer.InvalidCount == 0)
            {
                BaseContext.SetFunction01Enabled(false);
                txtFilePath.Enabled = false;
                btnFilePath.Enabled = false;
                BaseContext.SetFunction03Enabled(true);
                BaseContext.SetFunction04Enabled(true);
                rdoPossible.Checked = true;
                gbxPrint.Enabled = false;

            }
            else if (Importer.ValidCount == 0 && Importer.InvalidCount > 0)
            {
                BaseContext.SetFunction03Enabled(true);
                BaseContext.SetFunction06Enabled(true);
                rdoImpossible.Checked = true;
                gbxPrint.Enabled = false;
            }
            else if (Importer.ValidCount > 0 && Importer.InvalidCount > 0)
            {
                gbxPrint.Enabled = true;
                BaseContext.SetFunction03Enabled(true);
                BaseContext.SetFunction06Enabled(false);
            }
            else
            {
                ShowWarningDialog(MsgWngNoImportData);
                gbxPrint.Enabled = false;
            }
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear)) return;
            ClearInner();
        }

        private void ClearInner()
        {
            ClearStatusMessage();
            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Enabled(true);
            BaseContext.SetFunction06Enabled(false);
            txtPatternNo.Clear();
            txtFilePath.Clear();
            lblName.Clear();
            txtPatternNo.Enabled = true;
            txtPatternNo.Focus();
            txtFilePath.Enabled = false;
            btnFilePath.Enabled = false;
            rdoUpdate.Checked = true;
            rdoIgnore.Checked = true;
            rdoPossible.Checked = true;
            rdoClear.Checked = true;
            gbxBilling.Enabled = true;
            lblReadNo.Clear();
            lblPossibleNo.Clear();
            lblImpossibleNo.Clear();
            lblNumber.Clear();
            lblMoney.Clear();
            Modified = false;
            btnNumberSearch.Enabled = true;
        }

        [OperationLog("印刷")]
        private void DoPrint()
        {
            try
            {
                var serverPath = "";
                PaymentScheduleImporterSectionReport report = null;

                ProgressDialog.Start(ParentForm, Task.Run(async () =>
                {
                    report = new PaymentScheduleImporterSectionReport();
                    report.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                    var isPossible = rdoPossible.Checked;
                    var source = Importer.GetReportSource(isPossible);
                    report.Name = $"入金予定フリーインポーター　取込{(isPossible ? "可能" : "不可能")}データ一覧{DateTime.Today:yyyyMMdd}";
                    report.SetData(source, isPossible, ApplicationControl.UseForeignCurrency, Note1);
                    report.Run(false);
                    serverPath = await Util.GetGeneralSettingServerPathAsync(Login);
                    serverPath = Util.GetDirectoryName(serverPath);
                }), false, SessionKey);

                ShowDialogPreview(ParentForm, report, serverPath);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        [OperationLog("登録")]
        private void Import()
        {
            try
            {
                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }
                ProgressDialog.Start(ParentForm, ImportInnerAsync(), false, SessionKey);
                Modified = false;
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrImportErrorWithoutLog);
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task ImportInnerAsync()
        {
            var importResult = await Importer.ImportAsync();

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

            if (importResult && moveResult)
                Modified = false;
        }

        [OperationLog("取込設定")]
        private void ShowImporterSetting()
        {
            var form = ApplicationContext.Create(nameof(PC1002));
            var result = ApplicationContext.ShowDialog(ParentForm, form);
        }

        [OperationLog("出力")]
        private void OutputErrorLog()
        {
            try
            {
                var dir = Path.GetDirectoryName(txtFilePath.Text);
                var fileName = $"入金予定フリーインポーター{DateTime.Today:yyyyMMdd}.log";
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

        [OperationLog("終了")]
        private void Exit()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;
            ParentForm.Close();
        }

        #endregion

        #region call web service
        private async Task LoadColumnNameSetting()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ColumnNameSettingMasterClient>();

                var result = await service.GetItemsAsync(SessionKey, CompanyId);

                if (result.ProcessResult.Result)
                {
                    var ColumnSettingInfo = result.ColumnNames.Where(x => x.TableName == "Billing").ToList();

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
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ImporterSettingServiceClient>();
                if (!string.IsNullOrEmpty(code))
                {
                    var result = await service.GetHeaderByCodeAsync(SessionKey, CompanyId, FormatId, code);
                    if (result.ImporterSetting != null)
                    {
                        Invoke(new System.Action(() =>
                        {
                            ImporterSetting = result.ImporterSetting;
                            txtFilePath.Text = ImporterSetting.InitialDirectory;
                            lblName.Text = ImporterSetting.Name;
                            txtFilePath.Enabled = !LimitAccessFolder;
                            txtFilePath.ReadOnly = LimitAccessFolder;
                            btnFilePath.Enabled = true;
                            txtPatternNo.Enabled = false;
                            btnNumberSearch.Enabled = false;
                            if (!LimitAccessFolder) { ActiveControl = txtFilePath; txtFilePath.Focus(); }
                            ClearStatusMessage();
                            errorFlag = 0;
                            BaseContext.SetFunction05Enabled(false);
                        }));
                        Modified = false;
                    }
                    else
                    {
                        ImporterSetting = new ImporterSetting();
                        txtPatternNo.Clear();
                        lblName.Clear();
                        txtFilePath.Clear();
                        errorFlag = 1;
                        Modified = true;
                        BaseContext.SetFunction05Enabled(true);
                        ShowWarningDialog(MsgWngNotRegistPatternNo, code);
                    }
                }
            });
        }

        #endregion

        #region event handlers
        private void rdoAdd_Click(object sender, EventArgs e)
        {
            gbxBilling.Enabled = false;
            rdoIgnore.Checked = true;
        }

        private void rdoUpdate_Click(object sender, EventArgs e)
        {
            gbxBilling.Enabled = true;
        }

        private void btnNumberSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var result = this.ShowScheduledPaymentImporterSettingSearchDialog();
                if (result != null)
                {
                    txtPatternNo.Text = result.Code;
                    lblName.Text = result.Name;
                }
                ProgressDialog.Start(ParentForm, LoadImporterSettingAsync(txtPatternNo.Text), false, SessionKey);
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

        private int errorFlag { get; set; } = 0;
        private void txtCode_Validated(object sender, EventArgs e)
        {
            try
            {
                ProgressDialog.Start(ParentForm, LoadImporterSettingAsync(txtPatternNo.Text), false, SessionKey);
                if (errorFlag == 1)
                {
                    this.ActiveControl = txtPatternNo;
                    txtPatternNo.Focus();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void rdoPossible_Click(object sender, EventArgs e)
        {
            BaseContext.SetFunction06Enabled(false);
        }

        private void rdoImpossible_Click(object sender, EventArgs e)
        {
            if (lblImpossibleNo.Text != "")
            {
                BaseContext.SetFunction06Enabled(true);
            }
        }

        private void AddHandlers()
        {
            txtPatternNo.TextChanged += new EventHandler(OnContentChanged);
            txtFilePath.TextChanged += new EventHandler(OnContentChanged);
        }

        private void OnContentChanged(object sender, EventArgs e)
        {
            if (!Modified)
            {
                Modified = true;
            }
        }

        #endregion

    }
}