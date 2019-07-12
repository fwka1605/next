using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client
{
    public interface IApplicationUsable
    {
        IApplication ApplicationContext { get; }
        System.Windows.Forms.Form ParentForm { get; }
    }
}
