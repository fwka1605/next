using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using NLog;

namespace Rac.VOne.Web.Api.Legacy.Library
{
    /// <summary>
    /// 例外のカスタイマイズ Logger
    /// </summary>
    public class CustomizeExceptionLogger : ExceptionLogger
    {
        private readonly ILogger logger;

        /// <summary></summary>
        public CustomizeExceptionLogger()
        {
            this.logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary></summary>
        public override void Log(ExceptionLoggerContext context)
        {
            logger.Error(context.Exception, "Global Handler Catched.");
        }
    }
}