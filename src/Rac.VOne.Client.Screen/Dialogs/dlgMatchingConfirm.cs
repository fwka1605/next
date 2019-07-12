using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public partial class dlgMatchingConfirm : Dialog
    {
        internal string MatchingMemo { get { return txtMatchingMemo.Text.Trim(); } }
        internal int RemainType { get; set; }
        internal string CustomerInformation { private get; set; }
        internal string RemainAmount { private get; set; }
        internal string CarryOverAmount { private get; set; }

        public dlgMatchingConfirm()
        { 
            InitializeComponent();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnNo);
            DialogResult = DialogResult.No;
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnYes);
            DialogResult = DialogResult.Yes;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnCancel);
            DialogResult = DialogResult.Cancel;
        }

        private void dlgMatchingConfirm_Load(object sender, EventArgs e)
        {
            if (this.RemainType == 3)
            {
                lblConfirmMessage.Text = $@"入金残計 {RemainAmount}、うち過入金 {CarryOverAmount} ですが、
消込してもよろしいですか？
はい          ：過入金を {CustomerInformation} の前受として処理します
いいえ        ：入金残をそのまま残します
キャンセル    ：処理を中断します";
            }
            else if (this.RemainType == 2)
            {
                lblConfirmMessage.Text = $@"入金残計 {RemainAmount} ですが、消込してもよろしいですか？";
                btnCancel.Visible = false;
                btnYes.Location = new Point(btnNo.Location.X, btnNo.Location.Y);
                btnNo.Location = new Point(btnCancel.Location.X, btnCancel.Location.Y);
            }
            else if (this.RemainType == 1)
            {
                lblConfirmMessage.Text = $@"請求残計 {RemainAmount} ですが、消込してもよろしいですか？";
                btnCancel.Visible = false;
                btnYes.Location = new Point(btnNo.Location.X, btnNo.Location.Y);
                btnNo.Location = new Point(btnCancel.Location.X, btnCancel.Location.Y);
            }
            else if (this.RemainType == 0)
            {
                lblConfirmMessage.Text = $@"消込してもよろしいですか？";
                btnCancel.Visible = false;
                btnYes.Location = new Point(btnNo.Location.X, btnNo.Location.Y);
                btnNo.Location = new Point(btnCancel.Location.X, btnCancel.Location.Y);
            }
            this.ViewOpened();
        }
    }
}
