using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class CustomerGroupQueryProcessor :
        ICustomerGroupByIdQueryProcessor,
        IAddCustomerGroupQueryProcessor,
        IDeleteCustomerGroupQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public CustomerGroupQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<CustomerGroup>> GetAsync(CustomerGroupSearch option, CancellationToken token = default(CancellationToken))
        {
            if (option.RequireSingleCusotmerRelation)
                return GetCustomerForCustomerGroupAsync(option, token);

            var query = @"
SELECT        cg.*
            , pcs.[Code]            [ParentCustomerCode]
            , pcs.[Name]            [ParentCustomerName]
            , pcs.[Kana]            [ParentCustomerKana]
            , ccs.[Code]            [ChildCustomerCode]
            , ccs.[Name]            [ChildCustomerName]
FROM        CustomerGroup cg
INNER JOIN  Customer pcs            ON pcs.Id       = cg.ParentCustomerId
INNER JOIN  Customer ccs            ON ccs.Id       = cg.ChildCustomerId
WHERE       cg.ParentCustomerId     = cg.ParentCustomerId";
            if (option.CompanyId.HasValue) query += @"
AND         pcs.CompanyId           = @CompanyId";
            if (option.ParentIds?.Any() ?? false) query += @"
AND         cg.ParentCustomerId     IN (SELECT Id   FROM @ParentIds)";

            if (option.ChildIds?.Any() ?? false) query += @"
AND         cg.ChildCustomerId      IN (SELECT Id   FROM @ChildIds)

UNION

SELECT        cs.Id                 [ParentCustomerId]
            , cs.Id                 [ChildCustomerId]
            , cs.[Code]             [ParentCustomerCode]
            , cs.[Name]             [ParentCustomerName]
            , cs.[Kana]             [ParentCustomerKana]
            , cs.[Code]             [ChildCustomerCode]
            , cs.[Name]             [ChildCustomerName]
FROM        Customer cs
WHERE       cs.Id                   IN (SELECT Id   FROM @ChildIds)
AND         cs.IsParent             = 1";

            return dbHelper.GetItemsAsync<CustomerGroup>(query, new {
                            option.CompanyId,
                ParentIds = option.ParentIds.GetTableParameter(),
                ChildIds  = option.ChildIds.GetTableParameter(),
            }, token);
        }

        private Task<IEnumerable<CustomerGroup>> GetCustomerForCustomerGroupAsync(CustomerGroupSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT        cs.Id                            [ChildCustomerId]
            , cs.Code                          [ChildCustomerCode]
            , cs.Name                          [ChildCustomerName]
            , COALESCE(pcs.Code, cs.Code)      [ParentCustomerCode]
            , COALESCE(pcs.Name, cs.Name)      [ParentCustomerName]
            , COALESCE(pcs.Id,
                CASE cs.IsParent WHEN 1 THEN cs.Id ELSE 0 END)
                                               [ParentCustomerId]
FROM        Customer cs
LEFT JOIN   CustomerGroup cg            ON cg.ChildCustomerId   = cs.Id
LEFT JOIN   Customer pcs                ON cg.ParentCustomerId  = pcs.Id
WHERE       cs.CompanyId                = @CompanyId
AND         cs.Code                     = @Code
";
            return dbHelper.GetItemsAsync<CustomerGroup>(query, option, token);
        }


        public async Task<bool> ExistCustomerAsync(int CustomerId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
 WHERE EXISTS(
       SELECT 1
         FROM CustomerGroup
        WHERE  ParentCustomerId = @CustomerId
           OR ChildCustomerId = @CustomerId)";
            return (await dbHelper.ExecuteAsync<int?>(query, new { CustomerId }, token)).HasValue;
        }

        public async Task<bool> HasChildAsync(int ParentCustomerId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT 1
 WHERE EXISTS (
       SELECT 1
         FROM CustomerGroup csg
        WHERE csg.ParentCustomerId  = @ParentCustomerId ) ";
            return (await dbHelper.ExecuteAsync<int?>(query, new { ParentCustomerId }, token)).HasValue;
        }

        public Task<int> GetUniqueGroupCountAsync(IEnumerable<int> Ids, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT COUNT(DISTINCT u.ParentCustomerId) FROM
(
 SELECT csg.ParentCustomerId
  FROM CustomerGroup csg
 WHERE ParentCustomerId IN (SELECT Id FROM @Ids)
UNION ALL
 SELECT csg.ParentCustomerId
  FROM CustomerGroup csg
 WHERE ChildCustomerId IN (SELECT Id FROM @Ids)
)u
";
            return dbHelper.ExecuteAsync<int>(query, new { Ids = Ids.GetTableParameter() }, token);
        }

        private string GetMergeQuery()
            => @"
MERGE INTO CustomerGroup AS Target
USING (
    SELECT
        @ParentCustomerId   ParentCustomerId
      , @ChildCustomerId    ChildCustomerId
      , @UpdateAt           UpdateAt
) AS Source
ON (
        Target.ParentCustomerId = Source.ParentCustomerId
    AND Target.ChildCustomerId  = Source.ChildCustomerId
)
WHEN MATCHED THEN
    UPDATE
       SET ParentCustomerId = @ParentCustomerId
         , ChildCustomerId  = @ChildCustomerId
         , UpdateBy         = @UpdateBy
         , UpdateAt         = GETDATE()
WHEN NOT MATCHED THEN
    INSERT ( ParentCustomerId,  ChildCustomerId,  CreateBy, CreateAt,   UpdateBy, UpdateAt)
    VALUES (@ParentCustomerId, @ChildCustomerId, @UpdateBy, GETDATE(), @UpdateBy, GETDATE())
OUTPUT inserted.*;
";

        public Task<CustomerGroup> SaveAsync(CustomerGroup group, CancellationToken token = default(CancellationToken))
            => dbHelper.ExecuteAsync<CustomerGroup>(GetMergeQuery(), group, token);


        public Task<int> DeleteAsync(CustomerGroup group, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE      CustomerGroup
WHERE       ParentCustomerId    = @ParentCustomerId
AND         ChildCustomerId     = @ChildCustomerId 
AND         UpdateAt            = @UpdateAt";
            return dbHelper.ExecuteAsync(query, group, token);
        }

    }
}
