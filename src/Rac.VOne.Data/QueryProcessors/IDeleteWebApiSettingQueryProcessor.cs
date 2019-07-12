using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Rac.VOne.Data.QueryProcessors
{
    public interface IDeleteWebApiSettingQueryProcessor
    {
        Task<int> DeleteAsync(int companyId, int? apiTypeId, CancellationToken token = default(CancellationToken));
    }
}
