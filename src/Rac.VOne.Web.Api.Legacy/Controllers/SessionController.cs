using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    /// web service として 不要
    /// </summary>
    public class SessionController : ApiControllerAuthorized
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
