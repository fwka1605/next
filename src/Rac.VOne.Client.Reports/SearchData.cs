using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Reports
{
    public class SearchData
    {
        public string SearchName { get; set; }
        public string SearchValue { get; set; }
        public SearchData() { }
        public SearchData(string Name, string Value)
        {
            SearchName = Name;
            SearchValue = Value;
        }
    }
}
