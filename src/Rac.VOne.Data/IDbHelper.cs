using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data
{
    public interface IDbHelper
    {
        void SetConnectionFactory(IConnectionFactory factory);

        /// <summary>
        /// Execute a query and resturns the number of rows affected asynchronously using .NET 4.5 Task.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <param name="cancellatioToken"></param>
        /// <returns></returns>
        Task<int> ExecuteAsync(string query, object param = null, CancellationToken cancellatioToken = default(CancellationToken));

        /// <summary>
        /// <seealso cref="T"/> を返す 非同期実装 キャンセル可
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> ExecuteAsync<T>(string query, object param = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// <seealso cref="IEnumerable{T}"/>を返す 非同期実装 キャンセル可
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <param name="cancellatioToken"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetItemsAsync<T>(string query, object param = null, CancellationToken cancellatioToken = default(CancellationToken));


        /// <summary>
        /// IDbConnection を張った後に、複数のクエリを実施する場合に利用
        /// ※ 一時テーブルの利用などは、このメソッドを利用する
        /// エラー時のlogging などは細かく実施していない
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        /// <returns></returns>
        Task<T> ExecuteQueriesAsync<T>(Func<IDbConnection, Task<T>> handler);
        /// <summary>一時テーブルの利用などで<see cref="ExecuteQueriesAsync"/>を利用し、内部でクエリを発行する場合に利用
        /// エラー時のlogging を wrap してある</summary>
        /// <param name="connection"></param>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <param name="token"></param>
        /// <returns></returns>

        Task<int> ExecuteAsync(IDbConnection connection, string query, object param = null, CancellationToken token = default(CancellationToken));
        /// <summary>一時テーブルの利用などで<see cref="ExecuteQueriesAsync"/>を利用し、内部でクエリを発行する場合に利用
        /// エラー時のlogging を wrap してある</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string query, object param = null, CancellationToken token = default(CancellationToken));

    }
}
