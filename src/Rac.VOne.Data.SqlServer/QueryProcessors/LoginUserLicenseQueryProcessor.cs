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
    public class LoginUserLicenseQueryProcessor : ILoginUserLicenseQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public LoginUserLicenseQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<LoginUserLicense> SaveAsync(LoginUserLicense license, CancellationToken token = default(CancellationToken))
        {
            string query = @"
MERGE INTO LoginUserLicense AS target
USING ( 
    SELECT 
     @CompanyId AS CompanyId
    ,@LicenseKey AS LicenseKey
) AS source
ON (
        target.CompanyId   = @CompanyId
    AND target.LicenseKey  = @LicenseKey
) 
WHEN MATCHED THEN
    UPDATE SET
        LicenseKey = @LicenseKey
WHEN NOT MATCHED THEN
    INSERT ( CompanyId,  LicenseKey)
    VALUES (@CompanyId, @LicenseKey)
OUTPUT inserted.*; ";
            return dbHelper.ExecuteAsync<LoginUserLicense>(query, license, token);
        }

    }
}
