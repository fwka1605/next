using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.Controls;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.LoginUserMasterService;
using Rac.VOne.Client.Screen.SectionMasterService;
using Rac.VOne.Client.Screen.SectionWithLoginUserMasterService;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Export;
using Rac.VOne.Import;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;
using static Rac.VOne.Web.Models.FunctionType;
using Section = Rac.VOne.Web.Models.Section;

namespace Rac.VOne.Client.Screen
{
    /// <summary>入金部門・担当者対応マスター</summary>
    public partial class PB1301 : VOneScreenBase
    {
        #region 画面のinitalize　とメッセージボックス
        private List<SectionWithLoginUser> ModifySection { get; set; }
        private List<SectionWithLoginUser> OriginSection { get; set; }
        private int? LoginUserId { get; set; }
        private int? SectionIdFrom = null;
        private int? SectionIdTo = null;
        private bool CloseFlg { get; set; }
        private string CellName(string value) => $"cel{value}";

        public PB1301()
        {
            InitializeComponent();
            grdLoginUserOrigin.SetupShortcutKeys();
            grdLoginUserModify.SetupShortcutKeys();
            Text = "入金部門・担当者対応マスター";
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();
            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Save;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = Clear;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);

            BaseContext.SetFunction04Caption("印刷");
            BaseContext.SetFunction04Enabled(true);
            OnF04ClickHandler = Print;

            BaseContext.SetFunction05Caption("インポート");
            BaseContext.SetFunction05Enabled(true);
            OnF05ClickHandler = Import;

            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction06Enabled(true);
            OnF06ClickHandler = Export;

            BaseContext.SetFunction07Caption("");
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Caption("");
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Caption("");
            BaseContext.SetFunction09Enabled(false);
            BaseContext.SetFunction10Caption("終了");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Exit;
        }

        private void SetFormat()
        {
            var expression = new DataExpression(ApplicationControl);
            txtLoginUserCode.Format = expression.LoginUserCodeFormatString;
            txtLoginUserCode.MaxLength = expression.LoginUserCodeLength;
            txtSectionFrom.Format = expression.SectionCodeFormatString;
            txtSectionFrom.MaxLength = expression.SectionCodeLength;
            txtSectionTo.Format = expression.SectionCodeFormatString;
            txtSectionTo.MaxLength = expression.SectionCodeLength;

            if (ApplicationControl != null)
            {
                if (ApplicationControl.SectionCodeType == 0)
                {
                    txtSectionFrom.PaddingChar = '0';
                    txtSectionTo.PaddingChar = '0';
                }

                if (ApplicationControl.LoginUserCodeType == 0)
                {
                    txtLoginUserCode.PaddingChar = '0';
                }
            }
        }

        private void BeforeParentSearch()
        {
            txtLoginUserCode.Enabled = true;
            btnLoginUserSearch.Enabled = true;
            txtLoginUserCode.Focus();
            txtSectionFrom.Enabled = false;
            txtSectionTo.Enabled = false;
            btnSectionFrom.Enabled = false;
            btnSectionTo.Enabled = false;
            btnAdd.Enabled = false;
            btnDelete.Enabled = false;
            btnDeleteAll.Enabled = false;
        }

        private void AfterParentSearch()
        {
            txtLoginUserCode.Enabled = false;
            btnLoginUserSearch.Enabled = false;
            txtSectionFrom.Enabled = true;
            txtSectionTo.Enabled = true;
            btnSectionFrom.Enabled = true;
            btnSectionTo.Enabled = true;
            btnAdd.Enabled = true;
            btnDelete.Enabled = true;
            btnDeleteAll.Enabled = true;
        }

