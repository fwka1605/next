using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using NLog;
using Rac.VOne.Common.Extensions;
using Rac.VOne.Common.Logging;

namespace Rac.VOne.Data
{
    public class DbHelper : IDbHelper
    {
        public static int CommandTimeoutSecond { get; set; } = 30;

        static DbHelper()
        {
            Dapper.SqlMapper.AddTypeMap(typeof(System.DateTime), System.Data.DbType.DateTime2);
            Dapper.SqlMapper.AddTypeMap(typeof(System.DateTime?), System.Data.DbType.DateTime2);
        }

        private IConnectionFactory connectionFactory;
        private readonly ILogger logger;

        public DbHelper(
            IConnectionFactory connectionFactory,
            ILogManager logManager
            )
        {
            this.connectionFactory = connectionFactory;
            logger = logManager.GetLogger(typeof(DbHelper));
        }

        public void SetConnectionFactory(IConnectionFactory factory)
        {
            connectionFactory = factory;
        }

        #region async method wrapper

        private async Task<T> ExecuteQueryAsync<T>(string query, object param = null,
            CancellationToken cancellationToken = default(CancellationToken),
            Func<IDbConnection, CommandDefinition, Task<T>> handler = null)
        {
            var result = default(T);
            using (var connection = connectionFactory.CreateConnection())
            {
                try
                {
                    connection.Open();
                    logger.Trace("Trace ExecuteQueryAsync", query: query, database: connection?.Database, param: param);
                    var commandDef = CreateCommand(query, param, cancellationToken);
                    result = await handler?.Invoke(connection, commandDef);
                }
                catch (DbException ex)
                {
                    if (!ex.HasCancelledException())
                        logger.Error("ExecuteQueryAsync Error", ex: ex, query: query, database: connection?.Database, param: param);
                    throw;
                }
            }
            return result;
        }

        private async Task<IEnumerable<T>> ExecuteEnamerableQueryAsync<T>(string query, object param = null,
            CancellationToken cancellationToken = default(CancellationToken),
            Func<IDbConnection, CommandDefinition, Task<IEnumerable<T>>> handler = null)
        {
            var result = Enumerable.Empty<T>();
            using (var connection = connectionFactory.CreateConnection())
            {
                try
                {
                    connection.Open();
                    logger.Trace("Trace ExecuteEnamerableQueryAsync", query: query, database: connection?.Database, param: param);
                    var commandDef = CreateCommand(query, param, cancellationToken);
                    result = await handler?.Invoke(connection, commandDef);
                }
                catch (DbException ex)
                {
                    if (!ex.HasCancelledException())
                        logger.Error("ExecuteEnamerableQueryAsync Error", ex: ex, query: query, database: connection?.Database, param: param);
                    throw;
                }
            }
            return result;
        }

        public async Task<int> ExecuteAsync(string query, object param = null, CancellationToken cancellationToken = default(CancellationToken))
            => await ExecuteQueryAsync(query, param, cancellationToken, async (connection, command) => await connection.ExecuteAsync(command));

        public async Task<T> ExecuteAsync<T>(string query, object param = null, CancellationToken cancellationToken = default(CancellationToken))
            => await ExecuteQueryAsync(query, param, cancellationToken, async (connection, command) =>
            {
                var result = await connection.QueryAsync<T>(command);
                return result.FirstOrDefault();
            });

        public async Task<IEnumerable<T>> GetItemsAsync<T>(string query, object param = null, CancellationToken cancellationToken = default(CancellationToken))
            => await ExecuteEnamerableQueryAsync(query, param, cancellationToken, async (connection, command) => await connection.QueryAsync<T>(command));


        /// <summary>一時テーブルを利用するなどして、<see cref="ExecuteQueriesAsync"/>の内部でクエリを発行するメソッド
        /// エラー時のlogging をラップしている
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(IDbConnection connection, string query, object param = null, CancellationToken token = default(CancellationToken))
        {
            try
            {
                return await connection.ExecuteAsync(CreateCommand(query, param, token));
            }
            catch (DbException ex)
            {
                if (!ex.HasCancelledException())
                    logger.Error("ExecuteAsync", ex: ex, query: query, database: connection?.Database, param: param);
                throw;
            }
        }

        /// <summary>一時テーブルを利用するなどして、<see cref="ExecuteQueriesAsync"/>の内部でクエリを発行するメソッド
        /// エラー時のlogging をラップしている
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string query, object param = null, CancellationToken token = default(CancellationToken))
        {
            try
            {
                return await connection.QueryAsync<T>(CreateCommand(query, param, token));
            }
            catch (DbException ex)
            {
                if (!ex.HasCancelledException())
                    logger.Error("QueryAsync", ex: ex, query: query, database: connection?.Database, param: param);
                throw;
            }
        }


        public async Task<T> ExecuteQueriesAsync<T>(Func<IDbConnection, Task<T>> handler)
        {
            var result = default(T);
            using (var connection = connectionFactory.CreateConnection())
            {
                try
                {
                    connection.Open();
                    result = await handler?.Invoke(connection);
                }
                catch (DbException ex)
                {
                    if (!ex.HasCancelledException())
                        logger.Error("ExecuteQueriesAsync", ex: ex, database: connection?.Database);
                    throw;
                }
            }
            return result;
        }

        private CommandDefinition CreateCommand(string query, object param = null, CancellationToken cancellationToken = default(CancellationToken))
            => new CommandDefinition(query, param, commandTimeout: CommandTimeoutSecond, cancellationToken: cancellationToken);

        #endregion
    }
}
