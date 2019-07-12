using System;
using NLog;

namespace Rac.VOne.Common.Logging
{
    public interface ILogManager
    {
        ILogger GetLogger(Type typeAssociatedWithRequestedLog);
    }
}
