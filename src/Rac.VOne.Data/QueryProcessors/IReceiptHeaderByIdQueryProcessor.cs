using Rac.VOne.Web.Models;
using System.Collections.Generic;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IReceiptHeaderByIDQueryProcessor
    {
        IEnumerable<ReceiptHeader> GetHeaderItems(int CompanyId);
    }
}
