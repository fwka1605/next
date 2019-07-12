using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.BillingService;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Screen.Dialogs;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>歩引額調整</summary>
    public partial class PC0302 : VOneScreenBase
    {
        public long BillingId;
        public decimal Discount1 { get; set; }
        public decimal Discount2 { get; set; }
        public decimal Discount3 { get; set; }
        public decimal Discount4 { get; set; }
        public decimal Discount5 { get; set; }
        public decimal TotalDiscount;
        public decimal BillingAmount;
        public int Precision;

        private bool Edited { get; set; }

        public PC0302()
        {
            InitializeComponent();
            InitializeUserComponent();
        }

        #region 画面初期設定
        private void InitializeUserComponent()
        {
            FormWidth = 340;
            FormHeight = 340;
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

            this.Load += (s, e) =>
            {
                var loadTask = new List<Task>();
                if (ApplicationControl == null)
                {
                    loadTask.Add(LoadApplicationControlAsync());
                }
                if (Company == null)
                {
                    loadTask.Add(LoadCompanyAsync());
                }
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                if (UseForeignCurrency)
                    GetNumberCurrencyFormat();
                else
                    GetNumberFormat();

                LoadBillingDiscountData();
            };

        }

        protected override void InitLayout()
        {
            base.InitLayout();
            if (ParentForm == null) return;

            nmbDiscountAmount1.ValueChanged += (sender, e) => DiscountAmount1_ValueChanged(sender, e);
            nmbDiscountAmount2.ValueChanged += (sender, e) => DiscountAmount2_ValueChanged(sender, e);
            nmbDiscountAmount3.ValueChanged += (sender, e) => DiscountAmount3_ValueChanged(sender, e);
            nmbDiscountAmount4.ValueChanged += (sender, e) => DiscountAmount4_ValueChanged(sender, e);
            nmbDiscountAmount5.ValueChanged += (sender, e) => DiscountAmount5_ValueChanged(sender, e);
        }
        #endregion

        #region PC0302 InitializeFunctionKeys
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();
            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Save;

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

        #region F1 登録処理
        private void Save()
        {
            ClearStatusMessage();
            if (TotalDiscount > BillingAmount)
            {
                ShowWarningDialog(MsgWngDiscountValueViolation);
                nmbDiscountAmount1.Focus();
                return;
            }

            if (!ShowConfirmDialog(MsgQstConfirmSave)) return;

            try
            {
                SaveDiscount();
                ParentForm.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void SaveDiscount()
        {
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BillingServiceClient>();

                BillingDiscount billDiscount = new BillingDiscount();
                billDiscount.BillingId = BillingId;
                billDiscount.DiscountAmount1 = Discount1;
                billDiscount.DiscountAmount2 = Discount2;
                billDiscount.DiscountAmount3 = Discount3;
                billDiscount.DiscountAmount4 = Discount4;
                billDiscount.DiscountAmount5 = Discount5;

                var result = await service.SaveDiscountAsync(SessionKey, billDiscount);
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
        }

        #endregion

        #region F10 キャンセル処理
        [OperationLog("キャンセル")]
        private void Exit()
        {
            if (Edited && !ShowConfirmDialog(MsgQstConfirmClose)) return;

            ParentForm.DialogResult = DialogResult.Cancel;
        }
        #endregion

        #region CommonFunction

        private void GetNumberFormat(string fieldFormat = "###,###,###,###", string displayFormat = "###,###,###,##0")
        {
            nmbDiscountAmount1.Fields.SetFields(fieldFormat, "", "", "-", "");
            nmbDiscountAmount1.DisplayFields.AddRange(displayFormat, "", "", "-", "");
            nmbDiscountAmount2.Fields.SetFields(fieldFormat, "", "", "-", "");
            nmbDiscountAmount2.DisplayFields.AddRange(displayFormat, "", "", "-", "");
            nmbDiscountAmount3.Fields.SetFields(fieldFormat, "", "", "-", "");
            nmbDiscountAmount3.DisplayFields.AddRange(displayFormat, "", "", "-", "");
            nmbDiscountAmount4.Fields.SetFields(fieldFormat, "", "", "-", "");
            nmbDiscountAmount4.DisplayFields.AddRange(displayFormat, "", "", "-", "");
            nmbDiscountAmount5.Fields.SetFields(fieldFormat, "", "", "-", "");
            nmbDiscountAmount5.DisplayFields.AddRange(displayFormat, "", "", "-", "");
            nmbBillingAmount.Fields.SetFields(fieldFormat, "", "", "-", "");
            nmbBillingAmount.DisplayFields.AddRange(displayFormat, "", "", "-", "");
        }

        private void GetNumberCurrencyFormat(string displayFormatString = "0")
        {
            var fieldString = "##,###,###,##0";
            if (Precision > 0)
            {
                fieldString += ".";
                for (int i = 0; i < Precision; i++)
                {
                    fieldString += "#";
                }
            }

            var displayFieldString = "##,###,###,##0";
            if (Precision > 0)
            {
                displayFieldString += ".";
                for (int i = 0; i < Precision; i++)
                {
                    displayFieldString += displayFormatString;
                }
            }

            GetNumberFormat(fieldFormat: fieldString, displayFormat: displayFieldString);
        }

        private void LoadBillingDiscountData()
        {
            try
            {
                txtId.Text = BillingId.ToString();

                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<BillingServiceClient>();

                    var result = await service.GetDiscountAsync(SessionKey, BillingId);

                    if (result.ProcessResult.Result)
                    {
                        var discounts = result.BillingDiscounts;

                        Discount1 = 0M;
                        Discount2 = 0M;
                        Discount3 = 0M;
                        Discount4 = 0M;
                        Discount5 = 0M;

                        foreach (var discount in discounts)
                        {
                            switch (discount.DiscountType)
                            {
                                case 1: Discount1 = discount.DiscountAmount; break;
                                case 2: Discount2 = discount.DiscountAmount; break;
                                case 3: Discount3 = discount.DiscountAmount; break;
                                case 4: Discount4 = discount.DiscountAmount; break;
                                case 5: Discount5 = discount.DiscountAmount; break;
                            }
                        }

                        nmbDiscountAmount1.Value = Discount1;
                        nmbDiscountAmount2.Value = Discount2;
                        nmbDiscountAmount3.Value = Discount3;
                        nmbDiscountAmount4.Value = Discount4;
                        nmbDiscountAmount5.Value = Discount5;
                        nmbBillingAmount.Value = BillingAmount;

                        nmbAmountsRange.Value = Discount1 + Discount2 + Discount3 + Discount4 + Discount5;
                    }
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                var modified = new Action(() => Edited = true);
                nmbDiscountAmount1.ValueChanged += (sdr, eArg) => modified();
                nmbDiscountAmount2.ValueChanged += (sdr, eArg) => modified();
                nmbDiscountAmount3.ValueChanged += (sdr, eArg) => modified();
                nmbDiscountAmount4.ValueChanged += (sdr, eArg) => modified();
                nmbDiscountAmount5.ValueChanged += (sdr, eArg) => modified();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void CalculateDiscount()
        {
            TotalDiscount = Discount1 + Discount2 + Discount3 + Discount4 + Discount5;
            nmbAmountsRange.Value = TotalDiscount;
        }

        #endregion

        #region ValueChangedイベント
        private void DiscountAmount1_ValueChanged(object sender, EventArgs e)
        {
            Discount1 = Convert.ToDecimal(nmbDiscountAmount1.Value);
            CalculateDiscount();
        }

        private void DiscountAmount2_ValueChanged(object sender, EventArgs e)
        {
            Discount2 = Convert.ToDecimal(nmbDiscountAmount2.Value);
            CalculateDiscount();
        }

        private void DiscountAmount3_ValueChanged(object sender, EventArgs e)
        {
            Discount3 = Convert.ToDecimal(nmbDiscountAmount3.Value);
            CalculateDiscount();
        }

        private void DiscountAmount4_ValueChanged(object sender, EventArgs e)
        {
            Discount4 = Convert.ToDecimal(nmbDiscountAmount4.Value);
            CalculateDiscount();
        }

        private void DiscountAmount5_ValueChanged(object sender, EventArgs e)
        {
            Discount5 = Convert.ToDecimal(nmbDiscountAmount5.Value);
            CalculateDiscount();
        }
        #endregion
    }
}
