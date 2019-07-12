using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.BillingService;
using Rac.VOne.Client.Screen.DepartmentMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.SectionWithDepartmentMasterService;
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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Web.Models.FunctionType;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>請求部門マスター</summary>
    public partial class PB0201 : VOneScreenBase
    {
        #region 変数宣言

        public int? StaffID { get; set; } = 0;
        public int DepartmentId { get; set; } = 0;
        private string CellName(string value) => $"cel{value}";
        #endregion

        #region 画面の初期化

        public PB0201()
        {
            InitializeComponent();
            grdDepartment.SetupShortcutKeys();
            Text = "請求部門マスター";
            AddHandlers();
        }

        private void PB0201_Load(object sender, EventArgs e)
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

                Task<List<Department>> departmentTask = GetDepartmentListAsync();
                loadTask.Add(departmentTask);
                loadTask.Add(LoadFunctionAuthorities(MasterImport, MasterExport));

                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
                SetFormatData();
                InitializeGrid();
                grdDepartment.DataSource = new BindingSource(departmentTask.Result, null);
                Clear();
                Modified = false;

                txtDepartmentCode.Focus();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task<List<Department>> LoadListAsync()
        {
            List<Department> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<DepartmentMasterClient>();
                DepartmentsResult result = await service.GetDepartmentAndStaffAsync(SessionKey, CompanyId);

                if (result.ProcessResult.Result)
                {
                    list = result.Departments;
                }
            });

            return list ?? new List<Department>();
        }

        private void SetFormatData()
        {
            if (ApplicationControl != null)
            {
                var expression = new DataExpression(ApplicationControl);
                txtDepartmentCode.Format = expression.DepartmentCodeFormatString;
                txtDepartmentCode.MaxLength = ApplicationControl.DepartmentCodeLength;
                txtDepartmentCode.PaddingChar = expression.DepartmentCodePaddingChar;

                txtStaffCode.Format = expression.StaffCodeFormatString;
                txtStaffCode.MaxLength = ApplicationControl.StaffCodeLength;
                txtStaffCode.PaddingChar = expression.StaffCodePaddingChar;

            }
        }

        #endregion

        #region PB0201 InitializeFunctionKeys

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
            OnF04ClickHandler = DoPrint;

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

        #region 画面処理

        private void InitializeGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "Header"                                                             , cell: builder.GetRowHeaderCell(), sortable: true, readOnly: false ),
                new CellSetting(height, 130, nameof(Department.Code)     , dataField: nameof(Department.Code)     , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), caption: "請求部門コード"  ),
                new CellSetting(height, 200, nameof(Department.Name)     , dataField: nameof(Department.Name)     , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft)  , caption: "請求部門名"      ),
                new CellSetting(height, 200, nameof(Department.StaffName), dataField: nameof(Department.StaffName), cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft)  , caption: "回収責任者"      ),
                new CellSetting(height,   0, nameof(Department.StaffCode), dataField: nameof(Department.StaffCode), visible: false ),
                new CellSetting(height,   0, nameof(Department.Note)     , dataField: nameof(Department.Note)     , visible: false ),
                new CellSetting(height,   0, nameof(Department.Id)       , dataField: nameof(Department.Id)       , visible: false ),
                new CellSetting(height,   0, nameof(Department.StaffId)  , dataField: nameof(Department.StaffId)  , visible: false )
            });

            grdDepartment.Template = builder.Build();
            grdDepartment.SetRowColor(ColorContext);
            grdDepartment.HideSelection = true;

            grdDepartment.CurrentCellBorderLine = new Line(LineStyle.None, Color.Empty);
            grdDepartment.CurrentRowBorderLine = new Line(LineStyle.None, Color.Empty);
        }

        private async Task<List<Department>> GetDepartmentListAsync()
        {
            List<Department> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<DepartmentMasterClient>();
                var result = await service.GetDepartmentAndStaffAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                {
                    list = result.Departments;
                }
                Modified = false;
            });

            return list ?? new List<Department>();
        }

        private string GetStaffNameByCode(string code)
        {
            var staffName = "";

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<StaffMasterClient>();
                var result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });
                if (result.ProcessResult.Result)
                {
                    var staff = result.Staffs.FirstOrDefault();

                    if (staff != null)
                    {
                        staffName = staff.Name;
                        StaffID = staff.Id;
                    }
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return staffName;
        }
        #endregion

        #region 入力項目チェック

        private bool CheckRequired()
        {
            ClearStatusMessage();

            if (string.IsNullOrWhiteSpace(txtDepartmentCode.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblDepartmentCode.Text);
                txtDepartmentCode.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDepartmentName.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblDepartmentName.Text);
                txtDepartmentName.Focus();
                return false;
            }

            return true;
        }

        #endregion

        #region  入力項目変更イベント処理

        private void AddHandlers()
        {
            foreach (Control control in gbxDepartmentInput.Controls)
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

        #region Validated処理

        /// <summary>
        /// 部門データ設定取得
        /// </summary>
        private void DisplayDepartmentData()
        {
            ClearStatusMessage();
            if (string.IsNullOrEmpty(txtDepartmentCode.Text)) return;

            DepartmentResult result = null;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<DepartmentMasterClient>();
                result = await service.GetByCodeAndStaffAsync(SessionKey, CompanyId, txtDepartmentCode.Text);
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (result.ProcessResult.Result && result.Department != null)
            {
                var department = result.Department;

                txtDepartmentName.Text = department.Name;
                txtStaffCode.Text = department.StaffCode;
                lblStaffName.Text = department.StaffName;
                txtNote.Text = department.Note;
                DepartmentId = department.Id;
                StaffID = department.StaffId;
                txtDepartmentCode.Enabled = false;
                BaseContext.SetFunction03Enabled(true);
                ClearStatusMessage();
                Modified = false;
            }
            else
            {
                DispStatusMessage(MsgInfSaveNewData, "請求部門");
                Modified = true;
                DepartmentId = 0;
            }
        }

        private void txtStaffCode_Validated(object sender, EventArgs e)
        {
            try
            {
                var code = txtStaffCode.Text;
                var staffName = "";

                if (code != "")
                {
                    staffName = GetStaffNameByCode(code);

                    if (!string.IsNullOrEmpty(staffName))
                    {
                        lblStaffName.Text = staffName;
                        ClearStatusMessage();
                    }
                    else
                    {
                        ShowWarningDialog(MsgWngMasterNotExist, "営業担当者", txtStaffCode.Text);
                        txtStaffCode.Clear();
                        txtStaffCode.Focus();
                        lblStaffName.Clear();
                    }
                }
                else
                {
                    lblStaffName.Clear();
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtDepartmentCode_Validated(object sender, EventArgs e)
        {
            try
            {
                DisplayDepartmentData();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void ZeroLeftPaddingWithoutValidated()
        {
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

        #endregion

        #region grdDepartment_CellDoubleClick

        private void grdDepartment_CellDoubleClick(object sender, CellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (Modified && !ShowConfirmDialog(MsgQstConfirmUpdateData))
                {
                    return;
                }

                BaseContext.SetFunction03Enabled(true);
                txtDepartmentCode.Enabled = false;
                txtDepartmentCode.Text = grdDepartment.Rows[e.RowIndex].Cells[CellName(nameof(Department.Code))].DisplayText;
                txtDepartmentName.Text = grdDepartment.Rows[e.RowIndex].Cells[CellName(nameof(Department.Name))].DisplayText;
                lblStaffName.Text = grdDepartment.Rows[e.RowIndex].Cells[CellName(nameof(Department.StaffName))].DisplayText;
                txtStaffCode.Text = grdDepartment.Rows[e.RowIndex].Cells[CellName(nameof(Department.StaffCode))].DisplayText;
                txtNote.Text = grdDepartment.Rows[e.RowIndex].Cells[CellName(nameof(Department.Note))].DisplayText;
                DepartmentId = int.Parse(grdDepartment.Rows[e.RowIndex].Cells[CellName(nameof(Department.Id))].DisplayText);

                if (grdDepartment.Rows[e.RowIndex].Cells[CellName(nameof(Department.StaffId))].DisplayText != "")
                {
                    StaffID = int.Parse(grdDepartment.Rows[e.RowIndex].Cells[CellName(nameof(Department.StaffId))].DisplayText);
                }
                else
                {
                    StaffID = null;
                }

                txtDepartmentName.Focus();
                ClearStatusMessage();
                Modified = false;
            }
        }

        #endregion

        #region 担当者検索処理

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var staff = this.ShowStaffSearchDialog();
            if (staff != null)
            {
                txtStaffCode.Text = staff.Code;
                lblStaffName.Text = staff.Name;
                StaffID = staff.Id;
                ClearStatusMessage();
            }
        }
        #endregion

        #region F1 登録処理

        [OperationLog("登録")]
        private void Save()
        {
            if (!CheckRequired()) return;

            ZeroLeftPaddingWithoutValidated();

            if (!ShowConfirmDialog(MsgQstConfirmSave))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            try
            {
                ProgressDialog.Start(ParentForm, SaveDepartment(), false, SessionKey);
                Clear();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task SaveDepartment()
        {
            var depInsert = new Department();
            depInsert.CompanyId = Login.CompanyId;
            depInsert.Code = txtDepartmentCode.Text;
            depInsert.Name = txtDepartmentName.Text.Trim();

            if (txtStaffCode.Text != "")
            {
                depInsert.StaffId = StaffID;
            }
            else
            {
                depInsert.StaffId = null;
            }

            depInsert.Note = txtNote.Text.Trim();
            depInsert.UpdateBy = Login.UserId;
            depInsert.UpdateAt = DateTime.Now;
            depInsert.CreateBy = Login.UserId;
            depInsert.CreateAt = DateTime.Now;
            depInsert.Id = DepartmentId;

            DepartmentResult result = null;
            List<Department> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<DepartmentMasterClient>();
                result = await service.SaveAsync(SessionKey, depInsert);
                list = await GetDepartmentListAsync();
            });

            if (result.ProcessResult.Result)
            {
                DispStatusMessage(MsgInfSaveSuccess);
            }
            else
            {
                ShowWarningDialog(MsgErrSaveError);
            }
            grdDepartment.DataSource = new BindingSource(list, null);
            return;
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

        public void Clear()
        {
            txtDepartmentCode.Clear();
            txtDepartmentName.Clear();
            txtStaffCode.Clear();
            lblStaffName.Clear();
            txtNote.Clear();
            txtDepartmentCode.Enabled = true;
            BaseContext.SetFunction03Enabled(false);
            txtDepartmentCode.Focus();
            Modified = false;
            DepartmentId = 0;
            StaffID = 0;
            BaseContext.SetFunction05Enabled(Authorities[MasterImport]);
            BaseContext.SetFunction06Enabled(Authorities[MasterExport]);
        }
        #endregion

        #region F3 削除処理

        [OperationLog("削除")]
        private void Delete()
        {
            if (!ValidateForDelete()) return;

            if (!ShowConfirmDialog(MsgQstConfirmDelete))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            try
            {
                ProgressDialog.Start(ParentForm, DeleteDepartment(), false, SessionKey);
                Clear();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private bool ValidateForDelete()
        {
            var valid = true;
            System.Action messaging = null;
            var task = Task.Run(async () =>
            {
                await ServiceProxyFactory.LifeTime(async factory =>
                {
                    var staffClient = factory.Create<StaffMasterClient>();
                    var staffExist = await staffClient.ExistDepartmentAsync(SessionKey, DepartmentId);
                    if (staffExist.Exist)
                    {
                        messaging = () => ShowWarningDialog(MsgWngDeleteConstraint, "担当者マスター", lblDepartmentCode.Text);
                        valid = false;
                        return;
                    }

                    var billingClient = factory.Create<BillingServiceClient>();
                    var billingExist = await billingClient.ExistDepartmentAsync(SessionKey, DepartmentId);
                    if (billingExist.Exist)
                    {
                        messaging = () => ShowWarningDialog(MsgWngDeleteConstraint, "請求テーブル", lblDepartmentCode.Text);
                        valid = false;
                        return;
                    }

                    var sectionClient = factory.Create<SectionWithDepartmentMasterClient>();
                    var sectionExist = await sectionClient.ExistDepartmentAsync(SessionKey, DepartmentId);
                    if (sectionExist.Exist)
                    {
                        messaging = () => ShowWarningDialog(MsgWngDeleteConstraint, "入金・請求部門対応マスター", lblDepartmentCode.Text);
                        valid = false;
                        return;
                    }
                });
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            if (!valid)
                messaging?.Invoke();

            return valid;
        }


        private async Task DeleteDepartment()
        {
            CountResult deleteDepartment = null;
            List<Department> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var departmentService = factory.Create<DepartmentMasterClient>();
                deleteDepartment = await departmentService.DeleteAsync(SessionKey, DepartmentId);
                list = await GetDepartmentListAsync();
            });
            if (deleteDepartment.Count > 0)
            {
                grdDepartment.DataSource = new BindingSource(list, null);
                DispStatusMessage(MsgInfDeleteSuccess);
            }
            else
            {
                ShowWarningDialog(MsgErrDeleteError);
            }
        }
        #endregion

        #region F4 印刷処理

        [OperationLog("印刷")]
        private void DoPrint()
        {
            try
            {
                DepartmentsResult result = null;
                DepartmentReport departmentReport = null;
                var path = string.Empty;
                var task = Task.Run(async () =>
                {
                    path = await LoadInitialDirectoryAsync();
                    await ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<DepartmentMasterClient>();
                        result = await service.GetDepartmentAndStaffAsync(SessionKey, CompanyId);
                    });
                    if (result.ProcessResult.Result)
                    {
                        var departments = result.Departments;
                        if (departments.Any())
                        {
                            departmentReport = new DepartmentReport();
                            departmentReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                            departmentReport.Name = "請求部門マスター一覧" + DateTime.Today.ToString("yyyyMMdd");

                            departmentReport.SetData(departments);

                            departmentReport.Run(false);
                        }
                    }
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (departmentReport != null)
                {
                    ShowDialogPreview(ParentForm, departmentReport, path);
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

        #endregion

        #region　F5 インポート処理

        [OperationLog("インポート")]
        private void Import()
        {
            ClearStatusMessage();
            try
            {
                ImportSetting importSetting = null;
                var task = Util.GetMasterImportSettingAsync(Login, ImportFileType.Department);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                importSetting = task.Result;

                var definition = new DepartmentFileDefinition(new DataExpression(ApplicationControl));
                definition.StaffIdField.GetModelsByCode = val =>
                {
                    StaffsResult result = null;
                    ServiceProxyFactory.LifeTime(factory =>
                    {
                        var staffMaster = factory.Create<StaffMasterClient>();
                        result = staffMaster.GetByCode(SessionKey, CompanyId, val);
                    });

                    if (result.ProcessResult.Result)
                    {
                        return result.Staffs
                            .ToDictionary(c => c.Code);
                    }
                    return new Dictionary<string, Staff>();
                };

                //その他
                definition.DepartmentCodeField.ValidateAdditional = (val, param) =>
                {
                    var reports = new List<WorkingReport>();
                    if (((ImportMethod)param) != ImportMethod.Replace) return reports;

                    MasterDatasResult resultstaff = null;
                    MasterDatasResult resultsectionWithDepartment = null;
                    MasterDatasResult resultbilling = null;
                    ServiceProxyFactory.LifeTime(factory =>
                    {
                        var departmentMaster = factory.Create<DepartmentMasterClient>();
                        var codes = val.Values.Where(x => !string.IsNullOrEmpty(x.Code)).Select(x => x.Code).Distinct().ToArray();
                        resultstaff = departmentMaster.GetImportItemsStaff(SessionKey, CompanyId, codes);
                        resultsectionWithDepartment = departmentMaster.GetImportItemsSectionWithDepartment(SessionKey, CompanyId, codes);
                        resultbilling = departmentMaster.GetImportItemsBilling(SessionKey, CompanyId, codes);//
                    });
                    foreach (MasterData ca in resultstaff.MasterDatas.Where(p => !val.Any(a => a.Value.Code == p.Code)))
                    {
                        reports.Add(new WorkingReport
                        {
                            LineNo = null,
                            FieldNo = definition.DepartmentCodeField.FieldIndex,
                            FieldName = definition.DepartmentCodeField.FieldName,
                            Message = $"営業担当者マスターに存在する{ ca.Code }：{ ca.Name }が存在しないため、インポートできません。",
                        });
                    }
                    foreach (MasterData ca in resultsectionWithDepartment.MasterDatas.Where(p => !val.Any(a => a.Value.Code == p.Code)))
                    {
                        reports.Add(new WorkingReport
                        {
                            LineNo = null,
                            FieldNo = definition.DepartmentCodeField.FieldIndex,
                            FieldName = definition.DepartmentCodeField.FieldName,
                            Message = $"入金・請求部門名対応マスターに存在する{ca.Code}：{ca.Name}が存在しないため、インポートできません。",
                        });
                    }
                    foreach (MasterData ca in resultbilling.MasterDatas.Where(p => !val.Any(a => a.Value.Code == p.Code)))
                    {
                        reports.Add(new WorkingReport
                        {
                            LineNo = null,
                            FieldNo = definition.DepartmentCodeField.FieldIndex,
                            FieldName = definition.DepartmentCodeField.FieldName,
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

                var importResult = DoImport(importer, importSetting);

                if (!importResult) return;
                Clear();
                var departmentProgress = new List<Department>();
                departmentProgress.Clear();
                Task<List<Department>> loadTask = LoadListAsync();
                ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);
                departmentProgress.AddRange(loadTask.Result);
                grdDepartment.DataSource = new BindingSource(departmentProgress, null);

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
        private async Task<ImportResult> RegisterForImportAsync(UnitOfWork<Department> imported)
        {
            ImportResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<DepartmentMasterClient>();
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
                List<Department> list = null;

                var loadTask = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<DepartmentMasterClient>();
                    DepartmentsResult result = await service.GetDepartmentAndStaffAsync(SessionKey, CompanyId);

                    if (result.ProcessResult.Result)
                    {
                        list = result.Departments;
                    }
                });
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                if (!list.Any())
                {
                    ShowWarningDialog(MsgWngNoExportData);
                    return;
                }

                string serverPath = GetServerPath();

                if (!Directory.Exists(serverPath))
                {
                    serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                var filePath = string.Empty;
                var fileName = $"請求部門マスター{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var definition = new DepartmentFileDefinition(
                                    new DataExpression(ApplicationControl));
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

        private string GetServerPath()
        {
            var task = LoadInitialDirectoryAsync();
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            var ServerPath = task.Result;
            return ServerPath;
        }

        private async Task<string> LoadInitialDirectoryAsync()
        {
            var dir = string.Empty;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<GeneralSettingMasterClient>();
                GeneralSettingResult result = await service.GetByCodeAsync(SessionKey, CompanyId, "サーバパス");

                if (result.ProcessResult.Result)
                    dir = result.GeneralSetting?.Value;
            });
            return dir;
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
