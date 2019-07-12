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
    public partial class CircleNumButton : UserControl
    {
        private Font _numFont = new Font("Arial", 14, FontStyle.Regular);
        private Font _textFont = new Font("Meiryo UI", 12, FontStyle.Regular);

        private Color _buttonBackColor = Color.White;
        private Color _circleBackColor = Color.White;
        private Color _buttonSelectBackColor = Color.FromArgb(229, 229, 229);
        private Color _circleSelectBackColor = Color.FromArgb(229, 229, 229);
        private Color _circleForeColor = Color.FromArgb(100, 43, 108, 169);
        private Color _borderColor = Color.RoyalBlue;

        [Browsable(true)]
        [Description("ボタン部分の背景色を設定します")]
        public Color ButtonBackColor
        {
            get
            {
                return _buttonBackColor;
            }
            set
            {
                _buttonBackColor = value;
                button.BackColor = value;
                Refresh();
            }
        }
        [Browsable(true)]
        [Description("ボタン部分の選択時の背景色を設定します")]
        public Color ButtonSelectBackColor
        {
            get
            {
                return _buttonSelectBackColor;
            }
            set
            {
                _buttonSelectBackColor = value;
                Refresh();
            }
        }
        [Browsable(true)]
        [Description("ボタンNo部分の背景色を設定します")]
        public Color CircleBackColor
        {
            get
            {
                return _circleBackColor;
            }
            set
            {
                _circleBackColor = value;
                pictureBox.BackColor = value;
                Refresh();
            }
        }
        [Browsable(true)]
        [Description("ボタンNo部分の選択時の背景色を設定します")]
        public Color CircleSelectBackColor
        {
            get
            {
                return _circleSelectBackColor;
            }
            set
            {
                _circleSelectBackColor = value;
                Refresh();
            }
        }
        [Browsable(true)]
        [Description("ボタンNo部分のForeColorを設定します")]
        public Color CircleForeColor
        {
            get
            {
                return _circleForeColor;
            }
            set
            {
                _circleForeColor = value;
                Refresh();
            }
        }
        [Browsable(true)]
        [Description("ボタン部分のテキストを設定します")]
        public string ButtonText
        {
            get
            {
                return button.Text;
            }
            set
            {
                button.Text = value;
                Refresh();
            }
        }
        [Browsable(true)]
        [Description("ボタンNo部分のナンバーを設定します")]
        public string ButtonNumber
        {
            get
            {
                return labelNum.Text;
            }
            set
            {
                labelNum.Text = value;
                Refresh();
            }
        }
        [Browsable(true)]
        [Description("ボタンNo部分の表示/非表示を設定します")]
        [DefaultValue(true)]
        public bool ButtonNumberVisible
        {
            get
            {
                return labelNum.Visible;
            }
            set
            {
                labelNum.Visible = value;
                Refresh();
            }
        }

        [Browsable(true)]
        [Description("任意の文字列を設定できます。")]
        public string InternalCode { get; set; }

        [Browsable(true)]
        [Description("Borderの表示/非表示を設定します")]
        public Boolean BorderVisible
        {
            get
            {
                return pictureBox_SBase.Visible;
            }
            set
            {
                pictureBox_SBase.Visible = value;
                Refresh();
            }
        }
        [Browsable(true)]
        [Description("Borderの色を設定します")]
        public Color BorderColor
        {
            get
            {
                return pictureBox_SBase.BackColor;
            }
            set
            {
                pictureBox_SBase.BackColor = value;
                Refresh();
            }
        }

        public CircleNumButton()
        {
            InitializeComponent();

            labelNum.Font = _numFont;
            labelNum.ForeColor = _circleForeColor;
            pictureBox.BackColor = _circleBackColor;

            button.Font = _textFont;
            button.ForeColor = _circleForeColor;
            button.BackColor = _buttonBackColor;

            pictureBox.Controls.Add(labelNum);
            pictureBox_SBase.BackColor = _borderColor;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawRectangle();
        }
        protected override void OnResize(EventArgs e)
        {
            if (DesignMode) { return; }
            DrawRectangle();
        }

        public void Reflected()
        {
            if (DesignMode) { return; }
            DrawRectangle();
        }
        private void DrawRectangle()
        {
            pictureBox.Location = new Point(1, 1);
            pictureBox.Width = 35;
            pictureBox.Height = Height - 2;

            labelNum.Width = pictureBox.Width - 2;
            labelNum.Height = pictureBox.Height - 2;
            labelNum.Top = (pictureBox.Height - labelNum.Height) / 2;
            labelNum.Left = (pictureBox.Width - labelNum.Width) / 2;

            button.Location = new Point(pictureBox.Width + 1 - 5, 1); //5 pixel 重ねる
            button.Width = Width - pictureBox.Width - 2 + 5;          //5 pixel 重ねた分伸ばす
            button.Height = Height - 2;

            Refresh();
        }

        public void SetText(string num, string text)
        {
            labelNum.Text = num;
            button.Text = text;
        }
        public void SetText(string text)
        {
            labelNum.Visible = false;
            button.Text = text;
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

        private void button_MouseEnter(object sender, EventArgs e)
        {
            pictureBox.BackColor = _circleSelectBackColor;
        }

        private void button_MouseLeave(object sender, EventArgs e)
        {
            pictureBox.BackColor = _circleBackColor;
        }
        private void numCircle_MouseEnter(object sender, EventArgs e)
        {
            button.BackColor = _buttonSelectBackColor;
            pictureBox.BackColor = _circleSelectBackColor;
        }
        private void numCircle_MouseLeave(object sender, EventArgs e)
        {
            button.BackColor = _buttonBackColor;
            pictureBox.BackColor = _circleBackColor;
        }
    }
}
