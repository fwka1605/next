using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class DestinationQueryProcessor :
        IDestinationQueryProcessor,
        IAddDestinationQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public DestinationQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<Destination>> GetAsync(DestinationSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = new StringBuilder($@"
SELECT d.*
  FROM Destination d
 WHERE d.Id         = d.Id
   AND d.CompanyId  = @CompanyId
{(option.CustomerId.HasValue     ? "   AND d.CustomerId = @CustomerId"               : string.Empty)}
{((option.Codes?.Any() ?? false) ? "   AND d.Code      IN (SELECT Code FROM @Codes)" : string.Empty)}
");
            if (!string.IsNullOrEmpty(option.Name))
            {
                option.Name = Sql.GetWrappedValue(option.Name);
                query.Append(@"
   AND (   d.Code           LIKE @Name
        OR d.Name           LIKE @Name
        OR d.PostalCode     LIKE @Name
        OR d.Address1       LIKE @Name
        OR d.Address2       LIKE @Name
        OR d.DepartmentName LIKE @Name
        OR d.Addressee      LIKE @Name
        OR d.Honorific      Like @Name
       )");
            }
            query.Append(@"
 ORDER BY d.CompanyId, d.CustomerId, d.Code");

            return dbHelper.GetItemsAsync<Destination>(query.ToString(), new {
                            option.CompanyId,
                            option.CustomerId,
                            option.Name,
                Codes   =   option.Codes.GetTableParameter(),
            }, token);
        }


        public Task<Destination> SaveAsync(Destination destination, CancellationToken token = default(CancellationToken))
        {
            #region merge query
            var query = @"
MERGE INTO Destination target
USING (
    SELECT @CompanyId   [CompanyId]
         , @CustomerId  [CustomerId]
         , @Code        [Code]
) source
ON    (
        target.CompanyId    = source.CompanyId
    AND target.CustomerId   = source.CustomerId
    AND target.Code         = source.Code
)
WHEN MATCHED THEN
    UPDATE SET 
         Code = @Code
        ,Name       = @Name
        ,PostalCode = @PostalCode
        ,Address1 = @Address1
        ,Address2 = @Address2
        ,DepartmentName = @DepartmentName
        ,Addressee = @Addressee
        ,Honorific = @Honorific
        ,UpdateBy = @UpdateBy
        ,UpdateAt = GETDATE()
WHEN NOT MATCHED THEN 
    INSERT (CompanyId, CustomerId, Name, Code, PostalCode, Address1, Address2, DepartmentName, Addressee, Honorific, CreateBy, CreateAt, UpdateBy, UpdateAt) 
    VALUES (@CompanyId, @CustomerId, @Name, @Code, @PostalCode, @Address1, @Address2, @DepartmentName, @Addressee, @Honorific, @UpdateBy, GETDATE(), @UpdateBy, GETDATE()) 
OUTPUT inserted.*; ";
            #endregion

            return dbHelper.ExecuteAsync<Destination>(query, destination, token);
        }

    }
}
