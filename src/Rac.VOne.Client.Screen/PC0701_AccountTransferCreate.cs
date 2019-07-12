using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Reports;
using Rac.VOne.AccountTransfer.Export;
using Rac.VOne.Client.Screen.AccountTransferService;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.PaymentAgencyMasterService;
using Rac.VOne.Common;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Common.Constants;
using static Rac.VOne.Message.Constants;
using Log = Rac.VOne.Web.Models.AccountTransferLog;

namespace Rac.VOne.Client.Screen
{
    /// <summary>
    /// 口座振替依頼データ作成
    /// </summary>
    public partial class PC0701 : VOneScreenBase
    {

        #region local member / properties
        private List<PaymentAgency> PaymentAgencies { get; set; }
        private List<PaymentFileFormat> PaymentFileFormats { get; set; }

        private List<AccountTransferDetail> _extractRawData;
        private List<AccountTransferDetail> ExtractRawData
        {
            get { return _extractRawData; }
            set
            {
                _extractRawData = value;
                ExtractAggregateData =
                    (_extractRawData != null && DoAggregate)
                    ? _extractRawData.AggregateByKey().ToList()
                    : _extractRawData;
            }
        }
        private List<AccountTransferDetail> ExtractAggregateData { get; set; }

        private string OutputDirectory { get; set; }

        private  PaymentFileFormat SelectedFileFormat { get; set; }

        /// <summary>
        /// 口座振替 口座単位で集計する
        /// </summary>
        private bool DoAggregate { get { return Company?.TransferAggregate == 1; } }

        #endregion

        #region initialization
        public PC0701()
        {
            InitializeComponent();
            InitializeUserComponent();
        }

        private void InitializeUserComponent()
        {
            grid.SetupShortcutKeys();
            InitializeHandlers();
            Text = "口座振替依頼データ作成";
            datNewDueAt2nd.Enabled = false;
        }

        private void InitializeHandlers()
        {
            this.Load += PC0701_Load;
            btnOutputFilePath.Click += btnOutputFilePath_Click;
            cmbCollectCategory.SelectedIndexChanged += cmbCollectCategory_SelectedIndexChanged;
            grid.CellEditedFormattedValueChanged += grid_CellEditedFormattedValueChanged;
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("抽出");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Extract;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = Clear;

            BaseContext.SetFunction03Caption("印刷");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = PrintReport;

            BaseContext.SetFunction04Caption("出力");
            BaseContext.SetFunction04Enabled(false);
            OnF04ClickHandler = Output;

            BaseContext.SetFunction06Caption("再出力");
            BaseContext.SetFunction06Enabled(false);
            OnF06ClickHandler = ReOutput;

            BaseContext.SetFunction07Caption("再印刷");
            BaseContext.SetFunction07Enabled(false);
            OnF07ClickHandler = RePrintReport;

            BaseContext.SetFunction08Caption("取消");
            BaseContext.SetFunction08Enabled(false);
            OnF08ClickHandler = Cancel;

            BaseContext.SetFunction10Caption("終了");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Close;

        }

        private void PC0701_Load(object sender, EventArgs e)
        {
            SetScreenName();
            InitializeGridTemplate();
            ProgressDialog.Start(ParentForm, InitializeAsync(), false, SessionKey);

        }

