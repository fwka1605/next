using NLog;
using Rac.VOne.Data;
using Rac.VOne.Common.Extensions;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using static System.Reflection.BindingFlags;

namespace Rac.VOne.Web.Service.Extensions
{
    public static class AuthorizationExtensions
    {
        private static readonly TimeSpan transactionTimeout;
        //private static ITransactionScopeBuilder transactionScopeBuilder;

        static AuthorizationExtensions()
        {
            string timeout
                    = System.Configuration.ConfigurationManager.AppSettings["TransactionTimeoutSecond"];
            int second = 0;
            if (int.TryParse(timeout, out second))
            {
                try
                {
                    typeof(TransactionManager)
                        .GetField("_cachedMaxTimeout", NonPublic | Static)
                        .SetValue(null, true);

                    transactionTimeout = TimeSpan.FromSeconds(second);
                    typeof(TransactionManager)
                        .GetField("_maximumTimeout", NonPublic | Static)
                        .SetValue(null, transactionTimeout);
                }
                catch (Exception ex)
                {
                    Debug.Fail("TransactionTimeout : " + ex.Message);
                }
            }
            else
            {
                Debug.Fail($"トランザクションタイムアウトの設定に失敗しました。(値：{timeout})");
            }
        }

        //public static void Initilize(SimpleInjector.Container container)
        //{
        //    transactionScopeBuilder = container.GetInstance<ITransactionScopeBuilder>();
        //}


        //public static TResult DoAuthorize<TResult>(
        //    this IAuthorizationProcessor authorizationProcess,
        //    string sessionKey,
        //    Func<ITransactionScopeBuilder, TResult> @do,
        //    ILogger logger,
        //    [CallerMemberName]string caller = "")
        //    where TResult : class, IProcessResult, new()
        //{
        //    var authResult = authorizationProcess.Authorize(sessionKey);
        //    if (!authResult.Item1.Result)
        //    {
        //        var result = new TResult();
        //        result.ProcessResult = authResult.Item1;
        //        return result;
        //    }

        //    //var builder = new Data.TransactionScopeBuilder()
        //    //    .Timeout(transactionTimeout);
        //    try
        //    {
        //        return @do(transactionScopeBuilder);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger?.Error($"{caller} error", ex: ex, sessionKey: sessionKey);
        //        return CreateErrorResult<TResult>(ex);
        //    }
        //}

        //public static TResult DoAuthorize<TResult>(
        //        this IAuthorizationProcessor authorizationProcess,
        //        string sessionKey,
        //        Func<TResult> @do,
        //        ILogger logger,
        //        [CallerMemberName]string caller = "")
        //    where TResult : class, IProcessResult, new()
        //{
        //    var authResult = authorizationProcess.Authorize(sessionKey);
        //    if (!authResult.Item1.Result)
        //    {
        //        var result = new TResult();
        //        result.ProcessResult = authResult.Item1;
        //        return result;
        //    }

        //    try
        //    {
        //        return @do();
        //    }
        //    catch (Exception ex)
        //    {
        //        logger?.Error($"{caller} error", ex: ex, sessionKey: sessionKey);
        //        return CreateErrorResult<TResult>(ex);
        //    }
        //}

        /// ※ WCF Web Service 上で TransactionScope を 取り扱わない
        /// Web.Common 上に ビジネスロジックを 寄せる関係上、
        /// 複数テーブル更新を行うかどうかは、Web.Common が一番熟知していなければいけない
        ///// <summary>
        ///// WCF Web Srevice を 非同期で実装するための method wrapper 複数テーブルのレコードを更新する場合に利用
        ///// connectionId は nullableのため、logger の後に配置
        ///// 指定漏れに注意すること
        ///// </summary>
        ///// <typeparam name="TResult"></typeparam>
        ///// <param name="authorizationProcessor"></param>
        ///// <param name="sessionKey"></param>
        ///// <param name="do"></param>
        ///// <param name="logger"></param>
        ///// <param name="connectionId"></param>
        ///// <param name="caller"></param>
        ///// <returns></returns>
        //[Obsolete] public static async Task<TResult> DoAuthorizeAsync<TResult>(
        //    this IAuthorizationProcessor autohrizationProcessor,
        //    string sessionKey,
        //    Func<ITransactionScopeBuilder, CancellationToken, Task<TResult>> @do,
        //    ILogger logger,
        //    string connectionId = null,
        //    [CallerMemberName]string caller = "")
        //    where TResult : class, IProcessResult, new()
        //{
        //    var authResult = await autohrizationProcessor.AuthorizeAsync(sessionKey);
        //    if (!authResult.Item1.Result)
        //    {
        //        var result = new TResult();
        //        result.ProcessResult = authResult.Item1;
        //        return result;
        //    }

        //    //var builder = new TransactionScopeBuilder().Timeout(transactionTimeout);
        //    try
        //    {
        //        var source = CreateCencellationTokenSource();
        //        Hubs.ProgressHub.AddCancellationTokenSource(connectionId, source);
        //        return await @do(transactionScopeBuilder, source.Token);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (!ex.HasCancelledException()
        //         && !ex.IsOperationCanceledException())
        //            logger?.Error($"{caller} error", ex: ex, sessionKey: sessionKey);

        //        return CreateErrorResult<TResult>(ex);
        //    }
        //    finally
        //    {
        //        Hubs.ProgressHub.RemoveCancellationTokenSource(connectionId);
        //    }
        //}

        /// <summary>
        /// WCF Web Srevice を 非同期で実装するための method wrapper
        /// connectionId は nullableのため、logger の後に配置
        /// 指定漏れに注意すること
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="authorizationProcessor"></param>
        /// <param name="sessionKey"></param>
        /// <param name="do"></param>
        /// <param name="logger"></param>
        /// <param name="connectionId"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public static async Task<TResult> DoAuthorizeAsync<TResult>(
            this IAuthorizationProcessor authorizationProcessor,
            string sessionKey,
            Func<CancellationToken, Task<TResult>> @do,
            ILogger logger,
            string connectionId = null,
            [CallerMemberName]string caller = "")
            where TResult : class, IProcessResult, new()
        {
            var authResult = await authorizationProcessor.AuthorizeAsync(sessionKey);
            if (!authResult.Item1.Result)
            {
                var result = new TResult();
                result.ProcessResult = authResult.Item1;
                return result;
            }
            try
            {
                var source = CreateCencellationTokenSource();
                Hubs.ProgressHub.AddCancellationTokenSource(connectionId, source);
                return await @do(source.Token);
            }
            catch (Exception ex)
            {
                if (!ex.HasCancelledException()
                &&  !ex.IsOperationCanceledException())
                    logger?.Error($"{caller} error", ex: ex, sessionKey: sessionKey);
                return CreateErrorResult<TResult>(ex);
            }
            finally
            {
                Hubs.ProgressHub.RemoveCancellationTokenSource(connectionId);
            }
        }

        private static CancellationTokenSource CreateCencellationTokenSource()
        {
            var source = new CancellationTokenSource();
            source.Token.ThrowIfCancellationRequested();
            return source;
        }

        private static TResult CreateErrorResult<TResult>(Exception ex)
            where TResult : class, IProcessResult, new()
        {
            return new TResult
            {
                ProcessResult = new ProcessResult {
                    ErrorCode = VOne.Common.ErrorCode.ExceptionOccured,
                    ErrorMessage = ex.Message,
                }
            };
        }

        public static bool IsOperationCanceledException(this Exception ex)
            => ex is OperationCanceledException;
    }
}
