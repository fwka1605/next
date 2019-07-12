using Microsoft.AspNetCore.Mvc.Filters;

namespace Rac.VOne.Web.Api.Filters
{
    /// <summary>認可処理をスキップする 属性</summary>
    public class SkipAuthorizationFilterAttribute : ActionFilterAttribute
    {
    }
}
