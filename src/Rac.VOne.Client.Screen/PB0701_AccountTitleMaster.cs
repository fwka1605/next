using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.AccountTitleMasterService;
using Rac.VOne.Client.Screen.BillingService;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.CustomerDiscountMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Export;
using Rac.VOne.Import;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;
using static Rac.VOne.Web.Models.FunctionType;

namespace Rac.VOne.Client.Screen
{
    /// <summary>科目マスター</summary>
    public partial class PB0701 : VOneScreenBase
    {

        public Func<IEnumerable<IMasterData>, bool> SavePostProcessor { get; set; }
        public Func<IEnumerable<IMasterData>, bool> DeletePostProcessor { get; set; }

        private List<AccountTitle> AccountTitleList { get; set; } = new List<AccountTitle>();
        private int AccountTitleId { get; set; }

        public PB0701()
        {
            InitializeComponent();
            grdAccountTitleMaster.SetupShortcutKeys();
            Text = "科目マスター";
            grdAccountTitleMaster.ClearSelection();
            AddHandlers();
        }

        private async Task LoadGrid()
        {
            AccountTitlesResult result = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<AccountTitleMasterClient>();
                result = await service.GetItemsAsync(SessionKey, new AccountTitleSearch { CompanyId = CompanyId });
            });

            if (result.ProcessResult.Result)
            {
                AccountTitleList = result.AccountTitles;
                grdAccountTitleMaster.DataSource = new BindingSource(AccountTitleList, null);
                grdAccountTitleMaster.CurrentCellBorderLine = new Line(LineStyle.None, Color.Empty);
                grdAccountTitleMaster.CurrentRowBorderLine = new Line(LineStyle.None, Color.Empty);
            }
        }

        private void txtCode_Validated(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCode.Text))
                {
                    Task<AccountTitle> task = RetrieveData();
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                    if (task.Result == null)
                    {
                        DispStatusMessage(MsgInfSaveNewData, "科目");
                    }
                    else
                    {
                        ClearStatusMessage();
                    }
                }
                else ClearStatusMessage();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task<AccountTitle> RetrieveData()
        {
            AccountTitle accountTitleResult = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<AccountTitleMasterClient>();
                AccountTitlesResult result = await service.GetByCodeAsync(
                    SessionKey, CompanyId, new string[] { txtCode.Text });

                if (result.ProcessResult.Result)
                {
                    accountTitleResult = result.AccountTitles.FirstOrDefault();
                    if (accountTitleResult == null)
                    {
                        Modified = true;
                    }
                    else
                    {
                        txtName.Text = accountTitleResult.Name;
                        txtContraAccountCode.Text = accountTitleResult.ContraAccountCode;
                        txtContraAccountName.Text = accountTitleResult.ContraAccountName;
                        txtContraAccountSubCode.Text = accountTitleResult.ContraAccountSubCode;
                        AccountTitleId = accountTitleResult.Id;
                        txtCode.Enabled = false;
                        BaseContext.SetFunction03Enabled(true);
                        Modified = false;
                    }
                }
            });
            return accountTitleResult;
        }

        private void CreateGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "header"                                                                    , cell: builder.GetRowHeaderCell()),
                new CellSetting(height, 115, "Code"                , dataField: nameof(AccountTitle.Code)                , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), caption: "科目コード"         ),
                new CellSetting(height, 140, "Name"                , dataField: nameof(AccountTitle.Name)                , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft)  , caption: "科目名"             ),
                new CellSetting(height, 115, "ContraAccountCode"   , dataField: nameof(AccountTitle.ContraAccountCode)   , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), caption: "相手科目コード"     ),
                new CellSetting(height, 140, "ContraAccountName"   , dataField: nameof(AccountTitle.ContraAccountName)   , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft)  , caption: "相手科目名"         ),
                new CellSetting(height, 115, "ContraAccountSubCode", dataField: nameof(AccountTitle.ContraAccountSubCode), cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), caption: "相手科目補助コード" ),
                new CellSetting(height,   0, "Id"                  , dataField: nameof(AccountTitle.Id)                  , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), visible: false                ),
            });
            grdAccountTitleMaster.Template = builder.Build();
        }

        private void grdAccountTitleMaster_CellDoubleClick(object sender, CellEventArgs e)
        {
            ClearStatusMessage();
            if (e.Scope != CellScope.Row || e.RowIndex < 0) return;

            if (Modified && !ShowConfirmDialog(MsgQstConfirmUpdateData))
            {
                return;
            }
            txtCode.Text = grdAccountTitleMaster.Rows[e.RowIndex].Cells[1].DisplayText;
            txtName.Text = grdAccountTitleMaster.Rows[e.RowIndex].Cells[2].DisplayText;
            txtContraAccountCode.Text = grdAccountTitleMaster.Rows[e.RowIndex].Cells[3].DisplayText;
            txtContraAccountName.Text = grdAccountTitleMaster.Rows[e.RowIndex].Cells[4].DisplayText;
            txtContraAccountSubCode.Text = grdAccountTitleMaster.Rows[e.RowIndex].Cells[5].DisplayText;
            AccountTitleId = int.Parse(grdAccountTitleMaster.Rows[e.RowIndex].Cells[6].DisplayText);
            txtCode.Enabled = false;
            txtName.Focus();
            BaseContext.SetFunction03Enabled(true);
            Modified = false;
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction05Caption("インポート");
            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction10Caption("終了");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction05Enabled(true);
            BaseContext.SetFunction06Enabled(true);

            OnF01ClickHandler = SaveAccountTitle;
            OnF02ClickHandler = ClearAccountTitle;
            OnF03ClickHandler = DeleteAccountTitle;
            OnF05ClickHandler = ImportAccountTitle;
            OnF06ClickHandler = ExportAccountTitle;
            OnF10ClickHandler = ExitAccountTitle;
        }

        [OperationLog("登録")]
        private void SaveAccountTitle()
        {
            try
            {
                if (!ValidateChildren()) return;

                ZeroLeftPaddingWithoutValidated();

                if (!CheckData()) return;

                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var task = SaveAccountTitleData();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (task.Result)
                {
                    Clear();
                    DispStatusMessage(MsgInfSaveSuccess);
                }
                else
                {
                    ShowWarningDialog(MsgErrSaveError);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void ZeroLeftPaddingWithoutValidated()
        {
            if (IsNeedValidate(ApplicationControl.AccountTitleCodeType, txtCode.TextLength, ApplicationControl.AccountTitleCodeLength))
            {
                txtCode.Text = ZeroLeftPadding(txtCode);
                txtCode_Validated(null, null);
            }
        }

        private bool CheckData()
        {
            if (string.IsNullOrEmpty(txtCode.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "科目コード");
                txtCode.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                ShowWarningDialog(MsgWngInputRequired, "科目名");
                txtName.Focus();
                return false;
            }

            return true;
        }

        private async Task<bool> SaveAccountTitleData()
        {
            var accountTitle = new AccountTitle();
            accountTitle.ContraAccountCode = txtContraAccountCode.Text ?? string.Empty;
            accountTitle.ContraAccountName = txtContraAccountName.Text.Trim() ?? string.Empty;
            accountTitle.ContraAccountSubCode = txtContraAccountSubCode.Text ?? string.Empty;

            accountTitle.CompanyId = CompanyId;
            accountTitle.UpdateBy = Login.UserId;
            accountTitle.CreateBy = Login.UserId;
            accountTitle.Code = txtCode.Text;
            accountTitle.Name = txtName.Text.Trim();

            var success = false;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<AccountTitleMasterClient>();
                AccountTitleResult saveResult = null;
                saveResult = await service.SaveAsync(SessionKey, accountTitle);
                success = saveResult?.ProcessResult.Result ?? false;
                var syncResult = true;
                if (SavePostProcessor != null && success)
                {
                    syncResult = SavePostProcessor.Invoke(new IMasterData[] { saveResult.AccountTitle as IMasterData });
                }
                success &= syncResult;
                if (success)
                {
                    await LoadGrid();
                }
            });
            return success;
        }

        [OperationLog("クリア")]
        private void ClearAccountTitle()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear))
            {
                return;
            }
            Clear();
        }

        [OperationLog("終了")]
        private void ExitAccountTitle()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose))
            {
                return;
            }
            ParentForm.Close();
        }

        private void Clear()
        {
            ClearStatusMessage();
            txtCode.Clear();
            txtName.Clear();
            txtContraAccountCode.Clear();
            txtContraAccountName.Clear();
            txtContraAccountSubCode.Clear();
            AccountTitleId = 0;
            txtCode.Enabled = true;
            txtCode.Select();
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction05Enabled(Authorities[MasterImport]);
            BaseContext.SetFunction06Enabled(Authorities[MasterExport]);
            Modified = false;
        }

        [OperationLog("削除")]
        private void DeleteAccountTitle()
        {
            try
            {
                if (!ValidateChildren()) return;

                if (!ValidateInputValueForDelete()) return;

                var confirmTask = ValidateDeleteAccountTitleIdAsync();
                ProgressDialog.Start(ParentForm, confirmTask, false, SessionKey);
                if (confirmTask.Exception != null)
                {
                    NLogHandler.WriteErrorLog(this, confirmTask.Exception, SessionKey);
                    ShowWarningDialog(MsgErrDeleteError);
                    return;
                }
                if (!confirmTask.Result) return;

                if (!ShowConfirmDialog(MsgQstConfirmDelete))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var task = DeleteAccountTitleAsync();

                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                if (task.Result)
                {
                    Clear();
                    DispStatusMessage(MsgInfDeleteSuccess);
                }
                else
                {
                    ShowWarningDialog(MsgErrDeleteError);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private bool ValidateInputValueForDelete()
        {
            if (!txtCode.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblCode.Text))) return false;
            return true;
        }

        private async Task<bool> ValidateDeleteAccountTitleIdAsync()
        {
            var categoryResult = await ServiceProxyFactory.DoAsync(async (CategoryMasterClient client)
                => await client.ExistAccountTitleAsync(SessionKey, AccountTitleId));
            if (categoryResult.Exist)
            {
                DoActionOnUI(() => ShowWarningDialog(MsgWngDeleteConstraint, "区分マスター", lblCode.Text));
                return false;
            }

            var discountResult = await ServiceProxyFactory.DoAsync(async (CustomerDiscountMasterClient client)
                => await client.ExistAccountTitleAsync(SessionKey, AccountTitleId));
            if (discountResult.Exist)
            {
                DoActionOnUI(() => ShowWarningDialog(MsgWngDeleteConstraint, "歩引きマスター", lblCode.Text));
                return false;
            }

            var billingResult = await ServiceProxyFactory.DoAsync(async (BillingServiceClient client)
                => await client.ExistAccountTitleAsync(SessionKey, AccountTitleId));
            if (billingResult.Exist)
            {
                DoActionOnUI(() => ShowWarningDialog(MsgWngDeleteConstraint, "請求データ", lblCode.Text));
                return false;
            }
            return true;
        }

        private async Task<bool> DeleteAccountTitleAsync()
        {
            var success = false;
            IEnumerable<IMasterData> accountTitles = null;
            await ServiceProxyFactory.DoAsync(async (AccountTitleMasterClient client)=>
            {
                if (DeletePostProcessor != null)
                {
                    var result = await client.GetAsync(SessionKey, new int[] { AccountTitleId });
                    if (result.ProcessResult.Result)
                        accountTitles = result.AccountTitles;
                }

                var deleteResult = await client.DeleteAsync(SessionKey, AccountTitleId);
                success = (deleteResult?.ProcessResult.Result ?? false)
                    && deleteResult?.Count > 0;
            });

            var syncResult = true;
            if (DeletePostProcessor != null && success)
            {
                syncResult = DeletePostProcessor.Invoke(accountTitles);
            }
            success &= syncResult;

            if (success)
            {
                await LoadGrid();
            }
            return success;
        }

        [OperationLog("インポート")]
        private void ImportAccountTitle()
        {
            try
            {
                ClearStatusMessage();

                ImportSetting importSetting = null;
                // 取込設定取得
                var task = Util.GetMasterImportSettingAsync(Login, ImportFileType.AccountTitle);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                importSetting = task.Result;

                var definition = new AccountTitleFileDefinition(new DataExpression(ApplicationControl));

                definition.AccountTitleCodeField.ValidateAdditional = (val, param) =>
                {
                    var reports = new List<WorkingReport>();
                    if (((ImportMethod)param) != ImportMethod.Replace) return reports;

                    MasterDatasResult categoryResult = null;
                    MasterDatasResult customerResult = null;
                    MasterDatasResult debitBillingResult = null;
                    MasterDatasResult creditBillingResult = null;
                    ServiceProxyFactory.LifeTime(factory =>
                    {
                        var accountTitle = factory.Create<AccountTitleMasterClient>();
                        var codes = val.Values.Where(x => !string.IsNullOrEmpty(x.Code)).Select(x => x.Code).Distinct().ToArray();
                        categoryResult = accountTitle.GetImportItemsForCategory(SessionKey, CompanyId, codes);
                        customerResult = accountTitle.GetImportItemsForCustomerDiscount(SessionKey, CompanyId, codes);
                        debitBillingResult = accountTitle.GetImportItemsForDebitBilling(SessionKey, CompanyId, codes);
                        creditBillingResult = accountTitle.GetImportItemsForCreditBilling(SessionKey, CompanyId, codes);
                    });

                    foreach (MasterData ca in categoryResult.MasterDatas.Where(p => !val.Any(a => a.Value.Code == p.Code)))
                    {
                        reports.Add(new WorkingReport
                        {
                            LineNo = null,
                            FieldNo = definition.AccountTitleCodeField.FieldIndex,
                            FieldName = definition.AccountTitleCodeField.FieldName,
                            Message = $"区分マスターに存在する{ca.Code}：{ca.Name}が存在しないため、インポートできません。",
                        });
                    }

                    foreach (MasterData ca in customerResult.MasterDatas.Where(p => !val.Any(a => a.Value.Code == p.Code)))
                    {
                        reports.Add(new WorkingReport
                        {
                            LineNo = null,
                            FieldNo = definition.AccountTitleCodeField.FieldIndex,
                            FieldName = definition.AccountTitleCodeField.FieldName,
                            Message = $"得意先マスター歩引設定に存在する{ca.Code}：{ca.Name}が存在しないため、インポートできません。",
                        });
                    }

                    foreach (MasterData ca in debitBillingResult.MasterDatas.Where(p => !val.Any(a => a.Value.Code == p.Code)))
                    {
                        reports.Add(new WorkingReport
                        {
                            LineNo = null,
                            FieldNo = definition.AccountTitleCodeField.FieldIndex,
                            FieldName = definition.AccountTitleCodeField.FieldName,
                            Message = $"請求データに存在する{ca.Code}：{ca.Name}が存在しないため、インポートできません。",
                        });
                    }

                    foreach (MasterData ca in creditBillingResult.MasterDatas.Where(p => !val.Any(a => a.Value.Code == p.Code)))
                    {
                        reports.Add(new WorkingReport
                        {
                            LineNo = null,
                            FieldNo = definition.AccountTitleCodeField.FieldIndex,
                            FieldName = definition.AccountTitleCodeField.FieldName,
                            Message = $"請求データに存在する{ca.Code}：{ca.Name}が存在しないため、インポートできません。",
                        });
                    }
                    return reports;
                };

                var importer = definition.CreateImporter(m => m.Code);
                importer.UserId = Login.UserId;
                importer.UserCode = Login.UserCode;
                importer.CompanyId = CompanyId;
                importer.CompanyCode = Login.CompanyCode;
                importer.LoadAsync = async () => await LoadGridAsync();
                importer.RegisterAsync = async unitOfWork => await RegisterForImportAsync(unitOfWork);

                System.Action clear = () =>
                {
                    AccountTitleList.Clear();
                    var loadTask = LoadGridAsync();
                    ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);
                    AccountTitleList.AddRange(loadTask.Result);
                    grdAccountTitleMaster.DataSource = new BindingSource(AccountTitleList, null);
                };
                var importResult = DoImport(importer, importSetting, clear);
                if (!importResult) return;
                txtCode.Clear();
                txtName.Clear();
                txtContraAccountCode.Clear();
                txtContraAccountName.Clear();
                txtContraAccountSubCode.Clear();
                AccountTitleId = 0;
                txtCode.Enabled = true;
                this.ActiveControl = txtCode;
                BaseContext.SetFunction03Enabled(false);
                Modified = false;

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrImportErrorWithoutLog);
            }
        }

        private async Task<ImportResult> RegisterForImportAsync(UnitOfWork<AccountTitle> imported)
        {
            ImportResultAccountTitle result = null;
            var syncResult = true;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<AccountTitleMasterClient>();
                result = await service.ImportAsync(SessionKey,
                        imported.New.ToArray(), imported.Dirty.ToArray(), imported.Removed.ToArray());
            });

            if (DeletePostProcessor != null && (imported.Removed?.Any() ?? false))
            {
                syncResult = DeletePostProcessor.Invoke(imported.Removed.Select(x => x as IMasterData));
            }

            if (syncResult && SavePostProcessor != null && (result.AccountTitles?.Any() ?? false))
            {
                syncResult = SavePostProcessor.Invoke(result.AccountTitles.Select(x => x as IMasterData));
            }

            return result;
        }

        [OperationLog("エクスポート")]
        private void ExportAccountTitle()
        {
            try
            {
                ClearStatusMessage();
                List<AccountTitle> list = null;
                var listTask = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var accountTitleMaster = factory.Create<AccountTitleMasterClient>();
                    AccountTitlesResult result = await accountTitleMaster.GetItemsAsync(
                        SessionKey, new AccountTitleSearch { CompanyId = CompanyId });

                    if (result.ProcessResult.Result)
                    {
                        list = result.AccountTitles;
                    }
                });

                var pathTask = Util.GetGeneralSettingServerPathAsync(Login);
                ProgressDialog.Start(ParentForm, Task.WhenAll(listTask, pathTask), false, SessionKey);
                var serverPath = pathTask.Result;

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
                var fileName = $"科目マスター{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var definition = new AccountTitleFileDefinition(new DataExpression(ApplicationControl));
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

                DispStatusMessage(MsgInfFinishExport);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                DispStatusMessage(MsgErrExportError);
            }
        }

        private void SetFormat()
        {
            var expression = new DataExpression(ApplicationControl);

            txtCode.Format = expression.AccountTitleCodeFormatString;
            txtCode.MaxLength = expression.AccountTitleCodeLength;

            txtContraAccountCode.Format = "9A";
            txtContraAccountCode.MaxLength = 10;

            if (ApplicationControl?.AccountTitleCodeType == 0)
            {
                txtCode.PaddingChar = '0';
            }
        }

        private void PB0701_Load(object sender, EventArgs e)
        {
            SetScreenName();
            try
            {
                var loadTask = new List<Task>();

                if (ApplicationControl == null)
                {
                    loadTask.Add(LoadApplicationControlAsync());
                }

                loadTask.Add(LoadControlColorAsync());

                if (Company == null)
                {
                    loadTask.Add(LoadCompanyAsync());
                }
                if (Authorities == null)
                {
                    loadTask.Add(LoadFunctionAuthorities(MasterImport, MasterExport));
                }
                var loadGridTask = LoadGridAsync();
                loadTask.Add(loadGridTask);
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
                AccountTitleList.AddRange(loadGridTask.Result);

                CreateGridTemplate();
                Clear();
                grdAccountTitleMaster.DataSource = new BindingSource(AccountTitleList, null);
                SetFormat();
                Modified = false;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task<List<AccountTitle>> LoadGridAsync()
        {
            List<AccountTitle> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<AccountTitleMasterClient>();
                var result = await service.GetItemsAsync(SessionKey, new AccountTitleSearch { CompanyId = CompanyId, });

                if (result.ProcessResult.Result)
                {
                    list = result.AccountTitles;
                }
            });

            return list ?? new List<AccountTitle>();
        }

        private void AddHandlers()
        {
            foreach (Control control in gbxAccountTitleInput.Controls)
            {
                if (control is Common.Controls.VOneTextControl)
                {
                    control.TextChanged += new EventHandler(OnContentChanged);
                }
            }
        }

        private void OnContentChanged(object sender, EventArgs e)
        {
            if (!Modified)
            {
                Modified = true;
            }
        }
    }
}
