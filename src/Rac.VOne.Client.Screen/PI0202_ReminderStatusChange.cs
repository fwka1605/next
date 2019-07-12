using System;
using System.Windows.Forms;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Screen.Dialogs;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>督促ステータス編集</summary>
    public partial class PI0202 : VOneScreenBase
    {
        public bool UpdateStatus { get; set; }
        public bool UpdateMemo { get; set; }
        public int StatusId { get; set; }
        public string Memo { get; set; }

        private int formWidth = 490;
        private int formHeight = 400;
        
        public PI0202() : base()
        {
            InitializeComponent();

            cbxStatus.CheckedChanged += (sender, e) => cmbStatus.Enabled = cbxStatus.Checked;
            cbxMemo.CheckedChanged += (sender, e) => txtMemo.Enabled = cbxMemo.Checked;
            cbxStatus.Checked = true;
            cbxMemo.Checked = true;
        }

        public void InitializeUserComponent(int precision, int calculateBaseDate, Reminder reminder = null)
        {
            if (calculateBaseDate == (int)CalculateBaseDate.OriginalDueAt) lblBilledAt.Text = "当初予定日";
            if (calculateBaseDate == (int)CalculateBaseDate.DueAt)         lblBilledAt.Text = "入金予定日";
            if (calculateBaseDate == (int)CalculateBaseDate.BilledAt)      lblBilledAt.Text = "請求日";

            if (reminder != null)
            {
                txtCustomerCode.Text = reminder.CustomerCode;
                txtCustomerName.Text = reminder.CustomerName;
                nmbRemainAmount.Value = reminder.RemainAmount;
                nmbReminderAmount.Value = reminder.ReminderAmount;
                datBilledAt.Value = reminder.CalculateBaseDate;

                cbxStatus.Checked = true;
                cmbStatus.Enabled = true;
                cbxMemo.Checked = true;
                txtMemo.Enabled = true;
            }
            else
            {
                pnlReminder.Visible = false;
            }

            FormWidth = formWidth;
            FormHeight = formHeight - (reminder != null ? 0 : pnlReminder.Height);

            nmbRemainAmount.Fields.DecimalPart.MaxDigits = precision;
            nmbRemainAmount.Fields.DecimalPart.MinDigits = precision;
            nmbReminderAmount.Fields.DecimalPart.MaxDigits = precision;
            nmbReminderAmount.Fields.DecimalPart.MinDigits = precision;

            SetFunctionKeys();
        }

        public void InitializeUserComponent(int precision, ReminderSummary reminderSummary)
        {
            if (reminderSummary != null)
            {
                txtCustomerCode.Text = reminderSummary.CustomerCode;
                txtCustomerName.Text = reminderSummary.CustomerName;
                nmbRemainAmount.Value = reminderSummary.RemainAmount;
                nmbReminderAmount.Value = reminderSummary.ReminderAmount;
            }
            else
            {
                pnlReminder.Visible = false;
            }
            pnlStatus.Visible = false;
            cbxStatus.Checked = false;
            cbxMemo.Checked = true;

            FormWidth = formWidth;
            FormHeight = formHeight - (reminderSummary != null ? 0 : pnlReminder.Height) - pnlStatus.Height;

            nmbRemainAmount.Fields.DecimalPart.MaxDigits = precision;
            nmbRemainAmount.Fields.DecimalPart.MinDigits = precision;
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
            ParentForm.Load += PI0202_Load;
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

            BaseContext.SetFunction10Caption("キャンセル");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = btnF10_Click;
        }

        public Action<Rac.VOne.Client.Common.Controls.VOneComboControl> ComboBoxInitializer { private get; set; }

        private void PI0202_Load(object sender, EventArgs e)
        {
            ProgressDialog.Start(ParentForm, LoadApplicationControlAsync(), false, SessionKey);
            ComboBoxInitializer?.Invoke(cmbStatus);
        }

        [OperationLog("登録")]
        private void btnF01_Click()
        {
            if (!cbxStatus.Checked && !cbxMemo.Checked)
            {
                ShowWarningDialog(Message.Constants.MsgWngSelectionRequired, "変更する項目");
                return;
            }

            if (cbxStatus.Checked && cmbStatus.SelectedIndex < 0)
            {
                ShowWarningDialog(Message.Constants.MsgWngSelectionRequired, cbxStatus.Text);
                return;
            }

            if (cbxStatus.Checked)
            {
                UpdateStatus = true;
                StatusId = (int)cmbStatus.SelectedValue;
            }

            if (cbxMemo.Checked)
            {
                UpdateMemo = true;
                Memo = txtMemo.Text;
            }

            ParentForm.DialogResult = DialogResult.OK;
        }

        [OperationLog("キャンセル")]
        private void btnF10_Click()
        {
            ParentForm.DialogResult = DialogResult.Cancel;
        }

    }
}
