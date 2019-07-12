using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IDeleteBillingBalanceBackQueryProcessor
    {
        int Delete(int CompanyId);
    }
}
