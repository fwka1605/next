using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Rac.VOne.Web.Api.Legacy.Startup))]

namespace Rac.VOne.Web.Api.Legacy
{
    /// <summary>
    /// owin startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// signal-r の構成
        /// </summary>
        /// <param name="builder"></param>
        public void Configuration(IAppBuilder builder)
        {
            builder.MapSignalR();
        }
    }
}