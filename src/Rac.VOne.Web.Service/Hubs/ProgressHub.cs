using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Rac.VOne.Web.Service.Hubs
{
    public class ProgressHub : Hub
    {
        private readonly static ConcurrentDictionary<string, CancellationTokenSource> cancellationTokenSources
            = new ConcurrentDictionary<string, CancellationTokenSource>();

        public static void AddCancellationTokenSource(string connectionId, CancellationTokenSource source)
        {
            if (string.IsNullOrEmpty(connectionId)) return;
            if (cancellationTokenSources.ContainsKey(connectionId))
                cancellationTokenSources[connectionId] = source;
            else
                cancellationTokenSources.TryAdd(connectionId, source);
        }

        public static void RemoveCancellationTokenSource(string connectionId)
        {
            if (string.IsNullOrEmpty(connectionId)) return;
            CancellationTokenSource source;
            cancellationTokenSources.TryRemove(connectionId, out source);
        }


        public void Cancel(string connectionId)
        {
            if (string.IsNullOrEmpty(connectionId)) return;
            CancellationTokenSource source;
            if (cancellationTokenSources.TryGetValue(connectionId, out source))
                source?.Cancel();
        }
    }
}