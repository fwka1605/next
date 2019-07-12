using Rac.VOne.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IDeleteBankAccountQueryProcessor
    {
        int Delete(int Id);
    }
}
