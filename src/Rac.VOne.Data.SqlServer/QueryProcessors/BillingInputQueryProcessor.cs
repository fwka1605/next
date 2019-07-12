using System;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class BillingInputQueryProcessor :
        IAddBillingInputQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public BillingInputQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<BillingInput> AddAsync(CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO BillingInput
OUTPUT inserted.*
 DEFAULT VALUES";
            return dbHelper.ExecuteAsync<BillingInput>(query, cancellationToken: token);
        }


        public Task<BillingInput> UpdateForPublishAsync(BillingInputSource source, CancellationToken token = default(CancellationToken))
        {
            var query = new StringBuilder(@"
UPDATE BillingInput
SET PublishAt               = GETDATE()");
            if (source.IsFirstPublish) query.Append(@"
  , PublishAt1st            = GETDATE()");
            if (source.InvoiceTemplateId.HasValue) query.AppendLine(@"
  , InvoiceTemplateId       = @InvoiceTemplateId");
            if (source.UseInvoiceCodeNumbering.HasValue) query.Append(@"
  , UseInvoiceCodeNumbering = @UseInvoiceCodeNumbering");
            query.Append(@"
OUTPUT INSERTED.*
WHERE Id = @Id");
            return dbHelper.ExecuteAsync<BillingInput>(query.ToString(), source, token);
        }

        public Task<int> UpdateForCancelPublishAsync(IEnumerable<long> Ids, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE BillingInput
   SET PublishAt               = NULL
     , PublishAt1st            = NULL
     , InvoiceTemplateId       = NULL
     , UseInvoiceCodeNumbering = 0
OUTPUT INSERTED.*
  FROM BillingInput bi
 WHERE bi.Id IN (SELECT Id FROM @Ids)
";
            return dbHelper.ExecuteAsync(query, new { Ids = Ids.GetTableParameter() }, token);
        }

        public Task<int> DeleteForCancelPublishAsync(IEnumerable<long> Ids, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE bi
  FROM BillingInput bi
  LEFT JOIN Billing b
    ON b.BillingInputId = bi.Id
 WHERE bi.Id IN (SELECT Id FROM @Ids)
   AND b.InputType = 1
";/* TODO:confirm where ? innter join と変わりが無くなる */
            return dbHelper.ExecuteAsync(query, new { Ids = Ids.GetTableParameter() }, token);
        }

    }
}
