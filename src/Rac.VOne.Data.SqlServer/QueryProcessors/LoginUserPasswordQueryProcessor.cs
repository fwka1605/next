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
    public class LoginUserPasswordQueryProcessor :
        IAddLoginUserPasswordQueryProcessor,
        ILoginUserPasswordQueryProcessor,
        IDeleteLoginUserPasswordQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public LoginUserPasswordQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<LoginUserPassword> GetAsync(int CompanyId, int LoginUserId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT lup.*
  FROM LoginUserPassword lup
 INNER JOIN Company     cm
    ON cm.Id            = @CompanyId
 INNER JOIN LoginUser   lu
    ON lu.Id            = @LoginUserId
   AND lu.CompanyId     = cm.Id
   AND lu.Id            = lup.LoginUserId
";
            return dbHelper.ExecuteAsync<LoginUserPassword>(query, new { CompanyId, LoginUserId }, token);
        }

        public Task<int> DeleteAsync(int LoginUserId, CancellationToken token = default(CancellationToken))
        {
            var query = @"DELETE LoginUserPassword WHERE LoginUserId = @LoginUserId";
            return dbHelper.ExecuteAsync(query, new { LoginUserId }, token);
        }

        public Task<LoginUserPassword> SaveAsync(LoginUserPassword password, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO LoginUserPassword target
USING (SELECT @LoginUserId        LoginUserId
      ) source
   ON (    target.LoginUserId       = source.LoginUserId
      )
 WHEN MATCHED THEN
      UPDATE SET
             PasswordHash           = @PasswordHash
           , UpdateAt               = GETDATE()
           , PasswordHash0          = @PasswordHash0
           , PasswordHash1          = @PasswordHash1
           , PasswordHash2          = @PasswordHash2
           , PasswordHash3          = @PasswordHash3
           , PasswordHash4          = @PasswordHash4
           , PasswordHash5          = @PasswordHash5
           , PasswordHash6          = @PasswordHash6
           , PasswordHash7          = @PasswordHash7
           , PasswordHash8          = @PasswordHash8
           , PasswordHash9          = @PasswordHash9
 WHEN NOT MATCHED THEN
      INSERT ( LoginUserId,  PasswordHash, UpdateAt
           ,  PasswordHash0, PasswordHash1
           ,  PasswordHash2, PasswordHash3
           ,  PasswordHash4, PasswordHash5
           ,  PasswordHash6, PasswordHash7
           ,  PasswordHash8, PasswordHash9
            )
      VALUES (@LoginUserId, @PasswordHash, GETDATE()
           , @PasswordHash0,@PasswordHash1
           , @PasswordHash2,@PasswordHash3
           , @PasswordHash4,@PasswordHash5
           , @PasswordHash6,@PasswordHash7
           , @PasswordHash8,@PasswordHash9
            )
      OUTPUT inserted.*; ";
            return dbHelper.ExecuteAsync<LoginUserPassword>(query, password, token);
        }
    }
}
