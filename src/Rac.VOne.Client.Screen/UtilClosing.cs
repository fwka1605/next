using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.ClosingService;
using Rac.VOne.Web.Models;


namespace Rac.VOne.Client.Screen
{
    public class UtilClosing
    {
        public static ClosingInformation GetClosingInformation(string sessionKey, int companyId) =>
            ServiceProxyFactory.Do((ClosingServiceClient client) =>
            {
               var result = client.GetClosingInformation(sessionKey, companyId);
                if (result == null || !result.ProcessResult.Result)
                    return null;
                return result.ClosingInformation;
            });
    }
}
