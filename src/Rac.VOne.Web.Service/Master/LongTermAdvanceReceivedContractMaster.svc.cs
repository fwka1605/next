using Rac.VOne.Web.Models;
using System;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class LongTermAdvanceReceivedContractMaster : ILongTermAdvanceReceivedContractMaster
    {
        public Task<CountResult> DeleteAsync(string SessionKey, int Id)
        {
            throw new NotImplementedException();
        }

        public Task<LongTermAdvanceReceivedContractResult> GetAsync(string SessionKey, int Id)
        {
            throw new NotImplementedException();
        }

        public Task<LongTermAdvanceReceivedContractResult> GetByCodeAsync(string SessionKey, int Id, string Code)
        {
            throw new NotImplementedException();
        }

        public Task<LongTermAdvanceReceivedContractsResult> GetItemsAsync(string SessionKey, int CompanyId)
        {
            throw new NotImplementedException();
        }

        public Task<LongTermAdvanceReceivedContractResult> SaveAsync(string SessionKey, LongTermAdvanceReceivedContract longTermAdvanceReceivedContract)
        {
            throw new NotImplementedException();
        }
    }
}
