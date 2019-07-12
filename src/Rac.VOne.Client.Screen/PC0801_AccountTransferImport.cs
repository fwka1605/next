using GrapeCity.Win.Editors;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Reports;
using Rac.VOne.AccountTransfer.Import;
using Rac.VOne.AccountTransfer.Import.Reader;
using Rac.VOne.Client.Screen.Dialogs;
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
    /// <summary>口座振替結果データ取込</summary>
    public partial class PC0801 : VOneScreenBase
    {
        #region メンバー

        /// <summary>
        /// 回収区分一覧
        /// </summary>
        private List<Category> CollectionCategoryList;

        /// <summary>
        /// 決済代行会社一覧
        /// </summary>
        private List<PaymentAgency> PaymentAgencyList;

        /// <summary>
        /// 取込フォーマット一覧
        /// </summary>
        private List<PaymentFileFormat> PaymentFileFormatList;

        private uint _AmountPrecision;
        /// <summary>
        /// 金額表示精度
        /// 外貨は使用しない想定なのでマスタからは取得せず固定値(0)とする。
        /// </summary>
        public uint AmountPrecision
        {
            get
            {
                return _AmountPrecision;
            }
            private set
            {
                _AmountPrecision = value;

                AmountDisplayFormat = "#,##0";
                if (value != 0)
                {
                    AmountDisplayFormat += "." + new string('0', (int)value);
                }
            }
        }

        /// <summary>
        /// 金額表示フォーマット
        /// </summary>
        private string AmountDisplayFormat = "#,##0";

        /// <summary>
        /// 整数値(カウント値など)表示フォーマット
        /// </summary>
        private string IntegerDisplayFormat = "#,##0";

        private string GetCountDisplayValue(int value) => string.Format($"{{0:{IntegerDisplayFormat}}}", value);

        private string GetAmountDisplayValue(decimal value) => string.Format($"{{0:{AmountDisplayFormat}}}", value);

        /// <summary>
        /// 取込・照合済データ
        /// </summary>
        private IEnumerable<AccountTransferSource> MatchingResultList;

        #endregion

        #region クラス定義 MatchingResultViewModel

        /// <summary>
        /// MatchingResultクラスに画面表示用の項目を追加
        /// </summary>
        public class MatchingResultViewModel : AccountTransferSource
        {
            public string CustomerCode { get { return Billings?.FirstOrDefault()?.CustomerCode ?? "---"; } }
            public string CustomerName { get { return Billings?.FirstOrDefault()?.CustomerName ?? "得意先不明"; } }
            public string TransferResult { get { return TransferResultCode == 0 ? "振替済" : "振替不能"; } }

            public MatchingResultViewModel(AccountTransferSource mr)
                : base(mr.Billings, mr.TransferResultCode, mr.TransferAmount)
            {
            }
        }

        #endregion

        #region 初期化

        public PC0801()
        {
            InitializeComponent();

            Text = "口座振替結果データ取込";
            AmountPrecision = 0;
            grdErrorDataList.SetupShortcutKeys();
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("取込");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = F01_LoadResultFile;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = F02_ClearScreen;

            BaseContext.SetFunction03Caption("印刷");
            BaseContext.SetFunction03Enabled(true);
            OnF03ClickHandler = F03_OutputReport;

            BaseContext.SetFunction04Caption("登録");
            BaseContext.SetFunction04Enabled(true);
            OnF04ClickHandler = F04_ImportData;

            BaseContext.SetFunction10Caption("終了");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = F10_Exit;
        }

        private void PC0801_Load(object sender, EventArgs e)
        {
            SetScreenName();

            var loadTask = new List<Task>
            {
                LoadApplicationControlAsync(),
                LoadCompanyAsync(),
                LoadControlColorAsync(),
                LoadCollectionCategoryListAsync(),
                LoadPaymentAgencyListAsync(),
                LoadPaymentFileFormatListAsync(),
            };
            ProgressDialog.Start(BaseForm, Task.WhenAll(loadTask), false, SessionKey);

            InitializeComboBox();
            InitializeGrid();
            SetInitialValue();

            ClearScreen();
        }

        private async Task LoadCollectionCategoryListAsync()
        {
            var list = await GetCategoryListAsync(SessionKey, CompanyId, 3);
            CollectionCategoryList = list.OrderBy(c => c.Code).ToList();
        }

        private async Task LoadPaymentAgencyListAsync()
        {
            PaymentAgencyList = await GetPaymentAgencyListAsync(SessionKey, CompanyId, null);
        }

        private async Task LoadPaymentFileFormatListAsync()
        {
            PaymentFileFormatList = await GetPaymentFileFormatListAsync(SessionKey);
        }

        private void InitializeComboBox()
        {
            if (CollectionCategoryList == null)
            {
                return;
            }

            foreach (var ctg in CollectionCategoryList)
            {
                cmbCollectCategory.Items.Add(new ListItem(ctg.CodeAndName, ctg.Id));
            }

            cmbCollectCategory.TextSubItemIndex = 0;
            cmbCollectCategory.ValueSubItemIndex = 1;
        }

        private void InitializeGrid()
        {
            grdErrorDataList.Template = GetGridTemplate();
            grdErrorDataList.HideSelection = true;
        }

        private Template GetGridTemplate()
        {
            var template = new Template();

            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var rowHeight = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(rowHeight,  90, "CustomerCode",   caption: "得意先コード", dataField: "CustomerCode",   cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
                new CellSetting(rowHeight, 627, "CustomerName",   caption: "得意先名",     dataField: "CustomerName",   cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft)),
                new CellSetting(rowHeight, 120, "TransferAmount", caption: "金額",         dataField: "TransferAmount", cell: builder.GetNumberCellCurrency((int)AmountPrecision, (int)AmountPrecision, 0)),
                new CellSetting(rowHeight, 120, "TransferResult", caption: "振替結果",     dataField: "TransferResult", cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
            });

            return builder.Build();
        }

        private void SetInitialValue()
        {
            txtTransferYear.Text = DateTime.Today.Year.ToString();
        }

        #endregion 初期化

        #region ファンクションキー
        [OperationLog("取込")]
        private void F01_LoadResultFile() // Load: Read & Match
        {
            ClearStatusMessage();

            try
            {
                var error = ValidateForLoading();
                if (error != null)
                {
                    if (error is ValidationError<Control>)
                    {
                        var actualError = (ValidationError<Control>)error;
                        if (actualError.FocusTarget != null)
                        {
                            actualError.FocusTarget.Focus();
                        }
                    }

                    ShowWarningDialog(error.MessageId, error.MessageArgs);
                    return;
                }

                IEnumerable<AccountTransferSource> matchingResultList = null; // ここでインスタンスフィールドのMatchingResultListをクリアしてしまうと制御上問題があるのでローカル変数を使う
                var task = Task.Run(() =>
                {
                    try
                    {
                        matchingResultList = LoadTransferResultFile();
                    }
                    catch (FormatException ex)
                    {
                        NLogHandler.WriteErrorLog(this, ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Debug.Fail(ex.Message);
                        NLogHandler.WriteErrorLog(this, ex, SessionKey);
                    }
                });
                ProgressDialog.Start(BaseForm, task, false, SessionKey);

                if (matchingResultList == null)
                {
                    ShowWarningDialog(MsgErrEBFileImportError);
                    return;
                }
                if (matchingResultList.IsEmpty())
                {
                    ShowWarningDialog(MsgWngNoDataAccountTransfer);
                    return;
                }

                MatchingResultList = matchingResultList; // 印刷・登録用に取り込み結果を保持

                var mrvmList = MatchingResultList?.Select(mr =>
                    new MatchingResultViewModel(mr)
                );

                var transferredRows = mrvmList.Where(mrvm => mrvm.TransferResultCode == 0);   // 振替済データ
                var transferErrorRows = mrvmList.Where(mrvm => mrvm.TransferResultCode != 0); // 振替不能データ
                var matchingErrorRows = mrvmList.Where(mrvm => !(mrvm.Billings?.Any() ?? false)); // 照合不能データ

                BaseContext.SetFunction03Enabled(true);

                dlblReadCount.Text = GetCountDisplayValue(mrvmList.Count());
                dlblTransferredCount.Text = GetCountDisplayValue(transferredRows.Count());
                dlblTransferredAmount.Text = GetAmountDisplayValue(transferredRows.Sum(r => r.TransferAmount));
                dlblTransferErrorCount.Text = GetCountDisplayValue(transferErrorRows.Count());
                dlblTransferErrorAmount.Text = GetAmountDisplayValue(transferErrorRows.Sum(r => r.TransferAmount));

                // 照合不能または振替不能データが存在する場合
                if (transferErrorRows.Any() || matchingErrorRows.Any())
                {
                    grdErrorDataList.DataSource =
                        // 表示順序を変えたくないので再検索
                        new BindingSource(mrvmList.Where(r => r.TransferResultCode != 0 || !(r.Billings?.Any() ?? false)), null);
                }

                // 全て照合済の場合は登録可能
                if (matchingErrorRows.IsEmpty())
                {
                    txtPaymentAgencyCode.Enabled = false;
                    btnPaymentAgency.Enabled = false;
                    txtImportFilePath.Enabled = false;
                    btnImportFilePath.Enabled = false;
                    BaseContext.SetFunction01Enabled(false);
                    BaseContext.SetFunction04Enabled(true);

                    grdErrorDataList.DataSource =
                        // transferErrorRowsが空の場合にグリッドに空行が表示される(？)ので、
                        // エラーが全く無い場合はnullをセットする
                        transferErrorRows.Any() ? new BindingSource(transferErrorRows, null) : null;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);

                ShowWarningDialog(MsgErrEBFileImportError);
            }
        }

        [OperationLog("クリア")]
        private void F02_ClearScreen()
        {
            try
            {
                ClearScreen();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("印刷")]
        private void F03_OutputReport()
        {
            ClearStatusMessage();

            if (MatchingResultList == null || MatchingResultList.IsEmpty())
            {
                return;
            }

            try
            {
                var report = new AccountTransferImportReport()
                {
                    Name = $"口座振替結果データ一覧_{DateTime.Now:yyyyMMdd_HHmmss}"
                };

                var paymentFileFormatId = PaymentAgencyList.Single(pa => pa.Code == txtPaymentAgencyCode.Text).FileFormatId;
                var serverPath = "";

                ProgressDialog.Start(ParentForm, Task.Run(() =>
                {
                    var reportRowList = MatchingResultList.Select(mr =>
                    {
                        Billing billing = null;
                        if (mr.Billings != null) // 照合済の場合
                        {
                            var minimumId = mr.Billings.Min(b => b.Id); // 複数時はIdが一番若い請求データ

                            billing = mr.Billings.Single(b => b.Id == minimumId);
                        }

                        return new AccountTransferImportReport.ReportRow
                        {
                            // 請求書情報 ※ 登録後の更新値ではなく照合時の値を出力
                            Billing = billing,
                            // 口座振替情報(ファイル記述情報)
                            TransferResultCode      = mr.TransferResultCode,
                            TransferAmount          = mr.TransferAmount,
                            TransferBankName        = mr.TransferBankName,
                            TransferBranchName      = mr.TransferBranchName,
                            TransferCustomerCode    = mr.TransferCustomerCode,
                            TransferAccountName     = mr.TransferAccountName,
                        };
                    });

                    // 帳票生成
                    report.SetBasicPageSetting(Company.Code, Company.Name);
                    report.SetData(reportRowList);
                    report.Run(false);

                    // サーバパス設定取得
                    serverPath = GetGeneralSetting(SessionKey, CompanyId, "サーバパス").Value;
                }), false, SessionKey);

                ShowDialogPreview(ParentForm, report, serverPath);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        [OperationLog("登録")]
        private void F04_ImportData()
        {
            ClearStatusMessage();

            try
            {
                var error = ValidateForImporting();
                if (error != null)
                {
                    if (error is ValidationError<Control>)
                    {
                        var actualError = (ValidationError<Control>)error;
                        if (actualError.FocusTarget != null)
                        {
                            actualError.FocusTarget.Focus();
                        }
                    }

                    ShowWarningDialog(error.MessageId, error.MessageArgs);
                    return;
                }

                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    return;
                }

                var paymentAgency = PaymentAgencyList.Single(pa => pa.Code == txtPaymentAgencyCode.Text);

                int? collectCategoryId = null;
                if (paymentAgency.ConsiderUncollected != 0)
                {
                    collectCategoryId = (int)cmbCollectCategory.SelectedValue;
                }


                var importDataList = MatchingResultList.Select(mr => new AccountTransferImportData
                {
                    BillingIdList                   = mr.Billings.Select(b => b.Id),
                    CustomerIds                     = mr.Billings.Select(x => x.CustomerId).ToArray(),
                    ResultCode                      = mr.TransferResultCode,
                    DoUpdateAccountTransferLogId    = !mr.IgnoreInitialization,
                    DueDate                         = mr.NewDueAt,
                    UpdateBy                        = Login.UserId,
                });

                var task = ImportAccountTransferResultAsync(
                    SessionKey,
                    importDataList,
                    paymentAgency.DueDateOffset,
                    cbxConsiderUncollected.Checked ? (int?)cmbCollectCategory.SelectedValue : null);

                ProgressDialog.Start(BaseForm, task, false, SessionKey);

                if (!task.Result)
                {
                    ShowWarningDialog(MsgErrSaveError);
                    return;
                }

                BaseContext.SetFunction04Enabled(false);
                ShowWarningDialog(MsgInfSaveSuccess);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);

                ShowWarningDialog(MsgErrSaveError);
            }
        }

        [OperationLog("終了")]
        private void F10_Exit()
        {
            BaseForm.Close();
        }
        #endregion

        #region 画面イベント

        /// <summary>
        /// 決済手段コード入力
        /// </summary>
        private void txtPaymentAgencyCode_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPaymentAgencyCode.Text))
            {
                return;
            }

            var paymentAgency = PaymentAgencyList?.FirstOrDefault(pa => pa.Code == txtPaymentAgencyCode.Text);
            if (paymentAgency == null)
            {
                ShowWarningDialog(MsgWngMasterNotExist, "決済手段", txtPaymentAgencyCode.Text);
                txtPaymentAgencyCode.Clear();
                txtPaymentAgencyName.Clear();
                txtImportFileFormat.Clear();
                txtImportFilePath.Enabled = false;
                txtTransferYear.Enabled = false;
                return;
            }

            txtPaymentAgencyName.Text = paymentAgency.Name;

            txtImportFilePath.Enabled = (!LimitAccessFolder);
            btnImportFilePath.Enabled = true;

            var fileFormatId = paymentAgency.FileFormatId;
            var fileFormat = PaymentFileFormatList.SingleOrDefault(f => f.Id == fileFormatId);
            txtImportFileFormat.Text = fileFormat?.Name;
            txtTransferYear.Enabled = ((fileFormat?.IsNeedYear ?? 0) == 1);

            cbxConsiderUncollected.Checked = (paymentAgency.ConsiderUncollected != 0);
            cmbCollectCategory.SelectedValue = paymentAgency.CollectCategoryId;
        }

        /// <summary>
        /// 決済手段コード検索
        /// </summary>
        private void btnPaymentAgency_Click(object sender, EventArgs e)
        {
            var paymentAgency = this.ShowPaymentAgencySearchDialog();
            if (paymentAgency == null)
            {
                return;
            }

            txtPaymentAgencyCode.Text = paymentAgency.Code;
            txtPaymentAgencyCode_Validated(null, null);
        }

        /// <summary>
        /// 取込ファイル選択
        /// </summary>
        private void btnImportFilePath_Click(object sender, EventArgs e)
        {
            var path = txtImportFilePath.Text.Trim();
            var fileNames = new List<string>();
            if (!LimitAccessFolder ?
                !ShowOpenFileDialog(path, out fileNames, index: 1) :
                !ShowRootFolderBrowserDialog(ApplicationControl.RootPath, out fileNames, FolderBrowserType.SelectFile)) return;

            txtImportFilePath.Text = fileNames?.FirstOrDefault() ?? string.Empty;
        }

        /// <summary>
        /// 「振替不能時に回収区分を自動更新する」チェックボックス
        /// </summary>
        private void cbxConsiderUncollected_CheckedChanged(object sender, EventArgs e)
        {
            var isChecked = cbxConsiderUncollected.Checked;

            cmbCollectCategory.Enabled = isChecked;

            if (!isChecked)
            {
                cmbCollectCategory.SelectedIndex = -1;
            }
        }

        #endregion

        #region Functions

        /// <summary>
        /// 画面をクリアする。
        /// </summary>
        private void ClearScreen()
        {
            ClearStatusMessage();

            txtPaymentAgencyCode.Clear();
            txtPaymentAgencyName.Clear();
            txtImportFileFormat.Clear();
            txtImportFilePath.Clear();

            cbxConsiderUncollected.Checked = false;
            cmbCollectCategory.SelectedIndex = -1;
            cmbCollectCategory.Enabled = false;

            txtTransferYear.Enabled = false;

            dlblReadCount.Clear();
            dlblTransferredCount.Clear();
            dlblTransferredAmount.Clear();
            dlblTransferErrorCount.Clear();
            dlblTransferErrorAmount.Clear();
            grdErrorDataList.DataSource = null;

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            txtPaymentAgencyCode.Enabled = true;
            btnPaymentAgency.Enabled = true;
            txtImportFilePath.Enabled = false;
            btnImportFilePath.Enabled = false;

            MatchingResultList = null;

            txtPaymentAgencyCode.Select();
        }

        private class ValidationError
        {
            public string MessageId { get; private set; }
            public string[] MessageArgs { get; private set; }

            public ValidationError(string messageId, params string[] messageArgs)
            {
                MessageId = messageId;
                MessageArgs = messageArgs;
            }
        }

        private class ValidationError<TFocusTarget> : ValidationError
        {
            public TFocusTarget FocusTarget { get; private set; }

            public ValidationError(TFocusTarget focusTarget, string messageId, params string[] messageArgs) : base(messageId, messageArgs)
            {
                FocusTarget = focusTarget;
            }
        }

        private ValidationError ValidateForLoading()
        {
            if (string.IsNullOrEmpty(txtPaymentAgencyCode.Text))
            {
                return new ValidationError<Control>(txtPaymentAgencyCode, MsgWngInputRequired, "決済手段コード");
            }
            if (string.IsNullOrEmpty(txtImportFilePath.Text))
            {
                return new ValidationError<Control>(txtImportFilePath, MsgWngInputRequired, "取込ファイル名");
            }
            if (txtTransferYear.Enabled && string.IsNullOrEmpty(txtTransferYear.Text))
            {
                return new ValidationError<Control>(txtImportFilePath, MsgWngInputRequired, "引落年");
            }

            if (!File.Exists(txtImportFilePath.Text))
            {
                return new ValidationError<Control>(txtImportFilePath, MsgWngOpenFileNotFound);
            }

            return null;
        }

        /// <summary>
        /// 口座振替結果データを取り込む。
        /// ファイルを読み込み、請求データと照合した結果を返す。
        /// </summary>
        /// <returns></returns>
        private List<AccountTransferSource> LoadTransferResultFile()
        {
            var filePath = txtImportFilePath.Text;
            var reader = CreateReader();
            reader.FileName = System.IO.Path.GetFileName(filePath);
            return reader.ReadAsync(filePath).Result;
        }

        private IReader CreateReader()
        {
            var filePath = txtImportFilePath.Text;
            var paymentAgency = PaymentAgencyList.First(pa => pa.Code == txtPaymentAgencyCode.Text);
            var aggregateBillings = Company.TransferAggregate != 0;
            var transferYear = int.Parse(txtTransferYear.Text);

            var formatId = (AccountTransferFileFormatId)paymentAgency.FileFormatId;
            var helper = new Helper{
                GetBillings = (companyId, paymentAgencyId, dueAt) => ServiceProxyFactory.Do((BillingService.BillingServiceClient client) => {
                    var result = client.GetAccountTransferMatchingTargetList(SessionKey, companyId, paymentAgencyId, dueAt);
                    if (result.ProcessResult.Result) return result.Billings;
                    return new List<Billing>();
                }),
                GetCustomers = (ids) => ServiceProxyFactory.Do((CustomerMasterService.CustomerMasterClient client) => {
                    var result = client.Get(SessionKey, ids);
                    if (result.ProcessResult.Result) return result.Customers.ToDictionary(x => x.Id);
                    return new Dictionary<int, Customer>();
                }),
            };
            var reader = helper.CreateReader(formatId);
            reader.CompanyId = CompanyId;
            reader.PaymentAgencyId = paymentAgency.Id;
            reader.AggregateBillings = aggregateBillings;
            reader.TransferYear = transferYear;
            return reader;
        }

        private ValidationError ValidateForImporting()
        {
            if (cbxConsiderUncollected.Checked)
            {
                if (cmbCollectCategory.SelectedIndex < 0)
                {
                    return new ValidationError<Control>(txtImportFilePath, MsgWngSelectionRequired, "回収区分");
                }
            }

            return null;
        }

        #endregion

        #region Web Service

        private static async Task<List<PaymentAgency>> GetPaymentAgencyListAsync(string sessionKey, int companyId, params string[] codes)
        {
            PaymentAgenciesResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<PaymentAgencyMasterService.PaymentAgencyMasterClient>();
                result = await client.GetByCodeAsync(sessionKey, companyId, codes);
            });

            if (result == null || result.ProcessResult.Result == false)
            {
                return null;
            }

            return result.PaymentAgencies;
        }

        private static async Task<List<PaymentFileFormat>> GetPaymentFileFormatListAsync(string sessionKey)
        {
            PaymentFileFormatResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<PaymentAgencyMasterService.PaymentAgencyMasterClient>();
                result = await client.GetFormatItemsAsync(sessionKey);
            });

            if (result == null || result.ProcessResult.Result == false)
            {
                return null;
            }

            return result.PaymentFileFormats;
        }

        private static async Task<List<Category>> GetCategoryListAsync(string sessionKey, int companyId, int categoryType)
        {
            CategoriesResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<CategoryMasterService.CategoryMasterClient>();
                result = await client.GetByCodeAsync(sessionKey, companyId, categoryType, null);
            });

            if (result == null || result.ProcessResult.Result == false)
            {
                return null;
            }

            return result.Categories;
        }

        private static async Task<bool> ImportAccountTransferResultAsync(string sessionKey,
            IEnumerable<AccountTransferImportData> importDataList,
            int? dueDateOffset,
            int? collectCategoryId)
        {
            AccountTransferImportResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<BillingService.BillingServiceClient>();
                result = await client.ImportAccountTransferResultAsync(sessionKey, importDataList.ToArray(), dueDateOffset, collectCategoryId);
            });

            if (result == null || result.ProcessResult.Result == false)
            {
                return false;
            }

            return true;
        }

        // sync
        private static GeneralSetting GetGeneralSetting(string sessionKey, int companyId, string code)
        {
            GeneralSettingResult result = null;

            ServiceProxyFactory.LifeTime(factory =>
            {
                var client = factory.Create<GeneralSettingMasterService.GeneralSettingMasterClient>();
                result = client.GetByCode(sessionKey, companyId, code);
            });

            if (result == null || result.ProcessResult.Result == false)
            {
                return null;
            }

            return result.GeneralSetting;
        }

        // sync
        private static List<Department> GetDepartmentList(string sessionKey, params int[] ids)
        {
            DepartmentsResult result = null;

            ServiceProxyFactory.LifeTime(factory =>
            {
                var client = factory.Create<DepartmentMasterService.DepartmentMasterClient>();
                result = client.Get(sessionKey, ids);
            });

            if (result == null || result.ProcessResult.Result == false)
            {
                return null;
            }

            return result.Departments;
        }

        #endregion Web Service
    }
}
