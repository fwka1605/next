using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Rac.VOne.Client.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Screen.Dialogs;
using static Rac.VOne.Common.Constants;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>得意先マスター インポート対象の選択</summary>
    public partial class PB0504 : VOneScreenBase
    {
        /// <summary>
        /// 取込ファイルタイプ
        ///  4 : 得意先マスター
        ///  5 : 得意先 登録手数料
        ///  6 : 歩引設定
        /// </summary>
        public int ImportFileType { get; set; }

        public string PatternNo { get; set; }

        private int FormatId { get { return (int)FreeImporterFormatType.Customer; } }

        private bool IsValidCode { get; set; }

        public PB0504()
        {
            InitializeComponent();
            InitializeUserComponent();
        }

        private void InitializeUserComponent()
        {
            FormWidth = 500;
            FormHeight = 220;
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

        protected override void InitLayout()
        {
            base.InitLayout();
            if (ParentForm == null) return;
            ParentForm.Load += PB0504_Load;
            rdoCustomer.CheckedChanged += CustomerCheckedChange;
            btnPatternSearch.Click += PatternNumberSearch;
            txtPatternNumber.Validated += PatternNumberValidated;
        }

        #region InitializeFunctionKeys
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("選択");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = btnSelect_Click;

            BaseContext.SetFunction02Caption("");
            BaseContext.SetFunction02Enabled(false);
            OnF02ClickHandler = null;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = null;


            BaseContext.SetFunction10Caption("キャンセル");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = btnCancel_Click;

        }
        #endregion

        #region フォームロード処理
        private void PB0504_Load(object sender, EventArgs e)
        {
            var tasks = new List<Task>();
            if (Company == null)
                tasks.Add(LoadCompanyAsync());
            if (ApplicationControl == null)
                tasks.Add(LoadApplicationControlAsync());

            Dialogs.ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);

            rdoDiscount.Visible = UseDiscount;
            ActiveControl = txtPatternNumber;
            txtPatternNumber.Focus();
        }
        #endregion

        #region 選択処理
        [OperationLog("選択")]
        private void btnSelect_Click()
        {
            if (rdoCustomer.Checked)
            {
                if (!IsValidPatternNo()) return;
                ImportFileType = Import.ImportFileType.Customer;
                PatternNo = txtPatternNumber.Text;
            }
            if (rdoFee.Checked)
            {
                ImportFileType = Import.ImportFileType.CustomerFee;
            }
            if (rdoDiscount.Checked)
            {
                ImportFileType = Import.ImportFileType.CustomerDiscount;
            }

            ParentForm.DialogResult = DialogResult.OK;

        }

        private bool IsValidPatternNo()
        {
            if (string.IsNullOrWhiteSpace(txtPatternNumber.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "パターンNo.");
                txtPatternNumber.Focus();
                return false;
            }
            if (!IsValidCode)
            {
                ShowWarningDialog(MsgWngNotRegistPatternNo, txtPatternNumber.Text);
                txtPatternNumber.Focus();
                return false;
            }

            return true;
        }

        #endregion

        #region キャンセル処理
        [OperationLog("キャンセル")]
        private void btnCancel_Click()
        {
            ParentForm.DialogResult = DialogResult.Cancel;
        }
        #endregion

        #region オプションボタンCheckedChangeイベント
        private void CustomerCheckedChange(object sender, EventArgs e)
        {
            txtPatternNumber.Clear();
            lblPatternName.Clear();
            txtPatternNumber.Enabled = rdoCustomer.Checked;
            btnPatternSearch.Enabled = rdoCustomer.Checked;
            txtPatternNumber.Focus();
        }
        #endregion

        #region パターンNo入力Validatedイベント
        private void PatternNumberValidated(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPatternNumber.Text)) return;

            try
            {
                txtPatternNumber.Text = txtPatternNumber.Text.PadLeft(2, '0');
                Task<ImporterSetting> task = LoadHeaderByCodeAsync(txtPatternNumber.Text);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                IsValidCode = task.Result != null;

                if (IsValidCode)
                    lblPatternName.Text = task.Result.Name;
                else
                    lblPatternName.Text = string.Empty;

                ClearStatusMessage();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region 検索ボタン押下処理
        private void PatternNumberSearch(object sender, EventArgs e)
        {
            try
            {
                var import = this.ShowCustomerImporterSettingSearchDialog();
                if (import == null) return;
                txtPatternNumber.Text = import.Code;
                lblPatternName.Text = import.Name;
                IsValidCode = true;
                ClearStatusMessage();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region Webサービス呼び出し
        private async Task<ImporterSetting> LoadHeaderByCodeAsync(string code)
        {
            return await Util.GetImporterSettingAsync(Login, FormatId, code);
        }
        #endregion

    }
}
