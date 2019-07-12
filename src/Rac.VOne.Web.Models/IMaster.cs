using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    public interface IMaster : IByCompany, IIdentical
    {
        string Code { get; set; }
    }
}
