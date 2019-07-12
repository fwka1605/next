using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    /// メール配信設定
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MailSettingController : ControllerBase
    {
        //public MailSettingResult Get(string SessionKey, int CompanyId)
        //{
        //    throw new NotImplementedException();
        //}

        //public MailSettingResult Save(string SessionKey, MailSetting mailSetting)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
