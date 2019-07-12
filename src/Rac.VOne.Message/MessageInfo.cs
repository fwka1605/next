using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Rac.VOne.Message
{
    public class MessageInfo
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public Color Color { get; set; }
        public bool DoBeep { get; set; }
        public MessageBoxButtons Buttons { get; set; }
        public MessageBoxIcon Icon { get; set; }

        private MessageCategory category;
        public MessageCategory Category
        {
            get { return category; }
            set
            {
                category = value;
                switch (category)
                {
                    case MessageCategory.Information:
                        Color = Color.FromArgb(-4128769);
                        Icon = MessageBoxIcon.Information;
                        DoBeep = false;
                        break;

                    case MessageCategory.Question:
                        Color = SystemColors.Control;
                        Icon = MessageBoxIcon.Question;
                        DoBeep = false;
                        break;

                    case MessageCategory.Warning:
                        Color = Color.FromArgb(-64);
                        Icon = MessageBoxIcon.Warning;
                        DoBeep = true;
                        break;

                    case MessageCategory.Error:
                        Color = Color.FromArgb(-128);
                        Icon = MessageBoxIcon.Error;
                        DoBeep = true;
                        break;
                }
                ;
            }
        }
    }

    public enum MessageCategory
    {
        Information = 0,
        Question,
        Warning,
        Error
    }
}
