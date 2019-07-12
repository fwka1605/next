using System;
using NLog;
using Rac.VOne.Common.Extensions;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Client.Common
{
    public class NLogHandler
    {
        public static void WriteErrorLog(object sender, Exception ex, string sessionKey)
        {
            WriteErrorLog(sender.GetType(), ex, sessionKey);
        }

        public static void WriteErrorLog(Type type, Exception ex, string sessionKey)
        {
            try
            {
                ServiceProxyFactory.Do<LogsService.LogsServiceClient>(client =>
                {
                    var log = new Logs();
                    log.Logger = type.Name;
                    log.Exception = ex.ToString();
                    log.Message = ex.Message;
                    log.SessionKey = sessionKey;

                    client.SaveErrorLog(log);
                });
            }
            catch { /* 揉み消す */ }

            var logger = LogManager.GetLogger(type.Name);
            logger.Error("Client Error", ex: ex);
        }

        public static void WriteErrorLog(object sender, string message)
        {
            WriteErrorLog(sender.GetType(), message);
        }
        public static void WriteErrorLog(Type type, string message)
        {
            var logger = LogManager.GetLogger(type.Name);
            logger.Error(message);
        }

        public static void WriteDebug(object sender, string message)
            => WriteDebug(sender.GetType(), message);

        public static void WriteDebug(Type type, string message)
            => LogManager.GetLogger(type.Name).Debug(message);
    }

}
