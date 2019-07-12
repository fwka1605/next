using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client
{
    public interface IScreenColors
    {
        void SetColorsContext(IColors context);

        void InitializeColors();
    }
}
