using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.JuridicalPersonalityMasterService;
using Rac.VOne.Client.Screen.KanaHistoryCustomerMasterService;
using Rac.VOne.Client.Screen.KanaHistoryPaymentAgencyMasterService;
using Rac.VOne.Client.Screen.PaymentAgencyMasterService;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Export;
using Rac.VOne.Import;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;
using static Rac.VOne.Web.Models.FunctionType;

namespace Rac.VOne.Client.Screen
{
    /// <summary>学習履歴データ管理</summary>
    public partial class PB1001 : VOneScreenBase
    {
        private bool IsCustomer { get { return rdoCustomer.Checked; } }
        private IEnumerable<string> legalPersonalities { get; set; }

        /// <summary>法人格除去用</summary>
        private async Task<IEnumerable<string>> GetLegalPersonalitiesAsync()
        {
            if (legalPersonalities == null
                && Login != null
                && !string.IsNullOrEmpty(Login.SessionKey))
            {
                await ServiceProxyFactory.LifeTime(async factory =>
                {
                    var client = factory.Create<JuridicalPersonalityMasterClient>();
                    var result = await client.GetItemsAsync(Login.SessionKey,
                        Login.CompanyId);

                    if (result.ProcessResult.Result)
                    {
                        legalPersonalities = result.JuridicalPersonalities.Select(x => x.Kana);
                    }
                });
            }
            return legalPersonalities;
        }

        public PB1001()
        {
            InitializeComponent();
            InitializeUserComponent();
        }

        private void InitializeUserComponent()
        {
            grdKanaHistoryCustomer.SetupShortcutKeys();
            InitializeRadio();
            Text = "学習履歴データ管理";
            txtPayerName.Validated += (sender, e) => txtPayerName.Text = EbDataHelper.ConvertToPayerName(txtPayerName.Text, legalPersonalities);
        }

        private void PB1001_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();

                var tasks = new List<Task>();

                if (Company == null)
                    tasks.Add(LoadCompanyAsync());
                if (ApplicationControl == null)
                    tasks.Add(LoadApplicationControlAsync());
                tasks.Add(LoadControlColorAsync());
                tasks.Add(LoadFunctionAuthorities(MasterImport, MasterExport));
                tasks.Add(GetLegalPersonalitiesAsync());

                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);

                Settings.SetCheckBoxValue<PB1001>(Login, cbxKana);

                SetFormat();
                BaseContext.SetFunction05Enabled(Authorities[MasterImport]);
                BaseContext.SetFunction06Enabled(Authorities[MasterExport]);

                txtPayerName.Focus();
                InitializeKanaHistoryGrid();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        #region 画面の Format & Warning Setting
        private void SetFormat()
        {
            if (ApplicationControl == null) return;
            var expression = new DataExpression(ApplicationControl);
            // 外貨利用を問わず 140 固定
            txtPayerName.MaxLength = 140;
            var format = IsCustomer ? expression.CustomerCodeFormatString : "9";
            var length = IsCustomer ? expression.CustomerCodeLength : 2;
            var ime = IsCustomer ? expression.CustomerCodeImeMode() : ImeMode.Disable;
            var paddingChar = format == "9" ? (char?)'0' : null;

            txtCustomerCodeFrom.Format = format;
            txtCustomerCodeTo.Format = format;
            txtCustomerCodeFrom.MaxLength = length;
            txtCustomerCodeTo.MaxLength = length;
            txtCustomerCodeFrom.ImeMode = ime;
            txtCustomerCodeTo.ImeMode = ime;
            txtCustomerCodeFrom.PaddingChar = paddingChar;
            txtCustomerCodeTo.PaddingChar = paddingChar;
        }


        /// <summary>ラジオボタンの初期選択</summary>
        private void InitializeRadio()
        {
            rdoCustomer.Checked = true;
            rdoPayerNameOrder.Checked = true;
        }
        #endregion

        #region 画面の radio click event
        /// <summary>決済代行会社ラジオボタンのClick　Event<summary>
        private void rdoPaymentAgency_Click(object sender, EventArgs e)
        {
            lblCustomerCode.Text = "決済代行会社コード";
            rdoCustomerCodeOrder.Text = "決済代行会社コード順";
            InitializeKanaHistoryGrid();
            Clear();
        }

