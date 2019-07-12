using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;
using GrapeCity.Win.MultiRow.InputMan;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Client.Common.MultiRow
{
    public class GcMultiRowTemplateBuilder// : IDisposable
    {
        public List<CellSetting> Items = new List<CellSetting>();

        public bool AutoLocationSet { get; set; }
        public bool AllowHorizontalResize { get; set; }
        public bool Sortable { get; set; }
        public Font Font { get; set; }
        public Border Border { get; set; }

        private int defaultRowHeight = 20;
        public int DefaultRowHeight { get { return defaultRowHeight; } }

        public Template Build()
        {
            var template = new Template();
            var header = new ColumnHeaderSection();
            var row = new Row();
            if (AutoLocationSet)
            {
                SetLocation();
            }

            int height = GetHeight();
            header.Height = height > 0 ? height : header.Height;
            row.Height = height > 0 ? height : row.Height;

            int width = GetWidth();
            template.Width = width > 0 ? width : template.Width;

            foreach (var setting in Items)
            {
                SetHeaderRow(header, setting);
                SetDetailRow(row, setting);
            }
            if (AllowHorizontalResize)
            {
                foreach (var cell in header.Cells)
                {
                    cell.ResizeMode = ResizeMode.Horizontal;
                }
            }
            template.ColumnHeaders.Add(header);
            template.Row = row;
            return template;
        }

        public void BuildHeaderOnly(Template tmp)
        {
            var header = new ColumnHeaderSection();

            var height = GetHeight();
            header.Height = height > 0 ? height : header.Height;

            if (AutoLocationSet) SetLocation();

            var width = GetWidth();
            tmp.Width = width > 0 ? width : tmp.Width;

            foreach(var setting in Items)
            {
                SetHeaderRow(header, setting);
            }

            if (AllowHorizontalResize)
            {
                foreach(var cell in header.Cells)
                {
                    cell.ResizeMode = ResizeMode.Horizontal;
                }
            }

            tmp.ColumnHeaders.Add(header);
        }

        public void BuildRowOnly(Template tmp)
        {
            var row = new Row();
            if (AutoLocationSet) SetLocation();

            var height = GetHeight();
            row.Height = height > 0 ? height : row.Height;

            var width = GetWidth();
            row.Width = width > 0 ? width : tmp.Width;

            foreach(var setting in Items)
            {
                SetDetailRow(row, setting);
            }

            foreach(var cell in row.Cells)
            {
                cell.ResizeMode = ResizeMode.None;
            }

            tmp.Row = row;
        }

        private void SetLocation()
        {
            int x = 0, y = 0;
            foreach (var setting in Items)
            {
                setting.Location = new Point(x, y);
                x += setting.Width;
            }
        }

        private void SetHeaderRow(ColumnHeaderSection header, CellSetting setting)
        {
            HeaderCell chCell;
            if (setting.CellInstance is RowHeaderCell)
            {
                chCell = new CornerHeaderCell();
            }
            else
            {
                chCell = new ColumnHeaderCell();
            }
            //var chCell = new ColumnHeaderCell();
            chCell.Name = string.Format("lbl{0}", setting.Name);
            chCell.Value = setting.Caption;
            chCell.Size = setting.Size;
            chCell.Location = setting.Location;
            var style = new CellStyle()
            {
                TextAlign = MultiRowContentAlignment.MiddleCenter,
                Multiline = MultiRowTriState.False,
                Font = setting.Font ?? this.Font,
            };
            chCell.Style = style;

            if (chCell is ColumnHeaderCell)
            {
                var h = chCell as ColumnHeaderCell;
                if (this.Sortable)
                {
                    h.SelectionMode = MultiRowSelectionMode.None;
                    h.SortMode = SortMode.Automatic;
                }
                else
                {
                    h.SortMode = SortMode.NotSortable;
                }

                if (setting.SortDropDown) h.DropDownContextMenuStrip = GetSortDropDownContextMenuStrip();
                if (setting.DropDownList != null) h.DropDownList = setting.DropDownList;
            }

            header.Cells.Add(chCell);
        }

        private void SetDetailRow(Row row, CellSetting setting)
        {
            var c = setting.CellInstance;
            if (c == null) c = GetTextBoxCell();
            c.Name = string.Format("cel{0}", setting.Name);
            c.DataField = setting.DataField;
            c.Size = setting.Size;
            c.Location = setting.Location;
            c.ReadOnly = setting.ReadOnly;
            c.Enabled = setting.Enabled;
            c.Visible = setting.Visible;
            c.TabStop = setting.TabStop;
            c.Selectable = setting.Selectable;
            if (setting.TabIndex > 0) c.TabIndex = setting.TabIndex;
            c.Value = setting.Value;
            if (setting.BackColor != Color.Empty) c.Style.BackColor = setting.BackColor;
            if (setting.SelectionBackColor != Color.Empty) c.Style.SelectionBackColor = setting.SelectionBackColor;
            if (setting.DisableBackColor != Color.Empty) c.Style.DisabledBackColor = setting.DisableBackColor;
            if (setting.Border != null) c.Style.Border = setting.Border;
            else if (Border != null) c.Style.Border = Border;
            if (setting.Font != null) c.Style.Font = setting.Font;
            row.Cells.Add(c);
        }

        private int GetHeight()
        {
            int height = 0;
            height = Items.Max(i => i.Y);
            height += Items.Where(i => i.Y == height).Max(i => i.Height);
            return height;
        }
        private int GetWidth()
        {
            int width = 0;
            width = Items.Max(i => i.X);
            width += Items.Where(i => i.X == width).Max(i => i.Width);
            return width;
        }

        private HeaderDropDownContextMenu sortDropDownContextMenuStrip = null;
        private HeaderDropDownContextMenu GetSortDropDownContextMenuStrip()
        {
            if (sortDropDownContextMenuStrip == null)
            {
                sortDropDownContextMenuStrip = new HeaderDropDownContextMenu();
                sortDropDownContextMenuStrip.Items.Add(GetSortToolStripItem(true));
                sortDropDownContextMenuStrip.Items.Add(GetSortToolStripItem(false));
            }
            return sortDropDownContextMenuStrip;
        }
        private SortToolStripItem GetSortToolStripItem(bool sortOrder)
        {
            var item = new SortToolStripItem();
            item.DisplayStyle = ToolStripItemDisplayStyle.Text;
            item.Size = new Size(160, 22);
            if (sortOrder)
            {
                item.Name = "AscendingSortItem";
                item.SortOrder = SortOrder.Ascending;
                item.Text = "昇順に並び替え";
            }
            else
            {
                item.Name = "DescendingSortItem";
                item.SortOrder = SortOrder.Descending;
                item.Text = "降順に並び替え";
            }
            return item;
        }

        public Border GetBorder(
            LineStyle left   = LineStyle.Thin,
            LineStyle top    = LineStyle.Thin,
            LineStyle right  = LineStyle.Thin,
            LineStyle bottom = LineStyle.Thin,
            Color? lineColor  = null)
            => GetBorder(
                GetLine(left  , lineColor),
                GetLine(top   , lineColor),
                GetLine(right , lineColor),
                GetLine(bottom, lineColor));

        public Border GetBorder(
            GrapeCity.Win.MultiRow.Line? left = null,
            GrapeCity.Win.MultiRow.Line? top = null,
            GrapeCity.Win.MultiRow.Line? right = null,
            GrapeCity.Win.MultiRow.Line? bottom = null)
            => new Border(
                left   ?? GetLine(),
                top    ?? GetLine(),
                right  ?? GetLine(),
                bottom ?? GetLine());

        public Border GetBorder(
            LineStyle all = LineStyle.Thin,
            Color? lineColor = null) => GetBorder(all, all, all, all, lineColor);

        public GrapeCity.Win.MultiRow.Line GetLine(LineStyle style = LineStyle.Thin, Color? lineColor = null)
        {
            var color = lineColor ?? Color.Black;
            return new GrapeCity.Win.MultiRow.Line(style, color);
        }

        #region コントロール初期化

        private Border border = new Border() { Outline = new GrapeCity.Win.MultiRow.Line(LineStyle.Thin, Color.Black) };

        public GcTextBoxCell GetTextBoxCell(MultiRowContentAlignment align = MultiRowContentAlignment.MiddleLeft
            , ImeMode ime = ImeMode.NoControl, string format = null, int? maxLength = null, bool isNumberCell = false
            , GrapeCity.Win.Editors.LengthUnit maxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Char)
        {
            var cell = !isNumberCell
                ? new GcTextBoxCell()
                : new GcNumberTextBoxCell();
            cell.AutoComplete.CandidateListItemFont = Font;
            cell.AutoComplete.HighlightStyle.Font = Font;
            cell.Format = format;
            if (maxLength.HasValue) cell.MaxLength = maxLength.Value;
            cell.ExitOnArrowKey = true;
            cell.ShortcutKeys.Remove(Keys.F2);
            cell.ShortcutKeys.Remove(Keys.F4);
            cell.Style = GetDefaultCellStyle(align, ime);
            cell.MaxLengthUnit = maxLengthUnit;
            return cell;
        }

        public GcTextBoxCell GetTextBoxCurrencyCell(int scale = 0, char padding = '0')
        {
            var cell = GetTextBoxCell(MultiRowContentAlignment.MiddleRight, isNumberCell: true);
            var format = "#,##0" + (scale > 0 ? "." + new string(padding, scale) : string.Empty);
            cell.Style.Format = format;
            return cell;
        }

        public GcNumberCell GetNumberCell(MultiRowContentAlignment align = MultiRowContentAlignment.MiddleRight,
            string fieldFormat = "#,###,###,###,###", string displayFormat = "#,###,###,###,##0")
        {
            var cell = new GcNumberCell();
            cell.ExitOnArrowKey = true;
            cell.Fields.SetFields(fieldFormat, "", "", "-", "");
            cell.DisplayFields.AddRange(displayFormat, "", "", "-", "");
            cell.Fields.IntegerPart.MinDigits = 1;
            cell.FlatStyle = FlatStyle.Flat;
            cell.Spin.AllowSpin = false;
            cell.ShortcutKeys.Remove(Keys.F2);
            cell.ShortcutKeys.Remove(Keys.F4);
            cell.Style = GetDefaultCellStyle(align);
            cell.SideButtons.Clear();
            cell.MaxValue = 9999999999999M;
            cell.MinValue = -9999999999999M;
            cell.MaxMinBehavior = GrapeCity.Win.Editors.MaxMinBehavior.AdjustToMaxMin;
            return cell;
        }

        /// <summary>金額表示用セル取得</summary>
        /// <param name="fieldScale">小数点以下桁数（値部分）</param>
        /// <param name="displayScale">小数点以下桁数（表示部分）</param>
        /// <param name="roundPattern">端数処理</param>
        /// <param name="displayFormatString">小数点以下の表示書式</param>
        /// <returns></returns>
        public GcNumberCell GetNumberCellCurrency(int fieldScale, int displayScale, int roundPattern, string displayFormatString = "0")
        {
            var fieldString = "#,###,###,###,##0";
            if (fieldScale > 0)
            {
                fieldString += ".";
                for (int i = 0; i < fieldScale; i++)
                {
                    fieldString += "#";
                }
            }

            var displayFieldString = "#,###,###,###,##0";
            if (displayScale > 0)
            {
                displayFieldString += ".";
                for (int i = 0; i < displayScale; i++)
                {
                    displayFieldString += displayFormatString;
                }
            }
            var cell = GetNumberCell(fieldFormat: fieldString, displayFormat: displayFieldString);

            cell.RoundPattern = Enum.IsDefined(typeof(GrapeCity.Win.Editors.RoundPattern), roundPattern) ?
                (GrapeCity.Win.Editors.RoundPattern)Enum.ToObject(typeof(GrapeCity.Win.Editors.RoundPattern), roundPattern) :
                GrapeCity.Win.Editors.RoundPattern.Ceiling;
            return cell;
        }

        /// <summary>金額入力用セル取得</summary>
        /// <param name="fieldScale">小数点以下桁数（値部分）</param>
        /// <param name="displayScale">小数点以下桁数（表示部分）</param>
        /// <param name="roundPattern">端数処理</param>
        /// <param name="displayFormatString">小数点以下の表示書式</param>
        /// <returns></returns>
        public GcNumberCell GetNumberCellCurrencyInput(int fieldScale, int displayScale, int roundPattern, string displayFormatString = "0")
        {
            var cell = GetNumberCellCurrency(fieldScale, displayScale, roundPattern, displayFormatString);
            cell.MaxValue = 99999999999M;
            cell.MinValue = -99999999999M;
            cell.Fields.IntegerPart.MaxDigits = 11;
            return cell;
        }

        public GcNumberCell GetNumberCellFreeInput(decimal minValue, decimal maxValue, int maxDigit, bool allowDeleteToNull = true)
        {
            var fieldString = "#,###,###,###,##0";
            var displayFieldString = "#,###,###,###,##0";

            var cell = GetNumberCell(fieldFormat: fieldString, displayFormat: displayFieldString);
            cell.MinValue = minValue;
            cell.MaxValue = maxValue;
            cell.Fields.IntegerPart.MaxDigits = maxDigit;
            if (minValue >= 0) cell.ValueSign = GrapeCity.Win.Editors.ValueSignControl.Positive;
            cell.AllowDeleteToNull = allowDeleteToNull;
            return cell;
        }

        public GcDateTimeCell GetDateCell_yyyyMMdd(MultiRowContentAlignment align = MultiRowContentAlignment.MiddleCenter, bool isInput = false)
            => GetDateCellEditBase(align, isInput);

        public GcDateTimeCell GetDateCell_yyyyMM(MultiRowContentAlignment align = MultiRowContentAlignment.MiddleCenter, bool isInput = false)
            => GetDateCellEditBase(align, isInput, "yyyy/MM");

        public GcDateTimeCell GetDateCell_yyyyMMddHHmmss(MultiRowContentAlignment align = MultiRowContentAlignment.MiddleCenter, bool isInput = false)
            => GetDateCellEditBase(align, isInput, "yyyy/MM/dd HH:mm:ss");

        public GcDateTimeCell GetDateCell_MMdd(MultiRowContentAlignment align = MultiRowContentAlignment.MiddleCenter, bool isInput = false)
            => GetDateCellEditBase(align, isInput, "MM/dd");

        public GcDateTimeCell GetDateCell_yyyyMMdd_Jp(MultiRowContentAlignment align = MultiRowContentAlignment.MiddleCenter, bool isInput = false)
            => GetDateCellEditBase(align, isInput, "yyyy年MM月dd日");

        private GcDateTimeCell GetDateCellEditBase(MultiRowContentAlignment align, bool isInput, string format = "yyyy/MM/dd")
        {
            var cell = new GcDateTimeCell();
            cell.Fields.Clear();
            cell.DisplayFields.Clear();
            cell.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            cell.Spin.AllowSpin = false;
            cell.ShortcutKeys.Remove(Keys.F2);
            cell.ShortcutKeys.Remove(Keys.F4);
            cell.ShortcutKeys.Remove(Keys.F5);
            cell.SideButtons[0].BackColor = SystemColors.Control;
            cell.Style = GetDefaultCellStyle(align);
            if (isInput)
            {
                cell.Style.DataSourceNullValue = null;
            }
            else
            {
                cell.SideButtons.Clear();
            }
            if (!string.IsNullOrEmpty(format)) cell.Fields.AddRange(format);
            return cell;
        }

        /// <summary>
        /// <see cref="CheckBoxCell"/>を返す
        /// </summary>
        /// <param name="align"></param>
        /// <param name="isBoolType">Bind するプロパティが bool の場合は true 通常は int なので、false</param>
        /// <returns></returns>
        public CheckBoxCell GetCheckBoxCell(ContentAlignment align = ContentAlignment.MiddleCenter, bool isBoolType = false)
        {
            var cell = new CheckBoxCell();
            if (isBoolType)
            {
                cell.FalseValue = false;
                cell.TrueValue = true;
            }
            else
            {
                cell.FalseValue = 0;
                cell.TrueValue = 1;
            }
            cell.CheckAlign = align;
            cell.Style = GetDefaultCellStyle(MultiRowContentAlignment.MiddleLeft);
            return cell;
        }

        public ComboBoxCell GetComboBoxCell(MultiRowContentAlignment align = MultiRowContentAlignment.MiddleLeft)
        {
            var cell = new ComboBoxCell();
            cell.FlatStyle = FlatStyle.Flat;
            cell.DropDownStyle = MultiRowComboBoxStyle.DropDownList;
            cell.Style = GetDefaultCellStyle(align);
            return cell;
        }

        public ButtonCell GetButtonCell(MultiRowContentAlignment align = MultiRowContentAlignment.MiddleCenter)
        {
            return new ButtonCell() { Style = GetDefaultCellStyle(align) };
        }

        public HeaderCell GetHeaderCell(MultiRowContentAlignment align = MultiRowContentAlignment.MiddleCenter)
        {
            return new HeaderCell()
            {
                FlatStyle = FlatStyle.System,
                Style = new CellStyle
                {
                    BackColor = SystemColors.Control,
                    Font = Font,
                    TextAlign = align
                }
            };
        }

        public RowHeaderCell GetRowHeaderCell()
        {
            return new RowHeaderCell()
            {
                ResizeMode = ResizeMode.None,
                FlatStyle = FlatStyle.Flat,
                ShowIndicator = false,
                ValueFormat = "%1%",
                Style = new CellStyle()
                {
                    BackColor = SystemColors.Control,
                    Font = Font,
                    TextAlign = MultiRowContentAlignment.MiddleCenter,
                    Border = border,
                    TextEffect = TextEffect.Flat,
                    TextImageRelation = MultiRowTextImageRelation.ImageBeforeText,
                    TextIndent = 0,
                    WordWrap = MultiRowTriState.False
                }
            };
        }

        private CellStyle GetDefaultCellStyle(MultiRowContentAlignment align, ImeMode imeMode = ImeMode.Disable)
        {
            return new CellStyle()
            {
                Border = border,
                DisabledForeColor = Color.Black,
                Font = Font,
                ForeColor = Color.Black,
                ImeMode = imeMode,
                //InputScope = GetInputScope(imeMode),
                ImeSentenceMode = ImeSentenceMode.Normal,
                Padding = new Padding(2, -1, 2, -1),
                TextEffect = TextEffect.Flat,
                TextAlign = align,
                WordWrap = MultiRowTriState.False
            };
        }

        private InputScopeNameValue GetInputScope(ImeMode ime)
        {
            if (ime == ImeMode.Hiragana)
                return InputScopeNameValue.Hiragana;
            if (ime == ImeMode.KatakanaHalf)
                return InputScopeNameValue.KatakanaHalfWidth;
            return InputScopeNameValue.Default;
        }
        #endregion

        /// <summary>
        /// 数値型 <see cref="GcTextBoxCell"/> マイナス金額の赤字表示
        /// マイナス数字の場合、<see cref="NegativeForeColor"/>で表示
        /// </summary>
        public class GcNumberTextBoxCell : GcTextBoxCell
        {
            public Color NegativeForeColor { get; set; } = Color.Red;
            protected override void OnCellFormatting(CellFormattingEventArgs e)
            {
                base.OnCellFormatting(e);
                if ((e.Value is int     && (int)    e.Value < 0)    ||
                    (e.Value is long    && (long)   e.Value < 0L)   ||
                    (e.Value is decimal && (decimal)e.Value < 0M))
                {
                    e.CellStyle.ForeColor = NegativeForeColor;
                    e.CellStyle.SelectionForeColor = NegativeForeColor;
                }
            }
        }

    }
}
