using Microsoft.AspNet.SignalR;
using Rac.VOne.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Legacy.Extensions
{

    /// <summary>
    /// asp.net core signalr の hubContext 拡張
    /// </summary>
    public static class HubContextExtentions
    {

        /// <summary>
        /// connectionId の client を内包した notifier の作成
        /// </summary>
        /// <param name="context"></param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        public static IProgressNotifier CreateNotifier(
            this IHubContext context, string connectionId)
            => string.IsNullOrEmpty(connectionId) ? null :
            new ProgressNotifier(context.Clients.Client(connectionId));

        /// <summary>
        /// キャンセル処理を内包する wrapper method
        /// TODO:適切なメソッド名
        /// cancellation token のテストを実施する
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="context"></param>
        /// <param name="connectionId"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static async Task<TResult> DoAsync<TResult>(
            this IHubContext context,
            string connectionId,
            Func<IProgressNotifier, CancellationToken, Task<TResult>> handler)
        {
            try
            {
                var source = CreateCancellationTokenSource();
                Hubs.ProgressHub.AddCancellationTokenSource(connectionId, source);
                var notifier = context.CreateNotifier(connectionId);
                return await handler(notifier, source.Token);
            }
            //catch (Exception) // logging は middleware で行うので、 catch 句自体不要
            //{
            //    throw;
            //}
            finally
            {
                Hubs.ProgressHub.RemoveCancellationTokenSource(connectionId);
            }
        }

        /// <summary>
        /// 非同期処理を wrapp する method cancel できないけど非同期
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="context"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static async Task<TResult> DoAsync<TResult>(
            this IHubContext context,
            Func<CancellationToken, Task<TResult>> handler)
        {
            var source = CreateCancellationTokenSource();
            return await handler(source.Token);
        }


        private static CancellationTokenSource CreateCancellationTokenSource()
        {
            var source = new CancellationTokenSource();
            source.Token.ThrowIfCancellationRequested();
            return source;
        }
    }
}
