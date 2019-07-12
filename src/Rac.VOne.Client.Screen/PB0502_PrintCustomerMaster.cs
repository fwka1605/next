using GrapeCity.Win.Editors;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.CustomerFeeMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.StaffMasterService;
using Rac.VOne.Export;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>得意先マスター印刷指示</summary>
    public partial class PB0502 : VOneScreenBase
    {
        public string ButtonKey { get; set; }

        public PB0502()
        {
            InitializeComponent();
        }

        #region フォームロード
        private void PB0502_Load(object sender, EventArgs e)
        {
            try
            {
                Text = ButtonKey == "F4" ? "得意先マスター 印刷" : "得意先マスター 出力";
                SetScreenName();
                this.ActiveControl = txtCustomerCodeFrom;
                var loadTask = new List<Task>();

                if (Company == null)
                    loadTask.Add(LoadCompanyAsync());

                if (ApplicationControl == null)
                    loadTask.Add(LoadApplicationControlAsync());

                loadTask.Add(LoadControlColorAsync());
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                if (ApplicationControl != null)
                {
                    if (ApplicationControl.CustomerCodeType == 0)
                    {
                        txtCustomerCodeFrom.PaddingChar = '0';
                        txtCustomerCodeTo.PaddingChar = '0';
                    }
                    if (ApplicationControl.StaffCodeType == 0)
                    {
                        txtStaffCodeFrom.PaddingChar = '0';
                        txtStaffCodeTo.PaddingChar = '0';
                    }
                }
                ComboDataBind();
                SetFormat();

                if (ButtonKey == "F4")
                {

                    rdoCustomerMasterList.Visible = true;
                    rdoCustomerLedger.Visible = true;
                    rdoRegistrationFeeList.Visible = true;
                    rdoCustomerMaster.Visible = false;
                    rdoRegistrationFee.Visible = false;
                    rdoArgSet.Visible = false;

                    rdoCustomerMasterList.Checked = true;
                    BaseContext.SetFunction04Enabled(true);
                    BaseContext.SetFunction04Caption("印刷");
                    OnF04ClickHandler = Print;
                }
                else
                {
                    rdoCustomerMasterList.Visible = false;
                    rdoCustomerLedger.Visible = false;
                    rdoRegistrationFeeList.Visible = false;

                    rdoCustomerMaster.Location = new System.Drawing.Point(rdoCustomerMaster.Location.X, rdoCustomerMasterList.Location.Y);
                    rdoRegistrationFee.Location = new System.Drawing.Point(rdoRegistrationFee.Location.X, rdoCustomerLedger.Location.Y);
                    rdoArgSet.Location = new System.Drawing.Point(rdoArgSet.Location.X, rdoRegistrationFeeList.Location.Y);
                    rdoCustomerMaster.Visible = true;
                    rdoRegistrationFee.Visible = true;
                    rdoArgSet.Visible = true;

                    rdoCustomerMaster.Checked = true;
                    BaseContext.SetFunction06Enabled(true);
                    BaseContext.SetFunction06Caption("エクスポート");
                    OnF06ClickHandler = Export;

                    // 歩引表示・非表示制御
                    rdoArgSet.Visible = Convert.ToBoolean(ApplicationControl.UseDiscount);
                }

                //Restore ChcckBoxControl Value
                Settings.SetCheckBoxValue<PB0501>(Login, cbxCustomerCodeTo);
                Settings.SetCheckBoxValue<PB0501>(Login, cbxStaffCodeTo);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region InitializeFunctionKeys
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("");
            BaseContext.SetFunction01Enabled(false);

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = ConfirmToClear;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);

            BaseContext.SetFunction10Caption("終了");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Exit;
        }
        #endregion

        #region CoboData Binding
        private void ComboDataBind()
        {
            cmbShareTransferFee.Items.Add(new ListItem("相手先", 0));
            cmbShareTransferFee.Items.Add(new ListItem("自社", 1));
        }
        #endregion

        #region Validate Event
        private void txtCustomerCodeFrom_Validated(object sender, EventArgs e)
        {
            if (txtCustomerCodeFrom.Text == "")
            {
                lblCustomerNameFrom.Clear();
                if (cbxCustomerCodeTo.Checked)
                {
                    txtCustomerCodeTo.Clear();
                    lblCustomerNameTo.Clear();
                }
                ClearStatusMessage();
                return;
            }

            try
            {
                Task<Customer> task = GetDataByCustomerCode(txtCustomerCodeFrom.Text);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                Customer customer = task.Result;

                if (customer == null)
                {
                    lblCustomerNameFrom.Clear();

                    if (cbxCustomerCodeTo.Checked)
                    {
                        txtCustomerCodeTo.Text = txtCustomerCodeFrom.Text;
                        lblCustomerNameTo.Clear();
                    }
                    ClearStatusMessage();
                }
                else
                {
                    lblCustomerNameFrom.Text = customer.Name;
                    if (cbxCustomerCodeTo.Checked)
                    {
                        txtCustomerCodeTo.Text = txtCustomerCodeFrom.Text;
                        lblCustomerNameTo.Text = customer.Name;
                    }
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtCustomerCodeTo_Validated(object sender, EventArgs e)
        {
            if (txtCustomerCodeTo.Text == "")
            {
                lblCustomerNameTo.Clear();
                ClearStatusMessage();
                return;
            }

            try
            {
                Task<Customer> task = GetDataByCustomerCode(txtCustomerCodeTo.Text);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                Customer customerList = task.Result;

                if (customerList == null)
                {
                    lblCustomerNameTo.Clear();
                    ClearStatusMessage();
                }
                else
                {
                    lblCustomerNameTo.Text = customerList.Name;
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtStaffCodeFrom_Validated(object sender, EventArgs e)
        {
            if (txtStaffCodeFrom.Text == "")
            {
                lblStaffNameFrom.Clear();
                if (cbxStaffCodeTo.Checked)
                {
                    txtStaffCodeTo.Clear();
                    lblStaffNameTo.Clear();
                }
                ClearStatusMessage();
                return;
            }

            try
            {
                Task<Staff> task = GetDataByStaffCode(txtStaffCodeFrom.Text);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                Staff staffData = task.Result;

                if (staffData != null)
                {
                    lblStaffNameFrom.Text = staffData.Name;
                    if (cbxStaffCodeTo.Checked)
                    {
                        txtStaffCodeTo.Text = txtStaffCodeFrom.Text;
                        lblStaffNameTo.Text = staffData.Name;
                    }
                    ClearStatusMessage();
                }
                else
                {
                    lblStaffNameFrom.Clear();
                    if (cbxStaffCodeTo.Checked)
                    {
                        txtStaffCodeTo.Text = txtStaffCodeFrom.Text;
                        lblStaffNameTo.Clear();
                    }
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtStaffCodeTo_Validated(object sender, EventArgs e)
        {
            if (txtStaffCodeTo.Text == "")
            {
                lblStaffNameTo.Clear();
                ClearStatusMessage();
                return;
            }

            try
            {
                Task<Staff> task = GetDataByStaffCode(txtStaffCodeTo.Text);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                Staff staffData = task.Result;

                if (staffData != null)
                {
                    lblStaffNameTo.Text = staffData.Name;
                    ClearStatusMessage();
                }
                else
                {
                    lblStaffNameTo.Clear();
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtClosingDay_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            string closingDay = txtClosingDay.Text;
            txtClosingDay.Text = ZeroFillClosingDate(closingDay);
        }

        private void txtClosingDay_TextChanged(object sender, EventArgs e)
        {
            var txtDay = sender as Common.Controls.VOneTextControl;
            int value;
            if (int.TryParse(txtDay.Text, out value))
            {
                if (value == 0)
                    txtDay.Clear();
            }
        }

        #endregion

        #region Get Data From Validate Event
        /// <summary> 得意先コードでデータを取得する </summary>
        /// <param name="code">得意先コード </param>
        /// <returns>Customer</returns>
        private async Task<Customer> GetDataByCustomerCode(string code)
        {
            Customer customerList = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerMasterClient>();
                CustomersResult result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });
                if (result.ProcessResult.Result)
                {
                    customerList = result.Customers.FirstOrDefault();
                }
            });
            return customerList;
        }

        /// <summary> 営業担当者コードでデータを取得する </summary>
        /// <param name="code">営業担当者コード </param>
        /// <returns>Staff</returns>
        private async Task<Staff> GetDataByStaffCode(string code)
        {
            Staff staffResult = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<StaffMasterClient>();
                StaffsResult result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });

                if (result.ProcessResult.Result)
                {
                    staffResult = result.Staffs.FirstOrDefault();
                }
            });

            return staffResult;
        }
        #endregion

        #region CheckChanged Event
        private void rdoCustomerMasterList_CheckedChanged(object sender, EventArgs e)
        {
            EnableCustomerCheckChange();
        }

        private void rdoCustomerLedger_CheckedChanged(object sender, EventArgs e)
        {
            EnableCustomerCheckChange();
        }

        private void rdoRegistrationFeeList_CheckedChanged(object sender, EventArgs e)
        {
            DisableCustomerCheckChange();
        }

        private void rdoCustomerMaster_CheckedChanged(object sender, EventArgs e)
        {
            EnableCustomerCheckChange();
        }

        private void rdoRegistrationFee_CheckedChanged(object sender, EventArgs e)
        {
            DisableCustomerCheckChange();
        }

        private void rdoArgSet_CheckedChanged(object sender, EventArgs e)
        {
            EnableCustomerCheckChange();
        }

        private void EnableCustomerCheckChange()
        {
            txtCustomerCodeFrom.Enabled = true;
            btnCustomerCodeFrom.Enabled = true;
            cbxCustomerCodeTo.Enabled = true;
            txtCustomerCodeTo.Enabled = true;
            btnCustomerCodeTo.Enabled = true;
            txtClosingDay.Enabled = true;
            cmbShareTransferFee.Enabled = true;
            txtStaffCodeFrom.Enabled = true;
            btnStaffCodeFrom.Enabled = true;
            cbxStaffCodeTo.Enabled = true;
            txtStaffCodeTo.Enabled = true;
            btnStaffCodeTo.Enabled = true;
            datUpdateAtFrom.Enabled = true;
            datUpdateAtTo.Enabled = true;
        }

        private void DisableCustomerCheckChange()
        {
            txtCustomerCodeFrom.Clear();
            lblCustomerNameFrom.Clear();
            lblCustomerNameTo.Clear();
            txtCustomerCodeTo.Clear();
            txtClosingDay.Clear();
            cmbShareTransferFee.SelectedIndex = -1;
            txtStaffCodeFrom.Clear();
            lblStaffNameFrom.Clear();
            lblStaffNameTo.Clear();
            txtStaffCodeTo.Clear();
            datUpdateAtFrom.Clear();
            datUpdateAtTo.Clear();

            txtCustomerCodeFrom.Enabled = false;
            btnCustomerCodeFrom.Enabled = false;
            cbxCustomerCodeTo.Enabled = false;
            txtCustomerCodeTo.Enabled = false;
            btnCustomerCodeTo.Enabled = false;
            txtClosingDay.Enabled = false;
            cmbShareTransferFee.Enabled = false;
            txtStaffCodeFrom.Enabled = false;
            btnStaffCodeFrom.Enabled = false;
            cbxStaffCodeTo.Enabled = false;
            txtStaffCodeTo.Enabled = false;
            btnStaffCodeTo.Enabled = false;
            datUpdateAtFrom.Enabled = false;
            datUpdateAtTo.Enabled = false;
            cbxCustomerCodeTo.Enabled = false;
            cbxStaffCodeTo.Enabled = false;
        }
        #endregion

        #region 検索ダイアログ Click Event
        private void btnCustomerCodeFrom_Click(object sender, EventArgs e)
        {
            var customer = this.ShowCustomerMinSearchDialog();
            if (customer != null)
            {
                txtCustomerCodeFrom.Text = customer.Code;
                lblCustomerNameFrom.Text = customer.Name;

                if (cbxCustomerCodeTo.Checked)
                {
                    txtCustomerCodeTo.Text = customer.Code;
                    lblCustomerNameTo.Text = customer.Name;
                }
                ClearStatusMessage();
            }
        }

        private void btnCustomerCodeTo_Click(object sender, EventArgs e)
        {
            var customer = this.ShowCustomerMinSearchDialog();
            if (customer != null)
            {
                txtCustomerCodeTo.Text = customer.Code;
                lblCustomerNameTo.Text = customer.Name;
                ClearStatusMessage();
            }
        }

        private void btnStaffCodeFrom_Click(object sender, EventArgs e)
        {
            var staff = this.ShowStaffSearchDialog();
            if (staff != null)
            {
                txtStaffCodeFrom.Text = staff.Code;
                lblStaffNameFrom.Text = staff.Name;

                if (cbxStaffCodeTo.Checked)
                {
                    txtStaffCodeTo.Text = txtStaffCodeFrom.Text;
                    lblStaffNameTo.Text = staff.Name;
                }
                ClearStatusMessage();
            }
        }

        private void btnStaffCodeTo_Click(object sender, EventArgs e)
        {
            var staff = this.ShowStaffSearchDialog();
            if (staff != null)
            {
                txtStaffCodeTo.Text = staff.Code;
                lblStaffNameTo.Text = staff.Name;
                ClearStatusMessage();
            }
        }
        #endregion

        #region クリア処理
        [OperationLog("クリア")]
        private void ConfirmToClear()
        {
            ClearStatusMessage();
            Clear();
        }

        private void Clear()
        {
            txtCustomerCodeFrom.Clear();
            lblCustomerNameFrom.Clear();
            txtCustomerCodeTo.Clear();
            lblCustomerNameTo.Clear();
            txtClosingDay.Clear();
            cmbShareTransferFee.SelectedIndex = -1;
            txtStaffCodeFrom.Clear();
            lblStaffNameFrom.Clear();
            txtStaffCodeTo.Clear();
            lblStaffNameTo.Clear();
            datUpdateAtFrom.Clear();
            datUpdateAtTo.Clear();
            txtCustomerCodeFrom.Focus();
        }
        #endregion

        #region 印刷処理
        [OperationLog("印刷")]
        private void Print()
        {
            try
            {
                PrintCustomerMaster();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        /// <summary>検索前にCustomerSearchデータを準備</summary>
        /// <returns>準備されたCustomerSearchModel</returns>
        private CustomerSearch CreateSearchCondition()
        {
            var customerSearch = new CustomerSearch();
            customerSearch.CompanyId = Login.CompanyId;
            customerSearch.CustomerCodeFrom = txtCustomerCodeFrom.Text;
            customerSearch.CustomerCodeTo = txtCustomerCodeTo.Text;
            if (txtClosingDay.Text != "")
                customerSearch.ClosingDay = int.Parse(txtClosingDay.Text);
            if (cmbShareTransferFee.SelectedIndex == -1)
                customerSearch.ShareTransferFee = null;
            else
                customerSearch.ShareTransferFee = cmbShareTransferFee.SelectedIndex;
            customerSearch.StaffCodeFrom = txtStaffCodeFrom.Text;
            customerSearch.StaffCodeTo = txtStaffCodeTo.Text;
            customerSearch.UpdateAtFrom = datUpdateAtFrom.Value;
            customerSearch.UpdateAtTo = datUpdateAtTo.Value;

            return customerSearch;
        }

        private void PrintCustomerMaster()
        {
            ZeroLeftPaddingWithoutValidated();

            if (!RequiredCheck()) return;

            CustomerSearch customerSearch = CreateSearchCondition();

            ClearStatusMessage();

            Task task = null;
            GrapeCity.ActiveReports.SectionReport report = null;
            string serverPath = null;
            if (rdoCustomerMasterList.Checked)
            {
                task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    serverPath = await GetServerPath();
                    var service = factory.Create<CustomerMasterClient>();
                    CustomersResult result = await service.GetItemsAsync(SessionKey, CompanyId, customerSearch);

                    if (result.ProcessResult.Result)
                    {
                        var customerList = new List<Customer>(result.Customers);
                        if (customerList.Any())
                        {
                            var cusReport = new CustomerSectionReport();
                            cusReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                            cusReport.Name = "得意先マスター一覧" + DateTime.Today.ToString("yyyyMMdd");
                            cusReport.SetData(customerList);
                            cusReport.Run(false);
                            report = cusReport;
                        }
                    }
                });
            }
            else if (rdoCustomerLedger.Checked)
            {
                ClearStatusMessage();

                task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    serverPath = await GetServerPath();
                    var service = factory.Create<CustomerMasterClient>();
                    CustomersResult result = await service.GetItemsAsync(SessionKey, CompanyId, customerSearch);

                    if (result.ProcessResult.Result)
                    {
                        var ledgerList = new List<Customer>(result.Customers);
                        if (ledgerList.Any())
                        {
                            var cusAccReport = new CustomerAccountSectionReport();
                            cusAccReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                            cusAccReport.Name = "得意先台帳" + DateTime.Now.ToString("yyyyMMdd");

                            cusAccReport.SetData(ledgerList, UsePublishInvoice, UseReminder);
                            cusAccReport.Run(false);
                            report = cusAccReport;
                        }
                    }
                });
            }
            else
            {
                ClearStatusMessage();
                task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    serverPath = await GetServerPath();
                    var service = factory.Create<CustomerFeeMasterClient>();
                    CustomerFeesResult result = await service.GetForPrintAsync(SessionKey, CompanyId);

                    if (result.ProcessResult.Result)
                    {
                        var cusFeeList = new List<CustomerFee>(result.CustomerFeePrint);

                        int precision = 0;
                        if (UseForeignCurrency)
                            precision = cusFeeList.Select(x => x.CurrencyPrecision).Max();

                        if (cusFeeList.Any())
                        {
                            CustomerFeeSectionReport cusFeeReport = new CustomerFeeSectionReport();
                            cusFeeReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                            cusFeeReport.Name = "登録手数料一覧" + DateTime.Now.ToString("yyyyMMdd");
                            cusFeeReport.Precision = precision;
                            cusFeeReport.SetData(cusFeeList, ApplicationControl.UseForeignCurrency);
                            cusFeeReport.Run(false);
                            report = cusFeeReport;
                        }
                    }
                });
            }

            if (task != null)
            {
                ProgressDialog.Start(ParentForm, Task.Run(() => task), false, SessionKey);
                if (report == null)
                {
                    ShowWarningDialog(MsgWngPrintDataNotExist);
                    return;
                }
                ShowDialogPreview(ParentForm, report, serverPath);
            }
        }

        private bool RequiredCheck()
        {
            if (!txtCustomerCodeFrom.ValidateRange(txtCustomerCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblCustomerCodeFrom.Text))) return false;

            if (!txtStaffCodeFrom.ValidateRange(txtStaffCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblStaffCodeFrom.Text))) return false;

            if (!datUpdateAtFrom.ValidateRange(datUpdateAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblUpdateAtFrom.Text))) return false;

            return true;
        }
        #endregion

        #region 終了
        [OperationLog("終了")]
        private void Exit()
        {
            try
            {
                Settings.SaveControlValue<PB0501>(Login, cbxCustomerCodeTo.Name, cbxCustomerCodeTo.Checked);
                Settings.SaveControlValue<PB0501>(Login, cbxStaffCodeTo.Name, cbxStaffCodeTo.Checked);

                ParentForm.Close();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region エクスポート 処理
        [OperationLog("エクスポート")]
        private void Export()
        {
            try
            {
                ZeroLeftPaddingWithoutValidated();

                var customerSearch = new CustomerSearch();
                customerSearch.CompanyId = Login.CompanyId;
                customerSearch.CustomerCodeFrom = txtCustomerCodeFrom.Text;
                customerSearch.CustomerCodeTo = txtCustomerCodeTo.Text;
                if (txtClosingDay.Text != "")
                    customerSearch.ClosingDay = int.Parse(txtClosingDay.Text);
                if (cmbShareTransferFee.SelectedIndex == -1)
                    customerSearch.ShareTransferFee = null;
                else
                    customerSearch.ShareTransferFee = cmbShareTransferFee.SelectedIndex;
                customerSearch.StaffCodeFrom = txtStaffCodeFrom.Text;
                customerSearch.StaffCodeTo = txtStaffCodeTo.Text;
                customerSearch.UpdateAtFrom = datUpdateAtFrom.Value;
                customerSearch.UpdateAtTo = datUpdateAtTo.Value;

                if (!RequiredCheck())
                    return;

                ClearStatusMessage();

                if (rdoCustomerMaster.Checked)
                {
                    //得意先マスター
                    ExportCustomer(customerSearch);
                }
                else if (rdoRegistrationFee.Checked)
                {
                    // 得意先マスター登録手数料（エクスポート）
                    ExportRegistrationFee();
                }
                else
                {
                    // 歩引設定（エクスポート）
                    ExportDiscount(customerSearch);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                DispStatusMessage(MsgErrExportError);
            }
        }

        private void ExportCustomer(CustomerSearch customerData)
        {
            List<Customer> list = null;
            string serverPath = null;

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerMasterClient>();
                CustomersResult result = await service.GetItemsAsync(SessionKey, CompanyId, customerData);

                if (result.ProcessResult.Result)
                {
                    list = result.Customers;
                }
            });
            Task<string> pathTask = GetServerPath();
            ProgressDialog.Start(ParentForm, Task.WhenAll(task, pathTask), false, SessionKey);
            serverPath = pathTask.Result;

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
            var fileName = $"得意先マスター{DateTime.Today:yyyyMMdd}.csv";
            if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

            var definition = new CustomerFileDefinition(new DataExpression(ApplicationControl));
            definition.ExcludeInvoicePublishField.Ignored = !UsePublishInvoice;
            definition.ExcludeReminderPublishField.Ignored = !UseReminder;
            var exporter = definition.CreateExporter();
            exporter.UserId = Login.UserId;
            exporter.UserCode = Login.UserCode;
            exporter.CompanyId = CompanyId;
            exporter.CompanyCode = Login.CompanyCode;

            Func<int[], Dictionary<int, Category>> getter = ids =>
            {
                Dictionary<int, Category> product = null;
                ServiceProxyFactory.LifeTime(factory =>
                {
                    var categoryservice = factory.Create<CategoryMasterClient>();
                    CategoriesResult result = categoryservice.Get(SessionKey, ids);
                    if (result.ProcessResult.Result)
                    {
                        product = result.Categories.ToDictionary(c => c.Id);
                    }
                });
                return product ?? new Dictionary<int, Category>();
            };
            definition.CollectCategoryIdField.GetModelsById = getter;
            definition.LessThanCollectCategoryIdField.GetModelsById = getter;
            definition.GreaterThanCollectCategoryId3Field.GetModelsById = getter;
            definition.GreaterThanCollectCategoryId2Field.GetModelsById = getter;
            definition.GreaterThanCollectCategoryId1Field.GetModelsById = getter;

            NLogHandler.WriteDebug(this, "得意先マスター 出力開始");
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

            NLogHandler.WriteDebug(this, "得意先マスター 出力終了");

            DispStatusMessage(MsgInfFinishExport);
            Settings.SavePath<Customer>(Login, filePath);
        }

        /// <summary>管理マスターで設定したサーバパスを取得</summary>
        /// <returns>サーバパス</returns>
        private async Task<string> GetServerPath()
        {
            string serverPath = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var generalService = factory.Create<GeneralSettingMasterClient>();
                var result = await generalService.GetByCodeAsync(SessionKey, CompanyId, "サーバパス");

                if (result.ProcessResult.Result)
                {
                    serverPath = result.GeneralSetting?.Value;
                }
            });
            return serverPath;
        }

        private void ExportDiscount(CustomerSearch customerSearch)
        {
            List<CustomerDiscount> list = null;
            string serverPath = null;

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                serverPath = await GetServerPath();
                var service = factory.Create<CustomerMasterClient>();
                CustomerDiscountsResult result = await service.GetDiscountItemsAsync(SessionKey, customerSearch);

                if (result.ProcessResult.Result)
                {
                    list = result.CustomerDiscounts;
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

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
            var fileName = $"得意先マスター歩引設定{DateTime.Today:yyyyMMdd}.csv";
            if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

            var definition = new CustomerDiscountFileDefinition(new DataExpression(ApplicationControl));
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
            Settings.SavePath<CustomerDiscount>(Login, filePath);
        }

        private void ExportRegistrationFee()
        {
            List<CustomerFee> list = null;
            string serverPath = null;

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                serverPath = await GetServerPath();
                var service = factory.Create<CustomerFeeMasterClient>();
                CustomerFeeResult result = await service.GetForExportAsync(SessionKey, CompanyId);

                if (result.ProcessResult.Result)
                {
                    list = new List<CustomerFee>(result.CustomerFee);
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

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
            var fileName = $"得意先マスター登録手数料{DateTime.Today:yyyyMMdd}.csv";
            if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

            var definition = new CustomerRegistrationFeeFileDefinition(new DataExpression(ApplicationControl));
            var decimalFormat = "0" + ((!UseForeignCurrency) ? string.Empty : "." + new string('0', 5));
            definition.FeeField.Format = value => value.ToString(decimalFormat);
            definition.CurrencyCodeField.Ignored = !UseForeignCurrency;

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
            Settings.SavePath<CustomerFee>(Login, filePath);
        }
        #endregion

        #region その他Function
        private void SetFormat()
        {
            var expression = new DataExpression(ApplicationControl);

            txtCustomerCodeFrom.MaxLength = expression.CustomerCodeLength;
            txtCustomerCodeTo.MaxLength = expression.CustomerCodeLength;

            txtStaffCodeFrom.MaxLength = expression.StaffCodeLength;
            txtStaffCodeTo.MaxLength = expression.StaffCodeLength;

            txtCustomerCodeFrom.Format = expression.CustomerCodeFormatString;
            txtCustomerCodeTo.Format = expression.CustomerCodeFormatString;

            txtStaffCodeFrom.Format = expression.StaffCodeFormatString;
            txtStaffCodeTo.Format = expression.StaffCodeFormatString;

            txtCustomerCodeFrom.ImeMode = expression.CustomerCodeImeMode();
            txtCustomerCodeTo.ImeMode = expression.CustomerCodeImeMode();

            txtCustomerCodeFrom.PaddingChar = expression.CustomerCodePaddingChar;
            txtCustomerCodeTo.PaddingChar = expression.CustomerCodePaddingChar;
        }

        private string ZeroFillClosingDate(string number)
        {
            string fillnumber = null;

            if (!string.IsNullOrWhiteSpace(number))
            {
                if (Convert.ToInt16(number) > 27)
                {
                    fillnumber = "99";
                }
                else
                {
                    fillnumber = number.PadLeft(2, '0');
                }
            }
            return fillnumber;
        }

        private void ZeroLeftPaddingWithoutValidated()
        {
            if (IsNeedValidate(ApplicationControl.CustomerCodeType, txtCustomerCodeFrom.TextLength, ApplicationControl.CustomerCodeLength))
            {
                txtCustomerCodeFrom.Text = ZeroLeftPadding(txtCustomerCodeFrom);
                txtCustomerCodeFrom_Validated(null, null);
            }
            if (IsNeedValidate(ApplicationControl.CustomerCodeType, txtCustomerCodeTo.TextLength, ApplicationControl.CustomerCodeLength))
            {
                txtCustomerCodeTo.Text = ZeroLeftPadding(txtCustomerCodeTo);
                txtCustomerCodeTo_Validated(null, null);
            }
            if (IsNeedValidate(ApplicationControl.StaffCodeType, txtStaffCodeFrom.TextLength, ApplicationControl.StaffCodeLength))
            {
                txtStaffCodeFrom.Text = ZeroLeftPadding(txtStaffCodeFrom);
                txtStaffCodeFrom_Validated(null, null);
            }
            if (IsNeedValidate(ApplicationControl.StaffCodeType, txtStaffCodeTo.TextLength, ApplicationControl.StaffCodeLength))
            {
                txtStaffCodeTo.Text = ZeroLeftPadding(txtStaffCodeTo);
                txtStaffCodeTo_Validated(null, null);
            }
            if (!IsValidClosingDay(txtClosingDay.Text, txtClosingDay.MaxLength))
            {
                txtClosingDay_Validated(null, null);
            }
        }

        #endregion
    }
}
