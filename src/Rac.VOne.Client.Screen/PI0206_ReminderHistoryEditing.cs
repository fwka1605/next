using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Common;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary> 対応履歴編集 </summary>
    public partial class PI0206 : VOneScreenBase
    {
        public bool UpdateStatus { get; set; }
        public bool UpdateMemo { get; set; }
        public int StatusId { get; set; }
        public string Memo { get; set; }
        public bool IsDelete { get; set; }
        private ReminderHistory ReminderHistory { get; set; }
        private ReminderSummaryHistory ReminderSummaryHistory { get; set; }

        #region initialize

        public PI0206()
        {
            InitializeComponent();
            InitializeHandlers();
        }

        public void InitializeUserComponent(int precision, ReminderHistory reminderHistory)
        {
            FormWidth = 490;
            FormHeight = 400;
            ReminderHistory = reminderHistory;

            nmbReminderAmount.Fields.DecimalPart.MaxDigits = precision;
            nmbReminderAmount.Fields.DecimalPart.MinDigits = precision;

            SetFunctionKeys();
        }


        public void InitializeUserComponent(int precision, ReminderSummaryHistory reminderSummaryHistory)
        {
            FormWidth = 490;
            FormHeight = 400 - (reminderSummaryHistory != null ? 0 : pnlReminder.Height) - pnlStatus.Height;
            pnlStatus.Visible = false;
            ReminderSummaryHistory = reminderSummaryHistory;

            nmbReminderAmount.Fields.DecimalPart.MaxDigits = precision;
            nmbReminderAmount.Fields.DecimalPart.MinDigits = precision;

            SetFunctionKeys();
        }

        private void SetFunctionKeys()
        {
            FunctionKeysSetter = buttons =>
            {
                foreach (var button in buttons)
                {
                    if (button.Name == "btnF01"
                        || button.Name == "btnF02")
                    {
                        button.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                        button.Location = button.Name == "btnF01" ? new Point(1, 0) : new Point(102, 0);
                    }
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
            ParentForm.Shown += PI0206_Load;
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = btnF01_Click;

            BaseContext.SetFunction02Caption("削除");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = btnF02_Click;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = null;

            BaseContext.SetFunction10Caption("キャンセル");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = btnF10_Click;
        }

        private void InitializeHandlers()
        {
            txtMemo.TextChanged += OnMemoTextChanged;
            cmbStatus.SelectedIndexChanged += OnStatusChanged;
        }

        public Action<Common.Controls.VOneComboControl> ComboBoxInitializer { private get; set; }

        private void PI0206_Load(object sender, EventArgs e)
        {
            ProgressDialog.Start(ParentForm, InitializeLoadDataAsync(), false, SessionKey);
            ComboBoxInitializer?.Invoke(cmbStatus);
            SetData();
            UpdateStatus = false;
            UpdateMemo = false;
            IsDelete = false;
            Modified = false;
        }

        private async Task InitializeLoadDataAsync()
        {
            var tasks = new List<Task> {
                LoadCompanyAsync(),
                LoadApplicationControlAsync(),
            };
            await Task.WhenAll(tasks);
            foreach (var task in tasks.Where(x => x.Exception != null))
                NLogHandler.WriteErrorLog(this, task.Exception, SessionKey);
        }

        private void SetData()
        {
            if (ReminderHistory != null)
            {
                lblCreateAtDisplay.Text = ReminderHistory.CreateAt.ToString();
                lblInputTypeName.Text = ReminderHistory.InputTypeName;
                nmbReminderAmount.Value = ReminderHistory.ReminderAmount;
                lblCreateByName.Text = ReminderHistory.CreateByName;
                txtMemo.Text = ReminderHistory.Memo;
            }
            else if (ReminderSummaryHistory != null)
            {
                lblCreateAtDisplay.Text = ReminderSummaryHistory.CreateAt.ToString();
                lblInputTypeName.Text = ReminderSummaryHistory.InputTypeName;
                nmbReminderAmount.Value = ReminderSummaryHistory.ReminderAmount;
                lblCreateByName.Text = ReminderSummaryHistory.CreateByName;
                txtMemo.Text = ReminderSummaryHistory.Memo;
            }
        }

        #endregion

        #region event handler

        private void OnMemoTextChanged(object sender, EventArgs e)
        {
            UpdateMemo = true;
            Modified = true;
        }

        private void OnStatusChanged(object sender, EventArgs e)
        {
            UpdateStatus = true;
            Modified = true;
        }

        #endregion

        #region function keys

        #region save
        [OperationLog("登録")]
        private void btnF01_Click()
        {
            if (UpdateStatus)
                StatusId = (int)cmbStatus.SelectedValue;

            if (UpdateMemo)
                Memo = txtMemo.Text;

            ParentForm.DialogResult = DialogResult.OK;
        }
        #endregion


        #region delete
        [OperationLog("削除")]
        private void btnF02_Click()
        {
            if (!ShowConfirmDialog(MsgQstConfirmDelete))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            IsDelete = true;
            ParentForm.DialogResult = DialogResult.OK;
        }

        #endregion


        #region cancel
        [OperationLog("キャンセル")]
        private void btnF10_Click()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;
            ParentForm.DialogResult = DialogResult.Cancel;
        }

        #endregion

        #endregion

    }
}
