using Rac.VOne.Client.Common;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.Importer;
using Rac.VOne.Client.Screen.ImporterSettingService;
using Rac.VOne.Export;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Common.Constants;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>請求フリーインポーター</summary>
    public partial class PC0101 : VOneScreenBase
    {
        private string Filename { get; set; }
        private string Note1 { get; set; } = "";

        private ImporterSetting ImporterSetting { get; set; } = new ImporterSetting();
        private BillingImporter Importer { get; set; }

        public PC0101()
        {
            InitializeComponent();
            Text = "請求フリーインポーター";
            AddHandlers();
        }

        #region　画面の初期化
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("読込");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = ReadBillingCSV;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = ClearData;

            BaseContext.SetFunction03Caption("印刷");
            BaseContext.SetFunction03Enabled(true);
            OnF03ClickHandler = PrintBillingData;

            BaseContext.SetFunction04Caption("登録");
            BaseContext.SetFunction04Enabled(true);
            OnF04ClickHandler = SaveBillingData;

            BaseContext.SetFunction05Caption("取込設定");
            BaseContext.SetFunction05Enabled(true);
            OnF05ClickHandler = OpenSetting;

            BaseContext.SetFunction06Caption("出力");
            BaseContext.SetFunction06Enabled(true);
            OnF06ClickHandler = OutputError;

            BaseContext.SetFunction07Caption("得意先印刷");
            BaseContext.SetFunction07Enabled(false);
            OnF07ClickHandler = PrintNewCustomer;

            BaseContext.SetFunction08Caption("得意先出力");
            BaseContext.SetFunction08Enabled(false);
            OnF08ClickHandler = OutputNewCustomer;

            BaseContext.SetFunction10Caption("終了");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = ExitForm;
        }

        private void PC0101_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                var loadTask = new List<Task>();
                if (Company == null) loadTask.Add(LoadCompanyAsync());
                if (ApplicationControl == null) loadTask.Add(LoadApplicationControlAsync());
                loadTask.Add(LoadColumnNameSetting());
                loadTask.Add(LoadControlColorAsync());
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
                Importer = new BillingImporter(Login, ApplicationControl);

                if (UseForeignCurrency)
                {
                    dlblImportAmount.Visible = false;
                    lblImportAmount.Visible = false;
                }
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

        private void AddHandlers()
        {
            foreach (Control control in this.GetAll<Common.Controls.VOneTextControl>())
            {
                control.TextChanged += new EventHandler(OnContentChanged);
            }
        }

        private void OnContentChanged(object sender, EventArgs e)
        {
            Modified = true;
        }
        #endregion

        #region Function Key Event

        [OperationLog("読込")]
        private void ReadBillingCSV()
        {
            ClearStatusMessage();
            if (!ValidateForRead()) return;

            ReadCsv();
            Filename = txtFilePath.Text;
        }

        private bool ValidateForRead()
        {
            Action<Common.Controls.VOneTextControl, string, string> invalidHandler = (txt, id, arg1) =>
            {
                ShowWarningDialog(id, arg1, null);
                this.ActiveControl = txt;
            };

            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                invalidHandler(txtCode, MsgWngInputRequired, "パターンNo.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtFilePath.Text))
            {
                invalidHandler(txtFilePath, MsgWngInputRequired, "取込ファイル名");
                return false;
            }
            if (!File.Exists(txtFilePath.Text))
            {
                invalidHandler(txtFilePath, MsgWngOpenFileNotFound, null);
                return false;
            }
            return true;
        }

        #region 読込処理

        private void ReadCsv()
        {
            Importer.FilePath = txtFilePath.Text;
            Importer.ImporterSettingId = ImporterSetting.Id;

            NLogHandler.WriteDebug(this, "請求フリーインポーター 読込処理開始");
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
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            NLogHandler.WriteDebug(this, "請求フリーインポーター 読込処理終了");

            if (exOuter != null)
            {
                NLogHandler.WriteErrorLog(this, exOuter, SessionKey);
                ShowWarningDialog(MsgErrReadingError);
                return;
            }
            dlblReadingCount.Text = Importer.ReadCount.ToString("#,##0");
            dlblImportableCount.Text = Importer.ValidCount.ToString("#,##0");
            dlblUnImportableCount.Text = Importer.InvalidCount.ToString("#,##0");
            dlblNewCustomerCount.Text = Importer.NewCustomerCreationCount.ToString("#,##0");

            var hasNewCustomer = Importer.NewCustomerCreationCount > 0;

            if (Importer.ReadCount == 0) /* 読込件数ゼロ スキップした分は省く */
            {
                ShowWarningDialog(MsgWngNoImportData);
            }
            else if (Importer.ValidCount > 0 && Importer.InvalidCount == 0) /* 無効なし、有効のみ 望ましい形 */
            {
                BaseContext.SetFunction01Enabled(false);
                txtFilePath.Enabled = false;
                btnFilePath.Enabled = false;
                BaseContext.SetFunction03Enabled(true);
                BaseContext.SetFunction04Enabled(true);
                BaseContext.SetFunction07Enabled(hasNewCustomer);
                BaseContext.SetFunction08Enabled(hasNewCustomer);
                rdoPrintPossible.Checked = true;
                gbxPrint.Enabled = false;
            }
            else if (Importer.ValidCount == 0 && Importer.InvalidCount > 0) /* 有効なし、無効のみ */
            {
                BaseContext.SetFunction03Enabled(true);
                BaseContext.SetFunction06Enabled(true);
                BaseContext.SetFunction07Enabled(hasNewCustomer);
                BaseContext.SetFunction08Enabled(hasNewCustomer);
                rdoPrintImpossible.Checked = true;
                gbxPrint.Enabled = false;

            }
            else if (Importer.ValidCount > 0 && Importer.InvalidCount > 0) /* 有効・無効混在 */
            {
                BaseContext.SetFunction03Enabled(true);
                BaseContext.SetFunction06Enabled(rdoPrintImpossible.Checked);
                BaseContext.SetFunction07Enabled(hasNewCustomer);
                BaseContext.SetFunction08Enabled(hasNewCustomer);
                gbxPrint.Enabled = true;
            }
            else /* すべてスキップなど 読込はしたが、取り込むデータなし エラーもなし */
            {
                ShowWarningDialog(MsgWngNoImportData);
                gbxPrint.Enabled = false;
            }
        }
        #endregion

        [OperationLog("クリア")]
        private void ClearData()
        {
            ClearStatusMessage();
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear)) return;
            Clear();
        }

        private void Clear()
        {
            txtCode.Text = "";
            disLblCode.Text = "";
            txtFilePath.Text = "";
            txtFilePath.Enabled = false;
            btnFilePath.Enabled = false;
            btnCode.Enabled = true;
            txtCode.Enabled = true;
            txtCode.Focus();
            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Enabled(true);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            gbxPrint.Enabled = false;
            rdoPrintPossible.Checked = true;
            dlblReadingCount.Text = "";
            dlblImportableCount.Text = "";
            dlblUnImportableCount.Text = "";
            dlblNewCustomerCount.Text = "";
            dlblImportCount.Text = "";
            dlblImportAmount.Text = "";
            Modified = false;
        }

        [OperationLog("印刷")]
        private void PrintBillingData()
        {
            try
            {
                ClearStatusMessage();

                BillingImporterSectionReport report = null;
                var path = string.Empty;
                ProgressDialog.Start(ParentForm, Task.Run(() =>
                {
                    path = Util.GetGeneralSettingServerPathAsync(Login).Result;
                    var source = Importer.GetReportSource(rdoPrintPossible.Checked);
                    report = new BillingImporterSectionReport();
                    report.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);

                    var title = $"請求フリーインポーター　取込{(rdoPrintPossible.Checked ? "可能" : "不可能")}データ一覧";
                    report.Name = title + DateTime.Now.ToString("yyyyMMdd");
                    report.SetData(source, title, UseForeignCurrency, Note1);
                    report.Run(false);
                }), false, SessionKey);

                ShowDialogPreview(ParentForm, report, path);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        [OperationLog("登録")]
        private void SaveBillingData()
        {
            try
            {
                ClearStatusMessage();
                var confirmMessage = Importer.DoOverWrite ? MsgQstConfirmDoOverWrite : MsgQstConfirmSave;
                if (!ShowConfirmDialog(confirmMessage))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }
                ProgressDialog.Start(ParentForm, SaveBillingImport(), false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task SaveBillingImport()
        {
            NLogHandler.WriteDebug(this, "請求フリーインポーター 取込処理開始");
            var result = await Importer.ImportAsync();
            NLogHandler.WriteDebug(this, "請求フリーインポーター 取込処理終了");
            if (result)
            {
                dlblImportAmount.Text = Importer.SaveAmount.ToString("#,##0");
                dlblImportCount.Text = Importer.SaveCount.ToString("#,##0");
                var fileMoveResult = Importer.MoveFile();
                BaseContext.SetFunction04Enabled(false);
                DispStatusMessage(MsgInfProcessFinish);
                Modified = false;
            }
            else
            {
                ShowWarningDialog(MsgErrImportErrorWithoutLog);
            }
        }

        [OperationLog("出力")]
        private void OutputError()
        {
            ClearStatusMessage();
            try
            {
                var dir = Path.GetDirectoryName(Filename);
                var fileName = $"請求フリーインポーター{DateTime.Today:yyyyMMdd}.log";
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
            catch (Exception)
            {
                DispStatusMessage(MsgErrSomethingError, "取込不可能ログデータの出力");
            }
        }

        [OperationLog("取込設定")]
        private void OpenSetting()
        {
            var form = ApplicationContext.Create(nameof(PC0102));
            var result = ApplicationContext.ShowDialog(BaseForm, form);
        }

        [OperationLog("得意先印刷")]
        private void PrintNewCustomer()
        {
            try
            {
                ClearStatusMessage();

                BillingImporterNewCustomerSectionReport report = null;
                var path = string.Empty;
                ProgressDialog.Start(ParentForm, Task.Run(() =>
                {
                    path = Util.GetGeneralSettingServerPathAsync(Login).Result;
                    var source = Importer.GetBillingImportListForNewCustomers();
                    report = new BillingImporterNewCustomerSectionReport();
                    report.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);

                    var title = $"請求フリーインポーター　新規得意先一覧";
                    report.Name = title + DateTime.Now.ToString("yyyyMMdd");
                    report.SetData(source, title, UseForeignCurrency, Note1);
                    report.Run(false);
                }), false, SessionKey);

                ShowDialogPreview(ParentForm, report, path);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        [OperationLog("得意先出力")]
        private void OutputNewCustomer()
        {
            try
            {
                List<Customer> list = Importer.GetNewCustomers();
                if (!list.Any()) return;

                var serverPath = string.Empty;
                ProgressDialog.Start(ParentForm, Task.Run(() =>
                {
                    serverPath = Util.GetGeneralSettingServerPathAsync(Login).Result;
                }), false, SessionKey);

                if (!Directory.Exists(serverPath))
                    serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                var filePath = string.Empty;
                var fileName = $"新規得意先{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var definition = new CustomerFileDefinition(new DataExpression(ApplicationControl));
                var exporter = definition.CreateExporter();
                exporter.UserId = Login.UserId;
                exporter.UserCode = Login.UserCode;
                exporter.CompanyId = CompanyId;
                exporter.CompanyCode = Login.CompanyCode;

                Func<int[], Dictionary<int, Category>> getter = ids =>
                {
                    Dictionary<int, Category> product = null;
                    ServiceProxyFactory.LifeTime(factory =>
                    {
                        var categoryservice = factory.Create<CategoryMasterClient>();
                        CategoriesResult result = categoryservice.Get(SessionKey, ids);
                        if (result.ProcessResult.Result)
                        {
                            product = result.Categories.ToDictionary(c => c.Id);
                        }
                    });
                    return product ?? new Dictionary<int, Category>();
                };
                definition.CollectCategoryIdField.GetModelsById = getter;

                NLogHandler.WriteDebug(this, "新規得意先 出力開始");
                ProgressDialog.Start(ParentForm, (cancel, progress) =>
                {
                    return exporter.ExportAsync(filePath, list, cancel, progress);
                }, true, SessionKey);

                if (exporter.Exception != null)
                {
                    NLogHandler.WriteErrorLog(this, exporter.Exception, SessionKey);
                    ShowWarningDialog(MsgErrExportError);
                    return;
                }

                NLogHandler.WriteDebug(this, "新規得意先 出力終了");

                DispStatusMessage(MsgInfFinishExport);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                DispStatusMessage(MsgErrExportError);
            }
        }

        [OperationLog("終了")]
        private void ExitForm()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;
            ParentForm.Close();
        }
        #endregion

        #region Webサービス呼び出し
        private async Task LoadImporterSetting()
        {
            var result = new ImporterSettingResult();
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ImporterSettingServiceClient>();
                result = await service.GetHeaderByCodeAsync(SessionKey, CompanyId, (int)FreeImporterFormatType.Billing, txtCode.Text);
            });
            if (result.ProcessResult.Result)
            {
                ImporterSetting = result.ImporterSetting;
            }
        }

        private async Task LoadColumnNameSetting()
        {
            await Util.LoadColumnNameSettingAsync(Login, nameof(Billing), settings => {
                Note1 = settings.FirstOrDefault(x => x.ColumnName == "Note1")?.DisplayColumnName ?? "備考1";
            });
        }

        #endregion

        #region event handlers

        private void txtCode_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            try
            {
                if (txtCode.Text != "")
                {
                    ProgressDialog.Start(ParentForm, LoadImporterSetting(), false, SessionKey);
                    if (ImporterSetting != null)
                    {
                        txtFilePath.Text = ImporterSetting.InitialDirectory;

                        disLblCode.Text = ImporterSetting.Name;
                        txtFilePath.Enabled = !LimitAccessFolder;
                        txtFilePath.ReadOnly = LimitAccessFolder;
                        txtCode.Enabled = false;
                        btnCode.Enabled = false;
                        btnFilePath.Enabled = true;
                        if (!LimitAccessFolder) ActiveControl = txtFilePath;
                        BaseContext.SetFunction05Enabled(false);
                        Modified = false;
                    }
                    else
                    {
                        ShowWarningDialog(MsgWngNotRegistPatternNo, txtCode.Text);
                        txtCode.Text = "";
                        disLblCode.Text = "";
                        txtFilePath.Text = "";
                        this.ActiveControl = txtCode;
                        BaseContext.SetFunction05Enabled(true);
                        Modified = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnCode_Click(object sender, EventArgs e)
        {
            ImporterSetting = this.ShowBillingImporterSettingSearchDialog();
            if (ImporterSetting != null)
            {
                txtCode.Text = ImporterSetting.Code;
                disLblCode.Text = ImporterSetting.Name;
                txtFilePath.Text = ImporterSetting.InitialDirectory;
                txtFilePath.Enabled = !LimitAccessFolder;
                btnFilePath.Enabled = true;
                txtCode.Enabled = false;
                btnCode.Enabled = false;
                if (!LimitAccessFolder) ActiveControl = txtFilePath;
                BaseContext.SetFunction05Enabled(false);
                Modified = false;
            }
        }

        private void btnFilePath_Click(object sender, EventArgs e)
        {
            var path = txtFilePath.Text.Trim();
            var fileNames = new List<string>();
            if (!LimitAccessFolder ? 
                !ShowOpenFileDialog(path, out fileNames) : 
                !ShowRootFolderBrowserDialog(ApplicationControl.RootPath, out fileNames, FolderBrowserType.SelectFile)) return;

            txtFilePath.Text = fileNames?.FirstOrDefault() ?? string.Empty;
        }

        private void rdoPrint_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == rdoPrintPossible)
            {
                BaseContext.SetFunction06Enabled(false);
            }
            else if (sender == rdoPrintImpossible)
            {
                BaseContext.SetFunction06Enabled(true);
            }
        }

        #endregion

    }
}
