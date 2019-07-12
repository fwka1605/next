using System.Drawing;
using System.Windows.Forms;

namespace Rac.VOne.Client
{
    public interface IMessage
    {
        void DispStatusMessage(
            string message,
            string caption,
            Color backColor,
            bool doBeep,
            MessageBoxIcon icon = MessageBoxIcon.Information,
            MessageBoxButtons buttons = MessageBoxButtons.OK);
        void ClearStatusMessage();

        void DispNotEnteredMessage(
            string message,
            string caption,
            Color backColor,
            bool doBeep,
            MessageBoxIcon icon = MessageBoxIcon.Information,
            MessageBoxButtons buttons = MessageBoxButtons.OK);
    }
}
