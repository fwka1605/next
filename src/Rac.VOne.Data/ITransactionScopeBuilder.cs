using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Rac.VOne.Data
{
    public interface ITransactionScopeBuilder
    {
        TransactionScope Create();
    }
}
