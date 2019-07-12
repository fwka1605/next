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
    public class PaymentFileFormatQueryProcessor :
        IPaymentFileFormatQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public PaymentFileFormatQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<PaymentFileFormat>> GetAsync(CancellationToken token = default(CancellationToken))
        {
            var query = @"SELECT * FROM PaymentFileFormat";
            return dbHelper.GetItemsAsync<PaymentFileFormat>(query, token);
        }

    }
}
