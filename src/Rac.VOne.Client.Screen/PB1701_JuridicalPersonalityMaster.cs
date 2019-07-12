using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.JuridicalPersonalityMasterService;
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
    /// <summary>法人格マスター</summary>
    public partial class PB1701 : VOneScreenBase
    {
        private List<JuridicalPersonality> JuridicalPersonalityList { get; set; } = new List<JuridicalPersonality>();
        private JuridicalPersonality CurrentEntry { get; set; } = new JuridicalPersonality();

        public PB1701()
        {
            InitializeComponent();
            grdRemoveJuridicalPersonality.SetupShortcutKeys();
            Text = "法人格マスター";
        }

        #region initialize
        private void CreateGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height, 40, "header"                         , cell: builder.GetRowHeaderCell()),
                new CellSetting(height, 80, nameof(JuridicalPersonality.Kana), caption: "法人格", dataField: nameof(JuridicalPersonality.Kana), cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft)),
            });
            grdRemoveJuridicalPersonality.Template = builder.Build();
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = SaveJuridicalPersonality;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = ClearJuridicalPersonality;

            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction03Enabled(true);
            OnF03ClickHandler = DeleteJuridicalPersonality;

            BaseContext.SetFunction04Caption("印刷");
            BaseContext.SetFunction04Enabled(true);
            OnF04ClickHandler = PrintJuridicalPersonality;

            BaseContext.SetFunction05Caption("インポート");
            BaseContext.SetFunction05Enabled(true);
            OnF05ClickHandler = ImportJuridicalPersonality;

            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction06Enabled(true);
            OnF06ClickHandler = ExportJuridicalPersonality;

            BaseContext.SetFunction07Caption("");
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Caption("");
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Caption("");
            BaseContext.SetFunction09Enabled(false);

            BaseContext.SetFunction10Caption("終了");
            OnF10ClickHandler = ExitJuridicalPersonality;
        }
        #endregion

        #region button event
        [OperationLog("登録")]
        private void SaveJuridicalPersonality()
        {
            try
            {
                ClearStatusMessage();
                if (string.IsNullOrEmpty(txtKana.Text.Trim()))
                {
                    ShowWarningDialog(MsgWngInputRequired, "法人格");
                    txtKana.Focus();
                    return;
                }
                JuridicalPersonalityResult result = null;
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<JuridicalPersonalityMasterClient>();
                    result = await service.GetAsync(SessionKey, CompanyId, txtKana.Text);
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (result.ProcessResult.Result && result.JuridicalPersonality != null)
                {
                    ShowWarningDialog(MsgWngAlreadyExistedData);
                    return;
                }

                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }
                else
                {
                }
                var saveTask = SaveKana();
                ProgressDialog.Start(ParentForm, saveTask, false, SessionKey);
                bool succeeded = saveTask.Result;
                if (succeeded)
                {
                    DispStatusMessage(MsgInfSaveSuccess);
                }
                else
                {
                    ShowWarningDialog(MsgErrSaveError);
                }
                Modified = false;
                this.ActiveControl = txtKana;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task<bool> SaveKana()
        {
            List<JuridicalPersonality> newList = null;
            var succeeded = true;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<JuridicalPersonalityMasterClient>();

                var juridical = new JuridicalPersonality();
                juridical.CompanyId = CompanyId;
                juridical.UpdateBy = Login.UserId;
                juridical.CreateBy = Login.UserId;
                juridical.Kana = txtKana.Text.Trim();

                JuridicalPersonalityResult result = await service.SaveAsync(SessionKey, juridical);

                if (!result.ProcessResult.Result)
                {
                    succeeded = false;
                    return;
                }
                else if (result.JuridicalPersonality != null)
                {
                    newList = await LoadJuridicalPersonalityAsync();
                    txtKana.Clear();
                }
                else
                {
                    newList = await LoadJuridicalPersonalityAsync();
                    succeeded = false;
                }
            });

            if (newList != null)
            {
                grdRemoveJuridicalPersonality.Rows.Clear();
                JuridicalPersonalityList.AddRange(newList);
                grdRemoveJuridicalPersonality.DataSource = new BindingSource(JuridicalPersonalityList, null);
            }
            return succeeded;
        }

        [OperationLog("クリア")]
        private void ClearJuridicalPersonality()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear))
            {
                return;
            }

            ClearStatusMessage();
            txtKana.Clear();
            this.ActiveControl = txtKana;
            Modified = false;
        }

        [OperationLog("削除")]
        private void DeleteJuridicalPersonality()
        {
            ClearStatusMessage();

            try
            {
                if (!ValidateInput()) return;

                if (!ShowConfirmDialog(MsgQstConfirmDelete))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                JuridicalPersonalityResult result = null;
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<JuridicalPersonalityMasterClient>();
                    result = await service.GetAsync(SessionKey, CompanyId, txtKana.Text);
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (result.ProcessResult.Result && result.JuridicalPersonality != null)
                {
                    DeleteKana();
                    Modified = false;
                    this.ActiveControl = txtKana;
                }
                else
                {
                    ShowWarningDialog(MsgWngNoDeleteData);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(txtKana.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "法人格");
                txtKana.Focus();
                return false;
            }
            return true;
        }

        private void DeleteKana()
        {
            List<JuridicalPersonality> newList = null;
            CountResult deleteResult = null;
            var succeeded = false;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<JuridicalPersonalityMasterClient>();
                deleteResult = await service.DeleteAsync(SessionKey, CompanyId, txtKana.Text);

                succeeded = deleteResult.ProcessResult.Result && deleteResult.Count > 0;
                if (!deleteResult.ProcessResult.Result) return;

                newList = await LoadJuridicalPersonalityAsync();
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (succeeded)
            {
                DispStatusMessage(MsgInfDeleteSuccess);
                txtKana.Clear();
            }
            else
            {
                ShowWarningDialog(MsgErrDeleteError);
            }

            if (newList != null)
            {
                grdRemoveJuridicalPersonality.Rows.Clear();
                JuridicalPersonalityList.AddRange(newList);
                grdRemoveJuridicalPersonality.DataSource = new BindingSource(JuridicalPersonalityList, null);
            }
        }

        [OperationLog("インポート")]
        private void ImportJuridicalPersonality()
        {
            ClearStatusMessage();
            try
            {

                ImportSetting importSetting = null;
                var task = Util.GetMasterImportSettingAsync(Login, ImportFileType.JuridicalPersonality);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                importSetting = task.Result;

                var definition = new JuridicalPersonalityFileDefinition(new DataExpression(ApplicationControl));

                var importer = definition.CreateImporter(m => m.Kana);
                importer.UserId = Login.UserId;
                importer.UserCode = Login.UserCode;
                importer.CompanyId = CompanyId;
                importer.CompanyCode = Login.CompanyCode;
                importer.LoadAsync = async () => await LoadJuridicalPersonalityAsync();
                importer.RegisterAsync = async unitOfWork => await RegisterForImportAsync(unitOfWork);

                var importResult = DoImport(importer, importSetting);
                if (!importResult) return;
                txtKana.Clear();
                this.ActiveControl = txtKana;
                Modified = false;
                JuridicalPersonalityList.Clear();
                var loadTask = LoadJuridicalPersonalityAsync();
                ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);
                JuridicalPersonalityList.AddRange(loadTask.Result);
                grdRemoveJuridicalPersonality.DataSource = new BindingSource(JuridicalPersonalityList, null);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrImportErrorWithoutLog);
            }
        }

        private async Task<ImportResult> RegisterForImportAsync(UnitOfWork<JuridicalPersonality> imported)
        {
            ImportResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<JuridicalPersonalityMasterClient>();
                result = await service.ImportAsync(SessionKey, imported.New.ToArray(), imported.Dirty.ToArray(), imported.Removed.ToArray());
            });

            return result;
        }

        [OperationLog("エクスポート")]
        private void ExportJuridicalPersonality()
        {
            ClearStatusMessage();

            try
            {
                var loadTask = LoadJuridicalPersonalityAsync();
                var pathTask = GetServerPath();
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask, pathTask), false, SessionKey);
                var list = loadTask.Result;

                var serverPath = pathTask.Result;

                if (!Directory.Exists(serverPath))
                {
                    serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                if(!list.Any())
                {
                    ShowWarningDialog(MsgWngNoExportData);
                    return;
                }

                var filePath = string.Empty;
                var fileName = $"法人格マスター{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var definition = new JuridicalPersonalityFileDefinition(new DataExpression(ApplicationControl));
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

        [OperationLog("終了")]
        private void ExitJuridicalPersonality()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose))
            {
                return;
            }
            ParentForm.Close();
        }

        [OperationLog("印刷")]
        private void PrintJuridicalPersonality()
        {
            try
            {
                ClearStatusMessage();
                var juridicalList = new List<JuridicalPersonality>();
                string serverPath = null;

                JuridicalPersonalityReport juridicalReport = null;
                ProgressDialog.Start(ParentForm, Task.Run(async () =>
                {
                    serverPath = await GetServerPath();
                    juridicalList = await LoadJuridicalPersonalityAsync();

                    if (juridicalList.Any())
                    {
                        juridicalReport = new JuridicalPersonalityReport();
                        juridicalReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                        juridicalReport.Name = "法人格マスター" + DateTime.Today.ToString("yyyyMMdd");

                        juridicalReport.SetData(juridicalList);
                        juridicalReport.Run(false);
                    }
                }), false, SessionKey);

                if(!juridicalList.Any())
                {
                    ShowWarningDialog(MsgWngPrintDataNotExist);
                    return;
                }

                ShowDialogPreview(ParentForm, juridicalReport, serverPath);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        #endregion

        #region その他　event

        private void grdRemoveJuridicalPersonalityMaster_CellDoubleClick(object sender, CellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (Modified && !ShowConfirmDialog(MsgQstConfirmUpdateData))
                    return;

                SetJuridility(JuridicalPersonalityList[e.RowIndex]);
                txtKana.Focus();
                Modified = false;
            }
            ClearStatusMessage();
        }

        private void txtKana_TextChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        private void PB1701_Load(object sender, EventArgs e)
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
                    loadTask.Add(LoadFunctionAuthorities(MasterImport,MasterExport));
                }
                var loadGridTask = LoadJuridicalPersonalityAsync();
                loadGridTask.ContinueWith(task => JuridicalPersonalityList.AddRange(task.Result));
                loadTask.Add(loadGridTask);
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                CreateGridTemplate();
                grdRemoveJuridicalPersonality.DataSource = new BindingSource(JuridicalPersonalityList, null);
                Modified = false;
                this.ActiveControl = txtKana;
                BaseContext.SetFunction05Enabled(Authorities[MasterImport]);
                BaseContext.SetFunction06Enabled(Authorities[MasterExport]);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task<List<JuridicalPersonality>> LoadJuridicalPersonalityAsync()
        {
            List<JuridicalPersonality> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<JuridicalPersonalityMasterClient>();
                var result = await service.GetItemsAsync(SessionKey, CompanyId);

                if (result.ProcessResult.Result)
                {
                    list = result.JuridicalPersonalities;
                }
            });
            return list ?? new List<JuridicalPersonality>();
        }

        private void txtKana_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtKana.Text))
            {
                ClearStatusMessage();
                return;
            }
            var kana = EbDataHelper.ConvertToHankakuKatakana(txtKana.Text);
            kana = EbDataHelper.RemoveProhibitCharacter(kana);
            kana = EbDataHelper.ConvertProhibitCharacter(kana);
            txtKana.Text = kana;

            var target = JuridicalPersonalityList.FirstOrDefault(k => k.Kana == kana);
            if (target != null)
            {
                SetJuridility(target);
                ClearStatusMessage();
            }
            else
            {
                target = new JuridicalPersonality();
                target.Kana = kana;
                SetJuridility(target);
            }
        }

        private void SetJuridility(JuridicalPersonality judirical = null)
        {
            CurrentEntry = judirical ?? new JuridicalPersonality();
            txtKana.Text = CurrentEntry.Kana;
        }

        private async Task<string> GetServerPath()
        {
            var serverPath = "";

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<GeneralSettingMasterClient>();
                var result = await service.GetByCodeAsync(SessionKey, CompanyId, "サーバパス");

                if (result.ProcessResult.Result)
                {
                    serverPath = result.GeneralSetting?.Value;
                }
            });

            return serverPath;
        }
    }

    #endregion
}
