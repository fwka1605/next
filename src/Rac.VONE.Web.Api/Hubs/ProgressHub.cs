using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Rac.VOne.Web.Api.Hubs
{
    using System.Collections.Concurrent;
    using System.Threading;
    /// <summary>
    /// 進捗状況 報告用の Hub
    /// </summary>
    public class ProgressHub : Hub
    {
        private readonly static ConcurrentDictionary<string, CancellationTokenSource> cancellationTokenSources = new ConcurrentDictionary<string, CancellationTokenSource>();

        /// <summary>
        /// <see cref="CancellationTokenSource"/>の追加
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="source"></param>
        public static void AddCancellationTokenSource(string connectionId, CancellationTokenSource source)
        {
            if (string.IsNullOrEmpty(connectionId)) return;
            cancellationTokenSources.AddOrUpdate(connectionId, source, (_, __) => source);
        }


        /// <summary>
        /// <see cref="CancellationTokenSource"/>の削除
        /// </summary>
        /// <param name="connectionId"></param>
        public static void RemoveCancellationTokenSource(string connectionId)
        {
            if (string.IsNullOrEmpty(connectionId)) return;
            cancellationTokenSources.TryRemove(connectionId, out var source);
        }

        /// <summary>
        /// キャンセル
        /// </summary>
        /// <param name="connectionId"></param>
        public void Cancel(string connectionId)
        {
            if (string.IsNullOrEmpty(connectionId)) return;
            if (cancellationTokenSources.TryGetValue(connectionId, out var source))
                source?.Cancel();
        }
    }
}
