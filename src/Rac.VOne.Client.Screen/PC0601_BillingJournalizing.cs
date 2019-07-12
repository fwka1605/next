using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.BillingService;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Export;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Common.Constants;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>請求仕訳出力</summary>
    public partial class PC0601 : VOneScreenBase
    {
        private int CurrencyId = 0;
        private int Precision { get; set; }
        private string DisplayFormat { get; set; } = "#,##0";
        List<BillingJournalizing> ExtractList { get; set; } = null;
        private List<ColumnNameSetting> ColumnNameList { get; set; }
        private string CellName(string value) => $"cel{value}";
        public PC0601()
        {
            InitializeComponent();
            grdBilling.SetupShortcutKeys();
            Text = "請求仕訳出力";
        }

        #region 初期化
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("抽出");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Extraction;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = Clear;

            BaseContext.SetFunction03Caption("印刷");
            BaseContext.SetFunction03Enabled(true);
            OnF03ClickHandler = Print;

            BaseContext.SetFunction04Caption("出力");
            BaseContext.SetFunction04Enabled(true);
            OnF04ClickHandler = Output;

            BaseContext.SetFunction05Caption("");
            BaseContext.SetFunction05Enabled(false);

            BaseContext.SetFunction06Caption("再出力");
            BaseContext.SetFunction06Enabled(true);
            OnF06ClickHandler = ReOutput;

            BaseContext.SetFunction07Caption("再印刷");
            BaseContext.SetFunction07Enabled(true);
            OnF07ClickHandler = RePrint;

            BaseContext.SetFunction08Caption("取消");
            BaseContext.SetFunction08Enabled(true);
            OnF08ClickHandler = Cancel;

            BaseContext.SetFunction09Caption("");
            BaseContext.SetFunction09Enabled(false);

            BaseContext.SetFunction10Caption("終了");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Exit;
        }

        private void PC0601_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                var loadTask = new List<Task>();
                if (ApplicationControl == null)
                {
                    loadTask.Add(LoadApplicationControlAsync());
                }
                if (Company == null)
                {
                    Task loadCompany = LoadCompanyAsync();
                    loadTask.Add(loadCompany);
                }
                if (ColumnNameList == null)
                    loadTask.Add(LoadColumnNameInfoAsync());

                loadTask.Add(LoadControlColorAsync());
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                InitializeGrid();
                if (!UseForeignCurrency)
                {
                    List<Currency> currencyList = GetCurrencyData(new string[] { DefaultCurrencyCode });
                    if (currencyList != null && currencyList.Any())
                    {
                        CurrencyId = currencyList[0].Id;
                        List<JournalizingSummary> journalList = LoadGridData(0);
                        grdBilling.DataSource = new BindingSource(journalList, null);
                    }
                }
                var serverPath = GetServerPath();
                if (!string.IsNullOrWhiteSpace(serverPath))
                {
                    var fileName = $"請求仕訳_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                    txtWriteFile.Text = Path.Combine(serverPath, fileName);
                }
                lblCurrencyCode.Visible = UseForeignCurrency;
                txtCurrencyCode.Visible = UseForeignCurrency;
                btnCurrencyCode.Visible = UseForeignCurrency;
                txtWriteFile.Enabled = !LimitAccessFolder;
                Clear();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        #region 項目名称情報取得
        private async Task LoadColumnNameInfoAsync()
        {
            ColumnNameSettingsResult result = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ColumnNameSettingMasterClient>();
                result = await service.GetItemsAsync(SessionKey, CompanyId);
            });

            if (result.ProcessResult.Result)
            {
                ColumnNameList = result.ColumnNames;
            }
        }
        #endregion

        /// <summary>管理マスターで設定したサーバパスを取得</summary>
        /// <returns>サーバパス</returns>
        private string GetServerPath()
        {
            string serverPath = null;
            ServiceProxyFactory.LifeTime(factory =>
            {
                var service = factory.Create<GeneralSettingMasterClient>();
                Task<GeneralSettingResult> task = service.GetByCodeAsync(SessionKey, CompanyId, "サーバパス");
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (task.Result.ProcessResult.Result)
                {
                    serverPath = task.Result.GeneralSetting?.Value;
                }
            });
            return serverPath;
        }

        /// <summary> Billing Listを取得 </summary>
        /// <param name="outputFlg"> Flag処理
        /// 0:OutputAt IS NOT NULL
        /// 1:OutputAt IS NULL</param>
        /// <returns> JournalizingSummary List </returns>
        private List<JournalizingSummary> LoadGridData(int outputFlg)
        {
            DateTime? billAtFrom = null;
            DateTime? billAtTo = null;

            if (outputFlg == 1)
            {
                billAtFrom = datBillingFrom.Value;
                billAtFrom = billAtFrom?.Date;
                billAtTo = datBillingTo.Value;
                billAtTo = billAtTo?.Date.AddDays(1).AddMilliseconds(-1);
            }

            var jounalizingList = new List<JournalizingSummary>();
            JournalizingSummariesResult result = null;

            var journalizingTask = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BillingServiceClient>();
                result = await service.GetBillingJournalizingSummaryAsync(SessionKey, outputFlg, CompanyId, CurrencyId, billAtFrom, billAtTo);

                if (result.ProcessResult.Result)
                {
                    jounalizingList = result.JournalizingsSummaries;
                }
            });
            ProgressDialog.Start(ParentForm, Task.WhenAll(journalizingTask), false, SessionKey);

            return jounalizingList;
        }

        private void InitializeGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var widthCcy = UseForeignCurrency ? 80 : 0;
            var widthAmount = 180 - widthCcy;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,          50, "SelectFlag"                                                                               , cell: builder.GetCheckBoxCell()                                    , caption: "選択", readOnly: false             ),
                new CellSetting(height,         135, nameof(JournalizingSummary.OutputAt)    , dataField: nameof(JournalizingSummary.OutputAt)    , cell: builder.GetDateCell_yyyyMMddHHmmss()                         , caption: "仕訳日"                            ),
                new CellSetting(height,         100, nameof(JournalizingSummary.Count)       , dataField: nameof(JournalizingSummary.Count)       , cell: builder.GetNumberCell()                                      , caption: "件数"                              ),
                new CellSetting(height,    widthCcy, nameof(JournalizingSummary.CurrencyCode), dataField: nameof(JournalizingSummary.CurrencyCode), cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), caption: "通貨", visible: UseForeignCurrency ),
                new CellSetting(height, widthAmount, nameof(JournalizingSummary.Amount)      , dataField: nameof(JournalizingSummary.Amount)      , cell: builder.GetTextBoxCurrencyCell(Precision)                    , caption: "金額"                              ),
            });

            grdBilling.Template = builder.Build();
            grdBilling.SetRowColor(ColorContext);
            grdBilling.HideSelection = true;
        }
        #endregion

        #region 抽出
        [OperationLog("抽出")]
        private void Extraction()
        {
            try
            {
                if (!datBillingFrom.ValidateRange(datBillingTo,
                    () => ShowWarningDialog(MsgWngInputRangeChecked, lblBilling.Text))) return;

                var outputAt = new DateTime[0];
                ExtractList = GetBillingJournalizing(outputAt);

                if (ExtractList != null && ExtractList.Any())
                {
                    DispStatusMessage(MsgInfDataExtracted);
                    BaseContext.SetFunction03Enabled(true);
                    BaseContext.SetFunction04Enabled(true);
                    datBillingFrom.Enabled = false;
                    datBillingTo.Enabled = false;
                }
                else
                {
                    ShowWarningDialog(MsgWngNotExistSearchData);
                    BaseContext.SetFunction03Enabled(false);
                    BaseContext.SetFunction04Enabled(false);
                    datBillingFrom.Enabled = true;
                    datBillingTo.Enabled = true;
                }
                var extractBillingAmount = ExtractList.Sum(x => x.BillingAmount);
                lblExtractionNumber.Text = ExtractList.Count.ToString(DisplayFormat);
                lblExtractionAmount.Text = GetExtractBillingPrecision(extractBillingAmount);
                lblOutputNumber.Clear();
                lblOutputAmount.Clear();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region クリア処理
        [OperationLog("クリア")]
        private void Clear()
        {
            this.ActiveControl = txtWriteFile;
            datBillingFrom.Clear();
            datBillingFrom.Enabled = true;
            datBillingTo.Clear();
            datBillingTo.Enabled = true;
            txtCurrencyCode.Clear();
            lblExtractionNumber.Clear();
            lblExtractionAmount.Clear();
            lblOutputAmount.Clear();
            lblOutputNumber.Clear();
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            if (UseForeignCurrency)
            {
                BaseContext.SetFunction01Enabled(false);
                grdBilling.Rows.Clear();
                txtCurrencyCode.Enabled = true;
                btnCurrencyCode.Enabled = true;
            }
            else
            {
                int count = grdBilling.Rows
               .Where(x => Convert.ToBoolean(x[CellName("SelectFlag")].EditedFormattedValue)).Count();
                if (count > 0)
                {
                    BaseContext.SetFunction06Enabled(true);
                    BaseContext.SetFunction07Enabled(true);
                    BaseContext.SetFunction08Enabled(true);
                }
                BaseContext.SetFunction01Enabled(true);
            }
            ClearStatusMessage();
        }
        #endregion

        #region 印刷
        [OperationLog("印刷")]
        private void Print()
        {
            try
            {
                ClearStatusMessage();
                Invoke(new System.Action(() =>
                {
                    var billingJournalizingReport = new BillingJournalizingSectionReport();
                    billingJournalizingReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                    billingJournalizingReport.Name = "請求仕訳_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    billingJournalizingReport.SetData(ExtractList, UseForeignCurrency, Precision, ColumnNameList);

                    ProgressDialog.Start(ParentForm, Task.Run(() =>
                    {
                        billingJournalizingReport.Run(false);
                    }), false, SessionKey);

                    var result = ShowDialogPreview(ParentForm, billingJournalizingReport, GetServerPath());

                    if (result == DialogResult.Cancel)
                    {
                        DispStatusMessage(MsgInfPrintExtractedData);
                    }
                }));
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }
        #endregion 出力

        #region 出力
        [OperationLog("出力")]
        private void Output()
        {
            try
            {
                if (!CheckData()) return;

                string filePath = Path.Combine(txtWriteFile.Text);

                var definition = new BillingJournalizingFileDefinition(new DataExpression(ApplicationControl));

                if (definition.CurrencyCodeField.Ignored = (!UseForeignCurrency))
                {
                    definition.CurrencyCodeField.FieldName = null;
                }

                var decimalFormat = "0" + ((Precision == 0) ? string.Empty : "." + new string('0', Precision));

                definition.BillingAmountField.Format = value => value.ToString(decimalFormat);

                var exporter = definition.CreateExporter();
                exporter.UserId = Login.UserId;
                exporter.UserCode = Login.UserCode;
                exporter.CompanyId = CompanyId;
                exporter.CompanyCode = Login.CompanyCode;

                ProgressDialog.Start(ParentForm, (cancel, progress) =>
                {
                    return exporter.ExportAsync(filePath, ExtractList, cancel, progress);
                }, true, SessionKey);

                if (exporter.Exception != null)
                {
                    if (exporter.Exception.HResult == new UnauthorizedAccessException().HResult)
                    {
                        ShowWarningDialog(MsgErrUnauthorizedAccess);
                        return;
                    }
                    else if (string.IsNullOrEmpty(Path.GetFileName(txtWriteFile.Text)))
                    {
                        ShowWarningDialog(MsgWngInputRequired, "書込ファイル");
                        return;
                    }
                    else
                    {
                        NLogHandler.WriteErrorLog(this, exporter.Exception, SessionKey);
                        ShowWarningDialog(MsgErrSomethingError, "出力");
                        return;
                    }
                }

                UpdateOutputAt();
                List<JournalizingSummary> billingJournalizingList = LoadGridData(0);
                grdBilling.DataSource = new BindingSource(billingJournalizingList, null);
                var extractBillingAmount = ExtractList.Sum(x => x.BillingAmount);
                lblOutputNumber.Text = ExtractList.Count.ToString(DisplayFormat);
                lblOutputAmount.Text = GetExtractBillingPrecision(extractBillingAmount);
                DispStatusMessage(MsgInfFinishDataExtracting);
                BaseContext.SetFunction04Enabled(false);
                BaseContext.SetFunction06Enabled(false);
                BaseContext.SetFunction07Enabled(false);
                BaseContext.SetFunction08Enabled(false);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrSomethingError, "出力");
            }
        }

        private bool CheckData()
        {
            ClearStatusMessage();

            var file = txtWriteFile.Text;

            if (string.IsNullOrWhiteSpace(file))
            {
                ShowWarningDialog(MsgWngInputRequired, lblWriteFile.Text);
                txtWriteFile.Focus();
                return false;
            }

            var specialCharacters = "/:*?|<>\"";
            var index = 0;

            if (file.Length < 3)
            {
                index = file.IndexOfAny(specialCharacters.ToCharArray());
            }
            else
            {
                index = file.Substring(2).IndexOfAny(specialCharacters.ToCharArray());
            }

            try
            {
                if (index == -1)
                {
                    string filePath = Path.GetDirectoryName(file);

                    if (!Directory.Exists(filePath))
                    {
                        if (Directory.Exists(Path.GetPathRoot(file)) && file.Length == 3)
                        {
                            return true;
                        }
                        ShowWarningDialog(MsgErrNotExistsFolderAndCancelProcess, filePath);
                        txtWriteFile.Focus();
                        return false;
                    }
                }
                else
                {
                    ShowWarningDialog(MsgWngInvalidCharacterAtWriteFile);
                    txtWriteFile.Focus();
                    return false;
                }
            }
            catch
            {
                ShowWarningDialog(MsgWngInvalidCharacterAtWriteFile);
                txtWriteFile.Focus();
                return false;
            }

            List<JournalizingSummary> billingJournalizingList = LoadGridData(1);
            var outputBillingAmount = billingJournalizingList.Sum(x => x.Amount);
            var extractBillingAmount = ExtractList.Sum(x => x.BillingAmount);
            if (outputBillingAmount != extractBillingAmount)
            {
                ShowWarningDialog(MsgWngNotEqualAbstractAmountAndUpdateAmount);
                return false;
            }
            return true;
        }

        private void UpdateOutputAt()
        {
            DateTime? dateFrom = datBillingFrom.Value;
            dateFrom = dateFrom?.Date;
            DateTime? dateTo = datBillingTo.Value;
            dateTo = dateTo?.Date.AddDays(1).AddMilliseconds(-1);

            BillingsResult result = null;
            var updateTask = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BillingServiceClient>();
                result = await service.UpdateOutputAtAsync(SessionKey, CompanyId,
                    dateFrom, dateTo, CurrencyId, Login.UserId);
            });
            ProgressDialog.Start(ParentForm, updateTask, false, SessionKey);
        }
        #endregion

        #region 再印刷
        /// <summary> 出力したデータを再印刷　</summary>
        [OperationLog("再印刷")]
        private void RePrint()
        {
            try
            {
                var outputAtArr = grdBilling.Rows
                .Where(x => Convert.ToBoolean(x[CellName("SelectFlag")].Value))
                .Select(x => x.DataBoundItem as JournalizingSummary).Select(x => x.OutputAt).ToArray();
                DateTime[] outputAts = outputAtArr.Cast<DateTime>().ToArray();

                List<BillingJournalizing> rePrintList = GetBillingJournalizing(outputAts);

                Invoke(new System.Action(() =>
                {
                    var billingJournalizingReport = new BillingJournalizingSectionReport();
                    billingJournalizingReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                    billingJournalizingReport.Name = "再出力請求仕訳_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    billingJournalizingReport.SetData(rePrintList, UseForeignCurrency, Precision, ColumnNameList);

                    ProgressDialog.Start(ParentForm, Task.Run(() =>
                    {
                        billingJournalizingReport.Run(false);
                    }), false, SessionKey);

                    var result = ShowDialogPreview(ParentForm, billingJournalizingReport, GetServerPath());

                    if (result == DialogResult.Cancel)
                    {
                        DispStatusMessage(MsgInfReprintedSelectData);
                    }
                }));
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        /// <summary> BillingJournalizing List取得  </summary>
        /// <param name="outputAt"> 仕訳日 </param>
        /// <returns> BillingJournalizing List </returns>
        private List<BillingJournalizing> GetBillingJournalizing(DateTime[] outputAt)
        {
            List<BillingJournalizing> extractBillingList = null;
            DateTime? dateFrom = datBillingFrom.Value;
            dateFrom = dateFrom?.Date;
            DateTime? dateTo = datBillingTo.Value;
            dateTo = dateTo?.Date.AddDays(1).AddMilliseconds(-1);

            BillingJournalizingsResult result = null;
            var extractTask = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BillingServiceClient>();
                result = await service.ExtractBillingJournalizingAsync(SessionKey, CompanyId, dateFrom, dateTo, CurrencyId, outputAt);

                if (result.ProcessResult.Result)
                {
                    extractBillingList = result.BillingJournalizings;
                }
            });
            ProgressDialog.Start(ParentForm, extractTask, false, SessionKey);

            return extractBillingList;
        }
        #endregion

        #region 再出力
        /// <summary> 出力したデータを再出力　</summary>
        [OperationLog("再出力")]
        private void ReOutput()
        {
            ClearStatusMessage();
            try
            {
                var filePath = string.Empty;
                var fileName = $"再出力請求仕訳_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                if (!ShowSaveFileDialog(GetDirectory(), fileName, out filePath,
                        confirmMessagging: () => ShowConfirmDialog(MsgQstConfirmStartXXX, "再出力"),
                        cancellationMessaging: () => DispStatusMessage(MsgInfProcessCanceled))) return;

                var outputAtArr = grdBilling.Rows
                    .Where(x => Convert.ToBoolean(x[CellName("SelectFlag")].Value))
                    .Select(x => x.DataBoundItem as JournalizingSummary).Select(x => x.OutputAt).ToArray();
                DateTime[] outputAts = outputAtArr.Cast<DateTime>().ToArray();

                List<BillingJournalizing> reOutputList = GetBillingJournalizing(outputAts);

                var definition = new BillingJournalizingFileDefinition(new DataExpression(ApplicationControl));

                if (definition.CurrencyCodeField.Ignored = (!UseForeignCurrency))
                {
                    definition.CurrencyCodeField.FieldName = null;
                }

                var decimalFormat = "0" + ((Precision == 0) ? string.Empty : "." + new string('0', Precision));

                definition.BillingAmountField.Format = value => value.ToString(decimalFormat);

                var exporter = definition.CreateExporter();
                exporter.UserId = Login.UserId;
                exporter.UserCode = Login.UserCode;
                exporter.CompanyId = CompanyId;
                exporter.CompanyCode = Login.CompanyCode;

                ProgressDialog.Start(ParentForm, (cancel, progress) =>
                {
                    return exporter.ExportAsync(filePath, reOutputList, cancel, progress);
                }, true, SessionKey);

                if (exporter.Exception != null)
                {
                    NLogHandler.WriteErrorLog(this, exporter.Exception, SessionKey);
                    ShowWarningDialog(MsgErrSomethingError, "出力");
                    return;
                }

                DispStatusMessage(MsgInfFinishedSuccessReOutputProcess);
                Settings.SavePath<Web.Models.BillingJournalizing>(Login, filePath);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrSomethingError, "出力");
            }
        }
        #endregion

        #region 取消
        /// <summary> 出力したデータを取消　</summary>
        [OperationLog("取消")]
        private void Cancel()
        {
            if (!ShowConfirmDialog(MsgQstConfirmCancelJournalizing))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            try
            {
                var model = grdBilling.Rows
                                .Where(x => Convert.ToBoolean(x[CellName("SelectFlag")].Value))
                                .Select(x => x.DataBoundItem as JournalizingSummary).ToList();
                var outputAtArr = model.Select(x => x.OutputAt).ToArray();
                DateTime[] outputAts = outputAtArr.Cast<DateTime>().ToArray();

                BillingsResult result = null;
                var cancelTask = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<BillingServiceClient>();
                    result = await service.CancelBillingJournalizingAsync(SessionKey, CompanyId, CurrencyId, outputAts, Login.UserId);
                });
                ProgressDialog.Start(ParentForm, cancelTask, false, SessionKey);

                if (result.ProcessResult.Result && result.Billings.Any())
                {
                    DispStatusMessage(MsgInfFinishedSuccessJournalizingCancelingProcess);
                    datBillingFrom.Clear();
                    datBillingTo.Clear();
                    datBillingFrom.Enabled = true;
                    datBillingTo.Enabled = true;
                    lblExtractionAmount.Clear();
                    lblExtractionNumber.Clear();
                    lblOutputAmount.Clear();
                    lblOutputNumber.Clear();
                    BaseContext.SetFunction03Enabled(false);
                    BaseContext.SetFunction04Enabled(false);
                    BaseContext.SetFunction06Enabled(false);
                    BaseContext.SetFunction07Enabled(false);
                    BaseContext.SetFunction08Enabled(false);
                    if (UseForeignCurrency)
                    {
                        txtCurrencyCode.Clear();
                        txtCurrencyCode.Enabled = true;
                        btnCurrencyCode.Enabled = true;
                        grdBilling.Rows.Clear();
                        BaseContext.SetFunction01Enabled(false);
                    }
                    else
                    {
                        List<JournalizingSummary> updateList = LoadGridData(0);
                        grdBilling.DataSource = new BindingSource(updateList, null);
                    }
                }
                else
                {
                    ShowWarningDialog(MsgErrSomethingError, "取消");
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region 終了
        [OperationLog("終了")]
        private void Exit()
        {
            ClearStatusMessage();
            BaseForm.Close();
        }
        #endregion

        #region その他Function
        private void btnFilePath_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var filePath = string.Empty;
            var fileName = $"請求仕訳_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            if (!ShowSaveFileDialog(GetDirectory(), fileName, out filePath)) return;
            txtWriteFile.Text = filePath;
        }

        private string GetDirectory()
        {
            var filePath = txtWriteFile.Text;
            var initialDirectory = string.Empty;

            if (string.IsNullOrWhiteSpace(filePath))
            {
                initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                return initialDirectory;
            }

            if (Directory.Exists(filePath))
                return filePath;

            try
            {
                initialDirectory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(initialDirectory))
                {
                    initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }
            }
            catch (Exception)
            {
                initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            return initialDirectory;
        }

        private void txtCurrencyCode_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                if (!string.IsNullOrWhiteSpace(txtCurrencyCode.Text))
                {
                    var currencyList = GetCurrencyData(new string[] { txtCurrencyCode.Text });
                    if (currencyList != null && currencyList.Any())
                    {
                        CurrencyId = currencyList[0].Id;
                        Precision = currencyList[0].Precision;
                        InitializeGrid();
                        var journalList = LoadGridData(0);
                        grdBilling.DataSource = new BindingSource(journalList, null);
                        txtCurrencyCode.Enabled = false;
                        btnCurrencyCode.Enabled = false;
                        BaseContext.SetFunction01Enabled(true);
                    }
                    else
                    {
                        ShowWarningDialog(MsgWngMasterNotExist, "通貨", txtCurrencyCode.Text);
                        txtCurrencyCode.Clear();
                        txtCurrencyCode.Focus();
                        BaseContext.SetFunction01Enabled(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnCurrencyCode_Click(object sender, EventArgs e)
        {
            var currency = this.ShowCurrencySearchDialog();
            if (currency != null)
            {
                txtCurrencyCode.Text = currency.Code;
                CurrencyId = currency.Id;
                Precision = currency.Precision;
                txtCurrencyCode_Validated(this, e);
            }
        }

        /// <summary> 通貨データを取得　</summary>
        /// <param name="code"> 通貨コード　</param>
        /// <returns> Currency　List</returns>
        private List<Currency> GetCurrencyData(string[] code)
        {
            List<Currency> currency = null;
            CurrenciesResult result = null;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CurrencyMasterClient>();
                result = await service.GetByCodeAsync(SessionKey, CompanyId, code);

                if (result.ProcessResult.Result)
                {
                    currency = result.Currencies;
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            return currency;
        }

        /// <summary> Setting value with precision </summary>
        /// <param name="amount"> Value to set precision </param>
        /// <returns> Value with precision </returns>
        private string GetExtractBillingPrecision(decimal amount)
        {
            string billingAmount;
            var displayFormat = "#,###,###,###,##0";
            string displayFormatString = "0";

            if (Precision > 0)
            {
                displayFormat += ".";
                for (int i = 0; i < Precision; i++)
                {
                    displayFormat += displayFormatString;
                }
            }
            billingAmount = amount.ToString(displayFormat);

            return billingAmount;
        }

        private void grdBilling_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            ChangeValueByCheckBox();
        }

        private void grdBilling_CellValueChanged(object sender, CellEventArgs e)
        {
            ChangeValueByCheckBox();
        }

        private void ChangeValueByCheckBox()
        {
            int count = grdBilling.Rows
               .Where(x => Convert.ToBoolean(x[CellName("SelectFlag")].EditedFormattedValue)).Count();

            if (count > 0)
            {
                BaseContext.SetFunction06Enabled(true);
                BaseContext.SetFunction07Enabled(true);
                BaseContext.SetFunction08Enabled(true);
            }
            else
            {
                BaseContext.SetFunction06Enabled(false);
                BaseContext.SetFunction07Enabled(false);
                BaseContext.SetFunction08Enabled(false);
            }
        }

        private void lblExtractionNumber_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, lblExtractionNumber.DisplayRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        private void lblExtractionAmount_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, lblExtractionAmount.DisplayRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        private void lblOutputNumber_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, lblOutputNumber.DisplayRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        private void lblOutputAmount_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, lblOutputAmount.DisplayRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        #endregion
    }
}
