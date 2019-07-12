using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GrapeCity.Win.Editors;
using Rac.VOne.Client;

namespace Rac.VOne.Client.Common.Controls
{
    [LicenseProvider(typeof(LicenseProvider))]
    public class VOneTextControl :
        GrapeCity.Win.Editors.GcTextBox,
        IRequired,
        IIgnoreableFocusChange,
        IIgnoreableFontSet
    {
        /// <summary>LeftPad で利用する文字
        /// 値を設定した場合 OnValidated で 左側に指定された文字を MaxLength まで設定する</summary>
        [DefaultValue(null)]
        public char? PaddingChar { get; set; }

        public VOneTextControl()
            : base()
        {
            InitializeComponent();
        }

        public VOneTextControl(IContainer container)
            : base(container)
        {
            InitializeComponent();
        }

        protected virtual void InitializeComponent()
        {
            DropDown.AllowDrop = false;
            HighlightText = true;
            AcceptsCrLf = CrLfMode.Filter;
            AcceptsTabChar = TabCharMode.Filter;
        }

        protected override void InitLayout()
        {
            base.InitLayout();
        }


        [DefaultValue(CrLfMode.Filter)]
        public new CrLfMode AcceptsCrLf
        {
            get { return base.AcceptsCrLf; }
            set { base.AcceptsCrLf = value; }
        }

        [DefaultValue(TabCharMode.Filter)]
        public new TabCharMode AcceptsTabChar
        {
            get { return base.AcceptsTabChar; }
            set { base.AcceptsTabChar = value; }
        }

        protected override List<Type> GetDefaultSideButtonTypes()
        {
            return null;
        }

        protected override Dictionary<Keys, string> GetDefaultShortcuts()
        {
            return null;
        }

        private bool _required;
        public bool Required
        {
            get { return _required;}
            set
            {
                _required = value;
                RequiredChanged?.Invoke(this, new EventArgs());
            }
        }

        public event EventHandler RequiredChanged;

        private static GcIme GcIme = new GcIme();

        private InputScopeNameValue GetInputScope(ImeMode ime)
        {
            if (ime == ImeMode.Hiragana)
                return InputScopeNameValue.Hiragana;
            if (ime == ImeMode.KatakanaHalf)
                return InputScopeNameValue.KatakanaHalfWidth;
            return InputScopeNameValue.Default;
        }

        public new ImeMode ImeMode
        {
            get { return base.ImeMode;  }
            set
            {
                base.ImeMode = value;
                GcIme.SetInputScope(this, GetInputScope(value));
            }
        }

        /// <summary>フォーカス変更時の色変更を無視</summary>
        [DefaultValue(false)]
        public bool IgnoreFocusChange { get; set; }

        /// <summary>フォント設定の無効化 等幅フォントを利用したい場合に true</summary>
        [DefaultValue(false)]
        public bool IgnoreFontSet { get; set; }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            if (!PaddingChar.HasValue || string.IsNullOrEmpty(Text)) return;
            Text = Text.PadLeft(MaxLength, PaddingChar.Value);
        }

    }
}
