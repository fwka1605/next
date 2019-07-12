using Rac.VOne.Client.Common.MultiRow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GcMultiRow = GrapeCity.Win.MultiRow;

namespace Rac.VOne.Client.Common
{
    public static class ControlExtension
    {

        /// <summary>コントロール一覧取得</summary>
        /// <typeparam name="TControl">コントロールの型 <see cref="Control"/></typeparam>
        /// <param name="root">子コントロールを取得する root control</param>
        /// <returns></returns>
        /// <remarks>
        /// コントロールに属するコントロールを型を指定して取得 再帰処理を利用しない
        /// http://stackoverflow.com/a/20124683
        /// without recursion
        /// </remarks>
        public static IEnumerable<TControl> GetAll<TControl>(this Control root) where TControl : Control
        {
            var stack = new Stack<Control>();
            stack.Push(root);
            while (stack.Any())
            {
                var next = stack.Pop();
                foreach (Control child in next.Controls)
                    stack.Push(child);
                if (next is TControl) yield return next as TControl;
            }
        }


        public static void InitializeColor(this Control control, IColors colors)
        {
            DoAllControls(control, c =>
            {
                if (c is Label || c is Panel || c is GroupBox || c is CheckBox || c is RadioButton
                    || c is TabPage)
                {
                    SetDefaultColor(c, colors);
                }
                else if (c is Button)
                {
                    c.BackColor = colors.ButtonBackColor;
                }
                else if (c is TextBoxBase || c is ListControl || c is GrapeCity.Win.Editors.EditBase)
                {
                    SetTextBoxColor(c, colors);
                }
                else if (c is GcMultiRow.GcMultiRow)
                {
                    SetGridColor(c, colors);
                }
            });
        }

        public static void InitializeFont(this Control control, string fontName)
        {
            DoAllControls(control, c =>
            {
                if ((c as IIgnoreableFontSet)?.IgnoreFontSet ?? false) return;
                c.Font = new Font(fontName, c.Font.Size, c.Font.Style);
            });
        }

