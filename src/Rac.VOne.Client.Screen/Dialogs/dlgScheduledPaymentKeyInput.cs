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
    /// <summary>入金予定キー</summary>
    public partial class dlgScheduledPaymentKeyInput : Dialog
    {
        public string paymentKey = null;
        public dlgScheduledPaymentKeyInput()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            this.paymentKey = txtSearchKey.Text.Trim();
            DialogResult = DialogResult.Yes;
        }
    }
}
