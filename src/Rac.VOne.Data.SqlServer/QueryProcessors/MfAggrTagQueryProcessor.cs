using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class MfAggrTagQueryProcessor :
        IAddMfAggrTagQueryProcessor,
        IMfAggrTagQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public MfAggrTagQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<MfAggrTag> AddAsync(MfAggrTag tag, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO MfAggrTag target
USING (
    SELECT @Id [Id]
) source ON (
        target.Id           = source.Id
)
WHEN MATCHED THEN
    UPDATE SET
           Name             = @Name
WHEN NOT MATCHED THEN
    INSERT
         ( Id
         , Name
         )
    VALUES
         ( @Id
         , @Name )
OUTPUT inserted.*;";
            return dbHelper.ExecuteAsync<MfAggrTag>(query, tag, token);
        }

        public Task<IEnumerable<MfAggrTag>> GetAsync(CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      *
FROM        [dbo].[MfAggrTag]
ORDER BY    [Id]        ASC";
            return dbHelper.GetItemsAsync<MfAggrTag>(query, cancellatioToken: token);
        }
    }
}
