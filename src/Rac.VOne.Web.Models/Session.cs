using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    public class Session
    {
        public string SessionKey { get; set; }
        public string ConnectionInfo { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
