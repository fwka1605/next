using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface ITaskScheduleMaster
    {
        [OperationContract]
        Task<TaskScheduleResult> SaveAsync(string SessionKey, TaskSchedule TaskSchedule);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey, int CompanyId, int ImportType, int ImportSubType);

        [OperationContract]
        Task<TaskScheduleResult> GetAsync(string SessionKey, int Id);

        [OperationContract]
        Task<TaskSchedulesResult> GetItemsAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<ExistResult> ExistsAsync(string SessionKey, int CompanyId, int ImportType, int ImportSubType);
    }
}