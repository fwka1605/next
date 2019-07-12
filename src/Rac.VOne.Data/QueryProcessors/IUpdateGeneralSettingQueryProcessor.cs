using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;


namespace Rac.VOne.Data.QueryProcessors
{
    public interface IUpdateGeneralSettingQueryProcessor
    {
        Task<GeneralSetting> SaveAsync(GeneralSetting setting, CancellationToken token);
    }
}
