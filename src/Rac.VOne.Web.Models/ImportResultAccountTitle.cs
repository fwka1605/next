using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ImportResultAccountTitle : ImportResult
    {
        [DataMember] public List<AccountTitle> AccountTitles { get; set; } = new List<AccountTitle>();
    }
}
