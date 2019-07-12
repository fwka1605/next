using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.BillingService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.DepartmentMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.LoginUserMasterService;
using Rac.VOne.Client.Screen.StaffMasterService;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Web.Models.FunctionType;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>営業担当者マスター</summary>
    public partial class PB0401 : VOneScreenBase
    {

        public Func<IEnumerable<IMasterData>, bool> SavePostProcessor { get; set; }
        public Func<IEnumerable<IMasterData>, bool> DeletePostProcessor { get; set; }
        private int DeparmentId { get; set; }
        private int StaffId { get; set; }
        private List<Staff> StaffProgress { get; set; }


        public PB0401()
        {
            InitializeComponent();
            grdStaff.SetupShortcutKeys();
            StaffProgress = new List<Staff>();

            Text = "営業担当者マスター";
            AddHandlers();
        }

        private void AddHandlers()
        {
            foreach (Control control in gbxStaffInput.Controls)
            {
                if (control is Common.Controls.VOneTextControl)
                {
                    control.TextChanged += new EventHandler(OnContentChanged);
                }
            }
        }

        #region 画面の初期化
        private void InitializeStaffGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var middleCenter = MultiRowContentAlignment.MiddleCenter;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,          40, "Header1"                      , cell: builder.GetRowHeaderCell()),
                new CellSetting(height,         115, "StaffCode"                    , dataField: nameof(Staff.Code)          , caption: "営業担当者コード", cell: builder.GetTextBoxCell(middleCenter) ),
                new CellSetting(height,         170, "StaffName"                    , dataField: nameof(Staff.Name)          , caption: "営業担当者名"    , cell: builder.GetTextBoxCell() ),
                new CellSetting(height,         115, nameof(Staff.DepartmentCode)   , dataField: nameof(Staff.DepartmentCode), caption: "請求部門コード"  , cell: builder.GetTextBoxCell(middleCenter) ),
                new CellSetting(height,         170, nameof(Staff.DepartmentName)   , dataField: nameof(Staff.DepartmentName), caption: "請求部門名"      , cell: builder.GetTextBoxCell() ),
                new CellSetting(height,         130, "StaffMail"                    , dataField: nameof(Staff.Mail)          , caption: "メールアドレス"   , cell: builder.GetTextBoxCell() ),
                new CellSetting(height,         130, "StaffTel"                     , dataField: nameof(Staff.Tel)           , caption: "電話番号"        , cell: builder.GetTextBoxCell() ),
                new CellSetting(height,         130, "StaffFax"                     , dataField: nameof(Staff.Fax)           , caption: "FAX番号"         , cell: builder.GetTextBoxCell() ),
                new CellSetting(height,           0, "staffId"                      , dataField: nameof(Staff.Id)            , caption: "StaffId"        , visible: false ),
                new CellSetting(height,           0, "departmentId"                 , dataField: nameof(Staff.DepartmentId)  , caption: "DepartmentId"   , visible: false ),
                new CellSetting(height,           0, "UpdateAt"                     , dataField: nameof(Staff.UpdateAt)      , caption: "UpdateAt"       , visible: false ),
            });

            grdStaff.Template = builder.Build();
            grdStaff.AllowAutoExtend = false;
            grdStaff.HideSelection = true;
        }

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

            BaseContext.SetFunction04Caption("印刷");
            BaseContext.SetFunction04Enabled(true);
            OnF04ClickHandler = PrintStaffInfo;

            BaseContext.SetFunction05Caption("インポート");
            BaseContext.SetFunction05Enabled(true);
            OnF05ClickHandler = Import;

            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction06Enabled(true);
            OnF06ClickHandler = ExportData;

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
        #endregion

        #region FunctionKey Events
        [OperationLog("クリア")]
        private void ConfirmToClear()
        {
            ClearStatusMessage();
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear))
                return;
            Clear();
            txtStaffCode.Select();
        }

        [OperationLog("終了")]
        private void Exit()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose))
                return;

            ParentForm.Close();
        }

        [OperationLog("削除")]
        private void Delete()
        {
            ZeroLeftPaddingWithoutValidated();

            var isValid = ValidateForDelete();
            if (!isValid)
                return;
            if (!ShowConfirmDialog(MsgQstConfirmDelete))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            try
            {

                Task<bool> deleteTask = DeleteStaffInfo();
                ProgressDialog.Start(ParentForm, deleteTask, false, SessionKey);
                if (deleteTask.Result)
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

        private bool ValidateForDelete()
        {
            if (string.IsNullOrEmpty(txtStaffCode.Text))
            {
                DispStatusMessage(MsgWngInputRequired, lblStaffCode.Text);
                return false;
            }

            Task<bool> task = ValidateDeleteStaffId();
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return task.Result;
        }

        private async Task<bool> ValidateDeleteStaffId()
        {
            Action<string, string[]> errorHandler = (id, args) =>
            {
                ProgressDialog.Start(ParentForm, LoadStaffGrid(), false, SessionKey);
                ShowWarningDialog(id, args);
            };

            var loginUserValid = false;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<LoginUserMasterClient>();
                var result = await client.ExitStaffAsync(SessionKey, StaffId);
                loginUserValid = (result.Count == 0);
                if (!loginUserValid)
                {
                    errorHandler.Invoke(MsgWngDeleteConstraint, new string[] { "ログインユーザマスター", lblStaffCode.Text });
                }
            });
            if (!loginUserValid)
                return false;

            var customerMasterValid = false;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerMasterClient>();
                var result = await service.ExistStaffAsync(SessionKey, StaffId);
                customerMasterValid = !result.Exist;
                if (!customerMasterValid)
                {
                    errorHandler.Invoke(MsgWngDeleteConstraint, new string[] { "得意先マスター", lblStaffCode.Text });
                }
            });
            if (!customerMasterValid)
                return false;

            var billingDataValid = false;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BillingServiceClient>();
                var result = await service.ExistStaffAsync(SessionKey, StaffId);
                billingDataValid = !result.Exist;
                if (!billingDataValid)
                {
                    errorHandler.Invoke(MsgWngDeleteConstraint, new string[] { "請求データ", lblStaffCode.Text });
                }
            });
            if (!billingDataValid)
                return false;

            return true;
        }


        private async Task<bool> DeleteStaffInfo()
        {
            bool success = true;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<StaffMasterClient>();

                StaffsResult staffResult = null;
                CountResult deleteResult = null;
                if (DeletePostProcessor != null)
                {
                    staffResult = await service.GetAsync(SessionKey, new int[] { StaffId });
                }

                deleteResult = await service.DeleteAsync(SessionKey, StaffId);
                success = (deleteResult?.ProcessResult.Result ?? false)
                    && deleteResult?.Count > 0;

                var syncResult = true;
                if (DeletePostProcessor != null && success)
                {
                    syncResult = DeletePostProcessor.Invoke(staffResult
                        .Staffs.AsEnumerable().Select(x => x as IMasterData));
                }
                success &= syncResult;
                if (success) await LoadStaffGrid();
            });
            return success;
        }

        [OperationLog("登録")]
        private void Save()
        {
            if (!ValidateChildren()) return;

            ZeroLeftPaddingWithoutValidated();

            if (!CheckData()) return;

            if (!ShowConfirmDialog(MsgQstConfirmSave))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            try
            {

                Task<bool> task = SaveOrUpdate();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                if (task.Result)
                {
                    txtStaffCode.Select();
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

        private Task<bool> SaveOrUpdate()
        {
            if (txtStaffCode.Enabled)
            {
                return SaveStaff();
            }
            else
            {
                return UpdateStaff();
            }
        }

        private async Task<bool> SaveStaff()
        {
            bool success = true;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                Staff staff = new Staff();
                staff.CompanyId = CompanyId;
                staff.Code = txtStaffCode.Text;
                staff.Name = txtStaffName.Text.Trim();
                staff.DepartmentId = DeparmentId;
                staff.Mail = txtStaffMail.Text.Trim();
                staff.Tel = txtStaffTel.Text.Trim();
                staff.Fax = txtStaffFax.Text.Trim();
                staff.CreateBy = Login.UserId;
                staff.CreateAt = DateTime.Now;
                staff.UpdateBy = Login.UserId;
                staff.UpdateAt = DateTime.Now;

                var service = factory.Create<StaffMasterClient>();
                StaffResult saveResult = null;
                saveResult = await service.SaveAsync(SessionKey, staff);

                success = saveResult?.ProcessResult.Result ?? false;
                bool syncResult = true;
                if (SavePostProcessor != null && success)
                {
                    syncResult = SavePostProcessor.Invoke(new IMasterData[] { saveResult.Staff as IMasterData });
                }
                success &= syncResult;

                if (success) await LoadStaffGrid();
            });
            return success;
        }

        private async Task<bool> UpdateStaff()
        {
            var success = true;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                StaffResult saveResult = null;
                Staff staff = new Staff();
                staff.CompanyId = CompanyId;
                staff.Code = txtStaffCode.Text;
                staff.Name = txtStaffName.Text.Trim();
                staff.DepartmentId = DeparmentId;
                staff.Mail = txtStaffMail.Text.Trim();
                staff.Tel = txtStaffTel.Text.Trim();
                staff.Fax = txtStaffFax.Text.Trim();
                staff.UpdateBy = Login.UserId;
                staff.UpdateAt = DateTime.Now;
                staff.CreateBy = Login.UserId;
                staff.CreateAt = DateTime.Now;
                staff.Id = StaffId;
                staff.DepartmentCode = txtDepartmentCode.Text;
                staff.DepartmentName = lblDepartmentName.Text;

                var service = factory.Create<StaffMasterClient>();
                saveResult = await service.SaveAsync(SessionKey, staff);

                success = saveResult?.ProcessResult.Result ?? false;
                var syncResult = true;
                if (SavePostProcessor != null && success)
                {
                    syncResult = SavePostProcessor.Invoke(new IMasterData[] { saveResult.Staff as IMasterData });
                }
                success &= syncResult;

                if (success) await LoadStaffGrid();
            });
            return success;
        }


        [OperationLog("印刷")]
        private void PrintStaffInfo()
        {
            SaleRegisterSectionReport saleReport = null;
            string serverPath = null;
            try
            {
                ProgressDialog.Start(ParentForm, Task.Run(async () =>
                {
                    await ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var staffService = factory.Create<StaffMasterClient>();
                        StaffsResult result = await staffService.GetItemsAsync(SessionKey, new StaffSearch { CompanyId = CompanyId });

                        if (result.ProcessResult.Result && result.Staffs.Any())
                        {
                            var StaffList = result.Staffs;
                            saleReport = new SaleRegisterSectionReport();
                            saleReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                            saleReport.Name = "営業担当者マスター" + DateTime.Now.ToString("yyyyMMdd");
                            saleReport.SetData(StaffList);
                            saleReport.Run();
                        }

                        if (saleReport != null)
                        {
                            var generalService = factory.Create<GeneralSettingMasterClient>();
                            GeneralSettingResult settingResult = await generalService.GetByCodeAsync(SessionKey, CompanyId, "サーバパス");

                            if (settingResult.ProcessResult.Result)
                            {
                                serverPath = settingResult.GeneralSetting?.Value;
                                if (!Directory.Exists(serverPath))
                                {
                                    serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                                }
                            }
                        }
                    });
                }), false, SessionKey);

                if (saleReport != null)
                {
                    ShowDialogPreview(ParentForm, saleReport, serverPath);
                }
                else
                {
                    DispStatusMessage(MsgWngPrintDataNotExist);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }
        #endregion

        #region Webサービス呼び出し
        private async Task LoadStaffGrid()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<StaffMasterClient>();
                StaffsResult result = await service.GetItemsAsync(SessionKey, new StaffSearch { CompanyId = CompanyId });
                if (result.ProcessResult.Result)
                {
                    grdStaff.DataSource = new BindingSource(result.Staffs, null);
                }
                Modified = false;
            });
        }

        private async Task<List<Staff>> LoadListAsync()
        {
            List<Staff> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<StaffMasterClient>();
                StaffsResult result = await service.GetItemsAsync(SessionKey, new StaffSearch { CompanyId = CompanyId });

                if (result.ProcessResult.Result)
                {
                    list = result.Staffs;
                }
            });
            return list ?? new List<Staff>();
        }

        #endregion

        #region その他Function
        private void OnContentChanged(object sender, EventArgs e)
        {
            if (!Modified)
            {
                Modified = true;
            }
        }

        private void SetFormat()
        {
            var expression = new DataExpression(ApplicationControl);
            txtStaffCode.MaxLength = expression.StaffCodeLength;
            txtStaffCode.Format = expression.StaffCodeFormatString;
            txtDepartmentCode.MaxLength = expression.DepartmentCodeLength;
            txtDepartmentCode.Format = expression.DepartmentCodeFormatString;

            txtStaffMail.Required = UseDistribution;
            txtStaffTel.Format = expression.TelFaxFormatString;
            txtStaffFax.Format = expression.TelFaxFormatString;
        }

        private void PB0401_Load(object sender, EventArgs e)
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
                loadTask.Add(LoadFunctionAuthorities(MasterImport, MasterExport));

                if (Company == null)
                {
                    loadTask.Add(LoadCompanyAsync());
                }
                Task<List<Staff>> gridTask = LoadListAsync();
                loadTask.Add(gridTask);
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
                Modified = false;
                SetFormat();
                InitializeStaffGrid();
                grdStaff.DataSource = new BindingSource(gridTask.Result, null);
                txtStaffCode.Select();

                if (ApplicationControl != null)
                {
                    if (ApplicationControl.StaffCodeType == 0)
                    {
                        txtStaffCode.PaddingChar = '0';
                    }
                    if (ApplicationControl.DepartmentCodeType == 0)
                    {
                        txtDepartmentCode.PaddingChar = '0';
                    }
                }
                BaseContext.SetFunction05Enabled(Authorities[MasterImport]);
                BaseContext.SetFunction06Enabled(Authorities[MasterExport]);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        //CellDoubleClickEvent
        private void grdStaff_CellDoubleClick(object sender, CellEventArgs e)
        {
            if (e.Scope != CellScope.Row || e.RowIndex < 0) return;

            if (Modified && !ShowConfirmDialog(MsgQstConfirmUpdateData))
                return;

            try
            {
                ClearStatusMessage();
                txtStaffCode.Text = grdStaff.Rows[e.RowIndex].Cells[1].DisplayText;
                Task<bool> task = DisplayFocusOutDatas_Binding();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                if (task.Result)
                {
                    ClearStatusMessage();
                    txtStaffName.Focus();
                }
                else
                {
                    DispStatusMessage(MsgInfNewData, "営業担当者コード");
                    txtStaffName.Focus();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txt_StaffCode_Validated(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtStaffCode.Text))
                {
                    Task<bool> task = DisplayFocusOutDatas_Binding();
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                    if (task.Result)
                    {
                        ClearStatusMessage();
                    }
                    else
                    {
                        DispStatusMessage(MsgInfNewData, "営業担当者コード");
                        txtStaffName.Focus();
                    }
                }
                else
                {
                    ClearStatusMessage();
                    Modified = false;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txt_DepartmentCode_Validated(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtDepartmentCode.Text))
                {
                    Task<bool> task = DisplayFocusOutData_DepartmentName();
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                    if (task.Result)
                    {
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

        private async Task<bool> DisplayFocusOutDatas_Binding()
        {
            if (string.IsNullOrEmpty(txtStaffCode.Text)) return true;

            var succeeded = true;
            string staffCode = txtStaffCode.Text;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<StaffMasterClient>();
                StaffsResult stfresult = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { staffCode });
                List<Staff> staffResult = stfresult?.Staffs;

                succeeded = stfresult.ProcessResult.Result && staffResult.Any();
                if (staffResult.Any())
                {
                    txtStaffCode.Text = staffResult[0].Code;
                    txtStaffName.Text = staffResult[0].Name;
                    txtDepartmentCode.Text = staffResult[0].DepartmentCode;
                    lblDepartmentName.Text = staffResult[0].DepartmentName;
                    txtStaffMail.Text = staffResult[0].Mail;
                    txtStaffTel.Text = staffResult[0].Tel;
                    txtStaffFax.Text = staffResult[0].Fax;
                    txtStaffCode.Enabled = false;
                    BaseContext.SetFunction03Enabled(true);
                    DeparmentId = staffResult[0].DepartmentId;
                    StaffId = staffResult[0].Id;
                    Modified = false;
                }
            });

            return succeeded;
        }

        private async Task<bool> DisplayFocusOutData_DepartmentName()
        {
            var succeeded = false;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<DepartmentMasterClient>();
                DepartmentsResult result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtDepartmentCode.Text });

                if (result.ProcessResult.Result)
                {
                    if (result.Departments.Any())
                    {
                        succeeded = true;
                        Department deptData = result.Departments[0];
                        lblDepartmentName.Text = deptData.Name;
                        DeparmentId = deptData.Id;
                    }
                }
            });
            return succeeded;
        }

        private bool CheckData()
        {
            if (string.IsNullOrWhiteSpace(txtStaffCode.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblStaffCode.Text);
                txtStaffCode.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtStaffName.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblStaffName.Text);
                txtStaffName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDepartmentCode.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblDepartmentCode.Text);
                txtDepartmentCode.Focus();
                return false;
            }

            if (UseDistribution && string.IsNullOrWhiteSpace(txtStaffMail.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblStaffMail.Text);
                txtStaffMail.Focus();
                return false;
            }

            if (UseDistribution && !string.IsNullOrWhiteSpace(txtStaffMail.Text))
            {
                var mailformat = "[*@*]";
                if (!Regex.IsMatch(txtStaffMail.Text, mailformat))
                {
                    ShowWarningDialog(MsgWngInputInvalidLetter, lblStaffMail.Text);
                    txtStaffMail.Select();
                    return false;
                }
            }
            ClearStatusMessage();
            return true;
        }

        private void Clear()
        {
            txtStaffCode.Clear();
            txtStaffName.Clear();
            txtStaffMail.Clear();
            txtStaffTel.Clear();
            txtStaffFax.Clear();
            txtDepartmentCode.Clear();
            lblDepartmentName.Clear();
            txtStaffCode.Enabled = true;
            BaseContext.SetFunction03Enabled(false);
            ClearStatusMessage();
            DeparmentId = 0;
            StaffId = 0;
            txtStaffCode.Select();
            Modified = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var department = this.ShowDepartmentSearchDialog();
            if (department != null)
            {
                ClearStatusMessage();
                txtDepartmentCode.Text = department.Code;
                lblDepartmentName.Text = department.Name;
                DeparmentId = department.Id;
            }
        }

        private void ZeroLeftPaddingWithoutValidated()
        {
            if (IsNeedValidate(ApplicationControl.StaffCodeType, txtStaffCode.TextLength, ApplicationControl.StaffCodeLength))
            {
                txtStaffCode.Text = ZeroLeftPadding(txtStaffCode);
                txt_StaffCode_Validated(null, null);
            }
            if (IsNeedValidate(ApplicationControl.DepartmentCodeType, txtDepartmentCode.TextLength, ApplicationControl.DepartmentCodeLength))
            {
                txtDepartmentCode.Text = ZeroLeftPadding(txtDepartmentCode);
                txt_DepartmentCode_Validated(null, null);
            }
        }

        #endregion

        #region 画面のインポート
        [OperationLog("インポート")]
        private void Import()
        {
            ClearStatusMessage();
            try
            {
                ImportSetting importSetting = null;
                var task = Util.GetMasterImportSettingAsync(Login, ImportFileType.Staff);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                importSetting = task.Result;
                if (importSetting == null) return;

                var definition = new StaffFileDefinition(new DataExpression(ApplicationControl));

                definition.MailField.Required = UseDistribution;

                definition.DepartmentIdField.GetModelsByCode = val =>
                {
                    Dictionary<string, Department> product = null;
                    ServiceProxyFactory.LifeTime(factory =>
                    {
                        var departmentMaster = factory.Create<DepartmentMasterClient>();
                        DepartmentsResult result = departmentMaster.GetByCode(SessionKey, CompanyId, val);

                        if (result.ProcessResult.Result)
                        {
                            product = result.Departments.ToDictionary(c => c.Code);
                        }
                    });
                    return product ?? new Dictionary<string, Department>();
                };

                //その他
                definition.StaffCodeField.ValidateAdditional = (val, param) =>
                {
                    var reports = new List<WorkingReport>();
                    if (((ImportMethod)param) != ImportMethod.Replace)
                        return reports;

                    var codes = val.Values.Where(x => !string.IsNullOrEmpty(x.Code)).Select(x => x.Code).Distinct().ToArray();
                    MasterDatasResult resultlogin = null;
                    ServiceProxyFactory.LifeTime(factory =>
                    {
                        var staffMaster = factory.Create<StaffMasterClient>();
                        resultlogin = staffMaster.GetImportItemsLoginUser(SessionKey, CompanyId, codes);
                    });

                    foreach (MasterData ca in resultlogin.MasterDatas.Where(p => !val.Any(a => a.Value.Code == p.Code)))
                    {
                        reports.Add(new WorkingReport
                        {
                            LineNo = null,
                            FieldNo = definition.StaffCodeField.FieldIndex,
                            FieldName = definition.StaffCodeField.FieldName,
                            Message = $"ログインユーザーマスターに存在する{ca.Code}：{ca.Name}が存在しないため、インポートできません。",
                        });

                    }

                    MasterDatasResult resultcustomer = null;
                    MasterDatasResult resultbilling = null;
                    ServiceProxyFactory.LifeTime(factory =>
                    {
                        var staffMaster = factory.Create<StaffMasterClient>();
                        resultcustomer = staffMaster.GetImportItemsCustomer(SessionKey, CompanyId, codes);
                        resultbilling = staffMaster.GetImportItemsBilling(SessionKey, CompanyId, codes);
                    });
                    foreach (MasterData ca in resultcustomer.MasterDatas.Where(p => !val.Any(a => a.Value.Code == p.Code)))
                    {
                        reports.Add(new WorkingReport
                        {
                            LineNo = null,
                            FieldNo = definition.StaffCodeField.FieldIndex,
                            FieldName = definition.StaffCodeField.FieldName,
                            Message = $"得意先マスターに存在する{ca.Code}：{ca.Name}が存在しないため、インポートできません。",
                        });
                    }
                    foreach (MasterData ca in resultbilling.MasterDatas.Where(p => !val.Any(a => a.Value.Code == p.Code)))
                    {
                        reports.Add(new WorkingReport
                        {
                            LineNo = null,
                            FieldNo = definition.StaffCodeField.FieldIndex,
                            FieldName = definition.StaffCodeField.FieldName,
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
                importer.LoadAsync = async () => await LoadListAsync();
                importer.RegisterAsync = async unitOfWork => await RegisterForImportAsync(unitOfWork);

                var importResult = DoImport(importer, importSetting, Clear);
                if (!importResult) return;
                StaffProgress.Clear();
                Task<List<Staff>> loadTask = LoadListAsync();
                ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);
                StaffProgress.AddRange(loadTask.Result);
                grdStaff.DataSource = new BindingSource(StaffProgress, null);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrImportErrorWithoutLog);
            }
        }

        private async Task<ImportResult> RegisterForImportAsync(UnitOfWork<Staff> imported)
        {
            ImportResultStaff result = null;
            var syncResult = true;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<StaffMasterClient>();
                result = await service.ImportAsync(SessionKey, imported.New.ToArray(), imported.Dirty.ToArray(), imported.Removed.ToArray());
            });

            if (DeletePostProcessor != null && (imported.Removed?.Any() ?? false))
            {
                syncResult = DeletePostProcessor.Invoke(imported.Removed.Select(x => x as IMasterData));
            }

            if (syncResult && SavePostProcessor != null && (result.Staffs?.Any() ?? false))
            {
                syncResult = SavePostProcessor.Invoke(result.Staffs.Select(x => x as IMasterData));
            }

            return result;
        }
        #endregion

        #region 画面のエクスポート
        [OperationLog("エクスポート")]
        private void ExportData()
        {
            string serverPath = null;
            List<Staff> list = null;
            try
            {
                Task<List<Staff>> loadStaff = LoadListAsync();
                ProgressDialog.Start(ParentForm, Task.WhenAll(
                    loadStaff,
                    ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<GeneralSettingMasterClient>();
                        GeneralSettingResult result = await service.GetByCodeAsync(SessionKey, CompanyId, "サーバパス");

                        if (result.ProcessResult.Result)
                        {
                            serverPath = result.GeneralSetting?.Value;
                        }
                    })), false, SessionKey);

                list = loadStaff.Result;

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
                var fileName = $"営業担当者マスター{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var definition = new StaffFileDefinition(new DataExpression(ApplicationControl));
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
    }
}
