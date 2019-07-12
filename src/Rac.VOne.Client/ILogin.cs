using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client
{
    public interface ILogin
    {
        int CompanyId { get; set; }
        string CompanyCode { get; set; }
        string CompanyName { get; set; }
        int UserId { get; set; }
        string UserCode { get; set; }
        string UserName { get; set; }
        string SessionKey { get; set; }
    }
}
