using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Rac.VOne.Common;
using Rac.VOne.Common.Extensions;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.StaffMasterService;
using Rac.VOne.Client.Screen.WebApiSettingMasterService;
using static Rac.VOne.Message.Constants;
using GrapeCity.Win.Editors;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public partial class dlgMFBillingExtractSetting : Dialog
    {
        #region メンバー
      
        private List<Category> CollectCategoryList { get; set; }
        private string CategoryIndex { get; set; }
        private WebApiMFExtractSetting Setting { get; set; }
        /// <summary>編集状態管理用</summary>
        protected internal bool Modified { get; set; }
        private string selectedBillingCategoryCode { get; set; }
        private string selectedstaffCode { get; set; }

        #endregion

        #region 初期化処理
        public dlgMFBillingExtractSetting()
        {
            InitializeComponent();
        }
        private void dlgPublishInvoices_Load(object sender, EventArgs e)
        {
            try
            {
                InitilaizeControls();
                AddHandlers();
                var tasks = new List<Task>();
                tasks.Add(LoadCollectCategoryInfoAsync());
                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);

                SetComboForCollectCategoryId();
                SetControlValues();
                txtBillingCategory.Select();
                Modified = false;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void InitilaizeControls()
        {
            statusStrip.Visible = true;

            var expression = new DataExpression(ApplicationControl);
            txtStaffCode.Format = expression.StaffCodeFormatString;
            txtStaffCode.MaxLength = expression.StaffCodeLength;
            txtStaffCode.PaddingChar = expression.StaffCodePaddingChar;
        }

        private void SetControlValues()
        {
            Setting = GetWebApiSetting().ExtractSetting.ConvertToModel<WebApiMFExtractSetting>();

            if (Setting == null)
            {
                Setting = new WebApiMFExtractSetting();
                return;
            }

            if (Setting.BillingCategoryId.HasValue)
            {
                var billingCategory = GetCategory(Setting.BillingCategoryId.Value);
                if (billingCategory != null)
                {
                    txtBillingCategory.Text = billingCategory.Code;
                    selectedBillingCategoryCode = billingCategory.Code;
                    lblBillingCategoryName.Text = billingCategory.Name;
                }
            }
            if (Setting.StaffId.HasValue)
            {
                var staff = GetStaff(Setting.StaffId.Value);
                if (staff != null)
                {
                    txtStaffCode.Text = staff.Code;
                    selectedstaffCode = staff.Code;
                    lblStaffName.Text = staff.Name;
                }
            }

            txtClosingDay.Text = Setting.ClosingDay.ToString().PadLeft(2, '0');
            if (txtClosingDay.Text == "00")
            {
                cbxIssueBillEachTime.Checked = true;
            }
            txtCollectOffsetMonth.Text = Setting.CollectOffsetMonth.ToString().PadLeft(2, '0');
            txtCollectOffsetDay.Text = Setting.CollectOffsetDay.ToString().PadLeft(2, '0');


            if (Setting.CollectCategoryId.HasValue)
            {
                for (int i = 0; i < cmbCollectCategoryId.Items.Count; i++)
                {
                    var selected = cmbCollectCategoryId.Items[i].Tag as Category;
                    if (selected.Id == Setting.CollectCategoryId)
                    {
                        cmbCollectCategoryId.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void SetComboForCollectCategoryId()
        {
            if (CollectCategoryList != null)
            {
                for (int i = 0; i < CollectCategoryList.Count; i++)
                {
                    int index = cmbCollectCategoryId.Items.Add(new ListItem(CollectCategoryList[i].Code.ToString() + ":" + CollectCategoryList[i].Name.ToString(), CollectCategoryList[i].Id));
                    cmbCollectCategoryId.Items[i].Tag = CollectCategoryList[i];
                }
            }
        }

        private void AddHandlers()
        {
            foreach (Control control in gbxBilling.Controls)
            {
                if (control is Common.Controls.VOneTextControl)
                {
                    control.TextChanged += new EventHandler(OnContentChanged);
                }
            }
            foreach (Control control in gbxCustomer.Controls)
            {
                if (control is Common.Controls.VOneTextControl)
                {
                    control.TextChanged += new EventHandler(OnContentChanged);
                }
            }
        }
        #endregion

        #region ファンクションキー押下処理

        #region F01/登録
        [OperationLog("登録")]
        private void btnF03_Click(object sender, EventArgs e)
        {
            try
            {
                this.FunctionCalled(10, btnF01);

                if (!CheckData()) return;

                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                Setting.ClosingDay = GetNullableInteger(txtClosingDay.Text);
                Setting.CollectOffsetMonth = cbxIssueBillEachTime.Checked ? 0 : GetNullableInteger(txtCollectOffsetMonth.Text);
                Setting.CollectOffsetDay = GetNullableInteger(txtCollectOffsetDay.Text);

                var apiSetting = GetWebApiSetting();
                apiSetting.ExtractSetting = Setting.ConvertToJson();

                Task<bool> task = SaveAsync(apiSetting);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                if (task.Result)
                {
                    DispStatusMessage(MsgInfSaveSuccess);
                    Modified = false;
                }
                else
                {
                    ShowWarningDialog(MsgErrSaveError);
                }
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrSaveError);
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private bool CheckData()
        {
            if (string.IsNullOrWhiteSpace(txtBillingCategory.Text))
            {
                selectedBillingCategoryCode = "";
                ShowWarningDialog(MsgWngInputRequired, "請求区分");
                txtBillingCategory.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtStaffCode.Text))
            {
                selectedstaffCode = "";
                ShowWarningDialog(MsgWngInputRequired, "営業担当者");
                txtStaffCode.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtClosingDay.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "締日");
                txtClosingDay.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtCollectOffsetMonth.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "回収予定（月、日）");
                txtCollectOffsetMonth.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtCollectOffsetDay.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "回収予定（月、日）");
                txtCollectOffsetDay.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(cmbCollectCategoryId.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "回収方法");
                cmbCollectCategoryId.Focus();
                return false;
            }
            ClearStatusMessage();
            return true;
        }
        #endregion

        #region F10/戻る
        [OperationLog("戻る")]
        private void btnF10_Click(object sender, EventArgs e)
        {
            this.FunctionCalled(10, btnF10);
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose))
                return;
            Close();
        }
        #endregion
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F10: btnF10.PerformClick(); return true;
                case Keys.F1: btnF01.PerformClick(); return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        #region イベントハンドラー
        private void txtBillingCategory_Validated(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtBillingCategory.Text))
                {
                    ClearStatusMessage();
                    lblBillingCategoryName.Clear();
                    Setting.BillingCategoryId = null;
                    return;
                }
                if (selectedBillingCategoryCode == txtBillingCategory.Text) return;

                var categoryResult = new Category();
                var task = ServiceProxyFactory.DoAsync(async (CategoryMasterClient client) =>
                {
                    var result = await client.GetByCodeAsync(
                        SessionKey,
                        CompanyId, CategoryType.Billing,
                        new[] { txtBillingCategory.Text });
                    if (result.ProcessResult.Result)
                    {
                        categoryResult = result.Categories.FirstOrDefault();
                        if (categoryResult != null)
                        {
                            txtBillingCategory.Text = categoryResult.Code;
                            selectedBillingCategoryCode = categoryResult.Code;
                            lblBillingCategoryName.Text = categoryResult.Name;
                            Setting.BillingCategoryId = categoryResult.Id;
                            ClearStatusMessage();
                        }
                    }
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (categoryResult == null)
                {
                    ShowWarningDialog(MsgWngMasterNotExist, "請求区分", txtBillingCategory.Text);
                    txtBillingCategory.Clear();
                    selectedBillingCategoryCode = "";
                    lblBillingCategoryName.Clear();
                    Setting.BillingCategoryId = null;
                    txtBillingCategory.Focus();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private void btnBillingCategory_Click(object sender, EventArgs e)
        {
            var billingCategory = this.ShowBillingCategorySearchDialog();
            if (billingCategory != null)
            {
                txtBillingCategory.Text = billingCategory.Code;
                selectedBillingCategoryCode = billingCategory.Code;
                lblBillingCategoryName.Text = billingCategory.Name;
                Setting.BillingCategoryId = billingCategory.Id;
                ClearStatusMessage();
            }
        }
        private void txtStaffCode_Validated(object sender, EventArgs e)
        {
            try
            {
                if (txtStaffCode.Text == "")
                {
                    selectedstaffCode = "";
                    lblStaffName.Clear();
                    ClearStatusMessage();
                    Setting.StaffId = null;
                    return;
                }
                if (selectedstaffCode == txtStaffCode.Text) return;
                SetStaffCodeVal();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private void SetStaffCodeVal()
        {
            Staff staffResult = null;
            string staffCode = txtStaffCode.Text;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<StaffMasterClient>();
                StaffsResult result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { staffCode });

                if (result.ProcessResult.Result)
                {
                    staffResult = result.Staffs.FirstOrDefault();
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (staffResult != null)
            {
                lblStaffName.Text = staffResult.Name;
                Setting.StaffId = staffResult.Id;
                ClearStatusMessage();
            }
            else
            {
                ClearStatusMessage();
                ShowWarningDialog(MsgWngMasterNotExist, "営業担当者", txtStaffCode.Text);
                lblStaffName.Clear();
                txtStaffCode.Clear();
                selectedstaffCode = "";
                txtStaffCode.Focus();
                Setting.StaffId = null;
            }
        }
        private void btnStaffCode_Click(object sender, EventArgs e)
        {
            var staff = this.ShowStaffSearchDialog();
            if (staff != null)
            {
                txtStaffCode.Text = staff.Code;
                selectedstaffCode = staff.Code;
                lblStaffName.Text = staff.Name;
                Setting.StaffId = staff.Id;
                ClearStatusMessage();
            }
        }
        private void txtCloseDay_Validated(object sender, EventArgs e)
        {
            string closingDay = txtClosingDay.Text;
            txtClosingDay.Text = GetFormattedClosingDay(closingDay);

            // 00入力時は「都度請求」にチェックする
            if (txtClosingDay.Text == "00")
            {
                cbxIssueBillEachTime.Checked = true;
            }
        }
        private string GetFormattedClosingDay(string dayString, bool adjust99 = true)
        {
            if (string.IsNullOrWhiteSpace(dayString))
            {
                return null;
            }

            var day = int.Parse(dayString);

            if (adjust99 && 28 <= day)
            {
                day = 99;
            }

            return day.ToString("00");
        }
        private void txtCollectOffsetDay_Validated(object sender, EventArgs e)
        {
            string collectOffsetDay = txtCollectOffsetDay.Text;
            txtCollectOffsetDay.Text = GetFormattedClosingDay(collectOffsetDay, !cbxIssueBillEachTime.Checked);
        }
        private void txtCollectOffsetDay_TextChanged(object sender, EventArgs e)
        {
            var control = sender as Common.Controls.VOneTextControl;

            if (cbxIssueBillEachTime.Checked && control == txtCollectOffsetDay)
                return; // 都度請求時は0も入力可とする

            int value;
            if (!int.TryParse(control.Text, out value) || (value == 0))
                control.Clear();
        }
        private void cbxIssueBillEachTime_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxIssueBillEachTime.Checked)
            {
                txtClosingDay.Enabled = false;
                txtClosingDay.Text = "00";
                txtCollectOffsetMonth.Visible = false;
                txtCollectOffsetMonth.Text = "0";
                lblAfterMonth.Text = "請求日後";
                txtCollectOffsetDay.Clear();
                lblDay.Text = "日以内";
                lblLimitDay.Visible = false;
            }
            else
            {
                txtClosingDay.Enabled = true;
                txtClosingDay.Clear();
                txtCollectOffsetMonth.Visible = true;
                txtCollectOffsetMonth.Clear();
                lblAfterMonth.Text = "ヶ月後の";
                txtCollectOffsetDay.Clear();
                lblDay.Text = "日";
                lblLimitDay.Visible = true;
            }
        }
        private int? GetNullableInteger(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            else
            {
                return Convert.ToInt32(value);
            }
        }
        private void cmbCollectCategoryId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCollectCategoryId.SelectedIndex != -1)
            {
                var selected = cmbCollectCategoryId.Items[cmbCollectCategoryId.SelectedIndex].Tag as Category;
                Setting.CollectCategoryId = selected.Id;
            }
            else
            {
                Setting.CollectCategoryId = null;
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

        #region WebService
        private async Task LoadCollectCategoryInfoAsync()
        {
            var option = new CategorySearch();
            option.CompanyId = CompanyId;
            option.CategoryType = CategoryType.Collect;

            await ServiceProxyFactory.DoAsync(async (CategoryMasterClient client) =>
            {
                var result = await client.GetItemsAsync(SessionKey, option);
                if (result.ProcessResult.Result)
                {
                    CollectCategoryList = result.Categories;
                }
            });
        }

        private WebApiSetting GetWebApiSetting()
           => ServiceProxyFactory.Do((WebApiSettingMasterClient client) =>
           {
               var result = client.GetByIdAsync(SessionKey, CompanyId, WebApiType.MoneyForward);
               if (result == null || result.Result.ProcessResult.Result == false) return null;
               return result.Result.WebApiSetting;
           });

        private async Task<bool> SaveAsync(WebApiSetting setting)
           => ((await ServiceProxyFactory.DoAsync(async (WebApiSettingMasterClient client)
               => await client.SaveAsync(SessionKey, setting)))
           ?.ProcessResult.Result ?? false);

        private Category GetCategory(int Id)
         => ServiceProxyFactory.Do((CategoryMasterClient client) =>
         {
             var result = client.Get(SessionKey, new int[] { Id });
             if (result == null || result.ProcessResult.Result == false) return null;
             return result.Categories.FirstOrDefault();
         });

        private Staff GetStaff(int Id)
        => ServiceProxyFactory.Do((StaffMasterClient client) =>
        {
            var result = client.Get(SessionKey, new int[] { Id });
            if (result == null || result.ProcessResult.Result == false) return null;
            return result.Staffs.FirstOrDefault();
        });
        #endregion
    }
}
