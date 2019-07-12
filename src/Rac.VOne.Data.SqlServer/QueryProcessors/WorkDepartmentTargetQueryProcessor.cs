using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class WorkDepartmentTargetQueryProcessor :
        IWorkDepartmentTargetQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public WorkDepartmentTargetQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }
        public Task<int> DeleteAsync(byte[] ClientKey, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE WorkDepartmentTarget
 WHERE ClientKey = @ClientKey; ";
            return dbHelper.ExecuteAsync(query, new { ClientKey }, token);
        }

        public Task<int> SaveAsync(byte[] ClientKey, int CompanyId, int DepartmentId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO WorkDepartmentTarget
(ClientKey, CompanyId, DepartmentId, UseCollation)
VALUES (@ClientKey, @CompanyId, @DepartmentId, 1)";
            return dbHelper.ExecuteAsync(query, new { ClientKey, CompanyId, DepartmentId }, token);
        }
    }
}
