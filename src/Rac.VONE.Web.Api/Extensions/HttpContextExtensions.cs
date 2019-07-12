using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;


namespace Rac.VOne.Web.Api.Extensions
{

    /// <summary>
    /// <see cref="HttpContext"/>の拡張機能
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// <see cref="HttpContext"/>から、指定した request header の値を dictionary で取得
        /// </summary>
        /// <param name="context"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetRequestHeaders(this HttpContext context, IEnumerable<string> keys)
        {
            var result = new Dictionary<string, string>();
            foreach (var key in keys)
                if (context.Request.Headers.TryGetValue(key, out var values) && values.Any())
                    result.Add(key, values.FirstOrDefault());

            return result;
        }
    }
}
