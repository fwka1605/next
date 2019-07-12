using Rac.VOne.Data.QueryProcessors;
using Models = Rac.VOne.Web.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class MatchingOutputedQueryProcessor :
        IAddMatchingOutputedQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public MatchingOutputedQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<int> SaveOutputAtAsync(long MatchingHeaderId, CancellationToken token)
        {
            var query = @"
MERGE INTO
    MatchingOutputed AS MOut 
USING ( 
        SELECT @MatchingHeaderId AS Id 
      ) AS Target 
   ON ( 
        MOut.MatchingHeaderId = @MatchingHeaderId 
      ) 
WHEN MATCHED AND MOut.MatchingHeaderId = @MatchingHeaderId THEN 
    UPDATE SET OutputAt = GETDATE() 
WHEN NOT MATCHED THEN 
    INSERT (
        MatchingHeaderId
        , OutputAt
        )
    VALUES (
        @MatchingHeaderId
        , GETDATE()
        )  
OUTPUT inserted.*; ";
            return dbHelper.ExecuteAsync(query, new { MatchingHeaderId }, token);
        }
    }
}
