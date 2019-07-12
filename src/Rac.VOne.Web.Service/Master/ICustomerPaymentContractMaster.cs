﻿using System.ServiceModel;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface ICustomerPaymentContractMaster
    {
        [OperationContract]
        Task<ExistResult> ExistAsync(int Id, string SessionKey);
    }
}