        private void InitializeLoginGridLoad()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "Header"                                                                                     , cell: builder.GetRowHeaderCell()),
                new CellSetting(height, 115, nameof(SectionWithLoginUser.SectionCode), dataField: nameof(SectionWithLoginUser.SectionCode), cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)      , caption: "入金部門コード" ),
                new CellSetting(height, 100, nameof(SectionWithLoginUser.SectionName), dataField: nameof(SectionWithLoginUser.SectionName), cell: builder.GetTextBoxCell()                                           , caption: "入金部門名"     ),
                new CellSetting(height,   0, nameof(SectionWithLoginUser.SectionId)  , dataField: nameof(SectionWithLoginUser.SectionId)  , visible: true              )
            });
            grdLoginUserModify.Template = builder.Build();
            grdLoginUserModify.SetRowColor(ColorContext);
            grdLoginUserModify.HideSelection = true;
        }

        private void InitializeLoginGridOriginLoad()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "Header"                                                                                      , cell: builder.GetRowHeaderCell() ),
                new CellSetting(height, 115, nameof(SectionWithLoginUser.SectionCode), dataField: nameof(SectionWithLoginUser.SectionCode), cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), caption: "入金部門コード" ),
                new CellSetting(height, 100, nameof(SectionWithLoginUser.SectionName), dataField: nameof(SectionWithLoginUser.SectionName), cell: builder.GetTextBoxCell()                                     , caption: "入金部門名"     ),
                new CellSetting(height,   0, nameof(SectionWithLoginUser.SectionId)  , dataField: nameof(SectionWithLoginUser.SectionId)  , visible: true             )
            });
            grdLoginUserOrigin.Template = builder.Build();
            grdLoginUserOrigin.SetRowColor(ColorContext);
            grdLoginUserOrigin.HideSelection = true;
        }

        private void PB1301_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                txtLoginUserCode.Clear();
                lblLoginUserNames.Clear();
                txtLoginUserCode.Focus();
                BeforeParentSearch();
                ModifySection = new List<SectionWithLoginUser>();
                OriginSection = new List<SectionWithLoginUser>();
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

                InitializeLoginGridOriginLoad();
                InitializeLoginGridLoad();

                SetFormat();
                AddHandlers();
                ClearItems();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void grdLoginUserModify_CellDoubleClick(object sender, CellEventArgs e)
        {
            ClearStatusMessage();

            if (e.RowIndex >= 0)
            {
                if (txtSectionFrom.Text == "")
                {
                    txtSectionFrom.Text = grdLoginUserModify.Rows[e.RowIndex].Cells[CellName(nameof(SectionWithLoginUser.SectionCode))].DisplayText;
                    lblSectionFromName.Text = grdLoginUserModify.Rows[e.RowIndex].Cells[CellName(nameof(SectionWithLoginUser.SectionName))].DisplayText;
                    SectionIdFrom = int.Parse(grdLoginUserModify.Rows[e.RowIndex].Cells[CellName(nameof(SectionWithLoginUser.SectionId))].DisplayText);
                }
                else
                {
                    txtSectionTo.Text = grdLoginUserModify.Rows[e.RowIndex].Cells[CellName(nameof(SectionWithLoginUser.SectionCode))].DisplayText;
                    lblSectionToName.Text = grdLoginUserModify.Rows[e.RowIndex].Cells[CellName(nameof(SectionWithLoginUser.SectionName))].DisplayText;
                    SectionIdTo = int.Parse(grdLoginUserModify.Rows[e.RowIndex].Cells[CellName(nameof(SectionWithLoginUser.SectionId))].DisplayText);
                }
            }
        }

        private void AddHandlers()
        {
            foreach (Control control in gbxSectionInput.Controls)
            {
                if (control is VOneTextControl)
                {
                    control.TextChanged += new EventHandler(OnContentChanged);
                }
            }
        }

        private void OnContentChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        #endregion

        #region 画面のFunction keys events
        [OperationLog("登録")]
        private void Save()
        {
            try
            {
                ClearStatusMessage();

                ZeroLeftPaddingWithoutValidated();

                if (!ValidateChildren()
                    || !ValidateInputValues()) return;

                SaveSectionWithLoginUser();

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void ZeroLeftPaddingWithoutValidated()
        {
            if (IsNeedValidate(ApplicationControl.LoginUserCodeType, txtLoginUserCode.TextLength, ApplicationControl.LoginUserCodeLength))
            {
                txtLoginUserCode.Text = ZeroLeftPadding(txtLoginUserCode);
                txtLoginUserCode_Validated(null, null);
            }
            if (IsNeedValidate(ApplicationControl.SectionCodeType, txtSectionFrom.TextLength, ApplicationControl.SectionCodeLength))
            {
                txtSectionFrom.Text = ZeroLeftPadding(txtSectionFrom);
                txtSectionFrom_Validated(null, null);
            }
            if (IsNeedValidate(ApplicationControl.SectionCodeType, txtSectionTo.TextLength, ApplicationControl.SectionCodeLength))
            {
                txtSectionTo.Text = ZeroLeftPadding(txtSectionTo);
                txtSectionTo_Validated(null, null);
            }
        }

        private bool ValidateInputValues()
        {
            if (string.IsNullOrEmpty(txtLoginUserCode.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "ログインユーザーコード");
                txtLoginUserCode.Focus();
                return false;
            }

            var id = "";
            var args = new string[] { };
            if (string.IsNullOrWhiteSpace(txtSectionFrom.Text) &&
                string.IsNullOrWhiteSpace(txtSectionTo.Text))
            {
                id = MsgQstConfirmSave;
            }
            else
            {
                id = MsgQstConfirmUpdateSelectData; args = new string[] { "入金部門" };
            }

            if (!ShowConfirmDialog(id, args))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return false;
            }
            return true;
        }

        private void SaveSectionWithLoginUser()
        {
            CloseFlg = true;

            if (!string.IsNullOrWhiteSpace(txtSectionFrom.Text)
               || !string.IsNullOrWhiteSpace(txtSectionTo.Text))
            {
                Task<int> containsNew = AddLoginUserWithSectionAsync();
                ProgressDialog.Start(ParentForm, containsNew, false, SessionKey);

                if (containsNew.Result == 0)
                {
                    CloseFlg = false;
                    return;
                }
            }

            bool success = false;
            List<SectionWithLoginUser> list = null;

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var deleteList = GetPrepareSaveData();
                var service = factory.Create<SectionWithLoginUserMasterClient>();
                SectionWithLoginUserResult result = await service.SaveAsync(SessionKey, ModifySection.ToArray(), deleteList.ToArray());

                success = result?.ProcessResult.Result ?? false;

                if (success)
                {
                    list = await GetByLoginUser();
                }
            });

            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (success)
            {
                DispStatusMessage(MsgInfSaveSuccess);
                ModifySection.Clear();

                OriginSection = list;
                ModifySection = OriginSection;
                grdLoginUserModify.DataSource = new BindingSource(OriginSection, null);
                grdLoginUserOrigin.DataSource = new BindingSource(ModifySection, null);
                txtSectionFrom.Focus();
                ClearFromTo();
                Modified = false;
            }
            else
            {
                ShowWarningDialog(MsgErrSaveError);
            }

        }

        private async Task<int> AddLoginUserWithSectionAsync()
        {
            List<Section> sectionListToCheck = await GetAllSectionAsync();

            var codeFrom = string.IsNullOrWhiteSpace(txtSectionFrom.Text) ? txtSectionTo.Text : txtSectionFrom.Text;
            var codeTo = string.IsNullOrWhiteSpace(txtSectionTo.Text) ? txtSectionFrom.Text : txtSectionTo.Text;

            Func<Section, bool> range = d => (string.Compare(codeFrom, d.Code) <= 0 && string.Compare(d.Code, codeTo) <= 0);

            var newSections = (sectionListToCheck).Where(range)
                .Where(d => !ModifySection.Any(swd => swd.SectionCode == d.Code)).ToList();
            if (!newSections.Any())
            {
                //既存得意先を追加する場合
                ShowWarningDialog(MsgWngNoData, "追加");
                ClearFromTo();
            }
            else
            {
                ModifySection.AddRange(newSections.Select(d => new SectionWithLoginUser
                {
                    SectionId = d.Id,
                    SectionCode = d.Code,
                    SectionName = d.Name
                }));
                ModifySection = ModifySection.OrderBy(d => d.SectionCode).ToList();
            }

            return newSections.Count;
        }

        private List<SectionWithLoginUser> GetPrepareSaveData()
        {
            var deleteList = new List<SectionWithLoginUser>();
            var modifySectionCount = ModifySection.Count;

            if (modifySectionCount > 0)
            {
                for (var i = 0; i < modifySectionCount; i++)
                {
                    ModifySection[i].LoginUserId = LoginUserId.Value;
                    ModifySection[i].CreateBy = Login.UserId;
                    ModifySection[i].UpdateBy = Login.UserId;
                }
            }

            for (var j = 0; j < OriginSection.Count; j++)
            {
                bool result = ModifySection.Exists(x => x.SectionCode == OriginSection[j].SectionCode);
                if (!result)
                {
                    deleteList.Add(OriginSection[j]);
                }
            }

            return deleteList;
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            if (Modified
              && !ShowConfirmDialog(MsgQstConfirmClear))
            {
                return;
            }

            ClearItems();
            ClearStatusMessage();
            Modified = false;
        }

        //クリア項目とグッリド内容
        private void ClearItems()
        {
            txtLoginUserCode.Clear();
            lblLoginUserNames.Clear();
            grdLoginUserModify.DataSource = null;
            grdLoginUserOrigin.DataSource = null;
            txtSectionFrom.Clear();
            lblSectionFromName.Clear();
            txtSectionTo.Clear();
            lblSectionToName.Clear();
            BeforeParentSearch();
            Modified = false;
            BaseContext.SetFunction05Enabled(Authorities[MasterImport]);
            BaseContext.SetFunction06Enabled(Authorities[MasterExport]);
        }

        private void ClearFromTo()
        {
            txtSectionFrom.Clear();
            lblSectionFromName.Clear();
            txtSectionTo.Clear();
            lblSectionToName.Clear();
        }

        [OperationLog("印刷")]
        private void Print()
        {
            try
            {
                ClearStatusMessage();
                List<SectionWithLoginUser> list = null;
                string serverPath = null;
                SectionWithLoginUserReport sectionWithLoginUserReport = null;
                ProgressDialog.Start(ParentForm, Task.Run(async () =>
                {
                    list = await SectionWithLoginUserData();
                    if (!list.Any()) return;

                    serverPath = await GetServerPath();

                    sectionWithLoginUserReport = new SectionWithLoginUserReport();
                    sectionWithLoginUserReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                    sectionWithLoginUserReport.Name = "入金部門・担当者対応マスター一覧" + DateTime.Now.ToString("yyyyMMdd");
                    sectionWithLoginUserReport.SetData(list);
                    sectionWithLoginUserReport.Run(true);
                }), false, SessionKey);

                if (!list.Any())
                {
                    ShowWarningDialog(MsgWngPrintDataNotExist);
                    return;
                }

                ShowDialogPreview(ParentForm, sectionWithLoginUserReport, serverPath);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        private async Task<string> GetServerPath()
        {
            var serverPath = string.Empty;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<GeneralSettingMasterClient>();
                GeneralSettingResult result = await service.GetByCodeAsync(SessionKey, CompanyId, "サーバパス");

                if (result.ProcessResult.Result)
                {
                    serverPath = result.GeneralSetting?.Value;
                }
            });

            return serverPath;
        }

        [OperationLog("インポート")]
        private void Import()
        {
            ClearStatusMessage();
            try
            {
                ImportSetting importSetting = null;
                var task = Util.GetMasterImportSettingAsync(Login, ImportFileType.SectionWithLoginUser);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                importSetting = task.Result;
                var definition = new SectionWithLoginUserFileDefinition(new DataExpression(ApplicationControl));
                definition.SectionCodeField.GetModelsByCode = val =>
                {
                    Dictionary<string, Section> product = null;
                    ServiceProxyFactory.LifeTime(factory =>
                    {
                        var section = factory.Create<SectionMasterClient>();
                        SectionsResult result = section.GetByCode(SessionKey, CompanyId, val);

                        if (result.ProcessResult.Result)
                        {
                            product = result.Sections
                                .ToDictionary(c => c.Code);
                        }
                    });
                    return product ?? new Dictionary<string, Section>();
                };

                definition.LoginUserCodeField.GetModelsByCode = val =>
                {
                    Dictionary<string, LoginUser> product = null;
                    ServiceProxyFactory.LifeTime(factory =>
                    {
                        var loginUser = factory.Create<LoginUserMasterClient>();
                        UsersResult res = loginUser.GetByCode(SessionKey, CompanyId, val);

                        if (res.ProcessResult.Result)
                        {
                            product = res.Users
                            .ToDictionary(c => c.Code);
                        }
                    });
                    return product ?? new Dictionary<string, LoginUser>();
                };

                var importer = definition.CreateImporter(m => new { m.LoginUserId, m.SectionId });
                importer.UserId = Login.UserId;
                importer.UserCode = Login.UserCode;
                importer.CompanyId = CompanyId;
                importer.CompanyCode = Login.CompanyCode;
                importer.LoadAsync = async () => await SectionWithLoginUserData();
                importer.RegisterAsync = async unitOfWork => await RegisterForImportAsync(unitOfWork);

                var importResult = DoImport(importer, importSetting);
                if (!importResult) return;
                ClearFromTo();
                BeforeParentSearch();
                txtLoginUserCode.Clear();
                lblLoginUserNames.Clear();
                grdLoginUserModify.DataSource = null;
                grdLoginUserOrigin.DataSource = null;
                txtLoginUserCode.Focus();
                Modified = false;

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrImportErrorWithoutLog);
            }
        }

        private async Task<List<SectionWithLoginUser>> SectionWithLoginUserData()
        {
            var list = new List<SectionWithLoginUser>();

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<SectionWithLoginUserMasterClient>();

                SectionWithLoginUsersResult result = await service.GetItemsAsync(SessionKey, CompanyId, new SectionWithLoginUserSearch());

                if (result.ProcessResult.Result)
                {
                    list = result.SectionWithLoginUsers;
                }
            });

            return list ?? new List<SectionWithLoginUser>();
        }

        private async Task<ImportResult> RegisterForImportAsync(UnitOfWork<SectionWithLoginUser> imported)
        {
            ImportResultSectionWithLoginUser result = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<SectionWithLoginUserMasterClient>();
                result = await service.ImportAsync(SessionKey,
                                imported.New.ToArray(), imported.Dirty.ToArray(), imported.Removed.ToArray());
            });
            return result ?? new ImportResultSectionWithLoginUser();
        }

        [OperationLog("エスポート")]
        private void Export()
        {
            ClearStatusMessage();
            var serverPath = string.Empty;

            try
            {
                Task<List<SectionWithLoginUser>> loadTask = SectionWithLoginUserData();
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<GeneralSettingMasterClient>();
                    GeneralSettingResult result = await service.GetByCodeAsync(SessionKey, CompanyId, "サーバパス");

                    if (result.ProcessResult.Result)
                    {
                        serverPath = result.GeneralSetting?.Value;
                    }

                    if (!Directory.Exists(serverPath))
                    {
                        serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    }
                });
                ProgressDialog.Start(ParentForm, Task.WhenAll(task, loadTask), false, SessionKey);
                List<SectionWithLoginUser> list = loadTask.Result;

                if (!list.Any())
                {
                    ShowWarningDialog(MsgWngNoExportData);
                    return;
                }

                var filePath = string.Empty;
                var fileName = $"入金部門・担当者対応マスター{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var definition = new SectionWithLoginUserFileDefinition(
                                new DataExpression(ApplicationControl));
                var exporter = definition.CreateExporter();
                exporter.UserId = Login.UserId;
                exporter.UserCode = Login.UserCode;
                exporter.CompanyId = Login.CompanyId;
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

            if (Modified && IsConfirmRequired)
            {
                var result = ShowConfirmDialogYesNoCancel(MsgQstConfirmHasUpdateData, "終了");
                if (result == DialogResult.Cancel) return;
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        if (!CheckRangeData()) return;

                        SaveSectionWithLoginUser();
                        if (!CloseFlg) return;

                    }
                    catch (Exception ex)
                    {
                        Debug.Fail(ex.ToString());
                        NLogHandler.WriteErrorLog(this, ex, SessionKey);
                    }
                }
            }
            BaseForm.Close();
        }
        #endregion

        #region 画面の検索とValidated
        private void btnLoginUserSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                var loginUser = this.ShowLoginUserSearchDialog();
                ProgressDialog.Start(ParentForm, SetSectionWithLoginUser(loginUser), false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private Task SetSectionWithLoginUser(LoginUser loginUser)
        {
            if (loginUser == null)
            {
                txtLoginUserCode.Clear();
                lblLoginUserNames.Clear();
                return null;
            }

            txtLoginUserCode.Text = loginUser.Code;
            lblLoginUserNames.Text = loginUser.Name;
            LoginUserId = loginUser.Id;
            AfterParentSearch();

            if (string.IsNullOrWhiteSpace(txtLoginUserCode.Text))
            {
                txtSectionFrom.Focus();
                return null;
            }

            return GetByLoginUser()
                .ContinueWith(t =>
                {
                    ModifySection = t.Result;
                    OriginSection = OriginSection.OrderBy(x => x.SectionCode).ToList();
                    OriginSection = ModifySection;
                    grdLoginUserModify.DataSource = new BindingSource(ModifySection, null);
                    grdLoginUserOrigin.DataSource = new BindingSource(OriginSection, null);

                    if (!ModifySection.Any())
                    {
                        DispStatusMessage(MsgInfSaveNewData, "ログインユーザー");
                    }

                    txtSectionFrom.Focus();

                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void SetSectionData(Section section, VOneTextControl code, VOneDispLabelControl name, ref int? sectionId)
        {
            if (section == null)
            {
                code.Clear();
                name.Clear();
                sectionId = 0;
                return;
            }

            code.Text = section.Code;
            name.Text = section.Name;
            sectionId = section.Id;
        }

        private void btnSectionFrom_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var section = this.ShowSectionSearchDialog();
            if (section != null)
            {
                SetSectionData(section, txtSectionFrom, lblSectionFromName, ref SectionIdFrom);
                ClearStatusMessage();
            }
        }

        private void btnSectionTo_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var section = this.ShowSectionSearchDialog();
            if (section != null)
            {
                SetSectionData(section, txtSectionTo, lblSectionToName, ref SectionIdTo);
                ClearStatusMessage();
            }
        }

        private void txtLoginUserCode_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();

                if (string.IsNullOrWhiteSpace(txtLoginUserCode.Text))
                {
                    lblLoginUserNames.Clear();
                    return;
                }

                var success = true;
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<LoginUserMasterClient>();
                    UsersResult result = await service.GetByCodeAsync(
                            SessionKey, CompanyId, new[] { txtLoginUserCode.Text });

                    if (result.ProcessResult.Result && result.Users.Any())
                    {
                        await SetSectionWithLoginUser(result.Users[0]);
                    }
                    else
                    {
                        ShowWarningDialog(MsgWngMasterNotExist, "ログインユーザー", txtLoginUserCode.Text);
                        success = false;
                    }
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (!success)
                {
                    txtLoginUserCode.Clear();
                    lblLoginUserNames.Clear();
                    txtLoginUserCode.Focus();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        //From~ToテキストボックスのValidated
        private void GetDataFromValidate(VOneTextControl code, VOneDispLabelControl name, ref int? Id)
        {
            var secGetByCode = new List<Section>();

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<SectionMasterClient>();
                SectionsResult result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { code.Text });
                if (result.ProcessResult.Result && result.Sections.Any())
                {
                    secGetByCode = result.Sections;
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (secGetByCode.Any())
            {
                SetSectionData(secGetByCode[0], code, name, ref Id);
                ClearStatusMessage();
            }
            else
            {
                ShowWarningDialog(MsgWngMasterNotExist, "入金部門", code.Text);
                code.Focus();
                name.Clear();
                code.Clear();
            }

            Modified = true;
        }

        private void txtSectionFrom_Validated(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtSectionFrom.Text))
                {
                    GetDataFromValidate(txtSectionFrom, lblSectionFromName, ref SectionIdFrom);
                }
                else
                {
                    SetSectionData(null, txtSectionFrom, lblSectionFromName, ref SectionIdFrom);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtSectionTo_Validated(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtSectionTo.Text))
                {
                    GetDataFromValidate(txtSectionTo, lblSectionToName, ref SectionIdTo);
                }
                else
                {
                    SetSectionData(null, txtSectionTo, lblSectionToName, ref SectionIdTo);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region Web Service呼び出し
        private async Task<List<Section>> GetAllSectionAsync()
        {
            var list = new List<Section>();
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<SectionMasterClient>();
                var result = await service.GetByCodeAsync(SessionKey, CompanyId, Code: null);

                if (result.ProcessResult.Result)
                {
                    list = result.Sections;
                }
            });

            return list ?? new List<Section>();
        }

        public async Task<List<SectionWithLoginUser>> GetByLoginUser()
        {
            if (LoginUserId == 0) return new List<SectionWithLoginUser>();
            var list = new List<SectionWithLoginUser>();

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<SectionWithLoginUserMasterClient>();
                SectionWithLoginUsersResult result = await service.GetByLoginUserAsync(SessionKey, CompanyId, LoginUserId.Value);

                if (result.ProcessResult.Result)
                {
                    list = result.SectionWithLoginUsers;
                }
            });

            return list ?? new List<SectionWithLoginUser>();
        }
        #endregion

        #region 画面の一括削除ボタン、削除ボタン、追加ボタン
        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            try
            {
                this.ButtonClicked(btnDeleteAll);
                ClearStatusMessage();

                if (!ModifySection.Any())
                {
                    ShowWarningDialog(MsgWngNoData, "削除");
                    return;
                }

                bool result = ShowConfirmDialog(MsgQstConfirmDeleteAll);

                if (result)
                {
                    ModifySection.Clear();
                    grdLoginUserModify.DataSource = new BindingSource(ModifySection, null);
                    Task<List<SectionWithLoginUser>> getTask = GetByLoginUser();
                    ProgressDialog.Start(ParentForm, getTask, false, SessionKey);
                    OriginSection = getTask.Result;
                    grdLoginUserOrigin.DataSource = new BindingSource(OriginSection, null);
                    Modified = true;
                }

                ClearFromTo();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                this.ButtonClicked(btnDelete);
                ClearStatusMessage();

                if (!CheckRangeData()) return;

                if (string.IsNullOrWhiteSpace(txtSectionFrom.Text) && string.IsNullOrWhiteSpace(txtSectionTo.Text))
                {
                    ShowWarningDialog(MsgWngInputRequired, "入金部門コード");
                    txtSectionFrom.Focus();
                    return;
                }

                var codeFrom = string.IsNullOrWhiteSpace(txtSectionFrom.Text) ? txtSectionTo.Text : txtSectionFrom.Text;
                var codeTo = string.IsNullOrWhiteSpace(txtSectionTo.Text) ? txtSectionFrom.Text : txtSectionTo.Text;

                Func<SectionWithLoginUser, bool> range = d => (string.Compare(codeFrom, d.SectionCode) <= 0 && string.Compare(d.SectionCode, codeTo) <= 0);

                if (!ModifySection.Any(range))
                {
                    ShowWarningDialog(MsgWngNoData, "削除");
                    ClearFromTo();
                    return;
                }

                ModifySection = ModifySection.Except(ModifySection.Where(range)).OrderBy(s => s.SectionCode).ToList();
                grdLoginUserModify.DataSource = new BindingSource(ModifySection, null);

                Task<List<SectionWithLoginUser>> task = GetByLoginUser();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                OriginSection = task.Result;
                grdLoginUserOrigin.DataSource = new BindingSource(OriginSection, null);

                Modified = true;
                ClearFromTo();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.ButtonClicked(btnAdd);
                ClearStatusMessage();

                if (!CheckRangeData()) return;

                if (string.IsNullOrWhiteSpace(txtSectionFrom.Text) && string.IsNullOrWhiteSpace(txtSectionTo.Text))
                {
                    ShowWarningDialog(MsgWngInputRequired, "入金部門コード");
                    txtSectionFrom.Focus();
                    return;
                }

                Task<int> task = AddSectionAsync();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                grdLoginUserModify.DataSource = new BindingSource(ModifySection, null);
                ClearFromTo();
                Modified = true;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task<int> AddSectionAsync()
        {
            List<Section> sectionListToCheck = await GetAllSectionAsync();

            var codeFrom = string.IsNullOrWhiteSpace(txtSectionFrom.Text) ? txtSectionTo.Text : txtSectionFrom.Text;
            var codeTo = string.IsNullOrWhiteSpace(txtSectionTo.Text) ? txtSectionFrom.Text : txtSectionTo.Text;

            Func<Section, bool> range = d => (string.Compare(codeFrom, d.Code) <= 0 && string.Compare(d.Code, codeTo) <= 0);

            var newSections = (sectionListToCheck).Where(range)
                .Where(d => !ModifySection.Any(swd => swd.SectionCode == d.Code)).ToList();
            if (!newSections.Any())
            {
                //既存得意先を追加する場合
                ShowWarningDialog(MsgWngNoData, "追加");
                ClearFromTo();
            }
            else
            {
                ModifySection.AddRange(newSections.Select(d => new SectionWithLoginUser
                {
                    SectionId = d.Id,
                    SectionCode = d.Code,
                    SectionName = d.Name
                }));
                ModifySection = ModifySection.OrderBy(d => d.SectionCode).ToList();
            }

            return newSections.Count;
        }

        private bool CheckRangeData()
        {
            if (!string.IsNullOrWhiteSpace(txtSectionFrom.Text) && !string.IsNullOrWhiteSpace(txtSectionTo.Text)
                && !txtSectionFrom.ValidateRange(txtSectionTo, () => ShowWarningDialog(MsgWngInputRangeChecked, "入金部門コード")))
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