        /// <summary>
        ///  自分含め、すべての子コントロールに <see cref="handler"/>の処理を実施
        /// </summary>
        /// <param name="root">親コントロール</param>
        /// <param name="handler">処理</param>
        /// <remarks>親から順に処理を行っていく</remarks>
        private static void DoAllControls(Control root, Action<Control> handler)
        {
            var queue = new Queue<Control>();
            queue.Enqueue(root);
            while (queue.Any())
            {
                var next = queue.Dequeue();
                foreach (Control child in next.Controls)
                    queue.Enqueue(child);
                if (next == null) break;
                handler?.Invoke(next);
            }
        }
        private static void SetDefaultColor(Control control, IColors colorContext)
        {
            control.BackColor = colorContext.FormBackColor;
            control.ForeColor = colorContext.FormForeColor;
        }
        private static void SetTextBoxColor(Control control, IColors colorContext)
        {
            var ignorable = control as IIgnoreableFocusChange;
            if (!(ignorable?.IgnoreFocusChange ?? false))
            {
                control.GotFocus += (sender, e)
                    => control.BackColor = colorContext.ControlActiveBackColor;
                control.LostFocus += (sender, e)
                    => control.BackColor = ((control as IRequired)?.Required ?? false)
                        ? colorContext.ControlRequiredBackColor
                        : colorContext.ControlEnableBackColor;
            }
            control.ForeColor = colorContext.ControlForeColor;

            var editBase = control as GrapeCity.Win.Editors.EditBase;
            if (editBase == null) return;
            editBase.DisabledBackColor = colorContext.ControlDisableBackColor;
            editBase.DisabledForeColor = colorContext.ControlForeColor;

            var required = control as IRequired;
            if (required == null)
            {
                editBase.BackColor = colorContext.ControlEnableBackColor;
                return;
            }

            editBase.BackColor = required.Required
                ? colorContext.ControlRequiredBackColor
                : colorContext.ControlEnableBackColor;
            required.RequiredChanged += (sender, e)
                => editBase.BackColor = required.Required
                    ? colorContext.ControlRequiredBackColor
                    : colorContext.ControlEnableBackColor;
        }
        private static void SetGridColor(Control control, IColors colorContext)
        {
            var grid = control as Controls.VOneGridControl;
            if (grid == null) return;

            grid.DefaultCellStyle.BackColor = colorContext.GridRowBackColor;
            grid.DefaultCellStyle.DisabledBackColor = colorContext.GridRowBackColor;
            grid.DefaultCellStyle.Border = new GcMultiRow.Border(GcMultiRow.LineStyle.Thin, colorContext.GridLineColor);
            switch (grid.GridColorType)
            {
                case GridColorType.General:
                    grid.AlternatingRowsDefaultCellStyle.BackColor = colorContext.GridAlternatingRowBackColor;
                    grid.AlternatingRowsDefaultCellStyle.DisabledBackColor = colorContext.GridAlternatingRowBackColor;
                    grid.AlternatingRowsDefaultCellStyle.Border = new GcMultiRow.Border(GcMultiRow.LineStyle.Thin, colorContext.GridLineColor);

                    grid.CurrentCellBorderLine = new GcMultiRow.Line(GcMultiRow.LineStyle.Thick, Color.Black);
                    grid.CurrentRowBorderLine = new GcMultiRow.Line(GcMultiRow.LineStyle.None, Color.Black);
                    break;
                case GridColorType.Input:
                    grid.DefaultCellStyle.BackColor = colorContext.InputGridBackColor;
                    grid.DefaultCellStyle.DisabledBackColor = colorContext.InputGridBackColor;
                    grid.DefaultCellStyle.SelectionBackColor = colorContext.InputGridBackColor;
                    grid.DefaultCellStyle.SelectionForeColor = colorContext.ControlForeColor;
                    grid.AlternatingRowsDefaultCellStyle.BackColor = colorContext.InputGridAlternatingBackColor;
                    grid.AlternatingRowsDefaultCellStyle.DisabledBackColor = colorContext.InputGridAlternatingBackColor;
                    grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = colorContext.InputGridAlternatingBackColor;
                    grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = colorContext.ControlForeColor;
                    grid.AlternatingRowsDefaultCellStyle.Border = new GcMultiRow.Border(GcMultiRow.LineStyle.Thin, colorContext.GridLineColor);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// テキストボックス未入力検証処理
        /// </summary>
        /// <param name="text"></param>
        /// <param name="messaging"></param>
        /// <returns>
        /// 未入力の確認 <see cref="string.IsNullOrWhiteSpace(string)"/>を実施し、
        /// 未入力の場合に <see cref="false"/>を返す
        /// </returns>
        public static bool ValidateInputted(this Controls.VOneTextControl text,
            Action messaging)
        {
            if (!string.IsNullOrWhiteSpace(text.Text)) return true;
            text.Focus();
            messaging?.Invoke();
            return false;
        }

        /// <summary>
        /// コンボボックス未入力検証処理
        /// </summary>
        /// <param name="combo"></param>
        /// <param name="messaging"></param>
        /// <returns></returns>
        public static bool ValidateInputted(this Controls.VOneComboControl combo,
            Action messaging)
        {
            if (combo.SelectedIndex >= 0) return true;
            combo.Focus();
            messaging?.Invoke();
            return false;
        }

        /// <summary>
        /// 日付型コントロール未入力検証処理
        /// </summary>
        /// <param name="dat"></param>
        /// <param name="messaging"></param>
        /// <returns></returns>
        public static bool ValidateInputted(this Controls.VOneDateControl dat,
            Action messaging)
        {
            if (dat.Value.HasValue) return true;
            dat.Focus();
            messaging?.Invoke();
            return false;
        }

        /// <summary>
        /// 数値型コントロール未入力検証処理
        /// </summary>
        /// <param name="nmb"></param>
        /// <param name="messaging"></param>
        /// <returns></returns>
        public static bool ValidateInputted(this Controls.VOneNumberControl nmb,
            Action messaging)
        {
            if (nmb.Value.HasValue) return true;
            nmb.Focus();
            messaging?.Invoke();
            return false;
        }

        /// <summary>検索などでの範囲検索用 検証処理</summary>
        /// <param name="text1"><see cref="Controls.VOneTextControl"/>テキストボックス1</param>
        /// <param name="text2"><see cref="Controls.VOneTextControl"/>テキストボックス2</param>
        /// <param name="messaging">メッセージ処理を行うハンドラ</param>
        /// <returns>
        /// 範囲検索で、text1, text2 どちらも入力があり、text2 の文字が text1より大きい場合に検証エラーとする
        /// コントロールの選択、メッセージ処理の順序
        /// </returns>
        public static bool ValidateRange(this Controls.VOneTextControl text1,
            Controls.VOneTextControl text2, Action messaging)
        {
            if (string.IsNullOrEmpty(text1.Text)
                || string.IsNullOrEmpty(text2.Text)
                || text1.Text.CompareTo(text2.Text) <= 0) return true;
            text1.Focus();
            messaging?.Invoke();
            return false;
        }

        /// <summary>検索などでの範囲検索用 検証処理</summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <param name="messaging"></param>
        /// <returns></returns>
        public static bool ValidateRange(this Controls.VOneDateControl date1,
            Controls.VOneDateControl date2, Action messaging)
        {
            if (!date1.Value.HasValue
                || !date2.Value.HasValue
                || date1.Value.Value.CompareTo(date2.Value.Value) <= 0) return true;
            date1.Focus();
            messaging?.Invoke();
            return false;
        }

        /// <summary>検索などでの範囲検索用 日時用検証処理</summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <param name="messaging"></param>
        /// <returns></returns>
        public static bool ValidateRange(this Controls.VOneDateTimeControl date1,
            Controls.VOneDateTimeControl date2, Action messaging)
        {
            if (!date1.Value.HasValue
                || !date2.Value.HasValue
                || date1.Value.Value.CompareTo(date2.Value.Value) <= 0) return true;
            date1.Focus();
            messaging?.Invoke();
            return false;
        }

        /// <summary>検索などでの範囲検索用 検証処理</summary>
        /// <param name="number1"></param>
        /// <param name="number2"></param>
        /// <param name="messaging"></param>
        /// <returns></returns>
        public static bool ValidateRange(this Controls.VOneNumberControl number1,
            Controls.VOneNumberControl number2, Action messaging)
        {
            if (!number1.Value.HasValue
                || !number2.Value.HasValue
                || number1.Value.Value.CompareTo(number2.Value.Value) <= 0) return true;
            number1.Focus();
            messaging?.Invoke();
            return false;
        }

        #region print value

        private const string NotInputted = "(指定なし)";
        private const string DateFormatYMD = "yyyy/MM/dd";
        /// <summary>日付の印刷用文字列取得</summary>
        /// <param name="date"></param>
        /// <returns>
        /// 未入力：(指定なし)
        /// 入力済：{yyyy/MM/dd}
        /// </returns>
        public static string GetPrintValue(this Controls.VOneDateControl date, string format = "")
            => date?.Value.HasValue ?? false
            ? date.Value.Value.ToString(string.IsNullOrEmpty(format) ? DateFormatYMD : format)
            : NotInputted;

        public static string GetPrintValue(this DateTime? date, string format = "")
             => date.HasValue
            ? date.Value.ToString(string.IsNullOrEmpty(format) ? DateFormatYMD : format)
            : NotInputted;

        /// <summary>テキストボックスの印刷用文字列取得</summary>
        /// <param name="text"></param>
        /// <returns>
        /// 未入力：(指定なし)
        /// 入力済：{text.Text}
        /// </returns>
        public static string GetPrintValue(this Controls.VOneTextControl text)
            => string.IsNullOrEmpty(text?.Text) ? NotInputted : text?.Text;

        /// <summary>コード、名称の印刷用文字列取得</summary>
        /// <param name="text"></param>
        /// <param name="label"></param>
        /// <returns>
        /// コード未入力：(指定なし)
        /// コードのみ入力：{text.Text}
        /// コード、名称指定：{text.Text}：{label.Text}
        /// </returns>
        public static string GetPrintValueCode(this Controls.VOneTextControl text,
            Controls.VOneDispLabelControl label)
            => string.IsNullOrEmpty(text?.Text)
            ? NotInputted
            : text?.Text
                + (string.IsNullOrEmpty(label?.Text) ? "" :  "：" + label.Text);

        /// <summary>コンボボックス 印刷用文字列取得</summary>
        /// <param name="combo"></param>
        /// <returns>
        /// 表示されている文字を返す SelectedIndex = -1 の場合は (指定なし)
        /// </returns>
        public static string GetPrintValue(this Controls.VOneComboControl combo)
            => combo.SelectedIndex == -1
            ? NotInputted
            : string.IsNullOrEmpty(combo.Text)
                ? Convert.ToString(combo.SelectedValue)
                : combo.Text;

        /// <summary>金額 印刷用文字列取得</summary>
        /// <param name="number"></param>
        /// <param name="format"></param>
        /// <returns>
        /// 未入力：(指定なし)
        /// 入力時：フォーマットに指定してある桁区切り表記の文字列
        /// </returns>
        public static string GetPrintValue(this Controls.VOneNumberControl number, string format = null)
            => number?.Value.HasValue ?? false
            ? number.Value.Value.ToString(format ?? "#,##0")
            : NotInputted;
            

        #endregion
    }
}
