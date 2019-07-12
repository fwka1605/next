using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class StaffQueryProcessor :
        IStaffQueryProcessor,
        IAddStaffQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public StaffQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }
        public Task<IEnumerable<Staff>> GetAsync(StaffSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT        st.*
            , dp.Code   DepartmentCode
            , dp.Name   DepartmentName
FROM        Staff st
LEFT JOIN   Department dp       ON dp.Id = st.DepartmentId
WHERE       st.Id           = st.Id";
            if (option.CompanyId.HasValue) query += @"
AND         st.CompanyId    = @CompanyId";
            if (option.Ids?.Any() ?? false) query += @"
AND         st.Id           IN (SELECT Id   FROM @Ids)";
            if (option.Codes?.Any() ?? false) query += @"
AND         st.Code         IN (SELECT Code FROM @Codes)";
            if (!string.IsNullOrWhiteSpace(option.Name))
            {
                option.Name = Sql.GetWrappedValue(option.Name);
                query += @"
AND         st.Name         LIKE @Name";
            }
            if (option.DepartmentId.HasValue) query += @"
AND         st.DepartmentId = @DepartmentId";
            if (!string.IsNullOrWhiteSpace(option.Mail)) query += @"
AND         st.Mail         = @Mail";
            query += @"
ORDER BY      st.CompanyId      ASC
            , st.Code           ASC";
            return dbHelper.GetItemsAsync<Staff>(query, new {
                            option.CompanyId,
                            option.Name,
                            option.DepartmentId,
                            option.Mail,
                Ids     =   option.Ids.GetTableParameter(),
                Codes   =   option.Codes.GetTableParameter(),
            }, token);
        }


        public Task<Staff> SaveAsync(Staff Staff, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO Staff AS target 
USING ( 
    SELECT 
                  @CompanyId    AS CompanyId 
                , @Code         AS Code 
) AS source 
ON ( 
        target.CompanyId    = source.CompanyId
    AND target.Code         = source.Code
) 
WHEN MATCHED THEN 
    UPDATE SET 
              Code          = @Code
            , Name          = @Name
            , DepartmentId  = @DepartmentId
            , Mail          = @Mail
            , Tel           = @Tel
            , Fax           = @Fax
            , UpdateBy      = @UpdateBy
            , UpdateAt      = GETDATE()
WHEN NOT MATCHED THEN 
    INSERT ( CompanyId,  Code,  Name,  DepartmentId,  Mail,  Tel,  Fax,  CreateBy,  CreateAt,  UpdateBy,  UpdateAt)
    VALUES (@CompanyId, @Code, @Name, @DepartmentId, @Mail, @Tel, @Fax, @UpdateBy, GETDATE(), @UpdateBy, GETDATE())
OUTPUT inserted.*; ";

            return dbHelper.ExecuteAsync<Staff>(query, Staff, token);
        }

        public Task<IEnumerable<MasterData>> GetImportItemsLoginUserAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
        {
            var query =
                    @"SELECT * FROM Staff s
                          WHERE s.CompanyId = @CompanyId
                          AND EXISTS (
                               SELECT *
                                 FROM LoginUser login
                                 WHERE login.CompanyId = @CompanyId
                                 AND login.AssignedStaffId = s.Id )
                           AND s.Code NOT IN (SELECT Code FROM @Code ) ";
            return dbHelper.GetItemsAsync<MasterData>(query, new { CompanyId, Code = Code.GetTableParameter() }, token);
        }

        public Task<IEnumerable<MasterData>> GetImportItemsCustomerAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
        {
            var query =
                    @"SELECT * FROM Staff s
                          WHERE s.CompanyId = @CompanyId
                          AND EXISTS (
                               SELECT *
                                 FROM Customer customer
                                 WHERE customer.CompanyId = @CompanyId
                                 AND customer.StaffId = s.Id )
                           AND s.Code NOT IN (SELECT Code FROM @Code ) ";
            return dbHelper.GetItemsAsync<MasterData>(query, new { CompanyId, Code = Code.GetTableParameter() }, token);
        }

        public Task<IEnumerable<MasterData>> GetImportItemsBillingAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
        {
            var query =
                    @"SELECT * FROM Staff s
                          WHERE s.CompanyId = @CompanyId
                          AND EXISTS (
                               SELECT *
                                 FROM Billing b
                                 WHERE b.CompanyId = @CompanyId
                                 AND b.StaffId = s.Id )
                           AND s.Code NOT IN (SELECT Code FROM @Code) ";
            return dbHelper.GetItemsAsync<MasterData>(query, new { CompanyId, Code = Code.GetTableParameter() }, token);
        }


        public async Task<bool> ExistDepartmentAsync(int DepartmentId, CancellationToken token = default(CancellationToken))
        {
            var query = @"SELECT 1
                               WHERE EXISTS(
                              SELECT 1 
                                FROM Staff
                               WHERE DepartmentId = @DepartmentId)";
            return (await dbHelper.ExecuteAsync<int?>(query, new { DepartmentId }, token)).HasValue;
        }

    }
}
