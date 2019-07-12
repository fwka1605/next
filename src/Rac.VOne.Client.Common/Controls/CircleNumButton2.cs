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
    public partial class CircleNumButton2 : UserControl
    {
        private Font _numFont = new Font("Arial", 14, FontStyle.Regular);
        private Font _textFont = new Font("Meiryo UI", 12, FontStyle.Regular);
        private Padding _numPadding = new Padding(0, 0, 0, 0);
        private int _numWidth;

        private Color _buttonBackColor = Color.White;
        private Color _circleBackColor = Color.White;
        private Color _buttonForeColor = Color.White;
        private Color _buttonSelectBackColor = Color.FromArgb(229, 229, 229);
        private Color _circleSelectBackColor = Color.FromArgb(229, 229, 229);
        private Color _circleForeColor = Color.FromArgb(100, 43, 108, 169);
        private int _circleSize = 36;

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
        [Description("ボタン部分のForeColorを設定します")]
        public Color ButtonForeColor
        {
            get
            {
                return _buttonForeColor;
            }
            set
            {
                _buttonForeColor = value;
                button.ForeColor = value;
                Refresh();
            }
        }
        [Browsable(true)]
        [Description("ボタン部分のFontを設定します")]
        public Font ButtonFont
        {
            get
            {
                return _textFont;
            }
            set
            {
                _textFont = value;
                button.Font = value;
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
        [Description("ボタン部分のTextAlignを設定します")]
        public ContentAlignment ButtonAlign
        {
            get
            {
                return button.TextAlign;
            }
            set
            {
                button.TextAlign = value;
                Refresh();
            }
        }
        [Browsable(true)]
        [Description("ボタンNo部分の背景色を設定します")]
        public Color CircleBackCokor
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
                labelNum.ForeColor = value;
                Refresh();
            }
        }
        [Browsable(true)]
        [Description("ボタンNo部分のFontを設定します")]
        public Font CircleFont
        {
            get
            {
                return _numFont;
            }
            set
            {
                _numFont = value;
                labelNum.Font = value;
                Refresh();
            }
        }
        [Browsable(true)]
        [Description("ボタンNo部分の○のサイズを設定します")]
        [DefaultValue(36)]
        public int CircleSize
        {
            get
            {
                return labelNum.Width;
            }
            set
            {
                _circleSize = value;
                SetNumShapeSize();
                Refresh();
            }
        }
        [Browsable(true)]
        [Description("ボタンNo部分のサイズを設定します")]
        public int CircleSpace
        {
            get
            {
                return pictureBox.Width;
            }
            set
            {
                if (value == 0)
                {
                    _numWidth = pictureBox.Height;
                }
                else
                {
                    _numWidth = value;
                }
                SetPictureBoxSize();
                SetButtonSize();
                Refresh();
            }
        }
        [Browsable(true)]
        [Description("ボタンNo部分のナンバーを設定します")]
        public string CircleText
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
        [Description("ボタンNo部分のテキストのPaddingを設定します")]
        public Padding CirclePadding
        {
            get
            {
                return labelNum.Padding;
            }
            set
            {
                _numPadding = value;
                labelNum.Padding = value;
                Refresh();
            }
        }
        [Browsable(true)]
        [Description("ボタンNo部分の表示/非表示を設定します")]
        [DefaultValue(true)]
        public Boolean CircleVisible
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

        public CircleNumButton2()
        {
            InitializeComponent();

            labelNum.Font = _numFont;
            labelNum.ForeColor = _circleForeColor;

            button.Font = _textFont;
            button.ForeColor = _buttonForeColor;

            pictureBox.Controls.Add(labelNum);
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
            SetPictureBoxSize();
            SetNumShapeSize();
            SetButtonSize();
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
        private void SetNumShapeSize()
        {
            labelNum.Size = new Size(_circleSize, _circleSize);
            labelNum.Top = (pictureBox.Height - _circleSize) / 2;
            labelNum.Left = (pictureBox.Width - _circleSize) / 2;
            labelNum.Padding = _numPadding;
        }

        private void SetPictureBoxSize()
        {
            pictureBox.Location = new Point(1, 1);
            pictureBox.Size = new Size(_numWidth, pictureBox_SBase.Height - 2);
        }
        private void SetButtonSize()
        {
            button.Location = new Point(pictureBox.Width, 1);
            button.Width = pictureBox_SBase.Width - pictureBox.Width - 1;
            button.Height = pictureBox_SBase.Height - 2;
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
