using Microsoft.AspNetCore.Http;
using Rac.VOne.Web.Api.Library;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Middleware
{
    /// <summary>ログイン系の IP アドレス制限を行う middleware</summary>
    public class StrictIPAddressFilteringMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IIPWhiteList ipWhiteList;

        /// <summary></summary>
        public StrictIPAddressFilteringMiddleware(
            RequestDelegate next,
            IIPWhiteList ipWhiteList
            )
        {
            this.next = next;
            this.ipWhiteList = ipWhiteList;
        }

        /// <summary>IP アドレス制限を実施 許可されていないIPアドレスの場合 httpstatuscode 403 forbidden を返す </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            var addressBytes = context.Connection.RemoteIpAddress.GetAddressBytes();

            if (! ipWhiteList.GetIPAddresses().Any(x => x.GetAddressBytes().SequenceEqual(addressBytes)))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }

            await next(context);
        }
    }
}
