using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class DataMaintenanceQueryProcessor : IDataMaintenanceQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public DataMaintenanceQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        /// <summary>
        /// 指定日以前の請求データを削除する。
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public Task<int> DeleteBillingBeforeAsync(DateTime date, CancellationToken token = default(CancellationToken))
        {
            var query = $@"
DELETE
    Billing
WHERE
        DeleteAt IS NOT NULL    -- 論理削除済み
    AND AssignmentFlag = 0      -- 未消込
    AND InputType != 3          -- 期日入金予定ではない
    AND DueAt <= @date          -- 入金予定日が削除対象日以前
";
            return dbHelper.ExecuteAsync(query, new { date }, token);
        }


        /// <summary>
        /// 指定日以前の入金データを削除する。
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<int> DeleteReceiptBeforeAsync(DateTime date, CancellationToken token = default(CancellationToken))
        {
            // 削除対象の Receipt.Id リストを取得
            var query = $@"
SELECT
    Id
FROM
    Receipt r
WHERE
        DeleteAt IS NOT NULL        -- 論理削除済み
    AND AssignmentFlag = 0          -- 未消込
    AND EXISTS (SELECT 1 FROM ReceiptSectionTransfer WHERE SourceReceiptId != r.Id AND DestinationReceiptId != r.Id) -- 入金部門振替していない
    AND OriginalReceiptId IS NULL   -- 前受振替していない
    AND RecordedAt <= @date         -- 入金日が削除対象日以前
";
            var receiptIdList = (await dbHelper.GetItemsAsync<long>(query, new { date }, token)).ToArray();
            var countR = 0;
            var countRE = 0;
            if (receiptIdList.Any())
            {
                // Receipt.Id と紐付く ReceiptExclude を削除 (参照制約が付いているので先に削除する)
                countRE = await dbHelper.ExecuteAsync("DELETE ReceiptExclude WHERE ReceiptId IN (SELECT Id FROM @receiptIdList)",
                    new { receiptIdList = receiptIdList.GetTableParameter() }, token);

                // Receipt を削除
                countR = await dbHelper.ExecuteAsync("DELETE Receipt WHERE Id IN (SELECT Id FROM @receiptIdList)",
                    new { receiptIdList = receiptIdList.GetTableParameter() }, token);
            }

            // ReceiptHeader をクリーンアップ
            // 紐付く Receipt が1件も存在しない ReceiptHeader を削除 ※ 全体が対象 (削除対象の Receipt.Id リストとは無関係)
            var countRH = await dbHelper.ExecuteAsync(
                "DELETE rh FROM ReceiptHeader rh WHERE NOT EXISTS (SELECT 1 FROM Receipt WHERE ReceiptHeaderId = rh.Id)", null, token);

            // ImportFileLog をクリーンアップ
            // 紐付く ReceiptHeader が1件も存在しない ImportFileLog を削除 ※ 全体が対象
            var countIFL = await dbHelper.ExecuteAsync(
                "DELETE ifl FROM ImportFileLog ifl WHERE NOT EXISTS (SELECT 1 FROM ReceiptHeader WHERE ImportFileLogId = ifl.Id)", null, token);

            return countR;
        }

    }
}
