using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.CustomerFeeMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.PaymentAgencyFeeMasterService;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Common.Constants;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>登録手数料画面</summary>
    public partial class PB0505 : VOneScreenBase
    {

        private int CurrencyId { get; set; }
        private int Precision { get; set; }
        private string DisplayFormat { get; set; }
        public int CustomerId { get; set; }
        public bool PaymentAgencyFlag { get; set; }
        public bool PaymentChangeFee { get; set; }
        public bool RegFee { get; set; }
        private List<PaymentAgencyFee> AgencyCheckList { get; set; }
        private List<CustomerFee> CustomerCheckList { get; set; }


        public PB0505()
        {
            InitializeComponent();
            InitializeUserComponent();
            RegFee = true;
        }

        private void InitializeUserComponent()
        {
            FormWidth = 410;
            FormHeight = 560;
            FunctionKeysSetter = buttons =>
            {
                foreach (var button in buttons)
                {
                    if (button.Name == "btnF01")
                        button.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                    else if (button.Name == "btnF10")
                        button.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    else
                        button.Visible = false;
                }
            };

            btnCurrencyCode.Click += btnCode_Click;
            txtCurrencyCode.Validated += txtCode_Validated;
            txtCurrencyCode.TextChanged += OnContentChanged;
            grid.CellEditedFormattedValueChanged += grd_CellEditedFormattedValueChanged;
            grid.CurrentCellDirtyStateChanged += grd_CurrentCellDirtyStateChanged;

            grid.DataError += (sender, e) =>
            {
                Console.WriteLine(e);
            };
        }

        protected override void InitLayout()
        {
            base.InitLayout();
            if (ParentForm == null) return;
            ParentForm.Load += PB0504_Load;
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = btnF01_Click;

            BaseContext.SetFunction02Caption("");
            BaseContext.SetFunction02Enabled(false);
            OnF02ClickHandler = null;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = null;

            BaseContext.SetFunction10Caption("戻る");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = btnF10_Click;
        }

        #region 画面の初期化
        private void InitializeGrid()
        {
            var expression = new DataExpression(ApplicationControl);
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            var feeCell = builder.GetNumberCellCurrencyInput(Precision, Precision, 0);
            feeCell.MaxValue = expression.MaxCustomerFee;
            feeCell.MaxMinBehavior = GrapeCity.Win.Editors.MaxMinBehavior.CancelInput;
            feeCell.ValueSign = GrapeCity.Win.Editors.ValueSignControl.Positive;

            if (PaymentAgencyFlag)
            {
                builder.Items.AddRange(new CellSetting[]
                {
                    new CellSetting(height,  40, "Header"                       , caption: "No"                                                    , cell: builder.GetRowHeaderCell()),
                    new CellSetting(height, 125, "Create"                       , caption: "登録日付", dataField: nameof(PaymentAgencyFee.CreateAt), cell: builder.GetDateCell_yyyyMMdd()),
                    new CellSetting(height, 120, nameof(PaymentAgencyFee.NewFee), caption: "登録金額", dataField: nameof(PaymentAgencyFee.NewFee)  , cell: feeCell, readOnly: false),
                });
            }
            else
            {
                builder.Items.AddRange(new CellSetting[]
                {
                    new CellSetting(height,  40, "Header"                   , caption: "No"                                               , cell: builder.GetRowHeaderCell()),
                    new CellSetting(height, 125, "Create"                   , caption: "登録日付", dataField: nameof(CustomerFee.CreateAt), cell: builder.GetDateCell_yyyyMMdd()),
                    new CellSetting(height, 120, nameof(CustomerFee.NewFee) , caption: "登録金額", dataField: nameof(CustomerFee.NewFee)  , cell: feeCell, readOnly: false),
                });
            }
            grid.Template = builder.Build();
            grid.HideSelection = true;
        }
        #endregion

        #region フォームロード
        private void PB0504_Load(object sender, EventArgs e)
        {
            try
            {
                var loadTasks = new List<Task>();
                if (Company == null)
                    loadTasks.Add(LoadCompanyAsync());
                if (ApplicationControl == null)
                    loadTasks.Add(LoadApplicationControlAsync());
                loadTasks.Add(LoadControlColorAsync());
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTasks), false, SessionKey);

                InitializeGrid();

                if (!UseForeignCurrency)
                {
                    ProgressDialog.Start(ParentForm, LoadCurrencyCode(DefaultCurrencyCode), false, SessionKey);
                    txtCurrencyCode.Focus();
                }

                BaseContext.SetFunction01Enabled(RegFee);
                pnlCurrency.Visible = UseForeignCurrency;
                if (UseForeignCurrency)
                    ActiveControl = txtCurrencyCode;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region 検索ダイアログ Click Event
        private void btnCode_Click(object sender, EventArgs e)
        {
            var currency = this.ShowCurrencySearchDialog();
            if (currency != null)
            {
                txtCurrencyCode.Text = currency.Code;
                Precision = currency.Precision;
                txtCode_Validated(this, e);
                ClearStatusMessage();
                Modified = false;
            }
        }
        #endregion

        #region Validated Event
        private void txtCode_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();

                if (!string.IsNullOrEmpty(txtCurrencyCode.Text))
                    ProgressDialog.Start(ParentForm, LoadCurrencyCode(txtCurrencyCode.Text), false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region Check Code && Set Data in Grid
        /// <summary> 通貨コードを使ってCurrencyIdを取得する</summary>
        /// <param name="currencyCode">　通貨コード　</param>
        private async Task LoadCurrencyCode(string currencyCode)
        {
            Currency currency = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CurrencyMasterClient>();
                CurrenciesResult result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { currencyCode });
                if (result.ProcessResult.Result)
                    currency = result.Currencies?.FirstOrDefault();
            });

            if (currency != null)
            {
                CurrencyId = currency.Id;
                Precision = currency.Precision;
                InitializeGrid();
                await SetDataWithCurrencyId();
            }
            else
            {
                grid.Rows.Clear();
                grid.DataSource = null;
            }
        }

        /// <summary> CurrencyIdで取得したデータを設定</summary>
        private async Task SetDataWithCurrencyId()
        {
            if (PaymentAgencyFlag)
            {
                await ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<PaymentAgencyFeeMasterClient>();
                    PaymentAgencyFeesResult result = await service.GetAsync(SessionKey, CustomerId, CurrencyId);
                    {
                        if (result.ProcessResult.Result)
                        {
                            AgencyCheckList = result.PaymentAgencyFees.ToList();
                            List<PaymentAgencyFee> paymentFeeList = new List<PaymentAgencyFee>(result.PaymentAgencyFees);
                            if (paymentFeeList.Count > 0)
                            {
                                paymentFeeList.ForEach(f => f.NewFee = f.Fee);
                                Invoke(new System.Action(() =>
                                {
                                    paymentFeeList.Add(new PaymentAgencyFee { });
                                    grid.DataSource = new BindingSource(paymentFeeList, null);
                                    grid.CurrentCellBorderLine = new Line(LineStyle.None, Color.Empty);
                                    grid.CurrentRowBorderLine = new Line(LineStyle.None, Color.Empty);
                                }));
                            }
                            else
                            {
                                grid.Rows.Clear();
                                Invoke(new System.Action(() =>
                                {
                                    paymentFeeList.Add(new PaymentAgencyFee { });
                                    grid.DataSource = new BindingSource(paymentFeeList, null);
                                    grid.CurrentCellBorderLine = new Line(LineStyle.None, Color.Empty);
                                    grid.CurrentRowBorderLine = new Line(LineStyle.None, Color.Empty);
                                    grid.CurrentCell.Selected = false;
                                }));
                            }
                            Modified = false;
                        }
                    }
                });
            }
            else
            {
                await ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<CustomerFeeMasterClient>();
                    CustomerFeeResult result = await service.GetAsync(SessionKey, CustomerId, CurrencyId);
                    {
                        if (result.ProcessResult.Result)
                        {
                            CustomerCheckList = result.CustomerFee.ToList();
                            List<CustomerFee> customerFees = new List<CustomerFee>(result.CustomerFee);
                            if (customerFees.Count > 0)
                            {
                                customerFees.ForEach(f => f.NewFee = f.Fee);
                                Invoke(new System.Action(() =>
                                {
                                    customerFees.Add(new CustomerFee { });
                                    grid.DataSource = new BindingSource(customerFees, null);
                                    grid.CurrentCellBorderLine = new Line(LineStyle.None, Color.Empty);
                                    grid.CurrentRowBorderLine = new Line(LineStyle.None, Color.Empty);
                                }));
                            }
                            else
                            {
                                grid.Rows.Clear();
                                Invoke(new System.Action(() =>
                                {
                                    customerFees.Add(new CustomerFee { });
                                    grid.DataSource = new BindingSource(customerFees, null);
                                    grid.CurrentCellBorderLine = new Line(LineStyle.None, Color.Empty);
                                    grid.CurrentRowBorderLine = new Line(LineStyle.None, Color.Empty);
                                    grid.CurrentCell.Selected = false;
                                }));
                            }
                            Modified = false;
                        }
                    }
                });
            }
        }
        #endregion

        #region btnF01_Click
        [OperationLog("登録")]
        private void btnF01_Click()
        {
            if (UseForeignCurrency && string.IsNullOrEmpty(txtCurrencyCode.Text))
            {
                ShowWarningDialog(MsgWngSelectionRequired, lblCurrencyCode.Text);
                txtCurrencyCode.Focus();
                return;
            }

            try
            {
                ClearStatusMessage();

                if (PaymentAgencyFlag)
                {
                    List<PaymentAgencyFee> paymentAgencyFee = PrepareSaveGridDataForPaymentAgencyFee();

                    if (paymentAgencyFee == null) return;

                    if (!ShowConfirmDialog(MsgQstConfirmSave))
                    {
                        DispStatusMessage(MsgInfProcessCanceled);
                        return;
                    }

                    SavePaymentAgencyFee(paymentAgencyFee);
                }
                else
                {
                    List<CustomerFee> customerFee = PrepareSaveGridDataForCustomerFee();

                    if (customerFee == null) return;

                    if (!ShowConfirmDialog(MsgQstConfirmSave))
                    {
                        DispStatusMessage(MsgInfProcessCanceled);
                        return;
                    }

                    SaveCustomerFee(customerFee);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private List<PaymentAgencyFee> PrepareSaveGridDataForPaymentAgencyFee()
        {
            var source = (grid.DataSource as BindingSource)?.DataSource as List<PaymentAgencyFee> ?? null;

            if ((source != null && source[0].NewFee == null) || (source.Count == 1 && source[0].NewFee == 0))
            {
                ShowWarningDialog(MsgWngNoDataForProcess, "登録");
                return null;
            }

            var result = new List<PaymentAgencyFee>();
            var target = new List<PaymentAgencyFee>(source);
            foreach (PaymentAgencyFee paymentFee in source)
            {
                paymentFee.PaymentAgencyId = CustomerId;
                paymentFee.CurrencyId = CurrencyId;
                paymentFee.CreateBy = Login.UserId;
                paymentFee.UpdateBy = Login.UserId;
                target.Remove(paymentFee);
                if (target.Any(other => other.NewFee == paymentFee.NewFee && other.NewFee != 0))
                {
                    ShowWarningDialog(MsgWngAlreadyRegistData, paymentFee.NewFee.Value.ToString(DisplayFormat));
                    return null;
                }

                if (!(paymentFee.NewFee.HasValue))
                {
                    bool isEqual = false;
                    for (int i = 0; i < source.Count - 1; i++)
                    {
                        if (AgencyCheckList[i].Fee == source[i].NewFee)
                        {
                            isEqual = true;
                        }
                        else
                        {
                            isEqual = false;
                            break;
                        }
                    }
                    if (isEqual)
                    {
                        ShowWarningDialog(MsgWngNoDataForProcess, "登録");
                        return null;
                    }
                }
                result.Add(paymentFee);
            }

            return result;
        }

        private List<CustomerFee> PrepareSaveGridDataForCustomerFee()
        {
            var source = (grid.DataSource as BindingSource)?.DataSource as List<CustomerFee> ?? null;

            if ((source == null) || (source != null && source[0].NewFee == null) || (source.Count == 1 && source[0].NewFee == 0))
            {
                ShowWarningDialog(MsgWngNoDataForProcess, "登録");
                return null;
            }

            var result = new List<CustomerFee>();
            var target = new List<CustomerFee>(source);
            foreach (CustomerFee customerFee in source)
            {
                customerFee.CustomerId = CustomerId;
                customerFee.CurrencyId = CurrencyId;
                customerFee.CreateBy = Login.UserId;
                customerFee.UpdateBy = Login.UserId;
                target.Remove(customerFee);
                if (target.Any(other => other.NewFee == customerFee.NewFee && other.NewFee != 0))
                {
                    ShowWarningDialog(MsgWngAlreadyRegistData, customerFee.NewFee.Value.ToString(DisplayFormat));
                    return null;
                }

                if (!(customerFee.NewFee.HasValue))
                {
                    bool isEqual = false;
                    for (int i = 0; i < source.Count - 1; i++)
                    {
                        if (CustomerCheckList[i].Fee == source[i].NewFee)
                        {
                            isEqual = true;
                        }
                        else
                        {
                            isEqual = false;
                            break;
                        }
                    }
                    if (isEqual)
                    {
                        ShowWarningDialog(MsgWngNoDataForProcess, "登録");
                        return null;
                    }
                }
                result.Add(customerFee);
            }

            return result;
        }

        private void SaveCustomerFee(List<CustomerFee> fees)
        {
            var saveTask = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerFeeMasterClient>();
                CustomerFeeResult result = await service.SaveAsync(SessionKey, CustomerId, CurrencyId, fees.ToArray());
                if (result.ProcessResult.Result)
                {
                    if (result.CustomerFee != null)
                    {
                        DispStatusMessage(MsgInfSaveSuccess);

                        string currencyCode = UseForeignCurrency ? txtCurrencyCode.Text : DefaultCurrencyCode;
                        ProgressDialog.Start(ParentForm, LoadCurrencyCode(currencyCode), false, SessionKey);

                        Modified = false;
                        PaymentChangeFee = true;
                    }
                    else
                    {
                        ShowWarningDialog(MsgErrSaveError);
                    }
                }
            });
            ProgressDialog.Start(ParentForm, saveTask, false, SessionKey);
        }

        private void SavePaymentAgencyFee(List<PaymentAgencyFee> fees)
        {
            var saveTask = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<PaymentAgencyFeeMasterClient>();
                PaymentAgencyFeesResult result = await service.SaveAsync(SessionKey, CustomerId, CurrencyId, fees.ToArray());
                if (result.ProcessResult.Result)
                {
                    if (result.PaymentAgencyFees != null)
                    {
                        DispStatusMessage(MsgInfSaveSuccess);

                        string currencyCode = UseForeignCurrency ? txtCurrencyCode.Text : DefaultCurrencyCode;
                        ProgressDialog.Start(ParentForm, LoadCurrencyCode(currencyCode), false, SessionKey);

                        Modified = false;
                        PaymentChangeFee = true;
                    }
                    else
                    {
                        ShowWarningDialog(MsgErrSaveError);
                    }
                }
            });
            ProgressDialog.Start(ParentForm, saveTask, false, SessionKey);
        }
        #endregion

        #region btnF10_Click
        [OperationLog("戻る")]
        private void btnF10_Click()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose))
                return;

            ParentForm.DialogResult = DialogResult.Cancel;
        }
        #endregion

        #region その他Function
        private void OnContentChanged(object sender, EventArgs e)
        {
            if (!Modified)
                Modified = true;
        }

        private void grd_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            Modified = true;
        }

        private void grd_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            grid.CommitEdit(DataErrorContexts.Commit);
        }
        #endregion

    }
}
