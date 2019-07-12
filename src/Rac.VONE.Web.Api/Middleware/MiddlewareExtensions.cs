using Microsoft.AspNetCore.Builder;

namespace Rac.VOne.Web.Api.Middleware
{

    /// <summary>Middleware 利用用の拡張メソッド群</summary>
    public static class MiddlewareExtensions
    {
        /// <summary>厳密なIPアドレスでのフィルタを行う middleware</summary>
        public static IApplicationBuilder UseStrictIPAddressFilteringMiddleware(
            this IApplicationBuilder builder)
            => builder.UseMiddleware<StrictIPAddressFilteringMiddleware>();

        /// <summary>例外をハンドルする middleware を設定</summary>
        public static IApplicationBuilder UseExceptionHandlingMiddleware(
            this IApplicationBuilder builder)
            => builder.UseMiddleware<ExceptionHandlingMiddleware>();

    }
}
