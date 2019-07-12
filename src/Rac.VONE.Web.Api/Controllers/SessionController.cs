using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    /// web service として 不要
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        //private IAuthorizationProcessor authorizationProcessor;
        //private ISessionStorageProcessor sessionStorageProcessor;
        //private readonly ILogger logger;

        //public SessionController(
        //    IAuthorizationProcessor authorizationProcessor,
        //    ISessionStorageProcessor sessionStorageProcessor,
        //    ILogManager logManager)
        //{
        //    this.authorizationProcessor = authorizationProcessor;
        //    this.sessionStorageProcessor = sessionStorageProcessor;
        //    logger = logManager.GetLogger(typeof(SessionController));
        //}

        //public ConnectionInfoResult GetConnectionInfo(string SessionKey)
        //{
        //    return authorizationProcessor.DoAuthorize(SessionKey, () =>
        //    {
        //        Session result = sessionStorageProcessor.Get(SessionKey);

        //        return new ConnectionInfoResult
        //        {
        //            ProcessResult = new ProcessResult { Result = true },
        //            ConnectionInfo = result.ConnectionInfo,
        //        };
        //    }, logger);
        //}
    }
}
