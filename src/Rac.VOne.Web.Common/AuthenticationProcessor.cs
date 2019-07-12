using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Common.Security;
using Rac.VOne.Web.Models;
using static Rac.VOne.Common.ErrorCode;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class AuthenticationProcessor : IAuthenticationProcessor
    {
        private readonly IAuthenticationQueryProcessor authenticationQueryProcessor;
        private readonly IHashAlgorithm hashAlgorithm;
        private readonly IStringConnectionFactory stringConnectionFactory;

        public AuthenticationProcessor(
            IAuthenticationQueryProcessor authenticationQueryProcessor,
            IHashAlgorithm hashAlgorithm,
            IStringConnectionFactory stringConnectionFactory
            )
        {
            this.authenticationQueryProcessor = authenticationQueryProcessor;
            this.hashAlgorithm = hashAlgorithm;
            this.stringConnectionFactory = stringConnectionFactory;
        }


        public async Task<AuthenticationResult> AuthenticateAsync(string authenticationKey, string companyCode, string userCode, CancellationToken token = default(CancellationToken))
        {
            var hashed = hashAlgorithm.Compute(authenticationKey);
            var exist = await authenticationQueryProcessor.ExistsAuthencticationKeyAsync(hashed, token);
            if (!exist)
            {
                return CreateResult(code: InvalidAuthenticationKey, message: "認証キーが不正です。");
            }

            var connectionString = await authenticationQueryProcessor.GetConnectionStringAsync(companyCode, token);

            var companyExists = !string.IsNullOrEmpty(connectionString);
            if (!companyExists)
            {
                return CreateResult(code: InvalidCompanyCode, message: "会社コードが不正です。");
            }

            var factory = stringConnectionFactory.Create(connectionString);

            var connectable = await authenticationQueryProcessor.IsConnectableAsync(factory, token);

            if (!connectable)
            {
                return CreateResult(code: CompanyDataBaseConnectionFailure, message: "DB接続に失敗しました。");
            }
            var sessionKey = await authenticationQueryProcessor.CreateSessionKeyAsync(connectionString, token);

            var persisted = !string.IsNullOrEmpty(sessionKey);
            if (!persisted)
            {
                return CreateResult(code: SessionKeyCreationFailure, message: "セッションキーの生成に失敗しました。");
            }

            return CreateResult(result: true, sessionKey: sessionKey);
        }


        private AuthenticationResult CreateResult(bool result = false,
            string code = "",
            string message = "",
            string sessionKey = "")
        {
            return new AuthenticationResult
            {
                Result = result,
                ErrorCode = code,
                ErrorMessage = message,
                SessionKey  = sessionKey
            };
        }
    }
}
