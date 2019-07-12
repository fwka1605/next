using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class WorkSectionTargetQueryProcessor :
        IWorkSectionTargetQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public WorkSectionTargetQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }
        public Task<int> DeleteAsync(byte[] ClientKey, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE WorkSectionTarget
 WHERE ClientKey = @ClientKey; ";
            return dbHelper.ExecuteAsync(query, new { ClientKey }, token);
        }

        public Task<int> SaveAsync(byte[] ClientKey, int CompanyId, int SectionId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO WorkSectionTarget
(ClientKey, CompanyId, SectionId, UseCollation)
VALUES (@ClientKey, @CompanyId, @SectionId, 1)";
            return dbHelper.ExecuteAsync(query, new { ClientKey, CompanyId, SectionId }, token);
        }
    }
}
