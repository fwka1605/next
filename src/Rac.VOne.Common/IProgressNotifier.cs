using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Common
{
    public interface IProgressNotifier
    {
        void UpdateState();
        void Abort();
    }
}
