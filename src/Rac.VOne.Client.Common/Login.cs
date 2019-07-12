using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Common
{
    public class Login : ILogin
    {
        public string CompanyCode { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string SessionKey { get; set; }
        public string UserCode { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