        /// <summary>得意先ラジオボタンのClick　Event<summary>
        private void rdoCustomerCode_Click(object sender, EventArgs e)
        {
            lblCustomerCode.Text = "得意先コード";
            rdoCustomerCodeOrder.Text = "得意先コード順";
            InitializeKanaHistoryGrid();
            Clear();
        }
        #endregion

        #region 画面の初期化
        public void InitializeKanaHistoryGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var middleCenter = MultiRowContentAlignment.MiddleCenter;
            var middleRight = MultiRowContentAlignment.MiddleRight;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "Header"                                                                          , cell: builder.GetRowHeaderCell()                                                ),
                new CellSetting(height,  40, "Chk"                                                                             , cell: builder.GetCheckBoxCell()             , caption: "削除", readOnly: false  ),
                new CellSetting(height, 200, "PayerName"        , dataField: nameof(KanaHistoryCustomer.PayerName)             , cell: builder.GetTextBoxCell()              , caption: "振込依頼人名"           ),
                new CellSetting(height, 115, "CustomerCode"     , dataField: nameof(KanaHistoryCustomer.CustomerCode)          , cell: builder.GetTextBoxCell(middleCenter)  , caption: "得意先コード"           ),
                new CellSetting(height, 140, "CustomerName"     , dataField: nameof(KanaHistoryCustomer.CustomerName)          , cell: builder.GetTextBoxCell()              , caption: "得意先名"               ),
                new CellSetting(height, 115, "PaymentAgencyCode", dataField: nameof(KanaHistoryPaymentAgency.PaymentAgencyCode), cell: builder.GetTextBoxCell(middleCenter)  , caption: "決済代行会社コード"     ),
                new CellSetting(height, 140, "PaymentAgencyName", dataField: nameof(KanaHistoryPaymentAgency.PaymentAgencyName), cell: builder.GetTextBoxCell()              , caption: "決済代行会社名"         ),
                new CellSetting(height, 100, "SourceBankName"   , dataField: nameof(KanaHistoryPaymentAgency.SourceBankName)   , cell: builder.GetTextBoxCell()              , caption: "仕向銀行"               ),
                new CellSetting(height, 100, "SourceBranchName" , dataField: nameof(KanaHistoryPaymentAgency.SourceBranchName) , cell: builder.GetTextBoxCell()              , caption: "仕向支店"               ),
                new CellSetting(height,  80, "HitCount"         , dataField: nameof(KanaHistoryPaymentAgency.HitCount)         , cell: builder.GetTextBoxCell(middleRight)   , caption: "個別消込回数"           ),
                new CellSetting(height,   0, "CustomerId"       , dataField: nameof(KanaHistoryCustomer.CustomerId)            , cell: builder.GetTextBoxCell(middleCenter)  , caption: "Id", visible: false     ),
                new CellSetting(height,   0, "PaymentAgencyId"  , dataField: nameof(KanaHistoryPaymentAgency.PaymentAgencyId)  , cell: builder.GetTextBoxCell(middleCenter)  , caption: "Id", visible: false     ),
            });

            if (IsCustomer)
            {
                SetCustomerCellWidth(builder);
            }
            else
            {
                SetPaymentCellWidth(builder);
            }

            grdKanaHistoryCustomer.Template = builder.Build();
            grdKanaHistoryCustomer.HideSelection = true;
        }

        /// <summary>得意先設定</summary>
        private void SetCustomerCellWidth(GcMultiRowTemplateBuilder builder)
        {
            builder.Items[3].Width = 115; //得意先コード
            builder.Items[4].Width = 140; //得意先名
            builder.Items[5].Width = 0; //決済代行会社コード
            builder.Items[6].Width = 0; //決済代行会社名
        }

        /// <summary>決済代行会社設定</summary>
        private void SetPaymentCellWidth(GcMultiRowTemplateBuilder builder)
        {
            builder.Items[3].Width = 0; //得意先コード
            builder.Items[4].Width = 0; //得意先名
            builder.Items[5].Width = 115; //決済代行会社コード
            builder.Items[6].Width = 140; //決済代行会社名
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("検索");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Search;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = () => Clear();

            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = Delete;

            BaseContext.SetFunction04Caption("");
            BaseContext.SetFunction04Enabled(false);

            BaseContext.SetFunction05Caption("インポート");
            OnF05ClickHandler = Import;

            BaseContext.SetFunction06Caption("エクスポート");
            OnF06ClickHandler = Export;

            BaseContext.SetFunction07Caption("");
            BaseContext.SetFunction07Enabled(false);

            BaseContext.SetFunction08Caption("全選択");
            BaseContext.SetFunction08Enabled(false);
            OnF08ClickHandler = SelectAll;

            BaseContext.SetFunction09Caption("全解除");
            BaseContext.SetFunction09Enabled(false);
            OnF09ClickHandler = DeselectAll;

            BaseContext.SetFunction10Caption("終了");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Exit;
        }
        #endregion

        #region  画面の F1
        [OperationLog("検索")]
        private void Search()
        {
            ClearStatusMessage();
            if (!ValidateChildren()) return;
            if (!ValidateForSearch()) return;

            ZeroLeftPaddingWithoutValidated();

            try
            {

                if (rdoCustomer.Checked)
                {
                    txtPayerName.Text = txtPayerName.Text.Trim();
                    Task<List<KanaHistoryCustomer>> task = GetKanaHistoryCustomerAsync();
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);

                    if (!task.Result.Any())
                    {
                        Clear(clearHeader: false);
                        ShowWarningDialog(MsgWngNotExistSearchData);
                    }
                    else
                    {
                        grdKanaHistoryCustomer.DataSource = new BindingSource(task.Result, null);
                        BaseContext.SetFunction03Enabled(true);
                        BaseContext.SetFunction08Enabled(true);
                        BaseContext.SetFunction09Enabled(true);

                        if (rdoPayerNameOrder.Checked)
                        {
                            grdKanaHistoryCustomer.Sort(2);
                        }
                        else if (rdoCustomerCodeOrder.Checked)
                        {
                            grdKanaHistoryCustomer.Sort(3);
                        }
                    }
                }
                else if (rdoPaymentAgency.Checked)
                {
                    txtPayerName.Text = txtPayerName.Text.Trim();
                    Task<List<KanaHistoryPaymentAgency>> task = GetKanaHistoryPaymentAgencyAsync();
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);

                    if (!task.Result.Any())
                    {
                        Clear(clearHeader: false);
                        ShowWarningDialog(MsgWngNotExistSearchData);
                    }
                    else
                    {
                        grdKanaHistoryCustomer.DataSource = new BindingSource(task.Result, null);
                        BaseContext.SetFunction03Enabled(true);
                        BaseContext.SetFunction08Enabled(true);
                        BaseContext.SetFunction09Enabled(true);

                        if (rdoPayerNameOrder.Checked)
                        {
                            grdKanaHistoryCustomer.Sort(4);
                        }
                        else if (rdoCustomerCodeOrder.Checked)
                        {
                            grdKanaHistoryCustomer.Sort(5);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private bool ValidateForSearch()
        {
            if (!txtCustomerCodeFrom.ValidateRange(txtCustomerCodeTo,
                () => { ShowWarningDialog(MsgWngInputRangeChecked, lblCustomerCode.Text); })) return false;
            return true;
        }

        private void ZeroLeftPaddingWithoutValidated()
        {
            int codeLength = IsCustomer ? ApplicationControl.CustomerCodeLength : 2;

            if (IsNeedValidate(ApplicationControl.CustomerCodeType, txtCustomerCodeFrom.TextLength, codeLength))
            {
                txtCustomerCodeFrom.Text = ZeroLeftPadding(txtCustomerCodeFrom);
                txtCustomerCodeFrom_Validated(null, null);
            }
            if (IsNeedValidate(ApplicationControl.CustomerCodeType, txtCustomerCodeTo.TextLength, codeLength))
            {
                txtCustomerCodeTo.Text = ZeroLeftPadding(txtCustomerCodeTo);
                txtCustomerCodeTo_Validated(null, null);
            }
        }

        /// <summary>決済代行会社データ取得処理</summary>
        /// <returns>KanaHistoryPaymentAgency</returns>
        private async Task<List<KanaHistoryPaymentAgency>> GetKanaHistoryPaymentAgencyAsync()
        {
            List<KanaHistoryPaymentAgency> list = null;
            await ServiceProxyFactory.DoAsync<KanaHistoryPaymentAgencyMasterClient>(async client =>
            {
                var result = await client.GetListAsync(SessionKey, CompanyId, txtPayerName.Text, txtCustomerCodeFrom.Text, txtCustomerCodeTo.Text);
                if (result.ProcessResult.Result)
                    list = result.KanaHistoryPaymentAgencys;
            });
            return list ?? new List<KanaHistoryPaymentAgency>();
        }

        /// <summary>得意先データ取得処理</summary>
        /// <returns>KanaHistoryCustomer</returns>
        private async Task<List<KanaHistoryCustomer>> GetKanaHistoryCustomerAsync()
        {
            List<KanaHistoryCustomer> list = null;
            await ServiceProxyFactory.DoAsync<KanaHistoryCustomerMasterClient>(async client =>
            {
                var result = await client.GetListAsync(SessionKey, CompanyId, txtPayerName.Text, txtCustomerCodeFrom.Text, txtCustomerCodeTo.Text);
                if (result.ProcessResult.Result)
                    list = result.KanaHistoryCustomers;
            });
            return list ?? new List<KanaHistoryCustomer>();
        }
        #endregion

        #region 画面の F2
        [OperationLog("クリア")]
        private void Clear(bool clearHeader = true)
        {
            ClearStatusMessage();
            if (clearHeader)
            {
                txtPayerName.Clear();
                txtCustomerCodeFrom.Clear();
                lblCustomerNameFrom.Clear();
                txtCustomerCodeTo.Clear();
                lblCustomerNameTo.Clear();
                txtPayerName.Focus();
                SetFormat();
            }
            grdKanaHistoryCustomer.Rows.Clear();
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);
        }
        #endregion

        #region 画面の F3
        [OperationLog("削除")]
        private void Delete()
        {
            try
            {
                if (!ValidateChildren()) return;

                if (!ValidateDeleteData()) return;

                if (!ShowConfirmDialog(MsgQstConfirmDelete))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                ClearStatusMessage();
                DeleteKanaHistory();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>グリッドの「削除」セルのON / OFFチェック </summary>
        private bool IsChecked(Row row) => Convert.ToBoolean(row["celChk"].Value);

        /// <summary>削除データチェック</summary>
        /// <returns>bool</returns>
        private bool ValidateDeleteData()
        {
            grdKanaHistoryCustomer.EndEdit();
            if (!grdKanaHistoryCustomer.Rows.Any(x => IsChecked(x)))
            {
                ShowWarningDialog(MsgWngSelectAtleastOne);
                return false;
            }

            ClearStatusMessage();
            return true;
        }

        /// <summary>削除データ</summary>
        private void DeleteKanaHistory()
        {
            var companyId = Login.CompanyId;
            var sessionKey = Login.SessionKey;
            CountResult deleteResult = null;
            var target = grdKanaHistoryCustomer.Rows.Where(x => IsChecked(x));

            ProgressDialog.Start(ParentForm, async (cancel, progress) =>
            {
                if (IsCustomer)
                {
                    var items = target.Select(x => x.DataBoundItem as KanaHistoryCustomer);
                    await ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var client = factory.Create<KanaHistoryCustomerMasterClient>();
                        foreach (var item in items)
                        {
                            deleteResult = await client.DeleteAsync(sessionKey, companyId,
                                item.PayerName, item.SourceBankName, item.SourceBranchName, item.CustomerId);
                            if (!deleteResult.ProcessResult.Result
                            || deleteResult.Count == 0) break;
                        }
                    });
                }
                else
                {
                    var items = target.Select(x => x.DataBoundItem as KanaHistoryPaymentAgency);
                    await ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var client = factory.Create<KanaHistoryPaymentAgencyMasterClient>();
                        foreach (var item in items)
                        {
                            deleteResult = await client.DeleteAsync(sessionKey, companyId,
                                item.PayerName, item.SourceBankName, item.SourceBranchName, item.PaymentAgencyId);
                            if (!deleteResult.ProcessResult.Result
                            || deleteResult.Count == 0) break;
                        }
                    });
                }
            }, false, SessionKey);

            if (deleteResult.Count > 0)
            {
                Clear();
                DispStatusMessage(MsgInfDeleteSuccess);
                txtPayerName.Focus();
            }
            else
            {
                ShowWarningDialog(MsgErrDeleteError);
            }
        }
        #endregion

        #region  画面の F5
        [OperationLog("インポート")]
        private void Import()
        {
            ClearStatusMessage();
            try
            {
                if (rdoCustomer.Checked)
                {
                    ImportKanaHistoryCustomer();
                }
                else if (rdoPaymentAgency.Checked)
                {
                    ImportKanaHistoryPaymentAgency();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrImportErrorWithoutLog);
            }
        }

        private void ImportKanaHistoryCustomer()
        {
            ImportSetting importSetting = null;
            var task = Util.GetMasterImportSettingAsync(Login, ImportFileType.KanaHistory);
            // 取込設定取得
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            importSetting = task.Result;

            var definition = new KanaHistoryCustomerFileDefinition(new DataExpression(ApplicationControl));
            definition.CustomerIdField.GetModelsByCode = val =>
            {
                Dictionary<string, Customer> product = null;
                ServiceProxyFactory.LifeTime(factory =>
                {
                    var customerMaster = factory.Create<CustomerMasterClient>();
                    CustomersResult result = customerMaster.GetByCode(
                            Login.SessionKey, Login.CompanyId, val);
                    if (result.ProcessResult.Result)
                    {
                        product = result.Customers
                            .ToDictionary(c => c.Code);
                    }
                });
                return product ?? new Dictionary<string, Customer>();
            };

            var importer = definition.CreateImporter(m => new { m.PayerName, m.SourceBankName, m.SourceBranchName, m.CustomerId });
            importer.UserId = Login.UserId;
            importer.UserCode = Login.UserCode;
            importer.CompanyId = Login.CompanyId;
            importer.CompanyCode = Login.CompanyCode;
            importer.LoadAsync = async () => await GetKanaHistoryCustomerAsync();
            importer.RegisterAsync = async unitOfWork => await RegisterForCustomerImportAsync(unitOfWork);
            importer.InitializeWorker = async worker => worker.SetLegalPersonalities(await GetLegalPersonalitiesAsync());

            var importResult = DoImport(importer, importSetting);
            if (!importResult) return;
            grdKanaHistoryCustomer.Rows.Clear();
        }

        /// <summary>得意先インポートデータ登録</summary>
        /// <param name="imported">CSVから生成したModel</param>
        /// <returns>登録結果</returns>
        private async Task<ImportResult> RegisterForCustomerImportAsync(UnitOfWork<KanaHistoryCustomer> imported)
        {
            ImportResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<KanaHistoryCustomerMasterClient>();
                result = await service.ImportAsync(Login.SessionKey,
                        imported.New.ToArray(), imported.Dirty.ToArray(), imported.Removed.ToArray());
            });

            return result;
        }

        private void ImportKanaHistoryPaymentAgency()
        {
            ImportSetting importSetting = null;
            var task = Util.GetMasterImportSettingAsync(Login, ImportFileType.KanaHistory);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            importSetting = task.Result;

            var definition = new KanaHistoryPaymentAgencyFileDefinition(new DataExpression(ApplicationControl));
            definition.PaymentAgencyIdField.GetModelsByCode = val =>
            {
                Dictionary<string, PaymentAgency> product = null;
                ServiceProxyFactory.LifeTime(factory =>
                {
                    var paymentMaster = factory.Create<PaymentAgencyMasterClient>();
                    PaymentAgenciesResult result = paymentMaster.GetByCode(
                            Login.SessionKey, Login.CompanyId, val);
                    if (result.ProcessResult.Result)
                    {
                        product = result.PaymentAgencies
                            .ToDictionary(c => c.Code);
                    }
                });
                return product ?? new Dictionary<string, PaymentAgency>();
            };

            var importer = definition.CreateImporter(m => new { m.PayerName, m.SourceBankName, m.SourceBranchName, m.PaymentAgencyId });
            importer.UserId = Login.UserId;
            importer.UserCode = Login.UserCode;
            importer.CompanyId = Login.CompanyId;
            importer.CompanyCode = Login.CompanyCode;
            importer.LoadAsync = async () => await GetKanaHistoryPaymentAgencyAsync();
            importer.RegisterAsync = async unitOfWork => await RegisterForPaymentAgencyImportAsync(unitOfWork);

            var importResult = DoImport(importer, importSetting);
            if (!importResult) return;
            grdKanaHistoryCustomer.Rows.Clear();
            rdoCustomer.Focus();

        }

        /// <summary>決済代行会社インポートデータ登録</summary>
        /// <param name="imported">CSVから生成したModel</param>
        /// <returns>登録結果</returns>
        private async Task<ImportResult> RegisterForPaymentAgencyImportAsync(UnitOfWork<KanaHistoryPaymentAgency> imported)
        {
            ImportResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<KanaHistoryPaymentAgencyMasterClient>();
                result = await service.ImportAsync(Login.SessionKey,
                        imported.New.ToArray(), imported.Dirty.ToArray(), imported.Removed.ToArray());
            });

            return result;
        }
        #endregion

        #region  画面の F6
        [OperationLog("エクスポート")]
        private void Export()
        {
            try
            {
                ClearStatusMessage();
                if (rdoCustomer.Checked)
                {
                    ExportKanaHistoryCustomer();
                }
                else if (rdoPaymentAgency.Checked)
                {
                    ExportKanaHistoryPaymentAgency();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                DispStatusMessage(MsgErrExportError);
            }
        }

        /// <summary>得意先エクスポート</summary>
        private void ExportKanaHistoryCustomer()
        {
            string serverPath = null;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<GeneralSettingMasterClient>();
                GeneralSettingResult result = await service.GetByCodeAsync(
                        Login.SessionKey, Login.CompanyId, "サーバパス");

                if (result.ProcessResult.Result)
                {
                    serverPath = result.GeneralSetting?.Value;
                }
            });
            Task<List<KanaHistoryCustomer>> loadTask = GetKanaHistoryCustomerAsync();
            ProgressDialog.Start(ParentForm, Task.WhenAll(task, loadTask), false, SessionKey);
            List<KanaHistoryCustomer> list = loadTask.Result;
            if (!list.Any())
            {
                ShowWarningDialog(MsgWngNoExportData);
                return;
            }

            if (!Directory.Exists(serverPath))
            {
                serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            var filePath = string.Empty;
            var fileName = $"得意先学習履歴{DateTime.Today:yyyyMMdd}.csv";
            if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

            var definition = new KanaHistoryCustomerFileDefinition(new DataExpression(ApplicationControl));
            var exporter = definition.CreateExporter();
            exporter.UserId = Login.UserId;
            exporter.UserCode = Login.UserCode;
            exporter.CompanyId = CompanyId;
            exporter.CompanyCode = Login.CompanyCode;

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

            grdKanaHistoryCustomer.Rows.Clear();
            rdoCustomer.Focus();
            DispStatusMessage(MsgInfFinishExport);
        }

        /// <summary>決済代行会社エクスポート</summary>
        private void ExportKanaHistoryPaymentAgency()
        {
            string serverPath = null;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<GeneralSettingMasterClient>();
                GeneralSettingResult result = await service.GetByCodeAsync(
                        Login.SessionKey, Login.CompanyId, "サーバパス");

                if (result.ProcessResult.Result)
                {
                    serverPath = result.GeneralSetting?.Value;
                }
            });
            Task<List<KanaHistoryPaymentAgency>> loadTask = GetKanaHistoryPaymentAgencyAsync();
            ProgressDialog.Start(ParentForm, Task.WhenAll(task, loadTask), false, SessionKey);
            List<KanaHistoryPaymentAgency> list = loadTask.Result;

            if (!list.Any())
            {
                ShowWarningDialog(MsgWngNoExportData);
                return;
            }

            if (!Directory.Exists(serverPath))
            {
                serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            var filePath = string.Empty;
            var fileName = $"決済代行会社学習履歴{DateTime.Today:yyyyMMdd}.csv";
            if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

            var definition = new KanaHistoryPaymentAgencyFileDefinition(new DataExpression(ApplicationControl));
            var exporter = definition.CreateExporter();
            exporter.UserId = Login.UserId;
            exporter.UserCode = Login.UserCode;
            exporter.CompanyId = CompanyId;
            exporter.CompanyCode = Login.CompanyCode;

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

            grdKanaHistoryCustomer.Rows.Clear();
            rdoCustomer.Focus();
            DispStatusMessage(MsgInfFinishExport);
        }
        #endregion

        #region  画面の F8
        [OperationLog("全選択")]
        private void SelectAll()
        {
            ClearStatusMessage();
            grdKanaHistoryCustomer.EndEdit();
            for (int i = 0; i < grdKanaHistoryCustomer.RowCount; i++)
            {
                (grdKanaHistoryCustomer.Rows[i].Cells[1] as CheckBoxCell).Value = 1;
            }
        }
        #endregion

        #region  画面の F9
        [OperationLog("全解除")]
        private void DeselectAll()
        {
            ClearStatusMessage();
            grdKanaHistoryCustomer.EndEdit();
            for (int i = 0; i < grdKanaHistoryCustomer.RowCount; i++)
            {
                (grdKanaHistoryCustomer.Rows[i].Cells[1] as CheckBoxCell).Value = 0;
            }
        }
        #endregion

        #region 画面の F10
        [OperationLog("終了")]
        private void Exit()
        {
            try
            {
                Settings.SaveControlValue<PB1001>(Login, cbxKana.Name, cbxKana.Checked);
                ParentForm.Close();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region  画面の SearchFromButton Click
        private void btnCustomerCodeFromSearch_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();

            if (rdoPaymentAgency.Checked)
            {
                var paymentAgency = this.ShowPaymentAgencySearchDialog();
                if (paymentAgency != null)
                {
                    SetFromKanaHistory(paymentAgency.Code, paymentAgency.Name);
                }
            }
            else if (rdoCustomer.Checked)
            {
                var customer = this.ShowCustomerMinSearchDialog();
                if (customer != null)
                {
                    SetFromKanaHistory(customer.Code, customer.Name);
                }
            }
        }

        private void SetFromKanaHistory(string code, string name)
        {
            ClearStatusMessage();
            txtCustomerCodeFrom.Text = code;
            lblCustomerNameFrom.Text = name;

            if (cbxKana.Checked)
            {
                txtCustomerCodeTo.Text = code;
                lblCustomerNameTo.Text = name;
            }
        }
        #endregion

        #region  画面の SearchToButton Click
        private void btnCustomerCodeToSearch_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();

            if (rdoPaymentAgency.Checked)
            {
                var paymentAgency = this.ShowPaymentAgencySearchDialog();
                if (paymentAgency != null)
                {
                    SetToKanaHistory(paymentAgency.Code, paymentAgency.Name);
                }
            }
            else if (rdoCustomer.Checked)
            {
                var customer = this.ShowCustomerMinSearchDialog();
                if (customer != null)
                {
                    SetToKanaHistory(customer.Code, customer.Name);
                }
            }
        }

        private void SetToKanaHistory(string code, string name)
        {
            ClearStatusMessage();
            txtCustomerCodeTo.Text = code;
            lblCustomerNameTo.Text = name;
        }
        #endregion

        #region  画面の Sorting Event
        private void rdoPayerNameOrder_Click(object sender, EventArgs e)
        {
            grdKanaHistoryCustomer.Sort(2);
        }

        private void rdoCustomerCodeOrder_Click(object sender, EventArgs e)
        {
            if (rdoCustomer.Checked)
            {
                grdKanaHistoryCustomer.Sort(3);
            }
            else if (rdoPaymentAgency.Checked)
            {
                grdKanaHistoryCustomer.Sort(5);
            }
        }
        #endregion

        #region 画面の CustomerCodeTo Cusorout event
        private async Task<List<Customer>> GetCustomer(string[] CustomerCode)
        {
            List<Customer> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerMasterClient>();
                CustomersResult result = await service.GetByCodeAsync(
                                         Login.SessionKey,
                                         Login.CompanyId,
                                         CustomerCode);

                if (result.ProcessResult.Result)
                {
                    list = result.Customers;
                }
            });
            return list ?? new List<Customer>();
        }

        private async Task<List<PaymentAgency>> GetPaymentAgency(string[] CustomerCode)
        {
            List<PaymentAgency> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<PaymentAgencyMasterClient>();
                PaymentAgenciesResult result = await service.GetByCodeAsync(
                                         Login.SessionKey,
                                         Login.CompanyId,
                                         CustomerCode);
                if (result.ProcessResult.Result)
                {
                    list = result.PaymentAgencies;
                }
            });
            return list ?? new List<PaymentAgency>();
        }

        private void txtCustomerCodeTo_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                if (rdoPaymentAgency.Checked && !string.IsNullOrWhiteSpace(txtCustomerCodeTo.Text))
                {
                    Task<List<PaymentAgency>> task = GetPaymentAgency(new string[] { txtCustomerCodeTo.Text });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                    List<PaymentAgency> paymentAgencyList = task.Result;
                    if (!paymentAgencyList.Any())
                    {
                        ShowWarningDialog(MsgWngMasterNotExist, "決済代行会社", txtCustomerCodeTo.Text);
                        txtCustomerCodeTo.Clear();
                        lblCustomerNameTo.Clear();
                        txtCustomerCodeTo.Focus();
                    }
                    else
                    {
                        lblCustomerNameTo.Text = paymentAgencyList[0].Name;
                    }
                }
                else if (rdoCustomer.Checked && !string.IsNullOrWhiteSpace(txtCustomerCodeTo.Text))
                {
                    Task<List<Customer>> task = GetCustomer(new string[] { txtCustomerCodeTo.Text });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                    List<Customer> customerList = task.Result;
                    if (!customerList.Any())
                    {
                        ShowWarningDialog(MsgWngMasterNotExist, "得意先", txtCustomerCodeTo.Text);
                        txtCustomerCodeTo.Clear();
                        lblCustomerNameTo.Clear();
                        txtCustomerCodeTo.Focus();
                    }
                    else
                    {
                        lblCustomerNameTo.Text = customerList[0].Name;
                    }
                }
                else
                {
                    txtCustomerCodeTo.Clear();
                    lblCustomerNameTo.Clear();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtCustomerCodeFrom_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                if (rdoPaymentAgency.Checked && !string.IsNullOrWhiteSpace(txtCustomerCodeFrom.Text))
                {
                    Task<List<PaymentAgency>> task = GetPaymentAgency(new string[] { txtCustomerCodeFrom.Text });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                    List<PaymentAgency> paymentAgencyList = task.Result;
                    if (!paymentAgencyList.Any())
                    {
                        if (cbxKana.Checked)
                        {
                            ShowWarningDialog(MsgWngMasterNotExist, "決済代行会社", txtCustomerCodeFrom.Text);
                            txtCustomerCodeFrom.Clear();
                            txtCustomerCodeTo.Clear();
                            lblCustomerNameFrom.Clear();
                            lblCustomerNameTo.Clear();
                            txtCustomerCodeFrom.Focus();
                        }
                        else
                        {
                            ShowWarningDialog(MsgWngMasterNotExist, "決済代行会社", txtCustomerCodeFrom.Text);
                            txtCustomerCodeFrom.Clear();
                            lblCustomerNameFrom.Clear();
                            txtCustomerCodeFrom.Focus();
                        }
                    }
                    else
                    {
                        lblCustomerNameFrom.Text = paymentAgencyList[0].Name;
                        if (cbxKana.Checked)
                        {
                            txtCustomerCodeTo.Text = paymentAgencyList[0].Code;
                            lblCustomerNameTo.Text = paymentAgencyList[0].Name;
                        }
                    }
                }
                else if (rdoCustomer.Checked && !string.IsNullOrWhiteSpace(txtCustomerCodeFrom.Text))
                {
                    Task<List<Customer>> task = GetCustomer(new string[] { txtCustomerCodeFrom.Text });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                    List<Customer> customerList = task.Result;
                    if (!customerList.Any())
                    {
                        if (cbxKana.Checked)
                        {
                            ShowWarningDialog(MsgWngMasterNotExist, "得意先", txtCustomerCodeFrom.Text);
                            txtCustomerCodeFrom.Clear();
                            txtCustomerCodeTo.Clear();
                            lblCustomerNameFrom.Clear();
                            lblCustomerNameTo.Clear();
                            txtCustomerCodeFrom.Focus();
                        }
                        else
                        {
                            ShowWarningDialog(MsgWngMasterNotExist, "得意先", txtCustomerCodeFrom.Text);
                            txtCustomerCodeFrom.Clear();
                            lblCustomerNameFrom.Clear();
                            txtCustomerCodeFrom.Focus();
                        }
                    }
                    else
                    {
                        lblCustomerNameFrom.Text = customerList[0].Name;
                        if (cbxKana.Checked)
                        {
                            txtCustomerCodeTo.Text = customerList[0].Code;
                            lblCustomerNameTo.Text = customerList[0].Name;
                        }
                    }
                }
                else
                {
                    if (cbxKana.Checked)
                    {
                        txtCustomerCodeFrom.Clear();
                        lblCustomerNameFrom.Clear();
                        txtCustomerCodeTo.Clear();
                        lblCustomerNameTo.Clear();
                    }
                    else
                    {
                        txtCustomerCodeFrom.Clear();
                        lblCustomerNameFrom.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

    }
}