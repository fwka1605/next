using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.BankBranchMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
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
    /// <summary>銀行・支店マスター</summary>
    public partial class PB1801 : VOneScreenBase
    {
        #region 変数宣言

        private DateTime updateAt { get; set; }

        #endregion

        #region 画面の初期化

        public PB1801()
        {
            InitializeComponent();
            Text = "銀行・支店マスター";
            AddHandlers();
        }

        private void PB1801_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
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
                loadTask.Add(LoadFunctionAuthorities(MasterImport, MasterExport));

                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
                Clear();
                Modified = false;
                txtBankCode.PaddingChar = '0';
                txtBranchCode.PaddingChar = '0';
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        #endregion

        #region PB1801 InitializeFunctionKeys
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Save;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = ConfirmToClear;

            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = Delete;

            BaseContext.SetFunction05Caption("インポート");
            BaseContext.SetFunction05Enabled(true);
            OnF05ClickHandler = Import;

            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction06Enabled(true);
            OnF06ClickHandler = ExportData;

            BaseContext.SetFunction10Caption("終了");
            OnF10ClickHandler = Exit;
        }

        #endregion

        #region 入力項目チェック

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtBankCode.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "銀行コード");
                txtBankCode.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtBranchCode.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "支店コード");
                txtBranchCode.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtBankName.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "銀行名");
                txtBankName.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtBranchName.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "支店名");
                txtBranchName.Focus();
                return false;
            }
            else
            {
                ClearStatusMessage();
                return true;
            }
        }

        #endregion

        #region Validated処理

        private void txtBankCode_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                if (txtBranchCode.Text != "" && txtBankCode.Text != "")
                {
                    FillBankBranchInformation();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtBranchCode_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                if (txtBranchCode.Text != "" && txtBankCode.Text != "")
                {
                    FillBankBranchInformation();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void FillBankBranchInformation()
        {
            BankBranch bankBranchResult = null;

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BankBranchMasterClient>();
                var result = await service.GetAsync(SessionKey, CompanyId, txtBankCode.Text, txtBranchCode.Text);
                if (result.ProcessResult.Result)
                {
                    bankBranchResult = result.BankBranch;
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (bankBranchResult != null)
            {
                txtBankCode.Text = bankBranchResult.BankCode;
                txtBranchCode.Text = bankBranchResult.BranchCode;
                txtBankKana.Text = bankBranchResult.BankKana;
                txtBankName.Text = bankBranchResult.BankName;
                txtBranchKana.Text = bankBranchResult.BranchKana;
                txtBranchName.Text = bankBranchResult.BranchName;
                txtBankName.Focus();
                updateAt = bankBranchResult.UpdateAt;
                BaseContext.SetFunction03Enabled(true);
                Modified = false;
                txtBankCode.Enabled = false;
                txtBranchCode.Enabled = false;
                btnSearch.Enabled = false;
            }
            else
            {
                DispStatusMessage(MsgInfSaveNewData, "銀行・支店");
            }
        }

        private void ZeroLeftPaddingWithoutValidated()
        {
            if (IsNeedValidate(0, txtBankCode.TextLength, txtBankCode.MaxLength))
            {
                txtBankCode.Text = ZeroLeftPadding(txtBankCode);
            }
            if (IsNeedValidate(0, txtBranchCode.TextLength, txtBranchCode.MaxLength))
            {
                txtBranchCode.Text = ZeroLeftPadding(txtBranchCode);
            }
            //if (txtBankCode.TextLength == txtBankCode.MaxLength && txtBranchCode.TextLength == txtBranchCode.MaxLength)
            //{
            //    txtBranchCode_Validated(null, null);
            //}
        }

        #endregion

        #region  入力項目変更イベント処理

        private void AddHandlers()
        {
            foreach (Control control in gbxBankBranch.Controls)
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

        #endregion

        #region 検索処理

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (Modified
                && !ShowConfirmDialog(MsgQstConfirmUpdateData)) return;

            SearchClickEvent();
        }

        private void SearchClickEvent()
        {
            var bank = this.ShowBankBranchSearchDialog();
            if (bank != null)
            {
                txtBankCode.Text = bank.BankCode;
                txtBankKana.Text = bank.BankKana;
                txtBankName.Text = bank.BankName;
                txtBranchCode.Text = bank.BranchCode;
                txtBranchName.Text = bank.BranchName;
                txtBranchKana.Text = bank.BranchKana;
                txtBankName.Focus();
                txtBranchCode.Enabled = false;
                txtBankCode.Enabled = false;
                btnSearch.Enabled = false;
                BaseContext.SetFunction03Enabled(true);
                Modified = false;
                updateAt = bank.UpdateAt;
                ClearStatusMessage();
            }
        }

        #endregion

        #region F1 登録処理

        [OperationLog("登録")]
        private void Save()
        {
            if (!ValidateInput()) return;

            ZeroLeftPaddingWithoutValidated();
            
            if (!ShowConfirmDialog(MsgQstConfirmSave))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            try
            {
                SaveBankBranch();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void SaveBankBranch()
        {
            var bankInsert = new BankBranch();
            bankInsert.CompanyId = CompanyId;
            bankInsert.BankCode = txtBankCode.Text;
            bankInsert.BranchCode = txtBranchCode.Text;
            bankInsert.BankName = txtBankName.Text.Trim();
            bankInsert.BranchName = txtBranchName.Text.Trim();
            bankInsert.BankKana = txtBankKana.Text.Trim();
            bankInsert.BranchKana = txtBranchKana.Text.Trim();
            bankInsert.UpdateBy = Login.UserId;
            bankInsert.CreateBy = Login.UserId;
            bankInsert.UpdateAt = updateAt;

            BankBranchResult result = null;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BankBranchMasterClient>();
                result = await service.SaveAsync(SessionKey, bankInsert);
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (result.ProcessResult.Result)
            {
                Clear();
                DispStatusMessage(MsgInfSaveSuccess);
            }
            else
            {
                ShowWarningDialog(MsgErrSaveError);
            }
        }

        #endregion

        #region F2 クリア処理

        [OperationLog("クリア")]
        private void ConfirmToClear()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear))
            {
                return;
            }
            Clear();
            ClearStatusMessage();
        }

        /// <summary>画面クリア</summary>
        public void Clear()
        {
            txtBankCode.Clear();
            txtBankKana.Clear();
            txtBankName.Clear();
            txtBranchCode.Clear();
            txtBranchKana.Clear();
            txtBranchName.Clear();
            txtBankCode.Enabled = true;
            txtBranchCode.Enabled = true;
            BaseContext.SetFunction03Enabled(false);
            this.ActiveControl = txtBankCode;
            txtBankCode.Focus();
            Modified = false;
            txtBankCode.Enabled = true;
            txtBranchCode.Enabled = true;
            btnSearch.Enabled = true;
            BaseContext.SetFunction05Enabled(Authorities[MasterImport]);
            BaseContext.SetFunction06Enabled(Authorities[MasterExport]);
        }

        #endregion

        #region F3 削除処理

        [OperationLog("削除")]
        private void Delete()
        {
            if (!ShowConfirmDialog(MsgQstConfirmDelete))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            try
            {
                Task<bool> task = DeleteBankBranch();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                bool succeeded = task.Result;
                if (succeeded)
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

        private async Task<bool> DeleteBankBranch()
        {
            bool succeeded = false;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BankBranchMasterClient>();
                var deleteBankBranch = await service.DeleteAsync(SessionKey, CompanyId, txtBankCode.Text, txtBranchCode.Text);
                succeeded = deleteBankBranch.ProcessResult.Result && deleteBankBranch.Count > 0;
            });
            return succeeded;
        }

        #endregion

        #region　F5 インポート処理

        [OperationLog("インポート")]
        private void Import()
        {
            ClearStatusMessage();
            try
            {
                ImportSetting importSetting = null;
                var task = Util.GetMasterImportSettingAsync(Login, ImportFileType.BankBranch);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                importSetting = task.Result;

                var definition = new BankBranchFileDefinition(new DataExpression(ApplicationControl));

                var importer = definition.CreateImporter(m => new { m.BankCode, m.BranchCode });
                importer.UserId = Login.UserId;
                importer.UserCode = Login.UserCode;
                importer.CompanyId = CompanyId;
                importer.CompanyCode = Login.CompanyCode;
                importer.LoadAsync = async () => await GetBankBranchAsync();
                importer.RegisterAsync = async unitOfWork => await RegisterForImportAsync(unitOfWork);

                var importResult = DoImport(importer, importSetting, Clear);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrImportErrorWithoutLog);
            }
        }

        private async Task<List<BankBranch>> GetBankBranchAsync()
        {
            List<BankBranch> list = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BankBranchMasterClient>();
                var result = await service.GetItemsAsync(SessionKey, CompanyId, new BankBranchSearch());

                if (result.ProcessResult.Result)
                {
                    list = result.BankBranches;
                }
            });

            return list ?? new List<BankBranch>();
        }

        /// <summary>インポートデータ登録</summary>
        /// <param name="imported">CSVから生成したModel</param>
        /// <returns>登録結果</returns>
        private async Task<ImportResult> RegisterForImportAsync(UnitOfWork<BankBranch> imported)
        {
            imported.New.ForEach(b => b.CompanyId = CompanyId);
            imported.Dirty.ForEach(b => b.CompanyId = CompanyId);

            ImportResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BankBranchMasterClient>();
                result = await service.ImportAsync(SessionKey,
                    imported.New.ToArray(), imported.Dirty.ToArray(), imported.Removed.ToArray());
            });

            return result;
        }

        #endregion

        #region　F6 エクスポート処理

        [OperationLog("エクスポート")]
        private void ExportData()
        {
            try
            {
                List<BankBranch> list = null;
                var serverPath = "";
                var loadTask = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<BankBranchMasterClient>();

                    BankBranchsResult result = await service.GetItemsAsync(SessionKey, CompanyId, new BankBranchSearch());

                    if (result.ProcessResult.Result)
                    {
                        list = result.BankBranches;
                    }
                });

                var pathTask = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var generalService = factory.Create<GeneralSettingMasterClient>();
                    GeneralSettingResult result = await generalService.GetByCodeAsync(SessionKey, CompanyId, "サーバパス");

                    if (result.ProcessResult.Result)
                    {
                        serverPath = result.GeneralSetting?.Value;
                    }
                });
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask, pathTask), false, SessionKey);

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
                var fileName = $"銀行・支店マスター{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var definition = new BankBranchFileDefinition(new DataExpression(ApplicationControl));
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
        #endregion

        #region F10 終了処理

        [OperationLog("終了")]
        private void Exit()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose))
            {
                return;
            }
            ParentForm.Close();
        }
        #endregion
    }
}