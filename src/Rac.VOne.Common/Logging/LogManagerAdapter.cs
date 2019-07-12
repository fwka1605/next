using System;
using NLog;

namespace Rac.VOne.Common.Logging
{
    public class LogManagerAdapter : ILogManager
    {
        public ILogger GetLogger(Type typeAssociatedWithRequestedLog)
        {
            var logger = LogManager.GetLogger(typeAssociatedWithRequestedLog.Name);
            return logger;
        }

    }
}
