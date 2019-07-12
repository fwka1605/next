using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrapeCity.Win.MultiRow;

namespace Rac.VOne.Client.Common.MultiRow.Action
{
    public class CheckSelectedCells : GrapeCity.Win.MultiRow.Action
    {
        public override bool CanExecute(GcMultiRow target)
        {
            return ((target != null) && (target.SelectedCells != null) && target.SelectedCells.Count > 0);
        }

        protected override void OnExecute(GcMultiRow target)
        {
            if (target == null || target.SelectedCells == null || target.SelectedCells.Count == 0) return;
            target.EndEdit();

            bool? check = null;
            foreach (var c in target.SelectedCells)
            {
                var cbox = c as CheckBoxCell;
                if (cbox == null || !cbox.Enabled || cbox.ReadOnly) continue;
                if (check == null) check = Convert.ToBoolean(cbox.Value);
                if (cbox.Value is int)
                {
                    cbox.Value = check.Value ? 0 : 1;
                }
                else
                {
                    cbox.Value = !check;
                }
            }
        }
    }
}
