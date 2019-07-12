using Rac.VOne.Web.Models;
using System.Collections.Generic;

namespace Rac.VOne.AccountTransfer.Export
{
    public interface IAccountTransferExporter
    {
        void Export(string path, IEnumerable<AccountTransferDetail> source);
    }

}
