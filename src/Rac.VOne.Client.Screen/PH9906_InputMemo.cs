using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.ReceiptService;
using Rac.VOne.Client.Screen.BillingService;
using Rac.VOne.Web.Models;


namespace Rac.VOne.Client.Screen
{
    /// <summary>メモ入力</summary>
    public partial class PH9906 : VOneScreenBase
    {
        public string Memo { get; set; }
        /// <summary>
        ///  Billing/Receipt . Id
        /// </summary>
        public long Id { get; set; }

        private string Title { get; set; }


        private MemoType memotype;
        public MemoType MemoType
        {
            private get { return memotype; }
            set
            {
                memotype = value;
                if (memotype == MemoType.BillingMemo)
                    Title = "請求メモ";
                else if (memotype == MemoType.ReceiptMemo)
                    Title = "入金メモ";
                else if (memotype == MemoType.TransferMemo)
                    Title = "振替メモ";

            }
        }


        public PH9906()
        {
            InitializeComponent();
            InitializeUserComponent();
        }

        private void InitializeUserComponent()
        {
            FormWidth = 500;
            FormHeight = 150;
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
        }

        private async Task InitializeLoadDataAsync()
        {
            await Task.WhenAll(
                LoadCompanyAsync(),
                LoadApplicationControlAsync(),
                LoadControlColorAsync());
        }


        protected override void InitLayout()
        {
            base.InitLayout();
            if (ParentForm == null) return;
            ParentForm.Shown += PH9906_Shown;
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();
            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction02Caption("");
            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction10Caption("キャンセル");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(false);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction10Enabled(true);

            OnF01ClickHandler = Register;
            OnF10ClickHandler = Cancel;
        }


        #region F01:登録ボタン押下

        [OperationLog("登録")]
        private void Register()
        {
            try
            {
                Memo = txtMemo.Text.Trim();
                if (Id == 0)
                {
                    ParentForm.DialogResult = DialogResult.OK;
                    return;
                }

                if (MemoType == MemoType.BillingMemo)
                {
                    if (string.IsNullOrEmpty(Memo))
                        DeleteBillingMemo();
                    else
                        SaveBillingMemo();
                }
                else if (MemoType == MemoType.ReceiptMemo || MemoType == MemoType.TransferMemo)
                {
                    if (string.IsNullOrEmpty(Memo))
                        DeleteReceiptMemo();
                    else
                        SaveReceiptMemo();
                }
                ParentForm.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region F10:キャンセルボタン押下

        [OperationLog("キャンセル")]
        private void Cancel()
        {
            ParentForm.DialogResult = DialogResult.Cancel;
        }

        #endregion

        #region メモ取得
        private void SetMemo()
        {
            if (Id == 0)
            {
                txtMemo.Text = !string.IsNullOrEmpty(Memo) ? Memo : null;
                return;
            }

            if (MemoType == MemoType.BillingMemo)
                SetBillingMemo();
            else if (MemoType == MemoType.ReceiptMemo || MemoType == MemoType.TransferMemo)
                SetReceiptMemo();
        }
        #endregion

        #region 請求メモ取得
        private void SetBillingMemo()
        {
            var task = ServiceProxyFactory.DoAsync(async (BillingServiceClient client) =>
            {
                var result = await client.GetMemoAsync(SessionKey, Id);
                if (result.ProcessResult.Result)
                {
                    Memo = result.BillingMemo;
                    txtMemo.Text = Memo;
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
        }
        #endregion

        #region 入金メモ取得
        private void SetReceiptMemo()
        {
            var task = ServiceProxyFactory.DoAsync(async (ReceiptServiceClient client) =>
            {
                var result = await client.GetMemoAsync(SessionKey, Id);
                if (result.ProcessResult.Result)
                {
                    Memo = result.ReceiptMemo;
                    txtMemo.Text = Memo;
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
        }
        #endregion

        #region 請求メモ登録
        private void SaveBillingMemo()
        {
            var task = ServiceProxyFactory.DoAsync(async (BillingServiceClient client)
                => await client.SaveMemoAsync(SessionKey, Id, Memo));
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
        }
        #endregion

        #region 入金メモ登録
        private void SaveReceiptMemo()
        {
            var task = ServiceProxyFactory.DoAsync(async (ReceiptServiceClient client)
                => await client.SaveMemoAsync(SessionKey, Id, Memo));
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
        }
        #endregion

        #region 請求メモ削除
        private void DeleteBillingMemo()
        {
            var task = ServiceProxyFactory.DoAsync(async (BillingServiceClient client)
                => await client.DeleteMemoAsync(SessionKey, Id));
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
        }
        #endregion

        #region 入金メモ削除
        private void DeleteReceiptMemo()
        {
            var task = ServiceProxyFactory.DoAsync(async (ReceiptServiceClient client)
                => await client.DeleteMemoAsync(SessionKey, Id));
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
        }
        #endregion

        private void PH9906_Shown(object sender, EventArgs e)
        {
            try
            {
                ProgressDialog.Start(ParentForm, InitializeLoadDataAsync(), false, SessionKey);
                SetMemo();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

    }
}
