using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.Controls;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.DepartmentMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.SectionMasterService;
using Rac.VOne.Client.Screen.SectionWithDepartmentMasterService;
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
    /// <summary>入金・請求部門対応マスター</summary>
    public partial class PB1201 : VOneScreenBase
    {
        private List<SectionWithDepartment> OriginSectionWithDepList { get; set; }
        private List<SectionWithDepartment> ModifySectionWithDepList { get; set; }
        private bool CloseFlg { get; set; }
        private string CellName(string value) => $"cel{value}";
        private int? SectionId { get; set; }
        private int? DepartmentFromId;
        private int? DepartmentToId;

        public PB1201()
        {
            InitializeComponent();
            grdDepartmentModify.SetupShortcutKeys();
            grdDepartmentOrigin.SetupShortcutKeys();
            Text = "入金・請求部門対応マスター";
        }

        #region 画面のInitialize

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Save;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = ConfirmToClear;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);

            BaseContext.SetFunction04Caption("印刷");
            BaseContext.SetFunction04Enabled(true);
            OnF04ClickHandler = DoPrint;

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
        #endregion

        #region フォームロード
        private void PB1201_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                var loadTask = new List<Task>();

                if (ApplicationControl == null)
                {
                    loadTask.Add(LoadApplicationControlAsync());
                }

                if (Company == null)
                {
                    Task loadCompany = LoadCompanyAsync();
                    loadTask.Add(loadCompany);
                }

                loadTask.Add(LoadControlColorAsync());
                loadTask.Add(LoadFunctionAuthorities(MasterImport, MasterExport));
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                InitializeOrginGrid();
                InitializeDepartmentModifyGrid();

                if (ApplicationControl != null)
                {
                    if (ApplicationControl.SectionCodeType == 0)
                    {
                        txtSection.PaddingChar = '0';
                    }

                    if (ApplicationControl.DepartmentCodeType == 0)
                    {
                        txtDepartmentFrom.PaddingChar = '0';
                        txtDepartmentTo.PaddingChar = '0';
                    }
                }
                SetFormat();
                Clear();
                AddHandlers();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void SetFormat()
        {
            var expression = new DataExpression(ApplicationControl);

            txtSection.MaxLength = expression.SectionCodeLength;
            txtSection.Format = expression.SectionCodeFormatString;
            txtDepartmentFrom.MaxLength = expression.DepartmentCodeLength;
            txtDepartmentFrom.Format = expression.DepartmentCodeFormatString;
            txtDepartmentTo.MaxLength = expression.DepartmentCodeLength;
            txtDepartmentTo.Format = expression.DepartmentCodeFormatString;
        }

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
        #endregion

        #region Webサービス呼び出し
        private async Task<List<SectionWithDepartment>> GetSectionWithDepartmentData()
        {
            List<SectionWithDepartment> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<SectionWithDepartmentMasterClient>();
                var sectionWithDepartmentSearch = new SectionWithDepartmentSearch();
                var result = await service.GetItemsAsync(SessionKey, CompanyId, sectionWithDepartmentSearch);

                if (result.ProcessResult.Result)
                {
                    list = result.SectionDepartments;
                }
            });

            return list ?? new List<SectionWithDepartment>();
        }
        #endregion

        #region InitializeGrid
        private void InitializeOrginGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "Header"                                                                                             , cell: builder.GetRowHeaderCell()),
                new CellSetting(height, 115, nameof(SectionWithDepartment.DepartmentCode), dataField: nameof(SectionWithDepartment.DepartmentCode), cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), caption: "請求部門コード"),
                new CellSetting(height, 150, nameof(SectionWithDepartment.DepartmentName), dataField: nameof(SectionWithDepartment.DepartmentName), cell: builder.GetTextBoxCell()                                     , caption: "請求部門名"    ),
                new CellSetting(height,   0, nameof(SectionWithDepartment.DepartmentId)  , dataField: nameof(SectionWithDepartment.DepartmentId)  , visible: true )
            });
            grdDepartmentOrigin.Template = builder.Build();
            grdDepartmentOrigin.SetRowColor(ColorContext);
            grdDepartmentOrigin.HideSelection = true;
        }

        private void InitializeDepartmentModifyGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "Header"                                                                                             , cell: builder.GetRowHeaderCell()),
                new CellSetting(height, 115, nameof(SectionWithDepartment.DepartmentCode), dataField: nameof(SectionWithDepartment.DepartmentCode), cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), caption: "請求部門コード" ),
                new CellSetting(height, 150, nameof(SectionWithDepartment.DepartmentName), dataField: nameof(SectionWithDepartment.DepartmentName), cell: builder.GetTextBoxCell()                                     , caption: "請求部門名"     ),
                new CellSetting(height,   0, nameof(SectionWithDepartment.DepartmentId)  , dataField: nameof(SectionWithDepartment.DepartmentId)  , visible: true )
            });
            grdDepartmentModify.Template = builder.Build();
            grdDepartmentModify.SetRowColor(ColorContext);
            grdDepartmentModify.HideSelection = true;
        }
        #endregion

        #region 検索ダイアログ Click Event
        private void btnSectionSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var section = this.ShowSectionSearchDialog();
                ProgressDialog.Start(ParentForm, SetSectionData(section), false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnDepartmentFrom_Click(object sender, EventArgs e)
        {
            var departmentId = ModifySectionWithDepList.Select(d => d.DepartmentId).ToArray();
            var department = this.ShowDepartmentSearchDialog(loader: new DepartmentGridLoader(ApplicationContext)
            {
                Key = DepartmentGridLoader.SearchDepartment.WithSection,
                SectionId = SectionId.Value,
                DepartmentId = departmentId
            });

            if (department != null)
            {
                SetDepartmentData(department, txtDepartmentFrom, lblDepartmentFromName, ref DepartmentFromId);
                ClearStatusMessage();
            }
        }

        private void btnDepartmentTo_Click(object sender, EventArgs e)
        {
            var departmentId = ModifySectionWithDepList.Select(d => d.DepartmentId).ToArray();
            var department = this.ShowDepartmentSearchDialog(loader: new DepartmentGridLoader(ApplicationContext)
            {
                Key = DepartmentGridLoader.SearchDepartment.WithSection,
                SectionId = SectionId.Value,
                DepartmentId = departmentId
            });


            if (department != null)
            {
                SetDepartmentData(department, txtDepartmentTo, lblDepartmentToName, ref DepartmentToId);
                ClearStatusMessage();
            }
        }
        #endregion

        #region  Webサービス呼び出し
        /// <summary> SectionIdでSectionWithDepartmetからデータをを取得</summary>
        /// <returns></returns>
        private async Task<List<SectionWithDepartment>> LoadGridListAsync()
        {
            List<SectionWithDepartment> list = null;

            if (SectionId != null)
            {
                SectionWithDepartmentsResult result = null;
                await ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<SectionWithDepartmentMasterClient>();
                    result = await service.GetBySectionAsync(SessionKey, CompanyId, SectionId.Value);

                    if (result.ProcessResult.Result)
                    {
                        list = result.SectionDepartments;
                    }
                });

                list = list.OrderBy(x => x.DepartmentCode).ToList();
            }
            return list ?? new List<SectionWithDepartment>();
        }
        #endregion

        #region Validated Event
        private void txtSection_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                if (txtSection.Text != "")
                {
                    SectionsResult result = null;
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<SectionMasterClient>();
                        result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtSection.Text });
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);

                    if (result.ProcessResult.Result)
                    {
                        var section = result.Sections.FirstOrDefault();
                        if (section != null)
                        {
                            ProgressDialog.Start(ParentForm, SetSectionData(section), false, SessionKey);
                        }
                        else
                        {
                            ShowWarningDialog(MsgWngMasterNotExist, "入金部門", txtSection.Text);
                            txtSection.Clear();
                            lblPaymentName.Clear();
                            txtSection.Select();
                            SectionId = null;
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

        private void txtDepartmentFrom_Validated(object sender, EventArgs e)
        {
            try
            {
                if (txtDepartmentFrom.Text != "")
                {
                    GetDataFromValidate(txtDepartmentFrom, lblDepartmentFromName, ref DepartmentFromId);
                }
                else
                {
                    SetDepartmentData(null, txtDepartmentFrom, lblDepartmentFromName, ref DepartmentFromId);
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtDepartmentTo_Validated(object sender, EventArgs e)
        {
            try
            {
                if (txtDepartmentTo.Text != "")
                {
                    GetDataFromValidate(txtDepartmentTo, lblDepartmentToName, ref DepartmentToId);
                }
                else
                {
                    SetDepartmentData(null, txtDepartmentTo, lblDepartmentToName, ref DepartmentToId);
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region 画面の追加ボタン、一括削除ボタン、削除ボタン、
        #region btnAdd_Click
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.ButtonClicked(btnAdd);
                ClearStatusMessage();
                if (!CheckData())
                    return;
                if (string.IsNullOrWhiteSpace(txtDepartmentFrom.Text) && string.IsNullOrWhiteSpace(txtDepartmentTo.Text))
                {
                    ShowWarningDialog(MsgWngInputRequired, "請求部門コード");
                    txtDepartmentFrom.Focus();
                    return;
                }
                else
                {
                    var task = AddSectionWithDepartment();
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                    if (!task.Result)
                    {
                        ShowWarningDialog(MsgWngNoData, "追加");
                    }
                }
                grdDepartmentModify.DataSource = new BindingSource(ModifySectionWithDepList, null);
                ClearInput();
                Modified = true;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary> From/To　Rangeにある請求部門データをSectionWithDepartmetListに追加 </summary>
        private async Task<bool> AddSectionWithDepartment()
        {
            var departmentListToCheck = await GetDepartmentDataForCheckAsync();

            var codeFrom = string.IsNullOrWhiteSpace(txtDepartmentFrom.Text) ? txtDepartmentTo.Text : txtDepartmentFrom.Text;
            var codeTo = string.IsNullOrWhiteSpace(txtDepartmentTo.Text) ? txtDepartmentFrom.Text : txtDepartmentTo.Text;

            Func<Department, bool> range = d => (string.Compare(codeFrom, d.Code) <= 0 && string.Compare(d.Code, codeTo) <= 0);

            var newDepartments = (departmentListToCheck).Where(range)
                .Where(d => !ModifySectionWithDepList.Any(swd => swd.DepartmentCode == d.Code));
            if (!newDepartments.Any())
            {
                return false;
            }

            ModifySectionWithDepList.AddRange(newDepartments.Select(d => new SectionWithDepartment
            {
                DepartmentId = d.Id,
                DepartmentCode = d.Code,
                DepartmentName = d.Name
            }));
            ModifySectionWithDepList = ModifySectionWithDepList.OrderBy(d => d.DepartmentCode).ToList();
            return true;
        }

        private bool CheckData()
        {
            if (!string.IsNullOrWhiteSpace(txtDepartmentFrom.Text) && !string.IsNullOrWhiteSpace(txtDepartmentTo.Text)
                && !txtDepartmentFrom.ValidateRange(txtDepartmentTo, () => ShowWarningDialog(MsgWngInputRangeChecked, "請求部門コード")))
            {
                txtDepartmentFrom.Focus();
                return false;
            }
            return true;
        }
        #endregion

        #region btnDelete_Click
        /// <summary> From/To　RangeにあるデータをGridから削除 </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                this.ButtonClicked(btnDelete);
                if (string.IsNullOrWhiteSpace(txtDepartmentFrom.Text) && string.IsNullOrWhiteSpace(txtDepartmentTo.Text))
                {
                    ShowWarningDialog(MsgWngInputRequired, "請求部門コード");
                    txtDepartmentFrom.Focus();
                    return;
                }
                if (!string.IsNullOrWhiteSpace(txtDepartmentFrom.Text) && !string.IsNullOrWhiteSpace(txtDepartmentTo.Text)
                    && !txtDepartmentFrom.ValidateRange(txtDepartmentTo, () => ShowWarningDialog(MsgWngInputRangeChecked, "請求部門コード")))
                {
                    txtDepartmentFrom.Focus();
                    return;
                }

                var codeFrom = string.IsNullOrWhiteSpace(txtDepartmentFrom.Text) ? txtDepartmentTo.Text : txtDepartmentFrom.Text;
                var codeTo = string.IsNullOrWhiteSpace(txtDepartmentTo.Text) ? txtDepartmentFrom.Text : txtDepartmentTo.Text;

                Func<SectionWithDepartment, bool> range = d => (string.Compare(codeFrom, d.DepartmentCode) <= 0 && string.Compare(d.DepartmentCode, codeTo) <= 0);

                if (!ModifySectionWithDepList.Any(range))
                {
                    ShowWarningDialog(MsgWngNoData, "削除");
                    ClearInput();
                    return;
                }

                ModifySectionWithDepList = ModifySectionWithDepList
                    .Except(ModifySectionWithDepList.Where(range))
                    .OrderBy(d => d.DepartmentCode).ToList();
                grdDepartmentModify.DataSource = new BindingSource(ModifySectionWithDepList, null);

                Task<List<SectionWithDepartment>> task = LoadGridListAsync();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                OriginSectionWithDepList = task.Result;
                grdDepartmentOrigin.DataSource = new BindingSource(OriginSectionWithDepList, null);

                Modified = true;
                ClearInput();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region btnDeleteAll_Click
        /// <summary> Gridにある全データを削除 </summary>
        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            try
            {
                this.ButtonClicked(btnDeleteAll);
                ClearStatusMessage();

                if (!ModifySectionWithDepList.Any())
                {
                    ShowWarningDialog(MsgWngNoData, "削除");
                    return;
                }

                if (ShowConfirmDialog(MsgQstConfirmDeleteAll))
                {
                    ModifySectionWithDepList.Clear();
                    grdDepartmentModify.DataSource = new BindingSource(ModifySectionWithDepList, null);

                    Task<List<SectionWithDepartment>> task = LoadGridListAsync();
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                    OriginSectionWithDepList = task.Result;
                    grdDepartmentOrigin.DataSource = new BindingSource(OriginSectionWithDepList, null);

                    Modified = true;
                    ClearInput();
                }

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion
        #endregion

        #region 登録処理
        [OperationLog("登録")]
        private void Save()
        {
            try
            {
                ZeroLeftPaddingWithoutValidated();

                if (!CheckForConfirmMsg() || !CheckData())
                    return;
                SaveSectionWithDepartment();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void ZeroLeftPaddingWithoutValidated()
        {
            if (IsNeedValidate(ApplicationControl.SectionCodeType, txtSection.TextLength, ApplicationControl.SectionCodeLength))
            {
                txtSection.Text = ZeroLeftPadding(txtSection);
                txtSection_Validated(null, null);
            }
            if (IsNeedValidate(ApplicationControl.DepartmentCodeType, txtDepartmentFrom.TextLength, ApplicationControl.DepartmentCodeLength))
            {
                txtDepartmentFrom.Text = ZeroLeftPadding(txtDepartmentFrom);
                txtDepartmentFrom_Validated(null, null);
            }
            if (IsNeedValidate(ApplicationControl.DepartmentCodeType, txtDepartmentTo.TextLength, ApplicationControl.DepartmentCodeLength))
            {
                txtDepartmentTo.Text = ZeroLeftPadding(txtDepartmentTo);
                txtDepartmentTo_Validated(null, null);
            }
        }

        private bool CheckForConfirmMsg()
        {
            ClearStatusMessage();
            if (txtSection.Text == "" && txtSection.Enabled)
            {
                ShowWarningDialog(MsgWngInputRequired, "入金部門コード");
                txtSection.Focus();
                return false;
            }

            var id = "";
            var args = new string[] { };
            if (string.IsNullOrWhiteSpace(txtDepartmentFrom.Text) && string.IsNullOrWhiteSpace(txtDepartmentTo.Text))
            {
                id = MsgQstConfirmSave;
            }
            else
            {
                id = MsgQstConfirmUpdateSelectData; args = new string[] { "請求部門" };
            }

            if (!ShowConfirmDialog(id, args))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return false;
            }
            return true;
        }

        private List<SectionWithDepartment> GetPrepareSaveData()
        {
            var deleteList = new List<SectionWithDepartment>();
            if (ModifySectionWithDepList.Any())
            {
                for (int i = 0; i < ModifySectionWithDepList.Count; i++)
                {
                    ModifySectionWithDepList[i].SectionId = SectionId.Value;
                    ModifySectionWithDepList[i].UpdateBy = Login.UserId;
                    ModifySectionWithDepList[i].UpdateAt = DateTime.Today.ToLocalTime();
                    ModifySectionWithDepList[i].CreateBy = Login.UserId;
                    ModifySectionWithDepList[i].CreateAt = DateTime.Today.ToLocalTime();
                }
            }
            for (int i = 0; i < OriginSectionWithDepList.Count; i++)
            {
                var isExists = ModifySectionWithDepList.Exists(x => x.DepartmentCode == OriginSectionWithDepList[i].DepartmentCode);
                if (!isExists)
                {
                    deleteList.Add(OriginSectionWithDepList[i]);
                }
            }
            return deleteList;
        }

        private void SaveSectionWithDepartment()
        {
            CloseFlg = true;

            if (!string.IsNullOrWhiteSpace(txtDepartmentFrom.Text)
               || !string.IsNullOrWhiteSpace(txtDepartmentTo.Text))
            {
                Task<bool> containsNew = AddSectionWithDepartment();
                ProgressDialog.Start(ParentForm, containsNew, false, SessionKey);
                if (!containsNew.Result)
                {
                    ShowWarningDialog(MsgWngNoData, "追加");
                    ClearInput();
                    CloseFlg = false;
                    return;
                }
            }

            bool success = false;
            List<SectionWithDepartment> list = null;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var deleteList = GetPrepareSaveData();
                var service = factory.Create<SectionWithDepartmentMasterClient>();
                SectionWithDepartmentResult result = await service.SaveAsync(SessionKey, ModifySectionWithDepList.ToArray(), deleteList.ToArray());

                success = result?.ProcessResult.Result ?? false;

                if (success)
                {
                    list = await LoadGridListAsync();
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (success)
            {
                DispStatusMessage(MsgInfSaveSuccess);
                ModifySectionWithDepList.Clear();

                OriginSectionWithDepList = list;
                ModifySectionWithDepList = OriginSectionWithDepList;
                grdDepartmentModify.DataSource = new BindingSource(OriginSectionWithDepList, null);
                grdDepartmentOrigin.DataSource = new BindingSource(ModifySectionWithDepList, null);
                txtDepartmentFrom.Focus();
                ClearInput();
                Modified = false;
            }
            else
            {
                ShowWarningDialog(MsgErrSaveError);
            }
        }
        #endregion

        #region 印刷指示処理
        [OperationLog("印刷")]
        private void DoPrint()
        {
            try
            {
                ClearStatusMessage();
                List<SectionWithDepartment> printList = null;
                SectionWithDepartmentReport secReport = null;
                string serverPath = null;
                ProgressDialog.Start(ParentForm, Task.Run(async () =>
                {
                    serverPath = await GetServerPath();
                    printList = await GetSectionWithDepartmentData();
                    printList = printList.OrderBy(x => x.SectionCode).ThenBy(x => x.DepartmentCode).ToList();

                    if (printList.Any())
                    {
                        secReport = new SectionWithDepartmentReport();
                        secReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                        secReport.Name = "入金・請求部門対応マスター一覧" + DateTime.Now.ToString("yyyyMMdd");
                        secReport.SetData(printList);

                        secReport.Run(false);
                    }
                }), false, SessionKey);

                if (!printList.Any())
                {
                    ShowWarningDialog(MsgWngPrintDataNotExist);
                }
                else
                {
                    ShowDialogPreview(ParentForm, secReport, serverPath);
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

        #region インポート処理
        [OperationLog("インポート")]
        private void Import()
        {
            ClearStatusMessage();
            try
            {
                ImportSetting importSetting = null;
                var task = Util.GetMasterImportSettingAsync(Login, ImportFileType.SectionWithDepartment);
                // 取込設定取得
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                importSetting = task.Result;

                var definition = new SectionWithDepartmentFileDefinition(new DataExpression(ApplicationControl));

                definition.SectionCodeField.GetModelsByCode = val =>
                {
                    Dictionary<string, Section> product = null;
                    ServiceProxyFactory.LifeTime(factory =>
                    {
                        var section = factory.Create<SectionMasterClient>();
                        var result = section.GetByCode(SessionKey, CompanyId, val);

                        if (result.ProcessResult.Result)
                        {
                            product = result.Sections
                                .ToDictionary(c => c.Code);
                        }
                    });
                    return product ?? new Dictionary<string, Section>();
                };

                definition.DepartmentCodeField.GetModelsByCode = val =>
                {
                    Dictionary<string, Department> product = null;
                    ServiceProxyFactory.LifeTime(factory =>
                    {
                        var department = factory.Create<DepartmentMasterClient>();
                        var result = department.GetByCode(SessionKey, CompanyId, val);

                        if (result.ProcessResult.Result)
                        {
                            product = result.Departments.ToDictionary(d => d.Code);
                        }
                    });
                    return product ?? new Dictionary<string, Department>();

                };

                definition.DepartmentCodeField.ValidateAdditional = (val, param) =>
                {
                    var reports = new List<WorkingReport>();

                    Func<SortedList<int, SectionWithDepartment[]>, int,
                        Func<SectionWithDepartment[], bool>, bool> dupeCheck
                            = (list, key, condition) =>
                            {
                                if (!list.ContainsKey(key)) return false;
                                return condition?.Invoke(list[key]) ?? false;
                            };

                    List<SectionWithDepartment> dbItems = null;
                    ServiceProxyFactory.LifeTime(factory =>
                    {
                        var sectionWithDepartment = factory.Create<SectionWithDepartmentMasterClient>();
                        var sectionWithDepartmentSearch = new SectionWithDepartmentSearch();
                        dbItems = sectionWithDepartment
                                .GetItems(SessionKey, CompanyId, sectionWithDepartmentSearch)
                                .SectionDepartments;
                    });
                    var dbDepartmentCheckList = new SortedList<int, SectionWithDepartment[]>(dbItems.GroupBy(x => x.DepartmentId)
                            .ToDictionary(x => x.Key, x => x.ToArray()));
                    var csvDepartmentCheckList = new SortedList<int, SectionWithDepartment[]>(val.Select(x => x.Value)
                            .GroupBy(x => x.DepartmentId)
                            .ToDictionary(x => x.Key, x => x.ToArray()));

                    foreach (var pair in val)
                    {
                        if (dupeCheck(dbDepartmentCheckList,
                                pair.Value.DepartmentId,
                                g => g.Any(x => x.SectionId != pair.Value.SectionId)))
                        {
                            reports.Add(new WorkingReport(pair.Key,
                                    definition.DepartmentCodeField.FieldIndex,
                                    definition.DepartmentCodeField.FieldName,
                                    "他の入金部門に登録されているため、インポートできません。"
                            ));
                        }

                        if (dupeCheck(csvDepartmentCheckList,
                                pair.Value.DepartmentId,
                                g => g.Any(x => x.SectionId != pair.Value.SectionId && pair.Value.DepartmentId != 0)))
                        {
                            reports.Add(new WorkingReport(pair.Key,
                                definition.DepartmentCodeField.FieldIndex,
                                definition.DepartmentCodeField.FieldName,
                                "他の入金部門に登録されているため、インポートできません。"
                            ));
                        }
                    }
                    return reports;
                };

                var importer = definition.CreateImporter(m => new { m.SectionId, m.DepartmentId });
                importer.UserId = Login.UserId;
                importer.UserCode = Login.UserCode;
                importer.CompanyId = CompanyId;
                importer.CompanyCode = Login.CompanyCode;
                importer.LoadAsync = async () => await GetSectionWithDepartmentData();
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

        private async Task<ImportResult> RegisterForImportAsync(UnitOfWork<Web.Models.SectionWithDepartment> imported)
        {
            ImportResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<SectionWithDepartmentMasterClient>();
                result = await service.ImportAsync(SessionKey,
                        imported.New.ToArray(), imported.Dirty.ToArray(), imported.Removed.ToArray());
            });

            return result;
        }
        #endregion

        #region クリア処理
        [OperationLog("クリア")]
        private void ConfirmToClear()
        {
            ClearStatusMessage();

            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear))
                return;
            Clear();
        }

        private void Clear()
        {
            txtSection.Clear();
            txtDepartmentFrom.Clear();
            txtDepartmentTo.Clear();
            lblPaymentName.Clear();
            lblDepartmentFromName.Clear();
            lblDepartmentToName.Clear();
            if (grdDepartmentModify.Rows.Count != 0)
                grdDepartmentModify.Rows.Clear();
            if (grdDepartmentOrigin.Rows.Count != 0)
                grdDepartmentOrigin.Rows.Clear();
            txtSection.Enabled = true;
            btnSectionSearch.Enabled = true;
            txtDepartmentFrom.Enabled = false;
            txtDepartmentTo.Enabled = false;
            btnDepartmentFrom.Enabled = false;
            btnDepartmentTo.Enabled = false;
            btnAdd.Enabled = false;
            btnDelete.Enabled = false;
            btnDeleteAll.Enabled = false;
            BaseContext.SetFunction05Enabled(Authorities[MasterImport]);
            BaseContext.SetFunction06Enabled(Authorities[MasterExport]);
            ClearStatusMessage();
            txtSection.Focus();
            Modified = false;
        }
        #endregion

        #region エクスポート処理
        [OperationLog("エクスポート")]
        private void Export()
        {
            try
            {
                ClearStatusMessage();
                var loadTask = GetSectionWithDepartmentData();
                var pathTask = GetServerPath();
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask, pathTask), false, SessionKey);

                var list = loadTask.Result;
                list = list.OrderBy(x => x.SectionCode).ThenBy(x => x.DepartmentCode).ToList();

                if (!list.Any())
                {
                    ShowWarningDialog(MsgWngNoExportData, "エクスポート");
                    return;
                }

                string serverPath = pathTask.Result;
                if (!Directory.Exists(serverPath))
                {
                    serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                var filePath = string.Empty;
                string fileName = $"入金・請求部門対応マスター{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var definition = new SectionWithDepartmentFileDefinition(
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
                Settings.SavePath<SectionWithDepartment>(Login, filePath);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                DispStatusMessage(MsgErrExportError);
            }
        }

        /// <summary>管理マスターで設定したサーバパスを取得</summary>
        /// <returns>サーバパス</returns>
        private async Task<string> GetServerPath()
        {
            string serverPath = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var generalService = factory.Create<GeneralSettingMasterClient>();
                GeneralSettingResult result = await generalService.GetByCodeAsync(SessionKey, CompanyId, "サーバパス");

                if (result.ProcessResult.Result)
                {
                    serverPath = result.GeneralSetting?.Value;
                }
            });

            return serverPath;
        }
        #endregion
        #region 終了処理
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
                        if (!CheckData()) return;

                        SaveSectionWithDepartment();
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

        #region その他Function
        /// <summary> 請求部門コントロールにあるコードでデータを取得</summary>
        /// <param name="code">請求部門コントロールにあるコード</param>
        /// <param name="name">請求部門コントロールにあるコードの名前</param>
        /// <param name="id">請求部門コントロールにあるコードのId</param>
        private void GetDataFromValidate(VOneTextControl code, VOneDispLabelControl name, ref int? id)
        {
            var depResult = new List<Department>();
            SectionWithDepartment sectionWithDepValue = null;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<DepartmentMasterClient>();
                var result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { code.Text });

                if (result.ProcessResult.Result && result.Departments.Any())
                {
                    depResult = result.Departments;
                    sectionWithDepValue = await GetDepartmentIdInSWD(depResult[0].Id);
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (depResult.Any())
            {
                ///項目にあるコードはGridにあるかどうかをチェックする
                var isExists = OriginSectionWithDepList.Exists(
                        x => x.DepartmentId == sectionWithDepValue?.DepartmentId);
                if (sectionWithDepValue != null && isExists == false)
                {
                    ShowWarningDialog(MsgWngAlreadyRegSection, code.Text);
                    SetDepartmentData(null, code, name, ref id);
                    code.Focus();
                    return;
                }
                SetDepartmentData(depResult[0], code, name, ref id);
                ClearStatusMessage();
            }
            else
            {
                ShowWarningDialog(MsgWngMasterNotExist, "請求部門", code.Text);
                SetDepartmentData(null, code, name, ref id);
                code.Focus();
                return;
            }
        }

        /// <summary> DBから取得したデータを入力項目に設定 </summary>
        /// <param name="section"> DBから取得したデータのModel</param>
        private async Task SetSectionData(Section section)
        {
            if (section != null)
            {
                txtSection.Text = section.Code;
                lblPaymentName.Text = section.Name;
                SectionId = section.Id;

                ClearStatusMessage();
                SetSectionDisable();
                if (txtSection.Text != "" || txtSection.Enabled == false)
                {
                    OriginSectionWithDepList = await LoadGridListAsync();
                    ModifySectionWithDepList = new List<SectionWithDepartment>(OriginSectionWithDepList);
                    grdDepartmentModify.DataSource = new BindingSource(OriginSectionWithDepList, null);
                    grdDepartmentOrigin.DataSource = new BindingSource(ModifySectionWithDepList, null);
                    if (!OriginSectionWithDepList.Any())
                    {
                        DispStatusMessage(MsgInfSaveNewData, "入金部門");
                    }
                    txtDepartmentFrom.Select();
                    btnAdd.Enabled = true;
                    btnDelete.Enabled = true;
                    btnDeleteAll.Enabled = true;
                }
            }
        }

        /// <summary> 取得したデータをコントロールに設定 </summary>
        private void SetDepartmentData(Department department, VOneTextControl code, VOneDispLabelControl name, ref int? id)
        {
            if (department == null)
            {
                code.Text = string.Empty;
                name.Text = string.Empty;
                id = 0;
                return;
            }

            code.Text = department.Code;
            name.Text = department.Name;
            id = department.Id;
        }

        private void grdDepartmentModify_CellDoubleClick(object sender, CellEventArgs e)
        {
            ClearStatusMessage();
            if (e.RowIndex >= 0)
            {
                if (txtDepartmentFrom.Text == "")
                {
                    txtDepartmentFrom.Text = grdDepartmentModify.Rows[e.RowIndex].Cells[CellName(nameof(SectionWithDepartment.DepartmentCode))].DisplayText;
                    lblDepartmentFromName.Text = grdDepartmentModify.Rows[e.RowIndex].Cells[CellName(nameof(SectionWithDepartment.DepartmentName))].DisplayText;
                    DepartmentFromId = int.Parse(grdDepartmentModify.Rows[e.RowIndex].Cells[CellName(nameof(SectionWithDepartment.DepartmentId))].DisplayText);
                }
                else
                {
                    txtDepartmentTo.Text = grdDepartmentModify.Rows[e.RowIndex].Cells[CellName(nameof(SectionWithDepartment.DepartmentCode))].DisplayText;
                    lblDepartmentToName.Text = grdDepartmentModify.Rows[e.RowIndex].Cells[CellName(nameof(SectionWithDepartment.DepartmentName))].DisplayText;
                    DepartmentToId = int.Parse(grdDepartmentModify.Rows[e.RowIndex].Cells[CellName(nameof(SectionWithDepartment.DepartmentId))].DisplayText);
                }
            }
        }

        /// <summary> 入力項目にある請求部門データをクリアする </summary>
        private void ClearInput()
        {
            SetDepartmentData(null, txtDepartmentFrom, lblDepartmentFromName, ref DepartmentFromId);
            SetDepartmentData(null, txtDepartmentTo, lblDepartmentToName, ref DepartmentToId);
        }

        private void SetSectionDisable()
        {
            txtSection.Enabled = false;
            btnSectionSearch.Enabled = false;

            txtDepartmentFrom.Enabled = true;
            btnDepartmentFrom.Enabled = true;
            txtDepartmentTo.Enabled = true;
            btnDepartmentTo.Enabled = true;
        }

        /// <summary> DepartmentIdでSectionWithDepartmetからデータを取得 </summary>
        /// <param name="departmentId">　入力項目にある請求部門コードのId　</param>
        /// <returns>　SectionWithDepartmentデータ　</returns>
        private async Task<SectionWithDepartment> GetDepartmentIdInSWD(int departmentId)
        {
            SectionWithDepartment depValue = null;
            if (departmentId != 0)
            {
                await ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<SectionWithDepartmentMasterClient>();
                    SectionWithDepartmentResult result = await service.GetByDepartmentAsync(SessionKey, CompanyId, departmentId);

                    if (result.ProcessResult.Result)
                    {
                        depValue = result.SectionDepartment;
                    }
                });
            }
            return depValue;
        }
        /// <summary> ほうかのSectionIdとJoinすることをチェックのため </summary>
        private async Task<List<Department>> GetDepartmentDataForCheckAsync()
        {
            List<Department> departmentList = null;
            if (SectionId != null)
            {
                await ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<DepartmentMasterClient>();
                    DepartmentsResult result = await service.GetWithoutSectionAsync(SessionKey, CompanyId, SectionId.Value);

                    if (result.ProcessResult.Result)
                    {
                        departmentList = result.Departments;
                    }
                });
            }
            return departmentList ?? new List<Department>();
        }

        private void OnContentChanged(object sender, EventArgs e)
        {
            Modified = true;
        }
        #endregion
    }
}