        private async Task InitializeAsync()
        {
            try
            {
                var tasks = new List<Task>();
                if (Company == null)
                    tasks.Add(LoadCompanyAsync());
                if (ApplicationControl == null)
                    tasks.Add(LoadApplicationControlAsync());
                tasks.Add(LoadControlColorAsync());
                tasks.Add(LoadPaymentAgencyAsync());
                tasks.Add(LoadPaymentFileFormatAsync());
                var path = await Util.GetGeneralSettingServerPathAsync(Login);
                OutputDirectory = Util.GetDirectoryName(path);
                await Task.WhenAll(tasks);
                await SetAccountTransferLogAsync();
                var comboList = await GetCollectCategoryAsync();
                foreach (var category in comboList)
                    cmbCollectCategory.Items.Add(new GrapeCity.Win.Editors.ListItem(
                        category.CodeAndName, category.Id, category.PaymentAgencyId));
                txtOutputFilePath.Enabled = !LimitAccessFolder;
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void InitializeGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext, sortable: true);
            var height = builder.DefaultRowHeight;
            var scale = 0;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  50, nameof(Log.Checked)     , nameof(Log.Checked)     , "選択", readOnly: false, cell: builder.GetCheckBoxCell(isBoolType: true)),
                new CellSetting(height,  80, nameof(Log.RequestDate) , nameof(Log.RequestDate) , "作成日", cell:builder.GetDateCell_yyyyMMdd()),
                new CellSetting(height, 100, nameof(Log.CollectCategoryCodeAndName) , nameof(Log.CollectCategoryCodeAndName) , "回収区分"),
                new CellSetting(height,  80, nameof(Log.DueAt)       , nameof(Log.DueAt)       , "引落日", cell:builder.GetDateCell_yyyyMMdd()),
                new CellSetting(height,  80, nameof(Log.OutputCount) , nameof(Log.OutputCount) , "件数", cell:builder.GetTextBoxCurrencyCell(0)),
                new CellSetting(height, 100, nameof(Log.OutputAmount), nameof(Log.OutputAmount), "金額", cell:builder.GetTextBoxCurrencyCell(scale)),
            });
            grid.Template = builder.Build();
            grid.SetRowColor(ColorContext);
            grid.HideSelection = true;
        }

        #endregion

        #region function keys methods

        [OperationLog("抽出")]
        private void Extract()
        {
            ClearStatusMessage();
            if (!ValidateForExtract()) return;

            ProgressDialog.Start(ParentForm, LoadExtractDataAsync(), false, SessionKey);
            if (ExtractRawData == null)
            {
                ShowWarningDialog(MsgWngNotExistSearchData);
                return;
            }

            var count = 0;
            var invalidCount = 0;
            var amount = 0M;
            foreach (var detail in ExtractAggregateData)
            {
                count++;
                amount += detail.BillingAmount;
                if (!detail.Valid)
                {
                    invalidCount++;
                    Console.WriteLine(detail.DisplayBankName);
                }
            }
            dlblExtractCount.Text = count.ToString("#,##0");
            dlblExtractAmount.Text = amount.ToString("#,##0");
            dlblInvalidCount.Text = invalidCount.ToString("#,##0");
            if (count == 0)
            {
                ShowWarningDialog(MsgWngNotExistSearchData);
                return;
            }
            BaseContext.SetFunction01Enabled(false);
            cmbCollectCategory.Enabled = false;
            datDueAtFrom.Enabled = false;
            datDueAtTo.Enabled = false;


            var exportable = (invalidCount == 0);
            BaseContext.SetFunction03Enabled(true);
            BaseContext.SetFunction04Enabled(exportable);

        }

        private bool ValidateForExtract()
        {
            if (!cmbCollectCategory.ValidateInputted(
                () => ShowWarningDialog(MsgWngSelectionRequired, lblCollectCategory.Text))) return false;

            if (!datDueAtFrom.ValidateRange(datDueAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblDueAt.Text))) return false;

            return true;
        }


        [OperationLog("クリア")]
        private void Clear()
        {
            ClearStatusMessage();

            //if (!ShowConfirmDialog(MsgQstConfirmClear)) return;

            ExtractRawData?.Clear();
            ExtractRawData = null;

            cmbCollectCategory.SelectedIndex = -1;
            datDueAtFrom.Clear();
            datDueAtTo.Clear();
            datNewDueAt.Clear();
            datNewDueAt2nd.Clear();
            datNewDueAt2nd.Enabled = false;

            txtOutputFilePath.Clear();
            dlblExtractCount.Clear();
            dlblExtractAmount.Clear();
            dlblInvalidCount.Clear();
            dlblOutputCount.Clear();
            dlblOutputAmount.Clear();

            cmbCollectCategory.Enabled = true;
            datDueAtFrom.Enabled = true;
            datDueAtTo.Enabled = true;

            datNewDueAt.Enabled = true;
            txtOutputFilePath.Enabled = (!LimitAccessFolder);

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);

            SelectedFileFormat = null;

            ProgressDialog.Start(ParentForm, SetAccountTransferLogAsync(), false, SessionKey);

            cmbCollectCategory.Focus();

        }

        [OperationLog("印刷")]
        private void PrintReport()
        {
            ClearStatusMessage();

            if (!(ExtractAggregateData?.Any() ?? false))
            {
                return;
            }

            try
            {
                var report = new AccountTransferImportReport();
                var title = "口座振替依頼データ作成";
                report.lblTitle.Text = title;
                report.lblTransferResult.Text = "出力可否";
                report.txtTransferResult.DataField = nameof(AccountTransferImportReport.ReportRow.CreateResult);
                report.Name = $"{title}_{DateTime.Today:yyyyMMdd_HHmmss}";
                IEnumerable<AccountTransferImportReport.ReportRow> printSource = null;
                ProgressDialog.Start(ParentForm, Task.Run(() =>
                {
                    printSource = ConvertToReportRow(ExtractAggregateData);
                }), false, SessionKey);

                report.SetBasicPageSetting(Company.Code, Company.Name);
                report.SetData(printSource);
                report.Run(false);

                ShowDialogPreview(ParentForm, report, OutputDirectory);
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        private IEnumerable<AccountTransferImportReport.ReportRow> ConvertToReportRow(IEnumerable<AccountTransferDetail> source)
            => source.Select(x => new AccountTransferImportReport.ReportRow
            {
                IsImport = false,
                Billing = x.BilledAt.HasValue && x.SalesAt.HasValue
                && x.ClosingAt.HasValue && x.DueAt.HasValue ? new Billing
                {
                    InvoiceCode     = x.InvoiceCode,
                    Note1           = x.Note1,
                    BilledAt        = x.BilledAt.Value,
                    SalesAt         = x.SalesAt.Value,
                    ClosingAt       = x.ClosingAt.Value,
                    DueAt           = x.DueAt.Value,
                    BillingAmount   = x.BillingAmount,
                    CustomerCode    = x.CustomerCode,
                    CustomerName    = x.CustomerName,
                } : null,
                TransferResultCode = 0,
                TransferAmount = x.BillingAmount,
                TransferBankName = x.DisplayBankName,
                TransferBranchName = x.TransferBranchName,
                TransferCustomerCode = x.TransferCustomerCode,
                TransferAccountName = x.TransferAccountName,
                CreateResult = x.Valid ? "" : "不",
            });

        [OperationLog("出力")]
        private void Output()
        {
            ClearStatusMessage();

            if (!ValidateForOutput()) return;
            var dueAt = datNewDueAt.Value.Value;
            foreach (var detail in ExtractRawData)
            {
                detail.CreateBy = Login.UserId;
                detail.NewDueAt = dueAt;
                detail.DueAt2nd = datNewDueAt2nd.Value;
            }
            //  入金予定日の 大小検証処理

            var filePath = txtOutputFilePath.Text;
            // ファイル出力
            //  // ファイル出力失敗時のメッセージ
            var exporter = GetExporter(SelectedPaymentAgencyId(), dueAt, datNewDueAt2nd.Value);
            var exportSuccess = false;
            try
            {
                exporter.Export(filePath, ExtractAggregateData);
                exportSuccess = true;
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }

            if (!exportSuccess)
            {
                ShowWarningDialog(MsgErrExportError);
                return;
            }

            var task = SaveOutputLogAsync(ExtractRawData);
            // 更新処理
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            // 失敗した場合 web service 側での rollback
            //  //出力済ファイルの削除（例外握り潰し）
            if (!task.Result)
            {
                try
                {
                    System.IO.File.Delete(filePath);
                }
                catch
                {
                }
                // 他ユーザーに云々 再度抽出云々
                ShowWarningDialog(MsgErrUpdateError);
                return;
            }

            // 成功した場合 メッセージ処理/履歴再表示/ファンクションキー制御など
            var count = 0;
            var amount = 0M;
            foreach (var detail in ExtractAggregateData)
            {
                count++;
                amount += detail.BillingAmount;
            }
            dlblOutputCount.Text = count.ToString("#,##0");
            dlblOutputAmount.Text = amount.ToString("#,##0");

            BaseContext.SetFunction04Enabled(false); //出力直後の再出力禁止
            datNewDueAt.Enabled = false;
            txtOutputFilePath.Enabled = false;
            ProgressDialog.Start(ParentForm, SetAccountTransferLogAsync(), false, SessionKey);

            DispStatusMessage(MsgInfProcessFinish);
        }

        private bool ValidateForOutput()
        {
            if (!txtOutputFilePath.ValidateInputted(
                () => ShowWarningDialog(MsgWngInputRequired, lblOutputFilePath.Text))) return false;
            if (!Util.ValidateFilePath(txtOutputFilePath.Text))
            {
                txtOutputFilePath.Focus();
                ShowWarningDialog(MsgWngInvalidCharacterAtWriteFile);
                return false;
            }

            var path = txtOutputFilePath.Text;
            if (path.Length < 3)
            {
                txtOutputFilePath.Focus();
                ShowWarningDialog(MsgWngInvalidSaveFileNameLength);
                return false;
            }

            var dirExist = true;
            var dir = string.Empty;
            try
            {
                dir = System.IO.Path.GetDirectoryName(path);
                dirExist = System.IO.Directory.Exists(dir);
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            if (!dirExist)
            {
                txtOutputFilePath.Focus();
                ShowWarningDialog(MsgErrNotExistsFolderAndCancelProcess, dir);
                return false;
            }

            if (!datNewDueAt.ValidateInputted(
                () => ShowWarningDialog(MsgWngInputRequired, lblNewDueAt.Text))) return false;

            if (SelectedFileFormat.Id == (int)AccountTransferFileFormatId.InternetJPBankFixed
                && !datNewDueAt2nd.ValidateInputted(
                () => ShowWarningDialog(MsgWngInputRequired, lblNewDueAt2nd.Text))) return false;
            return true;
        }

        private IAccountTransferExporter GetExporter(int? paymentAgencyId,
            DateTime dueAt,
            DateTime? dueAt2nd)
        {
            ExporterBase exporter = null;
            var agency = GetSelectedPaymentAgency(paymentAgencyId);
            if (agency == null) return exporter;
            var formatId = (AccountTransferFileFormatId)agency.FileFormatId;
            switch (formatId)
            {
                case AccountTransferFileFormatId.ZenginCsv:
                case AccountTransferFileFormatId.RicohLeaseCollectCsv:
                case AccountTransferFileFormatId.RisonaNetCsv:
                    exporter = new ZenginCommaExporter();
                    break;
                case AccountTransferFileFormatId.ZenginFixed:
                    exporter = new ZenginFixedExporter();
                    break;
                case AccountTransferFileFormatId.MizuhoFactorWebFixed:
                    exporter = new MizuhoFactorExporter();
                    break;
                case AccountTransferFileFormatId.MitsubishiUfjFactorCsv:
                    exporter = new MUFJFactorExporter();
                    break;
                case AccountTransferFileFormatId.SMBCFixed:
                    exporter = new SMBCKoufuriExporter();
                    break;
                case AccountTransferFileFormatId.MitsubishiUfjNicosCsv:
                    exporter = new MUFJNicosExporter();
                    break;
                case AccountTransferFileFormatId.MizuhoFactorAspCsv:
                    exporter = new MizuhoAspExporter();
                    break;
                case AccountTransferFileFormatId.InternetJPBankFixed:
                    exporter = new InternetJPBankExporter { DueAt2nd = dueAt2nd.Value, };
                    break;
            }
            if (exporter == null) return null;
            exporter.Company = Company;
            exporter.PaymentAgency = agency;
            exporter.DueAt = dueAt;
            exporter.LogError = exception => NLogHandler.WriteErrorLog(exporter, exception, SessionKey);
            return exporter;
        }

        private async Task<bool> SaveOutputLogAsync(IEnumerable<AccountTransferDetail> details)
        {
            var result = false;
            try
            {
                var webResult = await SaveLogAsync(details);
                if (webResult.ProcessResult.Result)
                    result = true;
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            return result;
        }

        [OperationLog("再出力")]
        private void ReOutput()
        {
            var ids = GetSelectedLogIds();
            if (!ids.Any())
            {
                grid.Focus();
                ShowWarningDialog(MsgWngSelectionRequired, "再出力を行う履歴");
                return;
            }
            try
            {
                foreach (var id in ids)
                {
                    IEnumerable<AccountTransferDetail> source = null;
                    var task = Task.Run(async () =>
                    {
                        source = await GetAccountTransferDetailAsync( GetReOutputSearchOption( new long[] { id } ));
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                    var first = source.First();
                    var agency = GetSelectedPaymentAgency(first.PaymentAgencyId);

                    var path = string.Empty;
                    var file = agency.GetOutputFileName(reoutput: true);
                    if (!ShowSaveFileDialog(OutputDirectory, file, out path))
                    {
                        DispStatusMessage(MsgInfProcessCanceled);
                        return;
                    }
                    var exporter = GetExporter(agency.Id,
                        first.NewDueAt.Value,
                        first.DueAt2nd);
                    try
                    {
                        exporter.Export(path, source);
                    }
                    catch
                    {
                        DispStatusMessage(MsgErrExportError);
                        throw;
                    }
                }
                DispStatusMessage(MsgInfProcessFinish);
            }
            catch (Exception ex)
            {
                DispStatusMessage(MsgErrExportError);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("再印刷")]
        private void RePrintReport()
        {
            ClearStatusMessage();

            var ids = GetSelectedLogIds();
            if (!ids.Any())
            {
                grid.Focus();
                ShowWarningDialog(MsgWngSelectionRequired, "再印刷を行う履歴");
                return;
            }

            try
            {
                var report = new AccountTransferImportReport();
                var title = "口座振替依頼データ作成";
                report.lblTitle.Text = title;
                report.lblTransferResult.Text = "出力可否";
                report.txtTransferResult.DataField = nameof(AccountTransferImportReport.ReportRow.CreateResult);
                report.Name = $"{title}_{DateTime.Today:yyyyMMdd_HHmmss}";
                IEnumerable<AccountTransferImportReport.ReportRow> printSource = null;
                ProgressDialog.Start(ParentForm, Task.Run(() =>
                {
                    var getTask = GetAccountTransferDetailAsync(GetReOutputSearchOption(ids));
                    var source = getTask.Result;
                    printSource = ConvertToReportRow(source);
                }), false, SessionKey);

                report.SetBasicPageSetting(Company.Code, Company.Name);
                report.SetData(printSource);
                report.Run(false);

                ShowDialogPreview(ParentForm, report, OutputDirectory);
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        [OperationLog("取消")]
        private void Cancel()
        {
            ClearStatusMessage();
            if (!ShowConfirmDialog(MsgQstConfirmStartXXX, "取消"))
                return;

            var ids = GetSelectedLogIds();
            if (!ids.Any())
            {
                grid.Focus();
                ShowWarningDialog(MsgWngSelectionRequired, "取消を行う履歴");
                return;
            }
            try
            {
                ProgressDialog.Start(ParentForm, CancelLogAsync(ids), false, SessionKey);
                ProgressDialog.Start(ParentForm, SetAccountTransferLogAsync(), false, SessionKey);
                DispStatusMessage(MsgInfProcessFinish);
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrSomethingError, "取消");
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("終了")]
        private void Close()
        {
            ParentForm.Close();
        }

        #endregion

        #region event handlers

        private void cmbCollectCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combo = sender as Common.Controls.VOneComboControl;
            var nothing = (combo == null || combo.SelectedItem == null);
            SelectedFileFormat = null;
            if (nothing)
            {
                txtPaymentAgencyCode.Clear();
                lblPaymentAgencyName.Clear();
                lblFileFormatName.Clear();
            }
            else
            {
                var agency = GetSelectedPaymentAgency(SelectedPaymentAgencyId());
                if (agency != null)
                {
                    txtPaymentAgencyCode.Text = agency.Code;
                    lblPaymentAgencyName.Text = agency.Name;
                    try
                    {
                        txtOutputFilePath.Text = System.IO.Path.Combine(OutputDirectory, agency.GetOutputFileName());
                    }
                    catch (Exception ex)
                    {
                        NLogHandler.WriteErrorLog(this, ex, SessionKey);
                    }
                    SelectedFileFormat = GetPaymentFileFormat(agency.FileFormatId);
                    if (SelectedFileFormat != null)
                    {
                        lblFileFormatName.Text = SelectedFileFormat.Name;
                        if (SelectedFileFormat.Id == (int)AccountTransferFileFormatId.InternetJPBankFixed)
                        {
                            datNewDueAt2nd.Enabled = true;
                        }
                        else
                        {
                            datNewDueAt2nd.Enabled = false;
                            datNewDueAt2nd.Clear();
                        }
                    }
                }
            }
        }
        private int? SelectedCategroyId()
            => cmbCollectCategory.SelectedItem == null
            || cmbCollectCategory.SelectedItem.SubItems.Count < 3
            ? (int?)null : (int)cmbCollectCategory.SelectedItem.SubItems[1].Value;
        private int? SelectedPaymentAgencyId()
            => cmbCollectCategory.SelectedItem == null
            || cmbCollectCategory.SelectedItem.SubItems.Count < 3
            ? (int?)null : (int)cmbCollectCategory.SelectedItem.SubItems[2].Value;

        private PaymentAgency GetSelectedPaymentAgency(int? id)
            => id.HasValue ? PaymentAgencies?.FirstOrDefault(x => x.Id == id.Value) : null;

        private PaymentFileFormat GetPaymentFileFormat(int id)
            => PaymentFileFormats?.FirstOrDefault(x => x.Id == id);

        private void btnOutputFilePath_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var filePath = txtOutputFilePath.Text;
            var dir = Util.GetDirectoryName(filePath);
            var fileName = Util.GetFileName(filePath);
            if (!ShowSaveFileDialog(dir, fileName, out filePath)) return;
            txtOutputFilePath.Text = filePath;
        }
        private void grid_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            grid.EndEdit();
            var hasAnyChecked = grid.Rows.Select(x => x.DataBoundItem as Log)?.Any(x => x.Checked) ?? false;
            BaseContext.SetFunction06Enabled(hasAnyChecked);
            BaseContext.SetFunction07Enabled(hasAnyChecked);
            BaseContext.SetFunction08Enabled(hasAnyChecked);
        }


        #endregion

        #region web service

        private async Task<List<Log>> GetAccountTransferLogAsync()
        {
            List<Log> list = null;
            await ServiceProxyFactory.DoAsync<AccountTransferServiceClient>(async client =>
            {
                var result = await client.GetAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    list = result.AccountTransferLog;
            });
            return list;
        }

        private async Task<List<AccountTransferDetail>> GetAccountTransferDetailAsync(AccountTransferSearch SearchOption)
        {
            List<AccountTransferDetail> list = null;
            await ServiceProxyFactory.DoAsync<AccountTransferServiceClient>(async client =>
            {
                var result = await client.ExtractAsync(SessionKey, SearchOption);
                if (result.ProcessResult.Result)
                    list = result.AccountTransferDetail;
            });
            return list ?? new List<AccountTransferDetail>();
        }
        private AccountTransferSearch GetExtractSearchOption()
            => new AccountTransferSearch
            {
                CompanyId           = CompanyId,
                CollectCategoryId   = SelectedCategroyId().Value,
                DueAtFrom           = datDueAtFrom.Value,
                DueAtTo             = datDueAtTo.Value,
            };

        private AccountTransferSearch GetReOutputSearchOption(long[] ids)
            => new AccountTransferSearch
            {
                CompanyId               = CompanyId,
                AccountTransferLogIds   = ids,
            };

        private long[] GetSelectedLogIds()
            => grid.Rows.Select(x => x.DataBoundItem as Log)
                .Where(x => x.Checked)
                .Select(x => x.Id).ToArray();
        private async Task LoadExtractDataAsync()
            => ExtractRawData = await GetAccountTransferDetailAsync(GetExtractSearchOption());

        private async Task<List<Category>> GetCollectCategoryAsync()
        {
            List<Category> list = null;
            await ServiceProxyFactory.DoAsync<CategoryMasterClient>(async client =>
            {
                var result = await client.GetItemsAsync(SessionKey, new CategorySearch
                {
                    CompanyId           = CompanyId,
                    CategoryType        = CategoryType.Collect,
                    UseAccountTransfer  = 1,
                });
                if (result.ProcessResult.Result)
                    list = result.Categories;
            });
            return list;
        }

        private async Task<List<PaymentAgency>> GetPaymentAgencyAsync()
        {
            List<PaymentAgency> list = null;
            await ServiceProxyFactory.DoAsync<PaymentAgencyMasterClient>(async client =>
            {
                var result = await client.GetItemsAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    list = result.PaymentAgencies;
            });
            return list;
        }
        private async Task<List<PaymentFileFormat>> GetPaymentFileFormatAsync()
        {
            List<PaymentFileFormat> list = null;
            await ServiceProxyFactory.DoAsync<PaymentAgencyMasterClient>(async client =>
            {
                var result = await client.GetFormatItemsAsync(SessionKey);
                if (result.ProcessResult.Result)
                    list = result.PaymentFileFormats;
            });
            return list;
        }

        private async Task LoadPaymentAgencyAsync()
            => PaymentAgencies = await GetPaymentAgencyAsync();
        private async Task LoadPaymentFileFormatAsync()
            => PaymentFileFormats = await GetPaymentFileFormatAsync();
        private async Task SetAccountTransferLogAsync()
            => grid.DataSource = new BindingSource(await GetAccountTransferLogAsync(), null);

        private async Task<AccountTransferDetailsResult> SaveLogAsync(IEnumerable<AccountTransferDetail> details)
            => await ServiceProxyFactory.DoAsync(async (AccountTransferServiceClient client) =>
            {
                AccountTransferDetailsResult result = null;
                result = await client.SaveAsync(SessionKey, details.ToArray(), DoAggregate);
                return result;
            });

        private async Task CancelLogAsync(long[] ids)
            => await ServiceProxyFactory.DoAsync(async (AccountTransferServiceClient client)
                => await client.CancelAsync(SessionKey, ids, Login.UserId));

        #endregion

    }
}
