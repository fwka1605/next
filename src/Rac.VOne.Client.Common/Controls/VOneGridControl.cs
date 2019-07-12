using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;

namespace Rac.VOne.Client.Common.Controls
{
    public partial class VOneGridControl : GcMultiRow
    {
        public VOneGridControl()
            : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 背景色設定
        /// </summary>
        [DefaultValue(MultiRow.GridColorType.General)]
        public MultiRow.GridColorType GridColorType { get; set; } = MultiRow.GridColorType.General;

        protected virtual void InitializeComponent()
        {
            AllowAutoExtend = true;
            AllowClipboard = true;
            AllowUserToAddRows = false;
            AllowUserToShiftSelect = true;
            MultiSelect = true;
            ScrollBars = ScrollBars.Both;
            HorizontalScrollMode = ScrollMode.Cell;
            VerticalScrollMode = ScrollMode.Row;
            SplitMode = SplitMode.None;
        }
        public void SetupShortcutKeys()
        {
            ShortcutKeyManager.Clear();
            ShortcutKeyManager.Register(EditingActions.Copy, Keys.Control | Keys.C);
            ShortcutKeyManager.Register(SelectionActions.MoveToNextCell, Keys.Tab);
            ShortcutKeyManager.Register(SelectionActions.MoveToPreviousCell, Keys.Tab | Keys.Shift);

            ShortcutKeyManager.Register(SelectionActions.MoveUp, Keys.Up);
            ShortcutKeyManager.Register(SelectionActions.MoveDown, Keys.Down);
            ShortcutKeyManager.Register(SelectionActions.MoveLeft, Keys.Left);
            ShortcutKeyManager.Register(SelectionActions.MoveRight, Keys.Right);
            ShortcutKeyManager.Register(SelectionActions.MoveToFirstCellInRow, Keys.Home);
            ShortcutKeyManager.Register(SelectionActions.MoveToLastCellInRow, Keys.End);
            ShortcutKeyManager.Register(SelectionActions.MoveToFirstRow, Keys.Up | Keys.Control);
            ShortcutKeyManager.Register(SelectionActions.MoveToLastRow, Keys.Down | Keys.Control);
            ShortcutKeyManager.Register(SelectionActions.MoveToFirstCellInRow, Keys.Left | Keys.Control);
            ShortcutKeyManager.Register(SelectionActions.MoveToLastCellInRow, Keys.Right | Keys.Control);
            ShortcutKeyManager.Register(SelectionActions.MoveToFirstCell, Keys.Home | Keys.Control);
            ShortcutKeyManager.Register(SelectionActions.MoveToLastCell, Keys.End | Keys.Control);

            ShortcutKeyManager.Register(SelectionActions.ShiftUp, Keys.Up | Keys.Shift);
            ShortcutKeyManager.Register(SelectionActions.ShiftDown, Keys.Down | Keys.Shift);
            ShortcutKeyManager.Register(SelectionActions.ShiftLeft, Keys.Left | Keys.Shift);
            ShortcutKeyManager.Register(SelectionActions.ShiftRight, Keys.Right | Keys.Shift);
            ShortcutKeyManager.Register(SelectionActions.ShiftToFirstCellInRow, Keys.Home | Keys.Shift);
            ShortcutKeyManager.Register(SelectionActions.ShiftToLastCellInRow, Keys.End | Keys.Shift);
            ShortcutKeyManager.Register(SelectionActions.ShiftToFirstRow, Keys.Up | Keys.Shift | Keys.Control);
            ShortcutKeyManager.Register(SelectionActions.ShiftToLastRow, Keys.Down | Keys.Shift | Keys.Control);
            ShortcutKeyManager.Register(SelectionActions.ShiftToFirstCellInRow, Keys.Left | Keys.Shift | Keys.Control);
            ShortcutKeyManager.Register(SelectionActions.ShiftToLastCellInRow, Keys.Right | Keys.Shift | Keys.Control);
            ShortcutKeyManager.Register(SelectionActions.ShiftToFirstCell, Keys.Home | Keys.Shift | Keys.Control);
            ShortcutKeyManager.Register(SelectionActions.ShiftToLastCell, Keys.End | Keys.Shift | Keys.Control);

            ShortcutKeyManager.Register(new MultiRow.Action.CheckSelectedCells(), Keys.Space);

            ShortcutKeyManager.DefaultModeList.Add(new ShortcutKey(SelectionActions.MoveToNextCell, Keys.Return));

            ShortcutKeyManager.Register(EditingActions.ShowDropDown, Keys.Alt | Keys.Down);
        }
    }
}
