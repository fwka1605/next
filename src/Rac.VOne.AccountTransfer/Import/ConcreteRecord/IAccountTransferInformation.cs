using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import.ConcreteRecord
{
    public interface IAccountTransferInformation
    {
        string TransferBankName {get;set;}
        string TransferBranchName { get; set; }
        string TransferCustomerCode { get; set; }
        string TransferAccountName { get; set; }
    }
}
