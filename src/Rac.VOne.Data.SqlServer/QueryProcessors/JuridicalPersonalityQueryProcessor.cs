using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class JuridicalPersonalityQueryProcessor :
        IJuridicalPersonalityQueryProcessor,
        IAddJuridicalPersonalityQueryProcessor,
        IDeleteJuridicalPersonalityQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public JuridicalPersonalityQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }
        public Task<IEnumerable<JuridicalPersonality>> GetAsync(JuridicalPersonality personality, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      jp.*
FROM        JuridicalPersonality jp
WHERE       jp.CompanyId    = @CompanyId";
            if (!string.IsNullOrWhiteSpace(personality.Kana)) query += @"
AND         jp.Kana         = @Kana";
            query += @"
ORDER BY    jp.CompanyId    ASC
          , jp.Kana         ASC";
            return dbHelper.GetItemsAsync<JuridicalPersonality>(query, personality, token);
        }



        public Task<int> DeleteAsync(int CompanyId, string Kana, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE [dbo].[JuridicalPersonality]
 WHERE CompanyId  = @CompanyId
   AND Kana       = @Kana";
            return dbHelper.ExecuteAsync(query, new { CompanyId, Kana }, token);
        }


        public Task<int> InitializeAsync(int companyId, int loginUserId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO JuridicalPersonality
(CompanyId, Kana, CreateBy, CreateAt, UpdateBy, UpdateAt)
SELECT
  @CompanyId
, Kana
, @LoginUserId
, GETDATE()
, @LoginUserId
, GETDATE()
FROM JuridicalPersonalityBase";
            return dbHelper.ExecuteAsync(query, new { CompanyId = companyId, LoginUserId = loginUserId }, token);
        }

        public Task<JuridicalPersonality> SaveAsync(JuridicalPersonality personality, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO JuridicalPersonality AS target
USING (
    SELECT  @CompanyId      AS CompanyId
          , @Kana           AS Kana
     ) AS source ON (
            target.CompanyId    = source.CompanyId
        AND target.Kana         = source.Kana
)
      WHEN MATCHED THEN
         UPDATE SET
         UpdateBy       = @UpdateBy
        ,UpdateAt       = GETDATE()
     WHEN NOT MATCHED THEN
         INSERT ( CompanyId,  Kana,  CreateBy,  CreateAt,  UpdateBy,  UpdateAt)
         VALUES (@CompanyId, @Kana, @CreateBy, GETDATE(), @UpdateBy, GETDATE())
     OUTPUT inserted.*;";
            return dbHelper.ExecuteAsync<JuridicalPersonality>(query, personality, token);
        }

    }
}
