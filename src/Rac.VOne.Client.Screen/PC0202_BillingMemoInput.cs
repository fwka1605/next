using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>請求 メモ 備考入力画面</summary>
    public partial class PC0202 : VOneScreenBase
    {

        /// <summary>請求データ入力用 メモ/備考2..8 いずれかに入力があればtrue</summary>
        internal bool IsAnyNoteInputted =>
               !string.IsNullOrWhiteSpace(txtMemo.Text)
            || !string.IsNullOrWhiteSpace(txtNote2.Text)
            || !string.IsNullOrWhiteSpace(txtNote3.Text)
            || !string.IsNullOrWhiteSpace(txtNote4.Text)
            || !string.IsNullOrWhiteSpace(txtNote5.Text)
            || !string.IsNullOrWhiteSpace(txtNote6.Text)
            || !string.IsNullOrWhiteSpace(txtNote7.Text)
            || !string.IsNullOrWhiteSpace(txtNote8.Text);

        /// <summary> 請求書発行　備考欄印字用制限文字数フラグ </summary>
        public bool UseControlInputNote { get; set; }

        #region initialize

        public PC0202()
        {
            InitializeComponent();
            InitializeUserComponent();
            InitializeHandlers();
        }

        protected override void InitLayout()
        {
            base.InitLayout();
            if (ParentForm != null)
                ParentForm.Shown += PC0202_Shown;
        }

        private void InitializeUserComponent()
        {
            Text = "請求メモ/備考入力";
            FormWidth = 620;
            FormHeight = 350;
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

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("確定");
            BaseContext.SetFunction02Caption("");
            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction10Caption("キャンセル");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(false);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction10Enabled(true);

            OnF01ClickHandler = Confirm;
            OnF10ClickHandler = Cancel;
        }

        private void InitializeHandlers()
        {
            txtMemo.TextChanged += OnTextChanged;
            txtNote2.TextChanged += OnTextChanged;
            txtNote3.TextChanged += OnTextChanged;
            txtNote4.TextChanged += OnTextChanged;
            txtNote5.TextChanged += OnTextChanged;
            txtNote6.TextChanged += OnTextChanged;
            txtNote7.TextChanged += OnTextChanged;
            txtNote8.TextChanged += OnTextChanged;
        }

        private void PC0202_Shown(object sender, EventArgs e)
        {
            if (UseControlInputNote)
            {
                txtNote2.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
                txtNote2.MaxLength = NoteInputByteCount;
            }
            ProgressDialog.Start(ParentForm, InitializeLoadDataAsync(), false, SessionKey);
            Modified = false;
        }

        private async Task InitializeLoadDataAsync()
        {
            var tasks = new List<Task> {
                LoadCompanyAsync(),
                LoadApplicationControlAsync(),
                LoadControlColorAsync(),
                LoadColumnNameSettingsAsync()
            };
            await Task.WhenAll(tasks);
            foreach (var task in tasks.Where(x => x.Exception != null))
                NLogHandler.WriteErrorLog(this, task.Exception, SessionKey);
        }

        private async Task LoadColumnNameSettingsAsync()
        {
            await Util.LoadColumnNameSettingAsync(Login, nameof(Billing), settings => {
                foreach (var setting in settings)
                {
                    if (setting.ColumnName == "Note2") lblNote2.Text = setting.DisplayColumnName;
                    if (setting.ColumnName == "Note3") lblNote3.Text = setting.DisplayColumnName;
                    if (setting.ColumnName == "Note4") lblNote4.Text = setting.DisplayColumnName;
                    if (setting.ColumnName == "Note5") lblNote5.Text = setting.DisplayColumnName;
                    if (setting.ColumnName == "Note6") lblNote6.Text = setting.DisplayColumnName;
                    if (setting.ColumnName == "Note7") lblNote7.Text = setting.DisplayColumnName;
                    if (setting.ColumnName == "Note8") lblNote8.Text = setting.DisplayColumnName;
                }
            });
        }

        #endregion

        #region event handler

        private void OnTextChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        #endregion

        #region function keys

        [OperationLog("確定")]
        private void Confirm()
        {
            ParentForm.DialogResult = DialogResult.OK;
        }

        [OperationLog("キャンセル")]
        private void Cancel()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;
            ParentForm.DialogResult = DialogResult.Cancel;
        }

        #endregion
    }
}
