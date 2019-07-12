using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.DepartmentMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.ImportSettingMasterService;
using Rac.VOne.Client.Screen.LogDataService;
using Rac.VOne.Client.Screen.LoginUserMasterService;
using Rac.VOne.Client.Screen.SectionWithLoginUserMasterService;
using Rac.VOne.Client.Screen.StaffMasterService;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Export;
using Rac.VOne.Import;
using Rac.VOne.Message;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Web.Models.FunctionType;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>ログインユーザーマスター</summary>
    public partial class PB0301 : VOneScreenBase
    {
        public int LoginUserId { get; set; }
        public int DepartmentId { get; set; } = 0;
        public int? StaffId { get; set; }
        private List<LoginUser> LoginUserList { get; } = new List<LoginUser>();
        private LoginUser CurrentUserData { get; set; }

        public PB0301()
        {
            InitializeComponent();
            InitializeUserComponent();
            grdLoginUser.SetupShortcutKeys();
            Text = "ログインユーザーマスター";
            AddHandlers();
        }

        private void InitializeUserComponent()
        {
            txtMenuLevel.TextChanged += txtMenuLevel_TextChanged;
            txtFunctionLevel.TextChanged += txtFunctionLevel_TextChanged;

            cbxInitializePassword.Checked = true;
            cbxInitializePassword.Enabled = false;
            cbxInitializePassword.CheckedChanged += CbxInitializePassword_CheckedChanged;
        }

        #region 画面の初期化

        private void InitializeLoginUserGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,          40, "Header"                        , cell: builder.GetRowHeaderCell(), sortable: true),
                new CellSetting(height,         150, nameof(LoginUser.Code)          , dataField: nameof(LoginUser.Code)          , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), caption: "ユーザーコード"),
                new CellSetting(height,         150, nameof(LoginUser.Name)          , dataField: nameof(LoginUser.Name)          , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft)  , caption: "ユーザー名"    ),
                new CellSetting(height,         150, nameof(LoginUser.DepartmentName), dataField: nameof(LoginUser.DepartmentName), cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft)  , caption: "請求部門"      ),
                new CellSetting(height,         100, nameof(LoginUser.MenuLevel)     , dataField: nameof(LoginUser.MenuLevel)     , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleRight) , caption: "権限"          ),
                new CellSetting(height,         100, nameof(LoginUser.FunctionLevel) , dataField: nameof(LoginUser.FunctionLevel) , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleRight) , caption: "セキュリティ"  ),
                new CellSetting(height,         150, nameof(LoginUser.Mail)          , dataField: nameof(LoginUser.Mail)          , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft)  , caption: "メールアドレス"),
            });
            grdLoginUser.Template = builder.Build();
            grdLoginUser.SetRowColor(ColorContext);
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            OnF01ClickHandler = Save;

            BaseContext.SetFunction02Caption("クリア");
            OnF02ClickHandler = Clear;

            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = Delete;

            BaseContext.SetFunction04Caption("印刷");
            BaseContext.SetFunction04Enabled(true);
            OnF04ClickHandler = Print;

            BaseContext.SetFunction05Caption("インポート");
            BaseContext.SetFunction05Enabled(true);
            OnF05ClickHandler = ImportData;

            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction06Enabled(true);
            OnF06ClickHandler = ExportData;

            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);

            BaseContext.SetFunction10Caption("終了");
            OnF10ClickHandler = Exit;
        }

        #endregion

        #region Webサービス呼び出し

        private async Task<List<LoginUser>> GetUserDataAsync()
        {
            List<LoginUser> list = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<LoginUserMasterClient>();
                UsersResult result = await service.GetItemsAsync(SessionKey, CompanyId, new LoginUserSearch());

                if (result.ProcessResult.Result)
                {
                    list = result.Users;
                }
            });

            return list ?? new List<LoginUser>();
        }

        /// <summary>
        /// パスワードポリシー取得処理(PasswordPolicyMaster.svc:Get)を呼び出して結果を取得する。
        /// </summary>
        private async Task<PasswordPolicy> GetPasswordPolicyAsync(string sessionKey, int companyId)
        {
            PasswordPolicyResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<PasswordPolicyMasterService.PasswordPolicyMasterClient>();
                result = await client.GetAsync(sessionKey, companyId);
            });

            if (result == null || result.ProcessResult.Result == false)
            {
                return null;
            }

            return result.PasswordPolicy;
        }

        #endregion

        #region Search Dialog Setting

        private void btnEigyoTantoshaSearch_Click(object sender, EventArgs e)
        {
            var staff = this.ShowStaffSearchDialog();
            if (staff != null)
            {
                txtStaffCode.Text = staff.Code;
                lblStaffName.Text = staff.Name;
                StaffId = staff.Id;
                ClearStatusMessage();
            }
        }

        private void txtStaffCode_Validated(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtStaffCode.Text))
                {
                    Task<string> task = GetStaffName(txtStaffCode.Text);
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                    var name = task.Result;

                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        lblStaffName.Text = name;
                        ClearStatusMessage();
                    }
                    else
                    {
                        ShowWarningDialog(MsgWngMasterNotExist, "営業担当者", txtStaffCode.Text);
                        txtStaffCode.Clear();
                        lblStaffName.Clear();
                        StaffId = null;
                        txtStaffCode.Focus();
                    }
                }
                else
                {
                    txtStaffCode.Clear();
                    lblStaffName.Clear();
                    StaffId = null;
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnSeikyuSearch_Click(object sender, EventArgs e)
        {
            var department = this.ShowDepartmentSearchDialog();
            if (department != null)
            {
                txtDepartmentCode.Text = department.Code;
                lblDepartmentName.Text = department.Name;
                DepartmentId = department.Id;
                ClearStatusMessage();
            }
        }

        private void txtDepartmentCode_Validated(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtDepartmentCode.Text))
                {
                    Task<string> task = GetDepartmentName(txtDepartmentCode.Text);
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                    var name = task.Result;
                    if (!string.IsNullOrEmpty(name))
                    {
                        lblDepartmentName.Text = name;
                        ClearStatusMessage();
                    }
                    else
                    {
                        ShowWarningDialog(MsgWngMasterNotExist, "請求部門", txtDepartmentCode.Text);
                        txtDepartmentCode.Clear();
                        lblDepartmentName.Clear();
                        txtDepartmentCode.Focus();
                    }
                }
                else
                {
                    txtDepartmentCode.Clear();
                    lblDepartmentName.Clear();
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task<string> GetDepartmentName(string code)
        {
            var deptName = "";
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<DepartmentMasterClient>();
                DepartmentsResult result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });

                if (result.ProcessResult.Result)
                {
                    Department departmentResult = result.Departments.FirstOrDefault();

                    if (departmentResult != null)
                    {
                        deptName = departmentResult.Name;
                        DepartmentId = departmentResult.Id;
                    }

                }
            });

            return deptName;
        }

        private async Task<string> GetStaffName(string code)
        {
            var staffName = "";

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<StaffMasterClient>();
                StaffsResult result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });

                if (result.ProcessResult.Result)
                {
                    Staff staff = result.Staffs.FirstOrDefault();

                    if (staff != null)
                    {
                        staffName = staff.Name;
                        StaffId = staff.Id;
                    }
                }
            });

            return staffName;
        }

        #endregion

        #region FunctionKey Events
        [OperationLog("登録")]
        public async void Save()
        {
            if (!await ValidateLoginData()) return;

            ZeroLeftPaddingWithoutValidated();

            if (!ShowConfirmDialog(MsgQstConfirmSave))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }

            try
            {
                LoginUser userInsert = LoginUserDataInfo();
                UserResult result = null;
                bool succeeded = false;

                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<LoginUserMasterClient>();
                    result = await service.SaveAsync(SessionKey, userInsert);
                    succeeded = result.ProcessResult.Result;

                    if (succeeded)
                    {
                        LoginUserList.Clear();
                        LoginUserList.AddRange(await GetUserDataAsync());
                    }
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (succeeded)
                {
                    grdLoginUser.DataSource = new BindingSource(LoginUserList, null);
                    ClearAll();
                    DispStatusMessage(MsgInfSaveSuccess);
                }
                else
                {
                    ShowWarningDialog(MsgErrSaveError);
                }
                Modified = false;
                StaffId = null;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            ClearStatusMessage();

            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear))
                return;

            ClearAll();
        }

        private void ClearAll()
        {
            txtUserCode.Clear();
            txtUserCode.Enabled = true;
            ControlCbxInitializePassword();
            txtUserCode.Select();
            txtDepartmentCode.Clear();
            txtMailAddress.Clear();
            txtMenuLevel.Clear();
            txtFunctionLevel.Clear();
            txtStaffCode.Clear();
            txtUserName.Clear();
            txtInitialPassword.Clear();
            txtInitialPasswordConfirmation.Clear();
            lblDepartmentName.Clear();
            lblStaffName.Clear();
            cbxUseClient.Checked = false;
            cbxUseWebviewer.Checked = false;
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction05Enabled(Authorities[MasterImport]);
            BaseContext.SetFunction06Enabled(Authorities[MasterExport]);
            ClearStatusMessage();
            Modified = false;
            StaffId = null;
        }

        [OperationLog("印刷")]
        public void Print()
        {
            try
            {
                string serverPath = null;
                List<LoginUser> list = null;
                LoginUserSectionReport loginReport = null;
                ProgressDialog.Start(ParentForm, Task.Run(async () =>
                {
                    serverPath = await GetServerPath();
                    list = await GetUserDataAsync();
                    if (list.Any())
                    {
                        loginReport = new LoginUserSectionReport();
                        loginReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                        loginReport.Name = "ログインユーザーマスター" + DateTime.Today.ToString("yyyyMMdd");
                        loginReport.UseDistribution = UseDistribution;
                        loginReport.SetData(list);

                        loginReport.Run(false);
                    }
                }), false, SessionKey);

                if (list.Any())
                {
                    ShowDialogPreview(ParentForm, loginReport, serverPath);
                }
                else
                {
                    ShowWarningDialog(MsgWngPrintDataNotExist);
                }
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
            var serverPath = "";

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

        [OperationLog("削除")]
        private void Delete()
        {
            try
            {
                var isValid = true;
                Task<bool> task = ValidateDeleteLoginID();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                isValid &= task.Result;
                if (!isValid) return;

                if (!ShowConfirmDialog(MsgQstConfirmDelete))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                Task<bool> deleteTask = DeleteUserDataByCode();
                ProgressDialog.Start(ParentForm, deleteTask, false, SessionKey);
                if (deleteTask.Result)
                {
                    DispStatusMessage(MsgInfDeleteSuccess);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task<bool> ValidateDeleteLoginID()
        {
            Action<string, string[]> errorHandler = (id, args) =>
            {
                ShowWarningDialog(id, args);
            };

            if (Login?.UserId == LoginUserId)
            {
                errorHandler.Invoke(MsgWngCannotDelCurrentLoginUser, new string[] { });
                return false;
            }

            var loginUserValid = false;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<SectionWithLoginUserMasterClient>();
                var result = await client.ExistLoginUserAsync(SessionKey, LoginUserId);
                loginUserValid = !result.Exist;
                if (result.Exist)
                    errorHandler.Invoke(MsgWngDeleteConstraint, new string[] { "入金部門・担当者対応マスター", "ログインユーザーコード" });
                return;
            });
            if (!loginUserValid) return false;

            return true;
        }

        private async Task<bool> DeleteUserDataByCode()
        {
            CountResult userDataCount = null;
            List<LoginUser> list = null;
            bool flag = false;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<LoginUserMasterClient>();
                userDataCount = await service.DeleteAsync(SessionKey, LoginUserId);

                if (userDataCount.ProcessResult.Result)
                {
                    list = await GetUserDataAsync();
                    LoginUserList.Clear();
                    LoginUserList.AddRange(list);
                    grdLoginUser.DataSource = new BindingSource(LoginUserList, null);
                    ClearAll();
                    flag = true;
                }
            });

            return flag;
        }

        [OperationLog("インポート")]
        private void ImportData()
        {
            ClearStatusMessage();
            try
            {
                ImportSetting importSetting = null;
                var task = Util.GetMasterImportSettingAsync(Login, ImportFileType.LoginUser);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                importSetting = task.Result;

                if (importSetting == null) return;

                var definition = new LoginUserFileDefinition(new DataExpression(ApplicationControl));
                definition.MailField.Required = UseDistribution;
                definition.UseClientField.Ignored = !UseDistribution;
                definition.UseWebViewerField.Ignored = !UseDistribution;

                definition.DepartmentCodeField.GetModelsByCode = val =>
                {
                    Dictionary<string, Department> product = null;
                    ServiceProxyFactory.Do<DepartmentMasterClient>(client =>
                    {
                        var result = client.GetByCode(SessionKey, CompanyId, val);
                        if (result.ProcessResult.Result)
                        {
                            product = result.Departments
                                .ToDictionary(c => c.Code);
                        }
                    });
                    return product ?? new Dictionary<string, Department>();
                };

                definition.StaffCodeField.GetModelsByCode = val =>
                {
                    Dictionary<string, Staff> product = null;
                    ServiceProxyFactory.Do<StaffMasterClient>(client =>
                    {
                        var result = client.GetByCode(SessionKey, CompanyId, val);
                        if (result.ProcessResult.Result)
                        {
                            product = result.Staffs
                                .ToDictionary(c => c.Code);
                        }
                    });
                    return product ?? new Dictionary<string, Staff>();
                };

                definition.LoginUserCodeField.ValidateAdditional = (val, param) =>
                {
                    var reports = new List<WorkingReport>();
                    if (((ImportMethod)param) != ImportMethod.Replace) return reports;

                    UsersResult result = null;
                    ServiceProxyFactory.Do<LoginUserMasterClient>(client =>
                    {
                        result = client.GetImportItemsForSection(
                                SessionKey, CompanyId, val.Values
                                .Where(x => !string.IsNullOrEmpty(x.Code))
                                .Select(x => x.Code)
                                .Distinct().ToArray());
                    });

                    foreach (var user in result.Users.Where(p => !val.Any(a => a.Value.Code == p.Code)))
                    {
                        reports.Add(new WorkingReport
                        {
                            LineNo = null,
                            FieldNo = definition.LoginUserCodeField.FieldIndex,
                            FieldName = definition.LoginUserCodeField.FieldName,
                            Message = $"入金部門・担当者対応マスターに存在する{user.Code}：{user.Name}が存在しないため、インポートできません。",
                        });
                    }
                    return reports;
                };

                // AdditionalValidation: [初回パスワード] 新規登録ユーザーのみ必須チェック＆ポリシー検証
                definition.InitialPasswordField.ValidateAdditional = (val, _) => // val: CsvFileLineNo -> LoginUser
                {
                    var reports = new List<WorkingReport>();

                    var importUserCodeList = val.Values.Select(u => u.Code);
                    var existingUserCodeList = Enumerable.Empty<string>(); // 画面からではなくサーバから取得し直す
                    ServiceProxyFactory.Do<LoginUserMasterClient>(client =>
                    {
                        var result = client.GetByCode(SessionKey, CompanyId, importUserCodeList.ToArray());
                        if (result.ProcessResult.Result && result.Users != null)
                        {
                            existingUserCodeList = result.Users.Select(u => u.Code);
                        }
                    });

                    var validationTargetList = importUserCodeList.Except(existingUserCodeList); // 検証対象(＝新規登録ユーザ)

                    var addError = new Action<int, string, string>((lineNo, value, message) =>
                    {
                        reports.Add(new WorkingReport
                        {
                            LineNo = lineNo,
                            FieldNo = definition.InitialPasswordField.FieldIndex,
                            FieldName = definition.InitialPasswordField.FieldName,
                            Value = value,
                            Message = message,
                        });
                    });

                    var policy = ServiceProxyFactory.Do((PasswordPolicyMasterService.PasswordPolicyMasterClient client) =>
                    {
                        var res = client.Get(SessionKey, CompanyId);
                        if (res.ProcessResult.Result) return res.PasswordPolicy;
                        return null;
                    });

                    foreach (var lineNo in val.Keys.Where(lineNo => validationTargetList.Contains(val[lineNo].Code)))
                    {
                        var pw = val[lineNo].InitialPassword;

                        if (string.IsNullOrEmpty(pw))
                        {
                            addError(lineNo, pw, $"新規登録ユーザの初回パスワードが空白のため、インポートできません。");
                            continue;
                        }

                        var validationResult = policy.Validate(pw);
                        if (validationResult != PasswordValidateResult.Valid)
                        {
                            switch (validationResult)
                            {
                                case PasswordValidateResult.ProhibitionAlphabetChar:
                                    addError(lineNo, pw, $"アルファベットが使用されているため、インポートできません。");
                                    break;
                                case PasswordValidateResult.ProhibitionNumberChar:
                                    addError(lineNo, pw, $"数字が使用されているため、インポートできません。");
                                    break;
                                case PasswordValidateResult.ProhibitionSymbolChar:
                                    addError(lineNo, pw, $"記号が使用されているため、インポートできません。");
                                    break;
                                case PasswordValidateResult.ProhibitionNotAllowedSymbolChar:
                                    addError(lineNo, pw, $"使用できない文字が含まれているため、インポートできません。");
                                    break;
                                case PasswordValidateResult.ShortageAlphabetCharCount:
                                    addError(lineNo, pw, $"アルファベットが最低{policy.MinAlphabetUseCount}文字含まれていないため、インポートできません。");
                                    break;
                                case PasswordValidateResult.ShortageNumberCharCount:
                                    addError(lineNo, pw, $"数字が最低{policy.MinNumberUseCount}文字含まれていないため、インポートできません。");
                                    break;
                                case PasswordValidateResult.ShortageSymbolCharCount:
                                    addError(lineNo, pw, $"記号が最低{policy.MinSymbolUseCount}文字含まれていないため、インポートできません。");
                                    break;
                                case PasswordValidateResult.ShortagePasswordLength:
                                    addError(lineNo, pw, $"{policy.MinLength}文字以上でないため、インポートできません。");
                                    break;
                                case PasswordValidateResult.ExceedPasswordLength:
                                    addError(lineNo, pw, $"{policy.MaxLength}文字以下でないため、インポートできません。");
                                    break;
                                case PasswordValidateResult.ExceedSameRepeatedChar:
                                    addError(lineNo, pw, $"同じ文字が{policy.MinSameCharacterRepeat}文字以上続いているため、インポートできません。");
                                    break;
                                default:
                                    throw new NotImplementedException($"PasswordValidateResult = {validationResult.ToString()}");
                            }
                        }
                    }

                    return reports;
                };

                var importer = definition.CreateImporter(m => m.Code);
                importer.UserId = Login.UserId;
                importer.UserCode = Login.UserCode;
                importer.CompanyId = CompanyId;
                importer.CompanyCode = Login.CompanyCode;
                importer.LoadAsync = async () => await GetUserDataAsync();
                importer.RegisterAsync = async unitOfWork => await RegisterForImportAsync(unitOfWork);
                importer.PostImportHanlder = (worker, result) => PostImport(worker, result);

                var importResult = DoImport(importer, importSetting, ClearAll);

                if (!importResult) return;
                Task<List<LoginUser>> loadListTask = GetUserDataAsync();
                LoginUserList.Clear();
                loadListTask.ContinueWith(t => LoginUserList.AddRange(t.Result));
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadListTask), false, SessionKey);
                grdLoginUser.DataSource = new BindingSource(loadListTask.Result, null);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrImportErrorWithoutLog);
            }
        }

        private async Task<ImportResult> RegisterForImportAsync(UnitOfWork<LoginUser> imported)
        {
            ImportResult result = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<LoginUserMasterClient>();
                result = await service.ImportAsync(SessionKey,
                    CompanyId,
                    Login.UserId,
                    imported.New.ToArray(),
                    imported.Dirty.ToArray(),
                    imported.Removed.ToArray());
            });

            return result;
        }

        private void PostImport(ImportWorker<LoginUser> worker,
            ImportResult importResult)
        {
            var importResultLoginUsers = importResult as ImportResultLoginUser;
            if (importResultLoginUsers.LicenseIsOrver)
            {
                var messenger = XmlMessenger.GetMessageInfo(MsgWngLicenseIsMax);
                var msg = messenger.Text.Replace("\r", "").Replace("\n", "");
                worker.Reports.Add(new WorkingReport {Message = msg });
                worker.Models.Clear();
            }

            if (importResultLoginUsers.NotExistsLoginUser)
            {
                var msg = "現在ログオンしている担当者のデータがないため、インポートできません。";
                worker.Reports.Add(new WorkingReport { Message = msg });
                worker.Models.Clear();
            }

            if (importResultLoginUsers.LoginUserHasNotLoginLicense)
            {
                var msg = "現在ログインしている担当者のログオン権限がないため、インポートできません。";
                worker.Reports.Add(new WorkingReport { Message = msg });
                worker.Models.Clear();
            }
        }

        [OperationLog("エクスポート")]
        private void ExportData()
        {
            List<LoginUser> list = null;
            string serverPath = null;
            try
            {
                var loadListTask = GetUserDataAsync();
                ProgressDialog.Start(ParentForm, loadListTask, false, SessionKey);
                list = loadListTask.Result;
                if (!list.Any())
                {
                    ShowWarningDialog(MsgWngNoExportData);
                    return;
                }

                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<GeneralSettingMasterClient>();
                    GeneralSettingResult result = await service.GetByCodeAsync(SessionKey, CompanyId, "サーバパス");

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
                var fileName = $"ログインユーザーマスター{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var definition = new LoginUserFileDefinition(new DataExpression(ApplicationControl));
                definition.UseClientField.Ignored = !UseDistribution;
                definition.UseWebViewerField.Ignored = !UseDistribution;

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
                Settings.SavePath<LoginUser>(Login, filePath);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                DispStatusMessage(MsgErrExportError);
            }
        }

        [OperationLog("終了")]
        public void Exit()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose))
            {
                return;
            }
            BaseForm.Close();
        }

        #endregion

        #region その他Function
        private void PB0301_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                txtUserCode.Select();
                var tasks = new List<Task>();

                if (Company == null)
                    tasks.Add(LoadCompanyAsync());

                if (ApplicationControl == null)
                    tasks.Add(LoadApplicationControlAsync());

                tasks.Add(LoadControlColorAsync());
                tasks.Add(LoadFunctionAuthorities(MasterImport, MasterExport));
                var loadListTask = GetUserDataAsync();
                loadListTask.ContinueWith(task => LoginUserList.AddRange(task.Result));
                tasks.Add(loadListTask);
                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);

                GetFormatData();
                InitializeLoginUserGrid();
                grdLoginUser.DataSource = new BindingSource(LoginUserList, null);
                ClearAll();
                Modified = false;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void SetLoginDataEntry(LoginUser userData = null)
        {
            CurrentUserData = userData ?? new LoginUser();
            LoginUserId = CurrentUserData.Id;
            DepartmentId = CurrentUserData.DepartmentId;
            StaffId = CurrentUserData.AssignedStaffId;
            txtUserCode.Text = CurrentUserData.Code;
            txtUserName.Text = CurrentUserData.Name;
            txtDepartmentCode.Text = CurrentUserData.DepartmentCode;
            txtMenuLevel.Text = CurrentUserData.MenuLevel.ToString();
            txtFunctionLevel.Text = CurrentUserData.FunctionLevel.ToString();
            txtMailAddress.Text = CurrentUserData.Mail;
            lblDepartmentName.Text = CurrentUserData.DepartmentName;
            txtStaffCode.Text = CurrentUserData.StaffCode;
            lblStaffName.Text = CurrentUserData.StaffName;

            cbxUseClient.Checked = CurrentUserData.UseClient == 1;
            cbxUseWebviewer.Checked = CurrentUserData.UseWebViewer == 1;

        }

        //項目のデータを入れる
        private LoginUser LoginUserDataInfo()
        {
            var userInsert = new LoginUser();
            Model.CopyTo(CurrentUserData, userInsert, true);
            userInsert.Id = LoginUserId;
            userInsert.DepartmentCode = txtDepartmentCode.Text;
            userInsert.DepartmentName = lblDepartmentName.Text;
            userInsert.StaffCode = txtStaffCode.Text;
            userInsert.StaffName = lblStaffName.Text;
            userInsert.CompanyId = CompanyId;
            userInsert.Code = txtUserCode.Text;
            userInsert.Name = txtUserName.Text.Trim();
            userInsert.DepartmentId = DepartmentId;
            userInsert.Mail = txtMailAddress.Text;
            userInsert.MenuLevel = int.Parse(txtMenuLevel.Text);
            userInsert.FunctionLevel = int.Parse(txtFunctionLevel.Text);

            userInsert.UseClient = !UseDistribution ? 1 : cbxUseClient.Checked ? 1 : 0;
            userInsert.UseWebViewer = cbxUseWebviewer.Checked ? 1 : 0;

            userInsert.AssignedStaffId = StaffId;
            userInsert.InitialPassword = cbxInitializePassword.Checked ? txtInitialPassword.Text : null;
            userInsert.UpdateBy = Login.UserId;
            userInsert.CreateBy = Login.UserId;

            return userInsert;
        }

        private void grdLoginUser_CellDoubleClick(object sender, CellEventArgs e)
        {
            if (e.Scope != CellScope.Row || e.RowIndex < 0) return;

            if (Modified && !ShowConfirmDialog(MsgQstConfirmUpdateData))
            {
                return;
            }
            List<LoginUser> newlist = LoginUserList.OrderBy(x => x.Code).ToList();
            SetLoginDataEntry(newlist[e.RowIndex]);
            BaseContext.SetFunction03Enabled(true);
            txtUserCode.Enabled = false;
            ControlCbxInitializePassword();
            txtUserName.Select();
            ClearStatusMessage();
            Modified = false;
        }

        private async Task<bool> ValidateLoginData()
        {
            if (!ValidateLicenseCount()) return false;

            if (string.IsNullOrWhiteSpace(txtUserCode.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblUserCode.Text);
                txtUserCode.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                ShowWarningDialog(MsgWngInputRequired, lblUserName.Text);
                txtUserName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDepartmentCode.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblDepartmentCode.Text);
                txtDepartmentCode.Focus();
                return false;
            }

            if (UseDistribution && string.IsNullOrWhiteSpace(txtMailAddress.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblMailAddress.Text);
                txtMailAddress.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMenuLevel.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblMenuLevel.Text);
                txtMenuLevel.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFunctionLevel.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblFunctiionLevel.Text);
                txtFunctionLevel.Focus();
                return false;
            }

            if ((UseDistribution) && (!cbxUseClient.Checked && !cbxUseWebviewer.Checked))
            {
                ShowWarningDialog(MsgWngSelectVOneOrWebViewer);
                cbxUseClient.Focus();
                return false;
            }

            if (UseDistribution && !string.IsNullOrWhiteSpace(txtMailAddress.Text))
            {
                string s = "[*@*]";
                string s1 = txtMailAddress.Text.ToString();
                Match mailFlg = Regex.Match(s1, s);
                if (!mailFlg.Success)
                {
                    ShowWarningDialog(MsgWngInputInvalidLetter, "メールアドレス");
                    txtMailAddress.Focus();
                    txtMailAddress.Select();
                    return false;
                }
            }

            // パスワード
            if (!cbxInitializePassword.Checked)
            {
                return true;
            }

            if (string.IsNullOrEmpty(txtInitialPassword.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "パスワード");
                txtInitialPassword.Select();
                return false;
            }
            if (string.IsNullOrEmpty(txtInitialPasswordConfirmation.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "パスワード(確認)");
                txtInitialPasswordConfirmation.Select();
                return false;
            }
            if (txtInitialPassword.Text != txtInitialPasswordConfirmation.Text)
            {
                ShowWarningDialog(MsgWngInvalidPassword);
                txtInitialPasswordConfirmation.Select();
                return false;
            }

            var policy = await GetPasswordPolicyAsync(SessionKey, CompanyId);
            if (policy == null)
            {
                ShowWarningDialog(MsgErrSaveError);
                txtInitialPassword.Select();
                return false;
            }

            var validationResult = policy.Validate(txtInitialPassword.Text);
            if (validationResult != PasswordValidateResult.Valid)
            {
                switch (validationResult)
                {
                    case PasswordValidateResult.ProhibitionAlphabetChar:
                        ShowWarningDialog(MsgWngProhibitionAlphabetChar);
                        break;
                    case PasswordValidateResult.ProhibitionNumberChar:
                        ShowWarningDialog(MsgWngProhibitionNumberChar);
                        break;
                    case PasswordValidateResult.ProhibitionSymbolChar:
                        ShowWarningDialog(MsgWngProhibitionSymbolChar);
                        break;
                    case PasswordValidateResult.ProhibitionNotAllowedSymbolChar:
                        ShowWarningDialog(MsgWngProhibitionNotAllowedSymbolChar);
                        break;
                    case PasswordValidateResult.ShortageAlphabetCharCount:
                        ShowWarningDialog(MsgWngShortageAlphabetCharCount, policy.MinAlphabetUseCount.ToString());
                        break;
                    case PasswordValidateResult.ShortageNumberCharCount:
                        ShowWarningDialog(MsgWngShortageNumberCharCount, policy.MinNumberUseCount.ToString());
                        break;
                    case PasswordValidateResult.ShortageSymbolCharCount:
                        ShowWarningDialog(MsgWngShortageSymbolCharCount, policy.MinSymbolUseCount.ToString());
                        break;
                    case PasswordValidateResult.ShortagePasswordLength:
                        ShowWarningDialog(MsgWngShortagePasswordLength, policy.MinLength.ToString());
                        break;
                    case PasswordValidateResult.ExceedPasswordLength:
                        ShowWarningDialog(MsgWngExceedPasswordLength, policy.MaxLength.ToString());
                        break;
                    case PasswordValidateResult.ExceedSameRepeatedChar:
                        ShowWarningDialog(MsgWngExceedSameRepeatedChar, policy.MinSameCharacterRepeat.ToString());
                        break;
                    default:
                        throw new NotImplementedException($"PasswordValidateResult = {validationResult.ToString()}");
                }

                txtInitialPassword.Select();
                return false;
            }

            return true;
        }

        private void ZeroLeftPaddingWithoutValidated()
        {
            if (IsNeedValidate(ApplicationControl.LoginUserCodeType, txtUserCode.TextLength, ApplicationControl.LoginUserCodeLength))
            {
                txtUserCode.Text = ZeroLeftPadding(txtUserCode);
                txtUserCode_Validated(null, null);
            }
            if (IsNeedValidate(ApplicationControl.DepartmentCodeType, txtDepartmentCode.TextLength, ApplicationControl.DepartmentCodeLength))
            {
                txtDepartmentCode.Text = ZeroLeftPadding(txtDepartmentCode);
                txtDepartmentCode_Validated(null, null);
            }
            if (IsNeedValidate(ApplicationControl.StaffCodeType, txtStaffCode.TextLength, ApplicationControl.StaffCodeLength))
            {
                txtStaffCode.Text = ZeroLeftPadding(txtStaffCode);
                txtStaffCode_Validated(null, null);
            }
        }

        //フォーカスイベント
        private async Task DisplayLoginUserData()
        {
            UsersResult userData = null;
            LoginUser registered = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var loginUserSearch = new LoginUserSearch();
                loginUserSearch.Codes = new string[] { txtUserCode.Text };
                var service = factory.Create<LoginUserMasterClient>();
                userData = await service.GetItemsAsync(SessionKey, CompanyId, loginUserSearch);

                if (userData.ProcessResult.Result && userData.Users.Any())
                {
                    registered = userData.Users[0];
                }
            });

            if (registered != null)
            {
                SetLoginDataEntry(registered);
                txtUserCode.Enabled = false;
                ControlCbxInitializePassword();
                BaseContext.SetFunction03Enabled(true);
                ClearStatusMessage();
                Modified = false;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(txtUserCode.Text))
                {
                    DispStatusMessage(MsgInfNewData, "ログインユーザーコード");
                    txtUserCode.Enabled = true;
                    ControlCbxInitializePassword();
                }
                else
                {
                    ClearStatusMessage();
                }
            }
        }

        private void txtUserCode_Validated(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtUserCode.Text))
                {
                    var value = txtUserCode.Text;
                    ProgressDialog.Start(ParentForm, DisplayLoginUserData(), false, SessionKey);
                }
                else
                {
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        //基本設定のデータを取る
        private void GetFormatData()
        {
            var expression = new DataExpression(ApplicationControl);

            if (ApplicationControl != null)
            {
                txtMailAddress.Required = UseDistribution;
                cbxUseClient.Visible = UseDistribution;
                cbxUseWebviewer.Visible = UseDistribution;

                txtUserCode.Format = expression.LoginUserCodeFormatString;
                txtUserCode.MaxLength = expression.LoginUserCodeLength;

                txtStaffCode.Format = expression.StaffCodeFormatString;
                txtStaffCode.MaxLength = expression.StaffCodeLength;

                txtDepartmentCode.Format = expression.DepartmentCodeFormatString;
                txtDepartmentCode.MaxLength = expression.DepartmentCodeLength;

                if (expression.DepartmentCodeFormatString == "9")
                {
                    txtDepartmentCode.PaddingChar = '0';
                }

                if (expression.StaffCodeFormatString == "9")
                {
                    txtStaffCode.PaddingChar = '0';
                }

                if (expression.LoginUserCodeFormatString == "9")
                {
                    txtUserCode.PaddingChar = '0';
                }
            }
        }

        private void txtMenuLevel_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(txtMenuLevel.Text, "[^1-4]"))
            {
                txtMenuLevel.Text = "4";
            }

        }

        private void txtFunctionLevel_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(txtFunctionLevel.Text, "[^1-6]"))
            {
                txtFunctionLevel.Text = "6";
            }
        }

        private void AddHandlers()
        {
            foreach (Control control in gbxLoginUserInput.Controls)
            {
                if (control is Common.Controls.VOneTextControl)
                {
                    control.TextChanged += new EventHandler(OnContentChanged);
                }

                if (control is CheckBox)
                {
                    ((CheckBox)control).CheckedChanged += new EventHandler(OnContentChanged);
                }
            }
        }

        private void OnContentChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        /// <summary>
        /// 「パスワード初期化」チェックボックスの状態を制御する。
        /// </summary>
        /// <remarks>
        /// txtUserCode.Enabledに値をセットした後で呼び出す必要あり。
        /// txtUserCode.EnabledChangedイベントは同じ値をセットした場合に発火しないので、手動で呼び出しコードを記述する。
        /// </remarks>
        private void ControlCbxInitializePassword()
        {
            var isEditingNewUser = txtUserCode.Enabled;
            if (isEditingNewUser)
            {
                // 新規登録ユーザ編集中 → チェック＋固定
                cbxInitializePassword.Checked = true;
                cbxInitializePassword.Enabled = false;
            }
            else
            {
                // 既存ユーザ編集中 → アンチェック＋可変
                cbxInitializePassword.Checked = false;
                cbxInitializePassword.Enabled = true;
            }
        }

        /// <summary>
        /// 「パスワード初期化」チェックボックスのチェック状態により「初回パスワード」「(確認)」入力欄を制御する。
        /// </summary>
        /// <seealso cref="InitializeUserComponent"/>
        private void CbxInitializePassword_CheckedChanged(object sender, EventArgs e)
        {
            var cbx = (CheckBox)sender;

            txtInitialPassword.Enabled = cbx.Checked;
            txtInitialPasswordConfirmation.Enabled = cbx.Checked;

            // アンチェック時は入力内容をクリア
            if (!cbx.Checked)
            {
                txtInitialPassword.Clear();
                txtInitialPasswordConfirmation.Clear();
            }
        }

        private bool ValidateLicenseCount()
        {
            var licenseKeys = Util.GetLoginUserLicenses(Login);
            if (licenseKeys == null)
            {
                ShowWarningDialog(MsgErrSaveError);
                return false;
            }

            var useClientCount = LoginUserList.Count(x => x.Code != txtUserCode.Text && x.UseClient > 0);
            if (UseDistribution)
            {
                if (cbxUseClient.Checked)
                {
                    useClientCount++;
                }
                else if (txtUserCode.Text == Login.UserCode
                    && !cbxUseClient.Checked)
                {
                    ShowWarningDialog(MsgWngNoLoginAuthorization);
                    return false;
                }
            }
            else
            {
                useClientCount++;
            }

            if (useClientCount == 0)
            {
                ShowWarningDialog(MsgWngAdminIsNecessary);
                return false;
            }

            if (useClientCount > licenseKeys.Count)
            {
                ShowWarningDialog(MsgWngLicenseIsMax);
                return false;
            }
            return true;
        }

        #endregion
    }
}
