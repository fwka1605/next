using NLog;
using NLog.Fluent;
using System;

namespace Rac.VOne.Common.Extensions
{
    public static class LoggerExtensions
    {
        /// <summary>Debug のログ出力</summary>
        /// <param name="logger"><see cref="ILogger"/></param>
        /// <param name="message">メッセージ</param>
        /// <param name="ex">例外 null 可</param>
        /// <param name="sessionKey">セッションキー情報 (Web Service側での利用)</param>
        /// <param name="query">クエリ DBHelper での利用</param>
        /// <param name="database">データベース DBHelper での利用</param>
        /// <param name="param">クエリに対するパラメータのオブジェクト DBHelper での利用</param>
        public static void Debug(this ILogger logger, string message, Exception ex = null, string sessionKey = null, string query = null, string database = null, object param = null)
        {
            if (logger == null) return;
            logger.Debug().Append(message, ex, sessionKey, query, database, param).Write();
        }

        /// <summary>Error のログ出力</summary>
        /// <param name="logger"><see cref="ILogger"/></param>
        /// <param name="message">メッセージ</param>
        /// <param name="ex">例外 null 可</param>
        /// <param name="sessionKey">セッションキー情報 (Web Service側での利用)</param>
        /// <param name="query">クエリ DBHelper での利用</param>
        /// <param name="database">データベース DBHelper での利用</param>
        /// <param name="param">クエリに対するパラメータのオブジェクト DBHelper での利用</param>
        public static void Error(this ILogger logger, string message, Exception ex = null, string sessionKey = null, string query = null, string database = null, object param = null)
        {
            if (logger == null) return;
            logger.Error().Append(message, ex, sessionKey, query, database, param).Write();
        }

        /// <summary>Fatal のログ出力</summary>
        /// <param name="logger"><see cref="ILogger"/></param>
        /// <param name="message">メッセージ</param>
        /// <param name="ex">例外 null 可</param>
        /// <param name="sessionKey">セッションキー情報 (Web Service側での利用)</param>
        /// <param name="query">クエリ DBHelper での利用</param>
        /// <param name="database">データベース DBHelper での利用</param>
        /// <param name="param">クエリに対するパラメータのオブジェクト DBHelper での利用</param>
        public static void Fatal(this ILogger logger, string message, Exception ex, string sessionKey, string query, string database, object param)
        {
            if (logger == null) return;
            logger.Fatal().Append(message, ex, sessionKey, query, database, param).Write();
        }

        /// <summary>Info のログ出力</summary>
        /// <param name="logger"><see cref="ILogger"/></param>
        /// <param name="message">メッセージ</param>
        /// <param name="ex">例外 null 可</param>
        /// <param name="sessionKey">セッションキー情報 (Web Service側での利用)</param>
        /// <param name="query">クエリ DBHelper での利用</param>
        /// <param name="database">データベース DBHelper での利用</param>
        /// <param name="param">クエリに対するパラメータのオブジェクト DBHelper での利用</param>
        public static void Info(this ILogger logger, string message, Exception ex = null, string sessionKey = null, string query = null, string database = null, object param = null)
        {
            if (logger == null) return;
            logger.Info().Append(message, ex, sessionKey, query, database, param).Write();
        }

        /// <summary>Trace のログ出力</summary>
        /// <param name="logger"><see cref="ILogger"/></param>
        /// <param name="message">メッセージ</param>
        /// <param name="ex">例外 null 可</param>
        /// <param name="sessionKey">セッションキー情報 (Web Service側での利用)</param>
        /// <param name="query">クエリ DBHelper での利用</param>
        /// <param name="database">データベース DBHelper での利用</param>
        /// <param name="param">クエリに対するパラメータのオブジェクト DBHelper での利用</param>
        public static void Trace(this ILogger logger, string message, Exception ex = null, string sessionKey = null, string query = null, string database = null, object param = null)
        {
            if (logger == null) return;
            logger.Trace().Append(message, ex, sessionKey, query, database, param).Write();
        }

        /// <summary>Warn のログ出力</summary>
        /// <param name="logger"><see cref="ILogger"/></param>
        /// <param name="message">メッセージ</param>
        /// <param name="ex">例外 null 可</param>
        /// <param name="sessionKey">セッションキー情報 (Web Service側での利用)</param>
        /// <param name="query">クエリ DBHelper での利用</param>
        /// <param name="database">データベース DBHelper での利用</param>
        /// <param name="param">クエリに対するパラメータのオブジェクト DBHelper での利用</param>
        public static void Warn(this ILogger logger, string message, Exception ex = null, string sessionKey = null, string query = null, string database = null, object param = null)
        {
            if (logger == null) return;
            logger.Warn().Append(message, ex, sessionKey, query, database, param).Write();
        }


        public static LogBuilder Append(this LogBuilder buidler,
            string message, Exception ex,
            string sessionKey,
            string query, string database, object param)
        {
            if (buidler == null) return buidler;
            buidler.Message(message);
            if (ex != null)
                buidler.Exception(ex);

            if (!string.IsNullOrEmpty(sessionKey))
                buidler.Property("SessionKey", sessionKey);

            if (string.IsNullOrEmpty(query)) return buidler;
            buidler.Property("Query", query);
            buidler.Property("DatabaseName", database);
            if (param != null)
                buidler.Property("Parameters", param.ConvertParameterToJson());

            return buidler;
        }
    }
}
