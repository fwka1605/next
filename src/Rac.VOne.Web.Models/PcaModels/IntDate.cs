using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.PcaModels
{
    public class IntDate
    {
        public IntDate() { }
        public IntDate(DateTime value)
        {
            SetValue(value);
        }

        public int SerializeTarget { get; set; }

        public void SetValue(DateTime value)
            => SerializeTarget
            = value.Year  * 10000
            + value.Month * 100
            + value.Day;
    }
}
