using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rac.VOne.Client.Screen
{
    /// <summary>未消込入金メール配信</summary>
    public partial class PE0108 : VOneScreenBase
    {
        public PE0108()
        {
            InitializeComponent();
            vOneGridControl1.SetupShortcutKeys();
            Text = "未消込入金メール配信";
        }

        private void PE0108_Load(object sender, EventArgs e)
        {
            SetScreenName();
            panel1.BackColor = Color.FromArgb(146, 208, 80);
            label1.Text = "配\r\n\r\n信";
            label1.BackColor = Color.FromArgb(146, 208, 80);
        }
    }
}
