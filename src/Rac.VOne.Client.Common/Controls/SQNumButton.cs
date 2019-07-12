using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rac.VOne.Client.Common.Controls
{
    public partial class SQNumButton : UserControl
    {
        private Font _ButtonFont = new Font("Meiryo UI", 14, FontStyle.Regular);
        private Bitmap _ButtonIcon;
        private string _TextNumber = "00";
        private string _TextCaption = "";

        [Browsable(true)]
        [Description("ボタンのNo部分のテキストを設定します")]
        public string TextNumber
        {
            get
            {
                return _TextNumber;
            }
            set
            {
                _TextNumber = value;
                button.Text = _TextNumber + "  " + _TextCaption;
                this.Refresh();
            }
        }

        [Browsable(true)]
        [Description("ボタンのキャプション部分のテキストを設定します")]
        public string TextCaption
        {
            get
            {
                return _TextCaption;
            }
            set
            {
                _TextCaption = value;
                button.Text = _TextNumber + "  " + _TextCaption;
                this.Refresh();
            }
        }
        [Browsable(true)]
        [Description("ボタンのアイコンを設定します")]
        public Bitmap ButtonIcon
        {
            get
            {
                return _ButtonIcon;
            }
            set
            {
                _ButtonIcon = value;
                button.Image = value;
            }
        }
        [Browsable(true)]
        [Description("ボタンのフォントを設定します")]
        public Font ButtonFont
        {
            get
            {
                return _ButtonFont;
            }
            set
            {
                _ButtonFont = value;
                button.Font = value;
            }
        }

        [Browsable(true)]
        [Description("任意の文字列を設定できます。")]
        public string InternalCode { get; set; }

        public SQNumButton()
        {
            InitializeComponent();
        }
        private void button_MouseEnter(object sender, EventArgs e)
        {
            this.button.BackgroundImage = Properties.Resources.bg_menu_on;
            this.button.ForeColor = Color.White;
        }

        private void button_MouseLeave(object sender, EventArgs e)
        {
            this.button.BackgroundImage = Properties.Resources.bg_menu;
            this.button.ForeColor = Color.FromArgb(43, 108, 169);
        }


        public event EventHandler<EventArgs> ButtonClick;

        [Browsable(true)]
        protected virtual void onClick(EventArgs e)
        {
            ButtonClick?.Invoke(this, e);
        }
        private void button_Click(object sender, EventArgs e)
        {
            OnClick(e);
        }
    }
}
