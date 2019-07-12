using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.IgnoreKanaMasterService;
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
    /// <summary>除外カナマスター</summary>
    public partial class PB1501 : VOneScreenBase
    {
        private const int ExcludeCategoryType = 4;
        private const int ImportFormatType = 15;
        private const string ExportPathCode = "サーバパス";

        private bool IsModified { get; set; }

        private List<IgnoreKana> IgnoreKanaList { get; } = new List<IgnoreKana>();

        public PB1501()
        {
            InitializeComponent();
            Text = "除外カナマスター";
            grdExcludeKanaList.SetupShortcutKeys();
            InitializeHandlers();
        }

        private void InitializeHandlers()
        {
            System.Action valueChanged = () => IsModified = true;
            txtKana.TextChanged += (sender, e) => valueChanged();
            txtExcludeCategoryCode.TextChanged += (sender, e) => valueChanged();
        }

        #region 画面の初期化
        private void InitializeKanaGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "Header"                                                         , cell: builder.GetRowHeaderCell()                                                                 ),
                new CellSetting(height, 500, "Kana"        , dataField: nameof(IgnoreKana.Kana)               , cell: builder.GetTextBoxCell()                                     , caption: "カナ名"           ),
                new CellSetting(height, 100, "CategoryCode", dataField: nameof(IgnoreKana.ExcludeCategoryCode), cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), caption: "対象外区分コード" ),
                new CellSetting(height, 290, "CategoryName", dataField: nameof(IgnoreKana.ExcludeCategoryName), cell: builder.GetTextBoxCell()                                     , caption: "対象外区分名"     ),
            });
            grdExcludeKanaList.Template = builder.Build();
            grdExcludeKanaList.HideSelection = true;
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            OnF01ClickHandler = Register;

            BaseContext.SetFunction02Caption("クリア");
            OnF02ClickHandler = Clear;

            BaseContext.SetFunction03Caption("削除");
            OnF03ClickHandler = Delete;

            BaseContext.SetFunction04Enabled(false);

            BaseContext.SetFunction05Caption("インポート");
            OnF05ClickHandler = Import;

            BaseContext.SetFunction06Caption("エクスポート");
            OnF06ClickHandler = Export;

            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);

            BaseContext.SetFunction10Caption("終了");
            OnF10ClickHandler = Exit;
        }
        #endregion

        #region Webサービス呼び出し
        private async Task<List<IgnoreKana>> GetListAsync()
        {
            List<IgnoreKana> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<IgnoreKanaMasterClient>();
                IgnoreKanasResult result = await service.GetItemsAsync(SessionKey, CompanyId);

                if (result.ProcessResult.Result)
                {
                    list = result.IgnoreKanas;
                }
            });

            return list ?? new List<IgnoreKana>();
        }
        #endregion

        #region Entityの出し入れ
        /// <summary>グリッドから選択されたModel。</summary>
        private IgnoreKana CurrentEntry { get; set; } = new IgnoreKana();

        /// <summary>対象外区分コードに対応するID</summary>
        private int SelectedExcludeCategoryId { get; set; }

        private bool IsNewEntry()
        {
            return !IgnoreKanaList.Contains(CurrentEntry);
        }

        /// <summary>Modelを入力項目に設定する。</summary>
        /// <param name="kana">Model</param>
        private void SetIgnoreKanaEntry(IgnoreKana kana = null)
        {
            CurrentEntry = kana ?? new IgnoreKana();

            bool isNew = IsNewEntry();
            txtKana.Enabled = isNew;
            BaseContext.SetFunction03Enabled(!isNew);

            txtKana.Text = CurrentEntry.Kana;
            txtExcludeCategoryCode.Text = CurrentEntry.ExcludeCategoryCode;
            lblExcludeCategoryName.Text = CurrentEntry.ExcludeCategoryName;
            SelectedExcludeCategoryId = CurrentEntry.ExcludeCategoryId;
            IsModified = false;
        }

        /// <summary>入力項目からModelを生成する。</summary>
        /// <returns>生成されたModel</returns>
        private IgnoreKana CreateModified()
        {
            IgnoreKana kana = new IgnoreKana();
            Model.CopyTo(CurrentEntry, kana, true);

            kana.Kana = txtKana.Text.Trim();
            kana.ExcludeCategoryId = SelectedExcludeCategoryId;
            kana.ExcludeCategoryCode = txtExcludeCategoryCode.Text;
            kana.ExcludeCategoryName = lblExcludeCategoryName.Text;

            return kana;
        }
        #endregion

        private bool ValidateEntries()
        {         
            if (string.IsNullOrEmpty(txtExcludeCategoryCode.Text))
            {
                txtExcludeCategoryCode.Focus();
                ShowWarningDialog(MsgWngInputRequired, "対象外区分コード");
                return false;
            }
            return true;
        }

        private void ZeroLeftPaddingWithoutValidated()
        {
            if (IsNeedValidate(0, txtExcludeCategoryCode.TextLength, txtExcludeCategoryCode.MaxLength))
            {
                txtExcludeCategoryCode.Text = ZeroLeftPadding(txtExcludeCategoryCode);
                txtExcludeCategoryCode_Validated(null, null);
            }
        }

        #region Function event handler
        [OperationLog("登録")]
        private void Register()
        {
            if (!ValidateChildren()) return;

            if (!ValidateEntries()) return;

            ZeroLeftPaddingWithoutValidated();

            if (!ShowConfirmDialog(MsgQstConfirmSave))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }

            IgnoreKana kana = CreateModified();
            List<IgnoreKana> reloaded = null;
            IgnoreKanaResult result = null;
            try
            {
                kana.CompanyId = CompanyId;
                kana.UpdateBy = Login.UserId;

                Task task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<IgnoreKanaMasterClient>();
                    result = await service.SaveAsync(SessionKey, kana);

                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                ShowWarningDialog(MsgErrSaveError);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }

            if ((result?.ProcessResult.Result ?? false) && result.IgnoreKana != null)
            {
                var task = Task.Run(async () =>
                {
                    reloaded = await GetListAsync();
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                IgnoreKanaList.Clear();
                Clear(true, withStatus: false);
                IgnoreKanaList.AddRange(reloaded);
                grdExcludeKanaList.DataSource = new BindingSource(IgnoreKanaList, null);
                DispStatusMessage(MsgInfSaveSuccess);
            }
            else
            {
                ShowWarningDialog(MsgErrSaveError);
            }
        }

        [OperationLog("削除")]
        private void Delete()
        {
            if (!ValidateChildren()) return;

            List<IgnoreKana> reloaded = null;
            try
            {
                if (!ShowConfirmDialog(MsgQstConfirmDelete))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                CountResult deleteResult = null;
                Task task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<IgnoreKanaMasterClient>();
                    deleteResult = await service.DeleteAsync(SessionKey, CompanyId, CurrentEntry.Kana);

                    if ((deleteResult?.ProcessResult.Result ?? false) && deleteResult.Count > 0)
                    {
                        reloaded = await GetListAsync();
                        IgnoreKanaList.Clear();
                        Clear(true, withStatus: false);
                        IgnoreKanaList.AddRange(reloaded);

                        grdExcludeKanaList.DataSource = new BindingSource(IgnoreKanaList, null);
                        DispStatusMessage(MsgInfDeleteSuccess);
                    }
                    else
                    {
                        ShowWarningDialog(MsgErrDeleteError);
                    }
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                ShowWarningDialog(MsgErrDeleteError);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            if (IsModified && !ShowConfirmDialog(MsgQstConfirmClear)) return;
            Clear(false);
        }

        /// <summary>画面クリア</summary>
        /// <param name="force">確認メッセージを表示せずに処理を強制する</param>
        /// <param name="withStatus">ステータスメッセージもクリアする</param>
        private void Clear(bool force, bool withStatus = true)
        {
            SetIgnoreKanaEntry(null);

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction05Enabled(Authorities[MasterImport]);
            BaseContext.SetFunction06Enabled(Authorities[MasterExport]);
            BaseContext.SetFunction10Enabled(true);
            txtKana.Select();

            if (withStatus) ClearStatusMessage();

            IsModified = false;
        }

        [OperationLog("インポート")]
        private void Import()
        {
            ClearStatusMessage();
            try
            {
                ImportSetting importSetting = null;
                var task = Util.GetMasterImportSettingAsync(Login, ImportFileType.IgnoreKana);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                importSetting = task.Result;

                var definition = new IgnoreKanaFileDefinition(new DataExpression(ApplicationControl));
                definition.ExcludeCategoryIdField.GetModelsByCode = val =>
                {
                    CategoriesResult result = null;
                    ServiceProxyFactory.LifeTime(factory =>
                    {
                        var categoryMaster = factory.Create<CategoryMasterClient>();
                        result = categoryMaster.GetByCode(SessionKey, CompanyId, ExcludeCategoryType, val);
                    });
                    if (result.ProcessResult.Result)
                    {
                        return result.Categories
                            .ToDictionary(c => c.Code);
                    }
                    return new Dictionary<string, Category>();
                };

                var importer = definition.CreateImporter(m => m.Kana);
                importer.UserId = Login.UserId;
                importer.UserCode = Login.UserCode;
                importer.CompanyId = CompanyId;
                importer.CompanyCode = Login.CompanyCode;
                importer.LoadAsync = async () => await GetListAsync();
                importer.RegisterAsync = async unitOfWork => await RegisterForImportAsync(unitOfWork);

                var importResult = DoImport(importer, importSetting);
                if (!importResult) return;
                IgnoreKanaList.Clear();
                Task<List<IgnoreKana>> loadTask = GetListAsync();
                Clear(true, false);
                ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);
                IgnoreKanaList.AddRange(loadTask.Result);

                grdExcludeKanaList.DataSource = new BindingSource(IgnoreKanaList, null);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrImportErrorWithoutLog);
            }
        }

        /// <summary>インポートデータ登録</summary>
        /// <param name="imported">CSVから生成したModel</param>
        /// <returns>登録結果</returns>
        private async Task<ImportResult> RegisterForImportAsync(UnitOfWork<IgnoreKana> imported)
        {
            ImportResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<IgnoreKanaMasterClient>();
                result = await service.ImportAsync(SessionKey,
                        imported.New.ToArray(), imported.Dirty.ToArray(), imported.Removed.ToArray());
            });

            return result;
        }

        [OperationLog("エクスポート")]
        private void Export()
        {
            try
            {
                Task<List<IgnoreKana>> loadTask = GetListAsync();
                ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);
                List<IgnoreKana> list = loadTask.Result;

                if (!list.Any())
                {
                    ShowWarningDialog(MsgWngNoExportData);
                    return;
                }

                string serverPath = null;
                Task task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<GeneralSettingMasterClient>();
                    GeneralSettingResult result = await service.GetByCodeAsync(SessionKey, CompanyId, ExportPathCode);

                    if (result.ProcessResult.Result)
                    {
                        serverPath = result.GeneralSetting?.Value;
                    }
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (!Directory.Exists(serverPath))
                {
                    serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                var filePath = string.Empty;
                var fileName = $"除外カナマスター{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var definition = new IgnoreKanaFileDefinition(new DataExpression(ApplicationControl));
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
        private void Exit()
        {
            ClearStatusMessage();
            if (IsModified && !ShowConfirmDialog(MsgQstConfirmClose)) return;
            ParentForm.Close();
        }
        #endregion

        private void PB1501_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                var loadTask = new List<Task>();
                if (Company == null)
                {
                    loadTask.Add(LoadCompanyAsync());
                }
                if (ApplicationControl == null)
                {
                    loadTask.Add(LoadApplicationControlAsync());
                }
                loadTask.Add(LoadControlColorAsync());
                loadTask.Add(LoadFunctionAuthorities(MasterImport, MasterExport));

                Task<List<IgnoreKana>> loadListTask = GetListAsync();
                loadListTask.ContinueWith(task => IgnoreKanaList.AddRange(task.Result));
                loadTask.Add(loadListTask);

                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                var expression = new DataExpression(ApplicationControl);
                txtKana.MaxLength = expression.IgnoreKanaLength;

                InitializeKanaGrid();
                grdExcludeKanaList.DataSource = new BindingSource(IgnoreKanaList, null);
                Clear(true);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void grdExcludeKanaList_CellDoubleClick(object sender, CellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (IsModified && !ShowConfirmDialog(MsgQstConfirmUpdateData)) return;
            SetIgnoreKanaEntry(IgnoreKanaList[e.RowIndex]);
            txtExcludeCategoryCode.Focus();
            BaseContext.SetFunction03Enabled(true);
            ClearStatusMessage();
        }

        private void txtExcludeCategoryCode_Validated(object sender, EventArgs e)
        {
            try
            {
                Category result = null;
                if (string.IsNullOrEmpty(txtExcludeCategoryCode.Text))
                {
                    SetCategory(null);
                    ClearStatusMessage();
                    return;
                }

                Task task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<CategoryMasterClient>();
                    CategoriesResult res = await service.GetByCodeAsync(SessionKey, CompanyId,
                        ExcludeCategoryType, new[] { txtExcludeCategoryCode.Text });

                    if (res.ProcessResult.Result)
                    {
                        result = res.Categories.FirstOrDefault();
                    }
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (result == null)
                {
                    ShowWarningDialog(MsgWngMasterNotExist, "対象外区分", txtExcludeCategoryCode.Text);
                    txtExcludeCategoryCode.Clear();
                    lblExcludeCategoryName.Clear();
                    txtExcludeCategoryCode.Focus();
                }
                else
                {
                    ClearStatusMessage();
                    SetCategory(result);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtKana_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                string converted = txtKana.Text.Trim();

                IgnoreKana target = IgnoreKanaList.FirstOrDefault(k => k.Kana == converted);
                if (target != null)
                {
                    SetIgnoreKanaEntry(target);
                    BaseContext.SetFunction03Enabled(true);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void SetCategory(Category category)
        {
            if (category != null)
            {
                txtExcludeCategoryCode.Text = category.Code;
                lblExcludeCategoryName.Text = category.Name;
                SelectedExcludeCategoryId = category.Id;
            }
            else
            {
                txtExcludeCategoryCode.Clear();
                lblExcludeCategoryName.Clear();
                SelectedExcludeCategoryId = 0;
            }
        }

        private void btnSearchExcludeCategory_Click(object sender, EventArgs e)
        {
            var category = this.ShowExcludeCategorySearchDialog();
            if (category != null)
            {
                SetCategory(category);
                ClearStatusMessage();
            }
        }
    }
}
