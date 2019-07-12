using System.Text;
using System.Linq;
using System.Collections.Generic;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class SettingQueryProcessor :
        ISettingQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public SettingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<Setting>> GetAsync(IEnumerable<string> ItemId, CancellationToken token = default(CancellationToken))
        {
            var query = new StringBuilder(@"
SELECT [ItemId]
     , [ItemKey]
     , [ItemValue]
  FROM [dbo].[Setting ]");
            if (ItemId?.Any() ?? false)
                query.Append(@"
 WHERE [ItemId] IN (SELECT Code FROM @ItemId)");
            query.Append(@"
 ORDER BY ItemId ASC, ItemKey");
            return dbHelper.GetItemsAsync<Setting>(query.ToString(), new { ItemId = ItemId.GetTableParameter() }, token);
        }

    }
}
