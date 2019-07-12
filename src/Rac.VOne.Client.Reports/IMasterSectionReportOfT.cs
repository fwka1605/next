using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Reports
{
    public interface IMasterSectionReport<T>
    {
        void SetBasicPageSetting(string companyCode, string companyName);
        string Name { get; set; }
        void SetData(IEnumerable<T> items);
        void Run();

    }
}
