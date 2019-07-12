using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Data.Entities;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using static Rac.VOne.Common.ErrorCode;

namespace Rac.VOne.Web.Common
{
    public class AuthorizationProcessor : IAuthorizationProcessor
    {
        private readonly IAuthorizationQueryProcessor authorizationQueryProcessor;
        private readonly IStringConnectionFactory stringConnectionFactory;
        private readonly IDbHelper helper;

        public AuthorizationProcessor(
            IAuthorizationQueryProcessor authorizationQueryProcessor,
            IStringConnectionFactory stringConnectionFactory,
            IDbHelper helper
            )
        {
            this.authorizationQueryProcessor = authorizationQueryProcessor;
            this.stringConnectionFactory = stringConnectionFactory;
            this.helper = helper;
        }

        public async Task<Tuple<ProcessResult, IConnectionFactory>> AuthorizeAsync(string SessionKey)
        {
            var session = await authorizationQueryProcessor.GetSessionAsync(SessionKey);
            return CreateResult(session);
        }

        private Tuple<ProcessResult, IConnectionFactory> CreateResult(SessionStorage session)
        {
            if (session == null)
                return CreateResult(code: InvalidSessionKey, message: "セッションキーが無効です。");

            if (!ValidateSessionExpiration(session))
                return CreateResult(code: SessionKeyExpired, message: "セッションキーが有効期限切れです。");

            var factory = stringConnectionFactory.Create(session.ConnectionInfo);
            helper.SetConnectionFactory(factory);
            return CreateResult(result: true, factory: factory);
        }

        private bool ValidateSessionExpiration(SessionStorage session)
            => session.CreatedAt >= DateTime.Now.AddHours(-1 * Constants.SessionExpirationHour);

        private Tuple<ProcessResult, IConnectionFactory> CreateResult(bool result = false,
            string code = "",
            string message = "",
            IConnectionFactory factory = null)
        {
            return new Tuple<ProcessResult, IConnectionFactory>(
                new ProcessResult
                {
                    Result = result,
                    ErrorCode = code,
                    ErrorMessage = message,
                },
                factory);
        }
    }
}
