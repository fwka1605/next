using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    /// メール配信用 テンプレート
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MailTemplateController : ControllerBase
    {
        //public CountResult Delete(string sessionKey, int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public MailTemplateResult Get(string sessionKey, int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public MailTemplatesResult GetItems(string sessionKey, int id, int MailType)
        //{
        //    throw new NotImplementedException();
        //}

        //public MailTemplateResult Save(string sessionKey, MailSetting mailSetting)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
