using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    public interface ITransactionData
    {
        long Id { get; set; }
        DateTime UpdateAt { get; set; }
    }
}
