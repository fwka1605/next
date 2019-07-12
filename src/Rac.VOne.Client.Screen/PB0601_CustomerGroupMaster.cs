using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.Controls;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.CustomerGroupMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
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

namespace Rac.VOne.Client.Screen
{
    /// <summary>債権代表者マスター</summary>
    public partial class PB0601 : VOneScreenBase
    {

        private List<CustomerGroup> CustomerModifyList { get; set; }
        private List<CustomerGroup> CustomerOriginList { get; set; }
        private List<CustomerGroup> AddCustomer { get; set; }
        private List<CustomerGroup> DeleteCustomer { get; set; }
        private int? ChildCustomerIdFrom { get; set; }
        private int? ChildCustomerIdTo { get; set; }
        private int ParentCustomerId { get; set; }
        private bool ErrorFlag { get; set; }

        public PB0601()
        {
            InitializeComponent();
            grdCustomerOrigin.SetupShortcutKeys();
            grdCustomerModify.SetupShortcutKeys();
            Text = "債権代表者マスター";
        }

        #region 初期化
        private void SetFormat()
        {
            var expression = new DataExpression(ApplicationControl);
            txtCustomerCodeFrom.Format = expression.CustomerCodeFormatString;
            txtCustomerCodeFrom.MaxLength = expression.CustomerCodeLength;
            txtCustomerCodeFrom.ImeMode = expression.CustomerCodeImeMode();
            txtCustomerCodeTo.Format = expression.CustomerCodeFormatString;
            txtCustomerCodeTo.MaxLength = expression.CustomerCodeLength;
            txtCustomerCodeTo.ImeMode = expression.CustomerCodeImeMode();
            txtParentCustomerCode.Format = expression.CustomerCodeFormatString;
            txtParentCustomerCode.MaxLength = expression.CustomerCodeLength;
            txtParentCustomerCode.ImeMode = expression.CustomerCodeImeMode();
            txtParentCustomerCode.PaddingChar = expression.CustomerCodePaddingChar;
            txtCustomerCodeFrom.PaddingChar = expression.CustomerCodePaddingChar;
            txtCustomerCodeTo.PaddingChar = expression.CustomerCodePaddingChar;
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

        private void InitializeModifyGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "Header"         , cell: builder.GetRowHeaderCell() ),
                new CellSetting(height, 115, "Code"           , dataField: nameof(CustomerGroup.ChildCustomerCode), caption: "得意先コード", cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter) ),
                new CellSetting(height, 100, "Name"           , dataField: nameof(CustomerGroup.ChildCustomerName), caption: "得意先名"    , cell: builder.GetTextBoxCell() ),
                new CellSetting(height,   0, "ChildCustomerId", dataField: "Id", visible: false )
            });
            grdCustomerModify.Template = builder.Build();
            grdCustomerModify.SetRowColor(ColorContext);
            grdCustomerModify.HideSelection = true;
        }

        private void InitializeOrginGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            builder.AutoLocationSet = true;
            builder.AllowHorizontalResize = true;
            builder.Sortable = false;
            var height = builder.DefaultRowHeight;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "Header", cell: builder.GetRowHeaderCell() ),
                new CellSetting(height, 115, "Code"  , dataField: nameof(CustomerGroup.ChildCustomerCode), caption: "得意先コード", cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter) ),
                new CellSetting(height, 100, "Name"  , dataField: nameof(CustomerGroup.ChildCustomerName), caption: "得意先名"    , cell: builder.GetTextBoxCell() ),
                new CellSetting(height,   0, "Id"    , dataField: nameof(CustomerGroup.ChildCustomerId)        , visible: true )
            });
            grdCustomerOrigin.Template = builder.Build();
            grdCustomerOrigin.SetRowColor(ColorContext);
            grdCustomerOrigin.HideSelection = true;
        }

        private void PB0601_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                txtParentCustomerCode.Focus();
                AfterParentSearch();
                InitializeOrginGrid();
                InitializeModifyGrid();
                AddCustomer = new List<CustomerGroup>();
                DeleteCustomer = new List<CustomerGroup>();
                CustomerModifyList = new List<CustomerGroup>();
                CustomerOriginList = new List<CustomerGroup>();
                Modified = false;
                ErrorFlag = false;
                var loadTask = new List<Task>();

                if (Company == null)
                {
                    loadTask.Add(LoadCompanyAsync());
                }

                loadTask.Add(LoadControlColorAsync());
                loadTask.Add(LoadFunctionAuthorities(MasterImport, MasterExport));

                if (ApplicationControl == null)
                {
                    loadTask.Add(LoadApplicationControlAsync());
                }

                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
                AddHandlers();
                SetFormat();
                ClearControlValues();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void AddHandlers()
        {
            foreach (Control control in gbxChildCustomerInput.Controls)
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

        #region Function Key Event
        [OperationLog("登録")]
        private void Save()
        {
            try
            {
                ClearStatusMessage();

                ZeroLeftPaddingWithoutValidated();

                if (!ValidateChildren()
                    || !ValidateInputValues()) return;
                SaveCustomerGroup();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void ZeroLeftPaddingWithoutValidated()
        {
            if (IsNeedValidate(ApplicationControl.CustomerCodeType, txtParentCustomerCode.TextLength, ApplicationControl.CustomerCodeLength))
            {
                txtParentCustomerCode.Text = ZeroLeftPadding(txtParentCustomerCode);
                txtParentCustomerCode_Validated(null, null);
            }
            if (IsNeedValidate(ApplicationControl.CustomerCodeType, txtCustomerCodeFrom.TextLength, ApplicationControl.CustomerCodeLength))
            {
                txtCustomerCodeFrom.Text = ZeroLeftPadding(txtCustomerCodeFrom);
                txtCustomerFrom_Validated(null, null);
            }
            if (IsNeedValidate(ApplicationControl.CustomerCodeType, txtCustomerCodeTo.TextLength, ApplicationControl.CustomerCodeLength))
            {
                txtCustomerCodeTo.Text = ZeroLeftPadding(txtCustomerCodeTo);
                txtCustomerTo_Validated(null, null);
            }
        }

        private bool ValidateInputValues()
        {
            if (string.IsNullOrEmpty(txtParentCustomerCode.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "債権代表者コード");
                txtParentCustomerCode.Focus();
                return false;
            }

            var id = "";
            var args = new string[] { };
            if (string.IsNullOrEmpty(txtCustomerCodeFrom.Text)
                && string.IsNullOrEmpty(txtCustomerCodeTo.Text))
            {
                id = MsgQstConfirmSave;
            }
            else
            {
                id = MsgQstConfirmUpdateSelectData; args = new string[] { "得意先" };
            }

            if (!ShowConfirmDialog(id, args))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return false;
            }
            return true;
        }

        private void SaveCustomerGroup()
        {
            if (!string.IsNullOrWhiteSpace(txtCustomerCodeFrom.Text)
               || !string.IsNullOrWhiteSpace(txtCustomerCodeTo.Text))
            {
                if (!CheckRangeData()) return;
                var addTask = AddCustomerGroupAsync();
                ProgressDialog.Start(ParentForm, addTask, false, SessionKey);
            }

            var loadTask = GetChildCustomersByParentId(ParentCustomerId)
                .ContinueWith(t => PrepareCustomerGroup(t.Result));
            ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);

            var success = false;
            var saveTask = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerGroupMasterClient>();
                var saveResult = await service.SaveAsync(SessionKey,
                    AddCustomer.ToArray(), DeleteCustomer.ToArray());
                success = saveResult?.ProcessResult.Result ?? false;
                if (!success) return;

                AddCustomer.Clear();
                DeleteCustomer.Clear();

                var customerGroup = await GetChildCustomersByParentId(ParentCustomerId);
                customerGroup = customerGroup.OrderBy((x => x.ChildCustomerCode)).ToList();
                CustomerModifyList = customerGroup;
                CustomerOriginList = customerGroup;
            });
            ProgressDialog.Start(ParentForm, saveTask, false, SessionKey);

            if (!success)
            {
                ShowWarningDialog(MsgErrSaveError);
                return;
            }

            grdCustomerModify.DataSource = new BindingSource(CustomerModifyList, null);
            grdCustomerOrigin.DataSource = new BindingSource(CustomerOriginList, null);
            txtCustomerCodeFrom.Focus();
            ClearChildCustomerInfo();
            Modified = false;

            DispStatusMessage(MsgInfSaveSuccess);
        }

        /// <summary>
        ///  債権代表者マスター 登録前に AddCusotmer, DeleteCustomer を設定する処理
        /// </summary>
        /// <param name="customerGroupDB"></param>
        private void PrepareCustomerGroup(List<CustomerGroup> customerGroupDB)
        {
            if (CustomerModifyList.Any())
            {
                AddCustomer.Clear();
                DeleteCustomer.Clear();

                foreach (var item in CustomerModifyList
                    .Where(x => !customerGroupDB.Any(y => y.ChildCustomerCode == x.ChildCustomerCode)))
                {
                    item.ParentCustomerId = ParentCustomerId;
                    item.CreateBy = Login.UserId;
                    item.UpdateBy = Login.UserId;
                    AddCustomer.Add(item);
                }
            }

            foreach (var item in customerGroupDB
                .Where(x => !CustomerModifyList.Any(y => y.ChildCustomerCode == x.ChildCustomerCode)))
            {
                DeleteCustomer.Add(item);
            }
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            ClearStatusMessage();

            if (Modified
                && !ShowConfirmDialog(MsgQstConfirmClear))
            {
                return;
            }

            ClearControlValues();
            Modified = false;
        }

        private void ClearControlValues()
        {
            txtParentCustomerCode.Clear();
            lblParentCustomerKana.Clear();
            ParentCustomerId = 0;
            txtCustomerCodeFrom.Clear();
            txtCustomerCodeTo.Clear();
            lblCustomerNameFrom.Clear();
            lblCustomerNameTo.Clear();
            grdCustomerModify.DataSource = null;
            grdCustomerOrigin.DataSource = null;
            BaseContext.SetFunction05Enabled(Authorities[MasterImport]);
            BaseContext.SetFunction06Enabled(Authorities[MasterExport]);
            ClearStatusMessage();
            BeforeParentSearch();
            CustomerModifyList = null;
        }

        private void ClearChildCustomerInfo()
        {
            txtCustomerCodeFrom.Clear();
            lblCustomerNameFrom.Clear();
            txtCustomerCodeTo.Clear();
            lblCustomerNameTo.Clear();
            ChildCustomerIdFrom = null;
            ChildCustomerIdTo = null;
        }

        [OperationLog("印刷")]
        public void Print()
        {
            try
            {
                ClearStatusMessage();
                var customerGroupReport = new CustomerGroupSectionReport();
                var serverPath = string.Empty;
                var messageId = string.Empty;
                ProgressDialog.Start(ParentForm, Task.Run(async () =>
                {
                    serverPath = await GetServerPath();
                    List<CustomerGroup> result = await GetPrintDataAsync();
                    if (!result.Any())
                    {
                        messageId = MsgWngPrintDataNotExist;
                        return;
                    }
                    customerGroupReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                    customerGroupReport.Name = "債権代表者マスター" + DateTime.Today.ToString("yyyyMMdd");
                    customerGroupReport.DataSource = result;
                    customerGroupReport.Run(true);
                }), false, SessionKey);

                if (!string.IsNullOrEmpty(messageId))
                {
                    ShowWarningDialog(messageId);
                    return;
                }

                ShowDialogPreview(ParentForm, customerGroupReport, serverPath);
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
                var result = await service.GetByCodeAsync(
                        SessionKey, CompanyId, "サーバパス");

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
                var customerGroupList = new List<CustomerGroup>();

                ImportSetting importSetting = null;
                var task = Util.GetMasterImportSettingAsync(Login, ImportFileType.CustomerGroup);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                importSetting = task.Result;

                var definition = new CustomerGroupFileDefinition(new DataExpression(ApplicationControl));
                definition.GetCustomerDictionary = val => Util.ConvertToDictionary(Util.GetCustomerList(Login, val), x => x.Code);
                definition.GetDbCsutomerGroups = () => Task.Run(async () => await LoadListAsync()).Result;

                var importer = definition.CreateImporter(m => new { m.ParentCustomerId, m.ChildCustomerId });
                importer.UserId = Login.UserId;
                importer.UserCode = Login.UserCode;
                importer.CompanyId = Login.CompanyId;
                importer.CompanyCode = Login.CompanyCode;
                importer.LoadAsync = async () => await LoadListAsync();
                importer.RegisterAsync = async unitOfWork => await RegisterForImportAsync(unitOfWork);

                var importResult = DoImport(importer, importSetting, ClearControlValues);
                if (!importResult) return;
                Modified = false;
                Task<List<CustomerGroup>> loadTask = LoadListAsync();
                ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);
                customerGroupList.AddRange(loadTask.Result);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrImportErrorWithoutLog);
            }
        }

        [OperationLog("エクスポート")]
        private void Export()
        {
            ClearStatusMessage();
            try
            {
                List<CustomerGroup> listCustomerGroup = null;
                string serverPath = null;
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<GeneralSettingMasterClient>();
                    GeneralSettingResult result = await service.GetByCodeAsync(
                            SessionKey, CompanyId, "サーバパス");
                    if (result.ProcessResult.Result)
                    {
                        serverPath = result.GeneralSetting?.Value;
                    }

                });

                Task<List<CustomerGroup>> printDataTask = GetPrintDataAsync();
                ProgressDialog.Start(ParentForm, Task.WhenAll(task, printDataTask), false, SessionKey);

                listCustomerGroup = printDataTask.Result;

                if (!listCustomerGroup.Any())
                {
                    ShowWarningDialog(MsgWngNoExportData);
                    return;
                }

                if (!Directory.Exists(serverPath))
                {
                    serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                var filePath = string.Empty;
                var fileName = $"債権代表者マスター{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var definition = new CustomerGroupFileDefinition(new DataExpression(ApplicationControl));
                var exporter = definition.CreateExporter();
                exporter.UserId = Login.UserId;
                exporter.UserCode = Login.UserCode;
                exporter.CompanyId = CompanyId;
                exporter.CompanyCode = Login.CompanyCode;
                definition.ParentCustomerField.GetModelsById = ids =>
                {
                    Dictionary<int, Customer> product = null;
                    ServiceProxyFactory.LifeTime(factory =>
                    {
                        var customer = factory.Create<CustomerMasterClient>();
                        CustomersResult result = customer.Get(SessionKey, ids);
                        if (result.ProcessResult.Result)
                        {
                            product = result.Customers.ToDictionary(c => c.Id);
                        }
                    });
                    return product ?? new Dictionary<int, Customer>();
                };
                definition.ChildCustomerField.GetModelsById = definition.ParentCustomerField.GetModelsById;
                ProgressDialog.Start(ParentForm, (cancel, progress) =>
                {
                    return exporter.ExportAsync(filePath, listCustomerGroup, cancel, progress);
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
                        SaveCustomerGroup();
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

        #region 検索イベント
        private void btnCustomerGroupSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                var customer = this.ShowCustomerSearchDialog(loader: new CustomerGridLoader(ApplicationContext)
                {
                    Key = CustomerGridLoader.SearchCustomer.IsParent
                });

                if (customer != null)
                {
                    txtParentCustomerCode.Text = customer.Code;
                    txtParentCustomerCode_Validated(txtParentCustomerCode, EventArgs.Empty);
                }

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }


        private int? GetCustomerId(Customer customer, VOneTextControl code, VOneDispLabelControl name)
        {
            code.Text = customer?.Code ?? string.Empty;
            name.Text = customer?.Name ?? string.Empty;
            return customer?.Id;
        }

        private void BindParentCustomerData(Customer customer)
        {
            var valid = customer?.IsParent == 1;
            if (valid)
            {
                txtParentCustomerCode.Text = customer.Code;
                lblParentCustomerKana.Text = customer.Kana;
                ParentCustomerId = customer.Id;
                AfterParentSearch();
                grdCustomerModify.DataSource = new BindingSource(CustomerModifyList, null);
                grdCustomerOrigin.DataSource = new BindingSource(CustomerModifyList, null);
                //Console.WriteLine($"{DateTime.Now:yy/MM/dd HH:mm:ss.000} : set grid bindings");
            }
            var messageId = string.Empty;
            var args = new string[] { };
            var isNewCode = valid && !(CustomerModifyList?.Any() ?? false);

            if (customer == null)
            {
                messageId = MsgWngMasterNotExist;
                args = new string[] { "得意先", txtParentCustomerCode.Text };
            }
            else if (customer.IsParent == 0)
            {
                messageId = MsgWngNotParentCustomer;
                args = new string[] { txtParentCustomerCode.Text };
            }
            else
            {
                if (isNewCode)
                {
                    messageId = MsgInfSaveNewData;
                    args = new string[] { "債権代表者" };
                }
            }

            if (!valid)
            {
                ShowWarningDialog(messageId, args);
                txtParentCustomerCode.Clear();
                lblParentCustomerKana.Clear();
                txtParentCustomerCode.Focus();
            }
            else
            {
                if (isNewCode)
                {
                    DispStatusMessage(messageId, args);
                }
                txtCustomerCodeFrom.Focus();
            }
        }

        private void btnCustomerFrom_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var customer = ShowSearchCustomreWithParent();
            if (customer == null) return;
            ChildCustomerIdFrom = GetCustomerId(customer, txtCustomerCodeFrom, lblCustomerNameFrom);
        }

        private void btnCustomerTo_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var customer = ShowSearchCustomreWithParent();
            if (customer == null) return;
            ChildCustomerIdTo = GetCustomerId(customer, txtCustomerCodeTo, lblCustomerNameTo);
        }

        private Customer ShowSearchCustomreWithParent()
        {
            var customer = this.ShowCustomerSearchDialog(loader: new CustomerGridLoader(ApplicationContext)
            {
                Key = CustomerGridLoader.SearchCustomer.OtherChildrenWithoutGroup,
                CustomerId = new int[] { ParentCustomerId }
            });
            return customer;
        }

        #endregion

        #region その他メソッド
        // 前の検索
        private void BeforeParentSearch()
        {
            txtParentCustomerCode.Enabled = true;
            btnParentCustomerSearch.Enabled = true;
            txtCustomerCodeFrom.Enabled = false;
            txtCustomerCodeTo.Enabled = false;
            btnCustomerFrom.Enabled = false;
            btnCustomerTo.Enabled = false;
            btnAdd.Enabled = false;
            btnDelete.Enabled = false;
            btnDeleteAll.Enabled = false;
            txtParentCustomerCode.Focus();
        }

        // 後で 検索
        private void AfterParentSearch()
        {
            txtParentCustomerCode.Enabled = false;
            btnParentCustomerSearch.Enabled = false;
            txtCustomerCodeFrom.Enabled = true;
            txtCustomerCodeTo.Enabled = true;
            btnCustomerFrom.Enabled = true;
            btnCustomerTo.Enabled = true;
            btnAdd.Enabled = true;
            btnDelete.Enabled = true;
            btnDeleteAll.Enabled = true;
        }

        #endregion

        #region 追加、削除、一括削除
        private bool CheckRangeData()
        {
            if (!string.IsNullOrWhiteSpace(txtCustomerCodeFrom.Text) && !string.IsNullOrWhiteSpace(txtCustomerCodeTo.Text)
               && !txtCustomerCodeFrom.ValidateRange(txtCustomerCodeTo, () => ShowWarningDialog(MsgWngInputRangeChecked, "得意先")))
            {
                ClearChildCustomerInfo();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtCustomerCodeFrom.Text) && string.IsNullOrWhiteSpace(txtCustomerCodeTo.Text))
            {
                //テキストボックス未入力時、メッセージ【W00170, { 項目名}】を表示
                ShowWarningDialog(MsgWngInputRequired, "得意先");
                txtCustomerCodeFrom.Focus();
                return false;
            }
            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnAdd);
            try
            {
                ClearStatusMessage();

                if (!CheckRangeData()) return;

                Task<int> task = AddCustomerGroupAsync();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                if (task.Result == 0)
                {
                    ShowWarningDialog(MsgWngNoData, "追加");
                    ClearChildCustomerInfo();
                }

                grdCustomerModify.DataSource = new BindingSource(CustomerModifyList, null);
                ClearChildCustomerInfo();
                Modified = true;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task<int> AddCustomerGroupAsync()
        {
            List<Customer> customers = await GetChildCustomersWithParentOrOrphans();
            var codeFrom = string.IsNullOrEmpty(txtCustomerCodeFrom.Text) ? txtCustomerCodeTo.Text : txtCustomerCodeFrom.Text;
            var codeTo = string.IsNullOrEmpty(txtCustomerCodeTo.Text) ? txtCustomerCodeFrom.Text : txtCustomerCodeTo.Text;

            Func<Customer, bool> isNewChildCustomer = x =>
                (string.Compare(codeFrom, x.Code) <= 0
                && string.Compare(x.Code, codeTo) <= 0)
                && !CustomerModifyList.Any(group => group.ChildCustomerCode == x.Code);

            var newCustomers = customers.Where(isNewChildCustomer).ToList();
            if (newCustomers.Any())
            {
                CustomerModifyList.AddRange(newCustomers.Select(x => new CustomerGroup
                {
                    ChildCustomerId = x.Id,
                    ChildCustomerCode = x.Code,
                    ChildCustomerName = x.Name
                }));

                CustomerModifyList = CustomerModifyList.OrderBy(x => x.ChildCustomerCode).ToList();
            }

            return newCustomers.Count;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnDelete);

            try
            {
                ClearStatusMessage();

                if (!CheckRangeData()) return;

                string codeFrom = string.IsNullOrWhiteSpace(txtCustomerCodeFrom.Text) ? txtCustomerCodeTo.Text : txtCustomerCodeFrom.Text;
                string codeTo = string.IsNullOrWhiteSpace(txtCustomerCodeTo.Text) ? txtCustomerCodeFrom.Text : txtCustomerCodeTo.Text;

                Func<CustomerGroup, bool> range = d => (string.Compare(codeFrom, d.ChildCustomerCode) <= 0 && string.Compare(d.ChildCustomerCode, codeTo) <= 0);

                if (!CustomerModifyList.Any(range))
                {
                    ShowWarningDialog(MsgWngNoData, "削除");
                    ClearChildCustomerInfo();
                    return;
                }

                CustomerModifyList = CustomerModifyList.Except(CustomerModifyList.Where(range)).OrderBy(s => s.ChildCustomerCode).ToList();
                grdCustomerModify.DataSource = new BindingSource(CustomerModifyList, null);

                Task<List<CustomerGroup>> loadTask = GetChildCustomersByParentId(ParentCustomerId);
                ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);
                CustomerOriginList = loadTask.Result.OrderBy(s => s.ChildCustomerCode).ToList();
                grdCustomerOrigin.DataSource = new BindingSource(CustomerOriginList, null);

                Modified = true;
                ClearChildCustomerInfo();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnDeleteAll);

            try
            {
                ClearStatusMessage();

                if (!CustomerModifyList.Any())
                {
                    ShowWarningDialog(MsgWngNoData, "削除");
                    return;
                }

                if (ShowConfirmDialog(MsgQstConfirmDeleteAll))
                {
                    ClearChildCustomerInfo();
                    CustomerModifyList.Clear();
                    CustomerModifyList = CustomerModifyList.OrderBy(s => s.ChildCustomerCode).ToList();
                    grdCustomerModify.DataSource = new BindingSource(CustomerModifyList, null);

                    Task<List<CustomerGroup>> loadTask = GetChildCustomersByParentId(ParentCustomerId);
                    ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);
                    List<CustomerGroup> customerOriginList
                            = loadTask.Result.OrderBy(x => x.ChildCustomerCode).ToList();
                    grdCustomerOrigin.DataSource = new BindingSource(customerOriginList, null);
                    Modified = true;
                    ErrorFlag = false;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region Web Service
        /// <summary>
        ///  会社ID、得意先コードから得意先取得
        ///  他グループに属する得意先や、取得した得意先が子として登録してあるかを確認するため、
        ///  CustomerGroup で返す
        ///  ParentCustomerId
        ///   0 子として登録してあり、債権代表者グループの登録なし
        ///   * 子の得意先ID Customer.Id と同じ場合、親として登録あり
        ///   * それ以外は、債権代表者グループの親のCustomer.Id を表示
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private async Task<CustomerGroup> GetCustomerForCustomerGroup(string code)
        {
            CustomerGroup group = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<CustomerGroupMasterClient>();
                var result = await client.GetCustomerForCustomerGroupAsync(SessionKey, CompanyId, code);
                group = result.CustomerGroup;
            });
            return group;
        }

        /// <summary>
        /// 債権代表者ID に紐づく得意先 または どのグループにも属さない 子の得意先を取得
        /// </summary>
        /// <returns></returns>
        private async Task<List<Customer>> GetChildCustomersWithParentOrOrphans()
        {
            List<Customer> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerMasterClient>();
                CustomersResult result = await service.GetByChildDetailsAsync(
                    SessionKey, CompanyId, ParentCustomerId);
                if (result.ProcessResult.Result)
                {
                    list = result.Customers;
                }
            });
            return list ?? new List<Customer>();
        }

        private async Task<List<Customer>> GetCustomerByCode(string code)
        {
            List<Customer> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<CustomerMasterClient>();
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });
                if (result.ProcessResult.Result)
                {
                    list = result.Customers;
                }
            });
            return list ?? new List<Customer>();
        }

        private async Task<List<CustomerGroup>> GetChildCustomersByParentId(int parentCustomerId)
        {
            List<CustomerGroup> list = null;
            if (parentCustomerId != 0)
            {
                await ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<CustomerGroupMasterClient>();
                    CustomerGroupsResult result = await service.GetByParentAsync(
                        SessionKey, parentCustomerId);
                    if (result.ProcessResult.Result)
                    {
                        list = result.CustomerGroups;
                    }
                });
            }
            return list ?? new List<CustomerGroup>();
        }

        private async Task<List<CustomerGroup>> GetPrintDataAsync()
        {
            List<CustomerGroup> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var customerGroup = factory.Create<CustomerGroupMasterClient>();
                CustomerGroupsResult result = await customerGroup.GetPrintCustomerDataAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                {
                    list = result.CustomerGroups;
                }
            });
            return list ?? new List<CustomerGroup>();
        }

        private async Task<List<CustomerGroup>> LoadListAsync()
            => await Util.GetCustomerGroupListAsync(Login);

        private async Task<ImportResult> RegisterForImportAsync(UnitOfWork<CustomerGroup> imported)
            => await Util.ImportCustomerGroupAsync(Login, imported);

        #endregion

        #region Validateイベント
        private void grdCustomerModify_CellDoubleClick(object sender, GrapeCity.Win.MultiRow.CellEventArgs e)
        {
            ClearStatusMessage();
            if (e.RowIndex >= 0)
            {
                if (txtCustomerCodeFrom.Text == "")
                {
                    txtCustomerCodeFrom.Text = grdCustomerModify.Rows[e.RowIndex].Cells[1].DisplayText;
                    lblCustomerNameFrom.Text = grdCustomerModify.Rows[e.RowIndex].Cells[2].DisplayText;
                }
                else
                {
                    txtCustomerCodeTo.Text = grdCustomerModify.Rows[e.RowIndex].Cells[1].DisplayText;
                    lblCustomerNameTo.Text = grdCustomerModify.Rows[e.RowIndex].Cells[2].DisplayText;
                }
            }
        }

        private int? GetDataFromValidate(VOneTextControl code, VOneDispLabelControl name)
        {

            var task = GetCustomerForCustomerGroup(code.Text);

            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            var result = task.Result;

            var id = string.Empty;
            var args = new string[] { };
            if (result == null)
            {
                id = MsgWngMasterNotExist; args = new string[] { "得意先", code.Text };
            }
            else if (result.ParentCustomerId != 0 && result.ParentCustomerId == result.ChildCustomerId)
            {
                id = MsgWngAlreadyParentCustomer; args = new string[] { code.Text };
            }
            else if (result.ParentCustomerId != 0 && result.ParentCustomerId != ParentCustomerId)
            {
                id = MsgWngOtherChildCustomer; args = new string[] { code.Text };
            }

            if (!string.IsNullOrEmpty(id))
            {
                ShowWarningDialog(id, args);
                name.Clear();
                code.Clear();
                code.Focus();
                return null;
            }
            var customer = new Customer
            {
                Id = result.ChildCustomerId,
                Code = result.ChildCustomerCode,
                Name = result.ChildCustomerName,
            };
            return GetCustomerId(customer, code, name);

        }

        private void txtParentCustomerCode_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            if (string.IsNullOrEmpty(txtParentCustomerCode.Text))
            {
                lblParentCustomerKana.Clear();
                return;
            }

            Customer customer = null;
            try
            {
                var task = GetCustomerByCode(txtParentCustomerCode.Text)
                    .ContinueWith(t =>
                    {
                        customer = t.Result.FirstOrDefault();
                        if (customer?.IsParent == 1)
                        {
                            CustomerModifyList = GetChildCustomersByParentId(customer.Id).Result;
                            CustomerModifyList = CustomerModifyList.OrderBy(x => x.ChildCustomerCode).ToList();
                            //Console.WriteLine($"{DateTime.Now:yy/MM/dd HH:mm:ss.000} : getchildcustomer done");
                        }
                        else
                        {
                            CustomerModifyList = new List<CustomerGroup>();
                        }
                    });

                ProgressDialog.Start(ParentForm, task, false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }

            BindParentCustomerData(customer);
        }

        private void txtCustomerFrom_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            if (string.IsNullOrEmpty(txtCustomerCodeFrom.Text))
            {
                ChildCustomerIdTo = null;
                lblCustomerNameFrom.Clear();
                return;
            }
            try
            {
                if (!string.IsNullOrEmpty(txtCustomerCodeFrom.Text))
                {
                    ChildCustomerIdFrom = GetDataFromValidate(txtCustomerCodeFrom, lblCustomerNameFrom);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtCustomerTo_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            if (string.IsNullOrEmpty(txtCustomerCodeTo.Text))
            {
                ChildCustomerIdTo = null;
                lblCustomerNameTo.Clear();
                return;
            }
            try
            {
                if (!string.IsNullOrEmpty(txtCustomerCodeTo.Text))
                {
                    ChildCustomerIdTo = GetDataFromValidate(txtCustomerCodeTo, lblCustomerNameTo);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion
    }
}
