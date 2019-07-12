using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.MFBillingService;
using Rac.VOne.Client.Screen.StaffMasterService;
using Rac.VOne.Client.Screen.WebApiSettingMasterService;
using Rac.VOne.Common;
using Rac.VOne.Common.Extensions;
using Rac.VOne.Web.Models.MFModels;
using Rac.VOne.Web.Models.HttpClients;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Common.Constants;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>
    /// MFクラウド請求書 請求データ抽出
    /// </summary>
    public partial class PC1801 : VOneScreenBase
    {
        #region メンバー
        private enum TaskResult
        {
            Success = 0,
            AmountOrver,
            Error
        }
        private int CurrencyId { get; set; }
        private int Precision { get; set; }
        private string CellName(string value) => $"cel{value}";
        private class FKeyNames
        {
            internal const string F01 = "抽出";
            internal const string F02 = "クリア";
            internal const string F04 = "登録";
            internal const string F05 = "取込設定";
            internal const string F07 = "連携設定";
            internal const string F08 = "全選択";
            internal const string F09 = "全解除";
            internal const string F10 = "終了";
        }
        private readonly MFWebApiClient client;
        private List<billing> MFBilling { get; set; }
        private Web.Models.WebApiMFExtractSetting ExtractSetting { get; set; }
        #endregion

        #region 初期化
        public PC1801()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.PC1801_Load);
            client = new MFWebApiClient();
            Text = "MFクラウド請求書 データ抽出";
        }

        private void PC1801_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                InitializeEventHandler();
                ProgressDialog.Start(ParentForm, InitializeLoadDataAsync(), false, SessionKey);
                InitializeGridTemplate();
                ClearMembersAndControls();
                BaseContext.SetFunction01Enabled(CheckApiToken());
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void InitializeEventHandler()
        {
            grid.CellEditedFormattedValueChanged += grid_CellEditedFormattedValueChanged;
        }

        private bool CheckApiToken()
        {
            if (client.WebApiSetting == null)
            {
                ShowWarningDialog(MsgWngNotSettingMaster, "MFクラウド請求書 WebAPI 連携設定");
                return false;
            }
            var task = client.ValidateToken();
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            if (!task.Result)
            {
                ShowWarningDialog(MsgWngExpiredToken);
                return false;
            }
            else
            {
                if (client.TokenRefreshed)
                {
                    ProgressDialog.Start(ParentForm, SaveWebApiSettingAsync(client.WebApiSetting), false, SessionKey);
                    client.TokenRefreshed = false;
                }
            }
            return true;
        }

        private async Task InitializeLoadDataAsync()
        {
            var loadDefaultCurrencyTask = GetCurrencyAsync(DefaultCurrencyCode);
            var tasks = new List<Task> {
                LoadCompanyAsync(),
                LoadApplicationControlAsync(),
                LoadControlColorAsync(),
                loadDefaultCurrencyTask,
                LoadWebApiSettingAsync(),
            };
            await Task.WhenAll(tasks);
            var currency = loadDefaultCurrencyTask.Result;
            if (currency != null)
            {
                CurrencyId = currency.Id;
                Precision = currency.Precision;
            }
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption(FKeyNames.F01);
            BaseContext.SetFunction02Caption(FKeyNames.F02);
            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction04Caption(FKeyNames.F04);
            BaseContext.SetFunction05Caption(FKeyNames.F05);
            BaseContext.SetFunction06Caption("");
            BaseContext.SetFunction07Caption(FKeyNames.F07);
            BaseContext.SetFunction08Caption(FKeyNames.F08);
            BaseContext.SetFunction09Caption(FKeyNames.F09);
            BaseContext.SetFunction10Caption(FKeyNames.F10);

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Enabled(true);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(true);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);

            OnF01ClickHandler = Extract;
            OnF02ClickHandler = Clear;
            OnF04ClickHandler = Registration;
            OnF05ClickHandler = CallExportFieldSetting;
            OnF07ClickHandler = ShowWebApiSetting;
            OnF08ClickHandler = SelectAll;
            OnF09ClickHandler = DeselectAll;
            OnF10ClickHandler = Close;

        }

        private void InitializeGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
         
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  50, nameof(billing.Selected)         , dataField: nameof(billing.Selected)         , caption: "選択   "        , cell: builder.GetCheckBoxCell(isBoolType:true), sortable:true, readOnly: false),
                new CellSetting(height, 207, nameof(billing.GetCustomerName)  , dataField: nameof(billing.GetCustomerName)  , caption: "得意先名"    , cell: builder.GetTextBoxCell(), sortable:true),
                new CellSetting(height,  80, nameof(billing.billing_date)     , dataField: nameof(billing.billing_date)     , caption: "請求日"       , cell: builder.GetDateCell_yyyyMMdd(MultiRowContentAlignment.MiddleCenter), sortable:true),
                new CellSetting(height,  80, nameof(billing.due_date)         , dataField: nameof(billing.due_date)         , caption: "支払期限 "     , cell: builder.GetDateCell_yyyyMMdd(MultiRowContentAlignment.MiddleCenter), sortable:true),
                new CellSetting(height,  90, nameof(billing.billing_number)   , dataField: nameof(billing.billing_number)   , caption: "請求書番号 "   , cell: builder.GetTextBoxCell(), sortable:true),
                new CellSetting(height, 100, nameof(billing.total_price)      , dataField: nameof(billing.total_price)      , caption: "請求金額"     , cell: builder.GetTextBoxCurrencyCell(Precision), sortable:true),
                new CellSetting(height, 140, nameof(billing.title)            , dataField: nameof(billing.title)            , caption: "件名"        , cell: builder.GetTextBoxCell(), sortable:true),
                new CellSetting(height,  70, nameof(billing.GetPosting)       , dataField: nameof(billing.GetPosting)       , caption: "郵送"         , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), sortable:true),
                new CellSetting(height,  70, nameof(billing.GetEmail)         , dataField: nameof(billing.GetEmail)         , caption: "メール送信  "   , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), sortable:true),
                new CellSetting(height,  70, nameof(billing.GetDownload)      , dataField: nameof(billing.GetDownload)      , caption: "ダウンロード   " , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), sortable:true),
                new CellSetting(height,  207, nameof(billing.partner_name)     , dataField: nameof(billing.partner_name)     , caption: "MFクラウド請求書 取引先名" , cell: builder.GetTextBoxCell(), sortable:true),
            });

            grid.Template = builder.Build();
            grid.HideSelection = true;
            grid.FreezeLeftCellName = CellName(nameof(billing.Selected));
            grid.AllowUserToResize = true;
            grid.SetupShortcutKeys();
            grid.AllowAutoExtend = false;
        }
        #endregion

        #region ファンクションキーイベント
        #region F1/抽出
        [OperationLog(FKeyNames.F01)]
        private void Extract()
        {
            try
            {
                ClearStatusMessage();

                if (!CheckForExtract()) return;

                var task = ExtractBilling(datBilledAtFrom.Value, datBilledAtTo.Value);
                var result = ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (task.Result == TaskResult.Error || MFBilling == null)
                {
                    ShowWarningDialog(MsgErrSomethingError, "抽出");
                    return;
                }

                if (MFBilling.Count < 1)
                {
                    DisplayExtractCount();
                    ShowWarningDialog(MsgWngNotExistSearchData);
                }
                else
                {
                    grid.DataSource = new BindingSource(MFBilling, null);
                    DisplayExtractCount();
                    BaseContext.SetFunction01Enabled(false);
                    BaseContext.SetFunction04Enabled(true);
                    BaseContext.SetFunction05Enabled(false);
                    BaseContext.SetFunction07Enabled(false);
                    BaseContext.SetFunction08Enabled(true);
                    BaseContext.SetFunction09Enabled(true);
                    if (task.Result == TaskResult.Success)
                    {
                        ShowWarningDialog(MsgInfDataExtracted);
                    }
                    else if (task.Result == TaskResult.AmountOrver)
                    {
                        ShowWarningDialog(MsgWngOverAmount);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrSomethingError, "抽出");
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private bool CheckForExtract()
        {
            if (!datBilledAtFrom.Value.HasValue)
            {
                ShowWarningDialog(MsgWngInputRequired, lblBilledAt.Text);
                datBilledAtFrom.Select();
                return false;
            }
            if (!datBilledAtTo.Value.HasValue)
            {
                ShowWarningDialog(MsgWngInputRequired, lblBilledAt.Text);
                datBilledAtTo.Select();
                return false;
            }
            return true;
        }

        private async Task<TaskResult> ExtractBilling(DateTime? billedAtFrom, DateTime? billedAtTo)
        {
            try
            {
                var sources = await client.GetBillingAsync(billedAtFrom, billedAtTo);
                if (client.TokenRefreshed)
                {
                    await SaveWebApiSettingAsync(client.WebApiSetting);
                    client.TokenRefreshed = false;
                }
                var registered = await GetMFBillings();
                MFBilling = new List<billing>();
                var amountOrver = false;
                foreach (var billing in sources)
                {
                    if (billing.total_price > 99999999999M)
                    {
                        amountOrver = true;
                        continue;
                    }
                    if (billing.total_price != 0M
                        && !registered.Any(x => x.Id == billing.id))
                    {
                        billing.CustomerCode = billing.partner_id.Substring(0, ApplicationControl.CustomerCodeLength).ToUpper();
                        billing.Customer = GetCustomerByCode(billing.CustomerCode);
                        MFBilling.Add(billing);
                    }
                }
                if (amountOrver) return TaskResult.AmountOrver;
                return TaskResult.Success;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                return TaskResult.Error;
            }
        }

        private void DisplayExtractCount()
        {
            
            var selected = MFBilling.Where(x => x.Selected);
            txtSelectedCount.Text = $"{selected.Count().ToString("#,##0")} / {MFBilling.Count.ToString("#,##0")}";
            lblExtractAmount.Text = selected.Select(x => x.total_price).Sum().ToString("#,##0");
        }

        #endregion

        #region F2/クリア
        [OperationLog(FKeyNames.F02)]
        private void Clear()
        {
            ClearStatusMessage();
            ClearMembersAndControls();
            grid.DataSource = null;
            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);
            BaseContext.SetFunction05Enabled(true);
            BaseContext.SetFunction07Enabled(true);
        }
        private void ClearMembersAndControls()
        {
            SetDefaultValues();
            lblExtractAmount.Clear();
            txtSelectedCount.Clear();
            grid.Enabled = true;
            MFBilling = null;
        }
        private void SetDefaultValues()
        {
            var now = DateTime.Now;
            var first = new DateTime(now.Year, now.Month, 1);
            var last = first.AddMonths(1).AddDays(-1);
            datBilledAtFrom.Value = first;
            datBilledAtTo.Value = last;
        }

        #endregion

        #region F4/登録
        [OperationLog(FKeyNames.F04)]
        private void Registration()
        {
            try
            {
                ClearStatusMessage();
                if (!CheckForUpdate()) return;

                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var task = CreateBillings();
                var result = ProgressDialog.Start(ParentForm, task, false, SessionKey);
                if (task.Result)
                {
                    ShowWarningDialog(MsgInfSaveSuccess);
                    grid.Enabled = false;
                    BaseContext.SetFunction04Enabled(false);
                    BaseContext.SetFunction08Enabled(false);
                    BaseContext.SetFunction09Enabled(false);
                }
                else
                {
                    ShowWarningDialog(MsgErrSomethingError, "登録");
                }
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrSomethingError, "登録");
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private bool CheckForUpdate()
        {
            if (!CheckFixedValues()) return false;

            if (!MFBilling.Any(x => x.Selected))
            {
                ShowWarningDialog(MsgWngNotExistUpdateData, "更新を行うデータ");
                return false;
            }
            return true;
        }

        private bool CheckFixedValues()
        {
            ProgressDialog.Start(ParentForm, LoadWebApiSettingAsync(), false, SessionKey);
            if (ExtractSetting == null
                || !ExtractSetting.BillingCategoryId.HasValue
                || !ExtractSetting.StaffId.HasValue
                || !ExtractSetting.CollectCategoryId.HasValue
                || !ExtractSetting.ClosingDay.HasValue
                || !ExtractSetting.CollectOffsetMonth.HasValue
                || !ExtractSetting.CollectOffsetDay.HasValue)
            {
                ShowWarningDialog(MsgErrNoSettingError);
                return false;
            }

            if (GetCategory(ExtractSetting.BillingCategoryId.Value) == null)
            {
                ShowWarningDialog(MsgWngNotExistsMaster, "請求区分","区分");
                return false;
            }

            if (GetStaff(ExtractSetting.StaffId.Value) == null)
            {
                ShowWarningDialog(MsgWngNotExistsMaster, "営業担当者", "営業担当者");
                return false;
            }

            if (GetCategory(ExtractSetting.CollectCategoryId.Value) == null)
            {
                ShowWarningDialog(MsgWngNotExistsMaster, "回収区分", "区分");
                return false;
            }


            return true;
        }

        private async Task<bool> CreateBillings()
        {
            try
            {
                var targetBilling = new List<Web.Models.Billing>();
                var newCustomer = new List<Web.Models.Customer>();
                var targetCount = 0;
                var legalPersonalities = await Util.GetLegalPersonaritiesAsync(Login);
                foreach (var mfBilling in MFBilling.Where(x => x.Selected))
                {
                    targetCount++;

                    var departmentId = 0;
                    var partner = await client.GetPartnersAsync(mfBilling.partner_id);

                    if (mfBilling.Customer == null)
                    {
                        var newCusotmer = mfBilling.BuildNewCustomer(
                            ExtractSetting,
                            partner,
                            CompanyId,
                            Login.UserId,
                            legalPersonalities);

                        newCustomer.Add(newCusotmer);
                    }
                    else
                    {
                        departmentId = GetStaff(mfBilling.Customer.StaffId).DepartmentId;
                    }

                    var department = partner == null
                        ? null
                        : partner.departments.Where(x => x.id == mfBilling.department_id).FirstOrDefault();

                    var voneBilling = mfBilling.CreateVOneBilling(
                        departmentId,
                        CompanyId,
                        Login.UserId,
                        CurrencyId,
                        ExtractSetting,
                        department);

                    targetBilling.Add(voneBilling);
                }
                var result = await SaveMFBillings(targetBilling.ToArray(), newCustomer.ToArray());
                var isSuccess = targetCount == result.Count;
                return isSuccess;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                return false;
            }
        }
        #endregion

        #region F5/設定
        [OperationLog(FKeyNames.F05)]
        private void CallExportFieldSetting()
        {
            var form = new dlgMFBillingExtractSetting();
            form.ApplicationContext = ApplicationContext;
            form.ApplicationControl = ApplicationControl;
            form.CompanyInfo = Company;
            form.StartPosition = FormStartPosition.CenterScreen;
            ApplicationContext.ShowDialog(this, form);
        }
        #endregion

        #region F7/連携設定
        private void ShowWebApiSetting()
        {
            ClearStatusMessage();
            var settingSaveResult = DialogResult.Cancel;
            using (var form = ApplicationContext.Create(nameof(PH1401)))
            {
                var screen = form.GetAll<PH1401>().First();
                settingSaveResult = ApplicationContext.ShowDialog(ParentForm, form);
            }
            ProgressDialog.Start(ParentForm, LoadWebApiSettingAsync(), false, SessionKey);
            BaseContext.SetFunction01Enabled(CheckApiToken());
        }
        #endregion

        #region F8/全選択＆F9全解除
        [OperationLog(FKeyNames.F08)]
        private void SelectAll()
        {
            grid.EndEdit();
            grid.Focus();
            foreach (var row in grid.Rows)
            {
                var billing = row.DataBoundItem as billing;
                if (row.Cells[CellName(nameof(billing.Selected))].Enabled)
                {
                    billing.Selected = true;
                }
            }
            grid.EndEdit();
            SetF04Enable();
            DisplayExtractCount();
        }

        [OperationLog(FKeyNames.F09)]
        private void DeselectAll()
        {
            grid.EndEdit();
            grid.Focus();
            foreach (var row in grid.Rows)
            {
                var billing = row.DataBoundItem as billing;
                if (row.Cells[CellName(nameof(billing.Selected))].Enabled)
                {
                    billing.Selected = false;
                }
            }
            grid.EndEdit();
            SetF04Enable();
            DisplayExtractCount();
        }
        #endregion

        [OperationLog(FKeyNames.F10)]
        private void Close()
        {
            BaseForm.Close();
            return;
        }
        #endregion

        #region WebService
        private async Task<Web.Models.Currency> GetCurrencyAsync(string code)
        {
            var result = await ServiceProxyFactory.DoAsync(async (CurrencyMasterClient client)
                => await client.GetByCodeAsync(SessionKey, CompanyId, new string[] { code }));
            if (result.ProcessResult.Result)
                return result.Currencies.FirstOrDefault();
            return null;
        }
        private async Task LoadWebApiSettingAsync()
           => await ServiceProxyFactory.DoAsync(async (WebApiSettingMasterClient client) => {
               var result = await client.GetByIdAsync(SessionKey, CompanyId, WebApiType.MoneyForward);

               if (!(result?.ProcessResult.Result ?? false) || result.WebApiSetting == null)
               {
                   this.client.WebApiSetting = null;
                   return;
               }
               else
               {
                   this.client.WebApiSetting = result.WebApiSetting;
                   ExtractSetting = this.client.WebApiSetting.ExtractSetting.ConvertToModel<Web.Models.WebApiMFExtractSetting>();
               }
           });

        private async Task<List<Web.Models.MFBilling>> GetMFBillings() =>
         await ServiceProxyFactory.DoAsync(async (MFBillingServiceClient client) =>
         {
             var result = await client.GetItemsAsync(SessionKey, Login.CompanyId);
             if (result.ProcessResult.Result)
                 return result.MFBillings;
             return null;
         });

        private Web.Models.Category GetCategory(int Id)
       => ServiceProxyFactory.Do((CategoryMasterClient client) =>
       {
           var result = client.Get(SessionKey, new int[] { Id });
           if (result == null || result.ProcessResult.Result == false) return null;
           return result.Categories.FirstOrDefault();
       });

        private Web.Models.Staff GetStaff(int Id)
        => ServiceProxyFactory.Do((StaffMasterClient client) =>
        {
            var result = client.Get(SessionKey, new int[] { Id });
            if (result == null || result.ProcessResult.Result == false) return null;
            return result.Staffs.FirstOrDefault();
        });

        private Web.Models.Customer GetCustomerByCode(string code)
       => ServiceProxyFactory.Do((CustomerMasterClient client) =>
       {
           var result = client.GetByCode(SessionKey,Login.CompanyId, new string[] { code });
           if (result == null || result.ProcessResult.Result == false) return null;
           return result.Customers.FirstOrDefault();
       });

        private async Task<List<Web.Models.Billing>> SaveMFBillings(Web.Models.Billing[] billings, Web.Models.Customer[] customers) =>
                await ServiceProxyFactory.DoAsync(async ( MFBillingServiceClient client) =>
                {
                    var result = await client.SavingSetAsync(Login.SessionKey, billings, customers);
                    if (result.ProcessResult.Result)
                        return result.Billings;
                    return null;

                });

        private async Task<bool> SaveWebApiSettingAsync(Web.Models.WebApiSetting setting)
       => ((await ServiceProxyFactory.DoAsync(async (WebApiSettingMasterClient client)
       => await client.SaveAsync(SessionKey, setting)))
       ?.ProcessResult.Result ?? false);

        #endregion

        #region イベントハンドラー
        private void grid_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            if (e.RowIndex < 0) return;
            grid.EndEdit();
            if (e.CellName == CellName(nameof(billing.Selected)))
            {
                DisplayExtractCount();
                SetF04Enable();
            }
        }
        private void SetF04Enable()
        {
            var enable = grid.Rows.Any(x => Convert.ToBoolean(x[CellName(nameof(billing.Selected))].EditedFormattedValue));
            BaseContext.SetFunction04Enabled(enable);
        }
        #endregion
    }
}