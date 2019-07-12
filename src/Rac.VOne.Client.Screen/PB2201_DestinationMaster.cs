using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.BillingService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.DestinationMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.ReminderService;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    public partial class PB2201 : VOneScreenBase
    {
        #region 変数宣言
        private int DestinationId { get; set; }
        private int CustomerId { get; set; }
        private string CellName(string value) => $"cel{value}";
        #endregion

        public PB2201()
        {
            InitializeComponent();
            grdDestination.SetupShortcutKeys();
            Text = "送付先マスター";
            AddHandlers();
        }

        #region 画面の初期化
        private void PB2201_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();

                var loadTask = new List<Task>();
                if (ApplicationControl == null)
                    loadTask.Add(LoadApplicationControlAsync());
                if (Company == null)
                    loadTask.Add(LoadCompanyAsync());

                loadTask.Add(LoadControlColorAsync());
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                SetFormat();
                InitializeGrid();
                SetHonorificCombo();
                Clear();
            }
            catch(Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void SetFormat()
        {
            var expression = new DataExpression(ApplicationControl);

            txtCustomerCode.Format = expression.CustomerCodeFormatString;
            txtCustomerCode.MaxLength = expression.CustomerCodeLength;
            txtCustomerCode.ImeMode = expression.CustomerCodeImeMode();
            txtCustomerCode.PaddingChar = expression.CustomerCodePaddingChar;

            txtDestinationCode.PaddingChar = '0';
        }

        private void InitializeGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  35, "Header"                                                                           , cell: builder.GetRowHeaderCell()),
                new CellSetting(height,  70, nameof(Destination.Code)           , dataField: nameof(Destination.Code)           , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), caption: "送付先コード"),
                new CellSetting(height, 140, nameof(Destination.Name)           , dataField: nameof(Destination.Name)           , cell: builder.GetTextBoxCell() , caption: "送付先名"),
                new CellSetting(height,  65, nameof(Destination.PostalCode)     , dataField: nameof(Destination.PostalCode)     , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), caption: "郵便番号"),
                new CellSetting(height, 160, nameof(Destination.Address1)       , dataField: nameof(Destination.Address1)       , cell: builder.GetTextBoxCell() , caption: "住所1"),
                new CellSetting(height, 160, nameof(Destination.Address2)       , dataField: nameof(Destination.Address2)       , cell: builder.GetTextBoxCell() , caption: "住所2"),
                new CellSetting(height, 95, nameof(Destination.DepartmentName) , dataField: nameof(Destination.DepartmentName) , cell: builder.GetTextBoxCell() , caption: "部署"),
                new CellSetting(height, 95, nameof(Destination.Addressee)      , dataField: nameof(Destination.Addressee)      , cell: builder.GetTextBoxCell() , caption: "宛名"),
                new CellSetting(height,  60, nameof(Destination.Honorific)      , dataField: nameof(Destination.Honorific)      , cell: builder.GetTextBoxCell() , caption: "敬称"),
                new CellSetting(height,   0, nameof(Destination.Id)             , dataField: nameof(Destination.Id)             , visible: false )
            });

            grdDestination.Template = builder.Build();
            grdDestination.SetRowColor(ColorContext);
            grdDestination.HideSelection = true;
            grdDestination.CurrentCellBorderLine = new Line(LineStyle.None, Color.Empty);
            grdDestination.CurrentRowBorderLine = new Line(LineStyle.None, Color.Empty);
        }

        private void SetHonorificCombo()
        {
            cmbHonorific.MaxLength = 6;
            cmbHonorific.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Char;
            cmbHonorific.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            cmbHonorific.DropDownStyle = ComboBoxStyle.DropDown;
            cmbHonorific.Items.Add("御中");
            cmbHonorific.Items.Add("様");
            cmbHonorific.Items.Add("先生");
            cmbHonorific.AcceptsTabChar = GrapeCity.Win.Editors.TabCharMode.Filter;
            cmbHonorific.AcceptsCrLf = GrapeCity.Win.Editors.CrLfMode.Filter;
        }
        #endregion

        #region ファンクションキー初期化
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction10Caption("終了");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(false);

            OnF01ClickHandler = Save;
            OnF02ClickHandler = ConfirmToClear;
            OnF03ClickHandler = Delete;
            OnF10ClickHandler = Exit;
        }
        #endregion

        #region F1 登録処理

        [OperationLog("登録")]
        private void Save()
        {
            ClearStatusMessage();

            if (!RequireFieldsChecking()) return;

            ZeroLeftPaddingWithoutValidated();

            if (!ShowConfirmDialog(MsgQstConfirmSave))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }

            try
            {
                Task<bool> task = SaveDestination();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                if (task.Result)
                {
                    DispStatusMessage(MsgInfSaveSuccess);
                    Modified = false;
                    txtDestinationCode.Focus();
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

        private bool RequireFieldsChecking()
        {
            if (string.IsNullOrWhiteSpace(txtCustomerCode.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "得意先コード");
                txtCustomerCode.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtDestinationCode.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "送付先コード");
                txtDestinationCode.Focus();
                return false;
            }
            if (mskPostalCode.Text.Replace("_", string.Empty).Replace("-", string.Empty).Length > 0 &&
                mskPostalCode.Text.Replace("_", string.Empty).Replace("-", string.Empty).Length != 7)
            {
                ShowWarningDialog(MsgWngInputNeedxxDigits, lblPostalCode.Text, "7桁");
                mskPostalCode.Focus();
                return false;
            }
            return true;
        }

        private void ZeroLeftPaddingWithoutValidated()
        {
            if (IsNeedValidate(ApplicationControl.CustomerCodeType, txtCustomerCode.TextLength, ApplicationControl.CustomerCodeLength))
            {
                txtCustomerCode.Text = ZeroLeftPadding(txtCustomerCode);
                txtCustomerCode_Validated(null, null);
            }
            if (IsNeedValidate(0, txtDestinationCode.TextLength, txtDestinationCode.MaxLength))
            {
                txtDestinationCode.Text = ZeroLeftPadding(txtDestinationCode);
                txtDestinationCode_Validated(null, null);
            }
        }

        private async Task<bool> SaveDestination()
        {
            var destination = PrepareDestinationData();
            var result = false;

            try
            {
                var webResult = await SaveDestinationAsync(destination);
                if (webResult.ProcessResult.Result)
                {
                    var destinations = await GetDestinationListAsync(CustomerId);
                    grdDestination.DataSource = new BindingSource(destinations, null);
                    ClearInputFields();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            return result;
        }

        private Destination PrepareDestinationData()
        {
            var destination = new Destination();
            destination.CompanyId = Login.CompanyId;
            destination.CustomerId = CustomerId;
            destination.Name = txtDestinationName.Text.Trim();
            destination.Code = txtDestinationCode.Text;
            destination.PostalCode = mskPostalCode.Text.Replace("_", string.Empty).Replace("-", string.Empty).Length == 0 ?
                      string.Empty : mskPostalCode.Text;
            destination.Address1 = txtAddress1.Text.Trim();
            destination.Address2 = txtAddress2.Text.Trim();
            destination.DepartmentName = txtDepartmentName.Text.Trim();
            destination.Addressee = txtAddressee.Text.Trim();
            destination.Honorific = cmbHonorific.Text.Trim();
            destination.CreateBy = Login.UserId;
            destination.UpdateBy = Login.UserId;

            return destination;
        }
        #endregion

        #region F2 クリア処理
        [OperationLog("クリア")]
        private void ConfirmToClear()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear))
                return;

            Clear();
            ClearStatusMessage();
        }

        private void Clear()
        {
            lblCustomerName.Clear();
            txtCustomerCode.Clear();
            grdDestination.DataSource = null;
            ClearInputFields();
            txtDestinationCode.Enabled = false;
            btnCustomerSearch.Enabled = true;
            txtCustomerCode.Enabled = true;
            txtCustomerCode.Focus();
            Modified = false;
        }

        private void ClearInputFields()
        {
            BaseContext.SetFunction03Enabled(false);
            txtDestinationCode.Enabled = true;
            txtDestinationCode.Clear();
            txtDestinationName.Clear();
            mskPostalCode.Clear();
            txtAddress1.Clear();
            txtAddress2.Clear();
            txtDepartmentName.Clear();
            txtAddressee.Clear();
            cmbHonorific.Clear();
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
                ProgressDialog.Start(ParentForm, DeleteDestination(), false, SessionKey);
                txtDestinationCode.Focus();
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
                    var billingClient = factory.Create<BillingServiceClient>();
                    var billingExist = await billingClient.ExistDestinationAsync(SessionKey, DestinationId);
                    if (billingExist.Exist)
                    {
                        messaging = () => ShowWarningDialog(MsgWngDeleteConstraint, "請求データ", lblDestinationCode.Text);
                        valid = false;
                        return;
                    }
                    var reminderClient = factory.Create<ReminderServiceClient>();
                    var reminderOutputedExist = await reminderClient.ExistDestinationAsync(SessionKey, DestinationId);
                    if (reminderOutputedExist.Exist)
                    {
                        messaging = () => ShowWarningDialog(MsgWngDeleteConstraint, "督促状発行履歴", lblDestinationCode.Text);
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

        private async Task DeleteDestination()
        {
            CountResult result = await DeleteDestinationAsync(DestinationId);

            if (result.Count > 0)
            {
                var destinations = await GetDestinationListAsync(CustomerId);
                grdDestination.DataSource = new BindingSource(destinations, null);
                ClearInputFields();
                DispStatusMessage(MsgInfDeleteSuccess);
                Modified = false;
            }
            else
            {
                ShowWarningDialog(MsgErrDeleteError);
            }

        }
        #endregion

        #region F10 終了処理
        [OperationLog("終了")]
        private void Exit()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose))
                return;
            ParentForm.Close();
        }
        #endregion

        #region  入力項目変更イベント処理
        private void AddHandlers()
        {
            foreach (Control control in gbxDestinationInput.Controls)
            {
                if (control is Common.Controls.VOneTextControl 
                    || control is Common.Controls.VOneMaskControl
                    || control is Common.Controls.VOneComboControl)
                {
                    control.TextChanged += new EventHandler(OnContentChanged);
                }
            }
            foreach (Control control in this.GetAll<Common.Controls.VOneComboControl>())
            {
                ((Common.Controls.VOneComboControl)control).SelectedIndexChanged +=
                    new EventHandler(OnContentChanged);
            }
        }

        private void OnContentChanged(object sender, EventArgs e)
        {
            if (!Modified)
                Modified = true;
        }
        #endregion

        #region 検索ボタンClickイベント処理
        private void btnCustomerSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var customer = this.ShowCustomerMinSearchDialog();
                if (customer != null)
                {
                    txtCustomerCode.Text = customer.Code;
                    lblCustomerName.Text = customer.Name;
                    CustomerId = customer.Id;

                    var destinations = new List<Destination>();
                    ProgressDialog.Start(ParentForm, Task.Run(async () =>
                    {
                        destinations = await GetDestinationListAsync(CustomerId);
                    }), false, SessionKey);
                    grdDestination.DataSource = new BindingSource(destinations, null);

                    txtCustomerCode.Enabled = false;
                    btnCustomerSearch.Enabled = false;
                    txtDestinationCode.Enabled = true;
                    txtDestinationCode.Focus();
                    ClearStatusMessage();
                }
            }
            catch(Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region 郵便番号Enterイベント処理
        private void mskPostalCode_Enter(object sender, EventArgs e)
        {
            BeginInvoke((System.Action)delegate { SetMaskedTextBoxSelectAll((Common.Controls.VOneMaskControl)sender); });
        }

        private void SetMaskedTextBoxSelectAll(Common.Controls.VOneMaskControl txtbox)
        {
            mskPostalCode.SelectAll();
        }
        #endregion

        #region grdDestination_CellDoubleClick
        private void grdDestination_CellDoubleClick(object sender, CellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (Modified && !ShowConfirmDialog(MsgQstConfirmUpdateData))
                    return;

                BaseContext.SetFunction03Enabled(true);
                txtDestinationCode.Enabled = false;
                txtDestinationName.Text = grdDestination.Rows[e.RowIndex].Cells[CellName(nameof(Destination.Name))].DisplayText;
                txtDestinationCode.Text = grdDestination.Rows[e.RowIndex].Cells[CellName(nameof(Destination.Code))].DisplayText;
                mskPostalCode.Text = grdDestination.Rows[e.RowIndex].Cells[CellName(nameof(Destination.PostalCode))].DisplayText;
                txtAddress1.Text = grdDestination.Rows[e.RowIndex].Cells[CellName(nameof(Destination.Address1))].DisplayText;
                txtAddress2.Text = grdDestination.Rows[e.RowIndex].Cells[CellName(nameof(Destination.Address2))].DisplayText;
                txtDepartmentName.Text = grdDestination.Rows[e.RowIndex].Cells[CellName(nameof(Destination.DepartmentName))].DisplayText;
                txtAddressee.Text = grdDestination.Rows[e.RowIndex].Cells[CellName(nameof(Destination.Addressee))].DisplayText;
                cmbHonorific.Text = grdDestination.Rows[e.RowIndex].Cells[CellName(nameof(Destination.Honorific))].DisplayText;
                DestinationId = Convert.ToInt32(grdDestination.Rows[e.RowIndex].Cells[CellName(nameof(Destination.Id))].Value);

                mskPostalCode.Focus();
                ClearStatusMessage();
                Modified = false;
            }
        }
        #endregion

        #region Validated処理
        private void txtCustomerCode_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            string code = txtCustomerCode.Text;

            if (string.IsNullOrWhiteSpace(code))
            {
                txtCustomerCode.Clear();
                ClearStatusMessage();
                return;
            }

            try
            {
                Task<bool> task = LoadCustomerAsync(code);

                ProgressDialog.Start(ParentForm,
                    Task.WhenAll(task).ContinueWith(async t =>
                    {
                        if (task.Result)
                        {
                            var destinations = await GetDestinationListAsync(CustomerId);
                            grdDestination.DataSource = new BindingSource(destinations, null);
                        }
                    }, TaskScheduler.FromCurrentSynchronizationContext()).Unwrap(), false, SessionKey);

                if (task.Result)
                {
                    txtCustomerCode.Enabled = false;
                    btnCustomerSearch.Enabled = false;
                    txtDestinationCode.Enabled = true;
                    txtDestinationCode.Focus();
                }
                else
                {
                    ShowWarningDialog(MsgWngMasterNotExist, "得意先", txtCustomerCode.Text);
                    lblCustomerName.Clear();
                    txtCustomerCode.Clear();
                    txtCustomerCode.Focus();
                }
            }
            catch(Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtDestinationCode_Validated(object sender, EventArgs e)
        {
            string code = txtDestinationCode.Text;
            if (string.IsNullOrWhiteSpace(code))
                return;

            try
            {
                ProgressDialog.Start(ParentForm, LoadDestinationAsync(code), false, SessionKey);
            }
            catch(Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region 共通
        private async Task<bool> LoadCustomerAsync(string code)
        {
            var customer = await GetCustomerAsync(code);

            if (customer == null)
                return false;

            txtCustomerCode.Text = customer.Code;
            lblCustomerName.Text = customer.Name;
            CustomerId = customer.Id;

            return true;
        }

        private async Task LoadDestinationAsync(string code)
        {
            ClearStatusMessage();
            var destination = (await GetDestinationListAsync(CustomerId, code)).FirstOrDefault();

            if (destination == null)
            {
                DestinationId = 0;
                DispStatusMessage(MsgInfSaveNewData, "送付先");
                Modified = true;
                return;
            }

            BaseContext.SetFunction03Enabled(true);
            txtDestinationCode.Enabled = false;
            DestinationId = destination.Id;
            txtDestinationName.Text = destination.Name;
            mskPostalCode.Text = destination.PostalCode;
            txtAddress1.Text = destination.Address1;
            txtAddress2.Text = destination.Address2;
            txtDepartmentName.Text = destination.DepartmentName;
            txtAddressee.Text = destination.Addressee;
            cmbHonorific.Text = destination.Honorific;
            Modified = false;
        }
        #endregion

        #region WebService
        private async Task<Customer> GetCustomerAsync(string code)
            => await ServiceProxyFactory.DoAsync(async (CustomerMasterClient client) =>
            {
                if (string.IsNullOrEmpty(code)) return null;
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });
                if (result.ProcessResult.Result)
                    return result.Customers.FirstOrDefault();
                return null;
            });

        private async Task<List<Destination>> GetDestinationListAsync(int customerId, string code = null)
            => await ServiceProxyFactory.DoAsync(async (DestinationMasterClient client) =>
            {
                var option = new DestinationSearch { CompanyId = CompanyId, CustomerId = CustomerId };
                if (!string.IsNullOrEmpty(code)) option.Codes = new[] { code };
                var result = await client.GetItemsAsync(SessionKey, option);
                if (result.ProcessResult.Result)
                    return result.Destinations;
                return new List<Destination>();
            });

        private async Task<DestinationResult> SaveDestinationAsync(Destination destination)
            => await ServiceProxyFactory.DoAsync(async (DestinationMasterClient client) =>
            {
                DestinationResult result = null;
                result = await client.SaveAsync(SessionKey, destination);
                return result;
            });

        private async Task<CountResult> DeleteDestinationAsync(int Id)
            => await ServiceProxyFactory.DoAsync(async (DestinationMasterClient client) =>
            {
                CountResult result = null;
                result = await client.DeleteAsync(SessionKey, Id);
                return result;
            });

        #endregion

    }
}
