using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GrapeCity.Win.Editors;
using GrapeCity.Win.Editors.Fields;
using Rac.VOne.Client;

namespace Rac.VOne.Client.Common.Controls
{
    [LicenseProvider(typeof(LicenseProvider))]
    public class VOneDateControl : GrapeCity.Win.Editors.GcDate, IRequired
    {
        public VOneDateControl()
            : base()
        {
            InitializeComponent();
        }

        public VOneDateControl(IContainer conatiner)
            : base(conatiner)
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            HighlightText = HighlightText.All;
            ContentAlignment = ContentAlignment.MiddleCenter;
            Spin.AllowSpin = false;
            ImeMode = ImeMode.Disable;
            InitializeFields();
        }

        private DateType _dateType = DateType.YearMonthDay;

        [Category("VOneカスタマイズ項目")]
        [Description("日付の表示形式を設定します。入力・表示の書式、および、カレンダータイプを変更します。")]
        [DefaultValue(DateType.YearMonthDay)]
        public DateType InputDateType
        {
            get { return _dateType; }
            set
            {
                if (value == DateType.YearMonthDayHour)
                    value = DateType.YearMonthDay;
                var diff = _dateType != value;
                _dateType = value;
                if (diff)
                {
                    InitializeFields();
                }
            }
        }

        private CalendarFormat _calendarFormat = CalendarFormat.GregorianCalendar;

        [Category("VOneカスタマイズ項目")]
        [Description("日付の西暦/和暦を設定します。")]
        [DefaultValue(CalendarFormat.GregorianCalendar)]
        public CalendarFormat CalendarFormat
        {
            get { return _calendarFormat; }
            set
            {
                var diff = _calendarFormat != value;
                _calendarFormat = value;
                if (diff)
                {
                    InitializeFields();
                }
            }
        }

        private void InitializeFields()
        {
            Fields.Clear();
            DisplayFields.Clear();
            var inputFormat = "";
            var displayFormat = "";
            var button = new DropDownButton();
            var isWareki = CalendarFormat == CalendarFormat.JapaneseCalendar;
            switch (InputDateType)
            {
                case DateType.YearMonthDay:
                    inputFormat = isWareki ? "ee/MM/dd" : "yyyy/MM/dd";
                    displayFormat = isWareki ? "ggg ee/MM/dd" : "yyyy/MM/dd";
                    DropDownCalendar.CalendarType = CalendarType.MonthDay;
                    break;
                case DateType.YearMonth:
                    inputFormat = isWareki ? "ee/MM" : "yyyy/MM";
                    displayFormat = isWareki ? "ggg ee/MM" : "yyyy/MM";
                    DropDownCalendar.CalendarType = CalendarType.YearMonth;
                    break;
                case DateType.Year:
                    inputFormat = isWareki ? "ee" : "yyyy";
                    displayFormat = isWareki ? "ggg ee" : "yyyy";
                    DropDownCalendar.CalendarType = CalendarType.YearMonth;
                    button.Visible = ButtonVisibility.NotShown;
                    break;
                case DateType.MonthDay:
                    inputFormat = "MM/dd";
                    displayFormat = "MM/dd";
                    DropDownCalendar.CalendarType = CalendarType.MonthDay;
                    break;
                case DateType.YearMonthDayHour:
                    inputFormat = isWareki ? "ee/MM/dd HH時" : "yyyy/MM/dd HH時";
                    displayFormat = isWareki ? "ggg ee/MM/dd HH時" : "yyyy/MM/dd HH時";
                    DropDownCalendar.CalendarType = CalendarType.MonthDay;
                    break;
            }
            Fields.AddRange(inputFormat);
            DisplayFields.AddRange(displayFormat);
            SideButtons.Clear();
            SideButtons.Add(button);
        }

        protected override List<Type> GetDefaultSideButtonTypes()
        {
            return new List<Type> { typeof(DropDownButton) };
        }

        protected override Dictionary<Keys, string> GetDefaultShortcuts()
        {
            return null;
        }

        private bool _required;
        public bool Required
        {
            get { return _required; }
            set
            {
                _required = value;
                RequiredChanged?.Invoke(this, new EventArgs());
            }
        }

        public event EventHandler RequiredChanged;

        #region 継承コントロール Designer 配置値の対応
        /*
        GrapeCity InputMan Help 参照
        InputManの使い方
        └継承コントロールを作成する場合の注意点
         └デザイン機能を無効にして実行時に設定する方法
        */

        public new DateTimeFieldCollection Fields
        {
            get { return base.Fields; }
        }
        private bool ShouldSerializeFields() { return false; }

        public new DateTimeDisplayFieldCollection DisplayFields
        {
            get { return base.DisplayFields; }
        }

        private bool ShouldSerializeDisplayFields() { return false; }

        public new SideButtonCollection SideButtons
        {
            get { return base.SideButtons; }
        }

        private bool ShouldSerializeSideButtons() { return false; }

        #endregion

    }
}
