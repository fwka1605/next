using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>
    /// 個別消込 前受振替データ発生時 得意先コード指定
    /// </summary>
    public partial class PE0109 : VOneScreenBase
    {
        public string CustomerCode { get; set; }
        public int CustomerId { get; set; }
        public int[] CustomerIdList { get; set; }
        public int AdvancedCustomerId { get; set; }

        public PE0109() : base()
        {
            InitializeComponent();
            InitializeUserComponent();
        }

        #region 画面初期設定
        private void InitializeUserComponent()
        {
            FormWidth = 440;
            FormHeight = 200;
            FunctionKeysSetter = buttons =>
            {
                foreach (var button in buttons)
                {
                    if (button.Name == "btnF01")
                    {
                        button.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                    }
                    else if (button.Name == "btnF10")
                    {
                        button.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    }
                    else
                    {
                        button.Visible = false;
                    }
                }
            };

        }

        protected override void InitLayout()
        {
            base.InitLayout();
            if (ParentForm == null) return;

            txtCustomerCode.Validated += (sender, e) => CustomerCodeValidated(sender, e);
            btnCustomerCode.Click += (sender, e) => CustomerCodeSearchButtonClick(sender, e);
            ParentForm.Load += (sender, e) =>
            {
                var loadTask = new List<Task> {
                    LoadApplicationControlAsync(),
                    LoadCompanyAsync(),
                    LoadControlColorAsync(),
                };
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
                SetFormatData();
            };
        }
        #endregion

        #region PE0109 InitializeFunctionKeys
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();
            BaseContext.SetFunction01Caption("消込");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = ProcessMatching;

            BaseContext.SetFunction02Caption("");
            BaseContext.SetFunction02Enabled(false);
            OnF02ClickHandler = null;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = null;

            BaseContext.SetFunction04Caption("");
            BaseContext.SetFunction04Enabled(false);
            OnF04ClickHandler = null;

            BaseContext.SetFunction05Caption("");
            BaseContext.SetFunction05Enabled(false);
            OnF05ClickHandler = null;

            BaseContext.SetFunction06Caption("");
            BaseContext.SetFunction06Enabled(false);
            OnF06ClickHandler = null;

            BaseContext.SetFunction07Caption("");
            BaseContext.SetFunction07Enabled(false);
            OnF07ClickHandler = null;

            BaseContext.SetFunction08Caption("");
            BaseContext.SetFunction08Enabled(false);
            OnF08ClickHandler = null;

            BaseContext.SetFunction09Caption("");
            BaseContext.SetFunction09Enabled(false);
            OnF09ClickHandler = null;

            BaseContext.SetFunction10Caption("キャンセル");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Exit;
        }
        #endregion

        #region F1 消込処理
        [OperationLog("消込")]
        private void ProcessMatching()
        {
            if (string.IsNullOrEmpty(txtCustomerCode.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblCustomerCode.Text);
                txtCustomerCode.Clear();
                lblCustomerName.Clear();
                txtCustomerCode.Focus();
                return;
            }
            ParentForm.DialogResult = DialogResult.OK;
        }
        #endregion

        #region F10 キャンセル処理
        [OperationLog("キャンセル")]
        private void Exit()
        {
            ParentForm.DialogResult = DialogResult.Cancel;
        }
        #endregion

        #region CustomerCodeValidatedイベント
        private void CustomerCodeValidated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                if (!string.IsNullOrEmpty(txtCustomerCode.Text))
                {
                    SetCustomerName();
                }
                else
                {
                    lblCustomerName.Clear();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region CustomerCodeSearchButtonClickイベント
        private void CustomerCodeSearchButtonClick(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var customer = this.ShowCustomerSearchDialog(loader:
                new CustomerGridLoader(ApplicationContext)
                {
                    Key = CustomerGridLoader.SearchCustomer.WithList,
                    CustomerId = CustomerIdList ?? new int[] { CustomerId },
                });

            if (customer != null)
            {
                if (customer.Code == null && customer.Name == null)
                {
                    txtCustomerCode.Clear();
                    lblCustomerName.Clear();
                }
                else
                {
                    ClearStatusMessage();
                    txtCustomerCode.Text = customer.Code;
                    lblCustomerName.Text = customer.Name;
                    AdvancedCustomerId = customer.Id;
                }
            }
        }
        #endregion

        #region CommonFunction

        private void SetFormatData()
        {
            try
            {
                var expression = new DataExpression(ApplicationControl);
                txtCustomerCode.MaxLength = expression.CustomerCodeLength;
                txtCustomerCode.Format = expression.CustomerCodeFormatString;
                txtCustomerCode.ImeMode = expression.CustomerCodeImeMode();
                txtCustomerCode.PaddingChar = expression.CustomerCodePaddingChar;

                txtCustomerCode.Text = CustomerCode;
                txtCustomerCode.Focus();

                CustomerCodeValidated(txtCustomerCode, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void SetCustomerName()
        {
            var customerCode = txtCustomerCode.Text;
            var customerName = string.Empty;

            customerName = CustomerIdList != null ? GetCustomerName(customerCode) : GetCustomerByParentId(customerCode);

            if (!string.IsNullOrEmpty(customerName))
            {
                lblCustomerName.Text = customerName;
            }
            else
            {
                ShowWarningDialog(MsgWngNotContainsCustomerGroup);
                txtCustomerCode.Clear();
                lblCustomerName.Clear();
                txtCustomerCode.Focus();
            }
        }

        /// <summary> 得意先名取得 </summary>
        /// <param name="code"> 得意先コード</param>
        /// <returns>得意先名</returns>
        private string GetCustomerName(string code)
        {
            string customerName = string.Empty;
            CustomersResult result = null;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerMasterClient>();
                result = await service.GetCustomerWithListAsync(
                    SessionKey, CompanyId, CustomerIdList.Distinct().ToArray());
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            if (result.ProcessResult.Result)
            {
                var customer = result.Customers.SingleOrDefault(x => x.Code == code);
                if (customer != null)
                {
                    customerName = customer.Name;
                    AdvancedCustomerId = customer.Id;
                }
            }
            return customerName;
        }

        /// <summary> 得意先名取得「Group」 </summary>
        /// <param name="code"> 得意先コード</param>
        /// <returns>得意先名</returns>
        private string GetCustomerByParentId(string Code)
        {
            var customerGroupName = string.Empty;
            CustomersResult result = null;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerMasterClient>();
                result = await service.GetCustomerGroupAsync(
                    SessionKey, CompanyId, CustomerId);
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            if (result.ProcessResult.Result)
            {
                var customerList = result.Customers;
                var customer = customerList.SingleOrDefault(x => x.Code == Code);
                if (customer != null)
                {
                    customerGroupName = customer.Name;
                    AdvancedCustomerId = customer.Id;
                }
            }
            return customerGroupName;
        }
        #endregion


    }
}
