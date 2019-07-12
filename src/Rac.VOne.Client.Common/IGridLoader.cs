using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;

namespace Rac.VOne.Client.Common
{
    public interface IGridLoader<T>
    {
        IApplication Application { get; }
        Template CreateGridTemplate();
        Task<IEnumerable<T>> SearchInfo();
        Task<IEnumerable<T>> SearchByKey(params string[] keys);
    }
}
