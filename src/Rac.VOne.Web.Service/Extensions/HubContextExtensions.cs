using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rac.VOne.Web.Service.Extensions
{
    public static class HubContextExtensions
    {
        public static Rac.VOne.Common.IProgressNotifier CreateNotifier(
            this Microsoft.AspNet.SignalR.IHubContext context, string connectionId)
            => string.IsNullOrEmpty(connectionId) ? null
            : new Rac.VOne.Common.ProgressNotifier(context.Clients.Client(connectionId));
    }
}