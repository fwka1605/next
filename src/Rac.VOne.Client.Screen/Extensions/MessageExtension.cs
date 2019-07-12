using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Message;
using System.Windows.Forms;

namespace Rac.VOne.Client.Screen.Extensions
{
    public static class MessageExtension
    {
        public static DialogResult ShowMessageBox(this MessageInfo info, IWin32Window owner)
        {
            return MessageBox.Show(owner, info.Text, info.Title, info.Buttons, info.Icon);
        }
    }
}
