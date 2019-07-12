using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Common
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MasterServiceAttribute : Attribute
    {
    }

    namespace ApplicationControlMasterService
    {
        [MasterService] partial class ApplicationControlMasterClient { }
    }
    namespace AccountTitleMasterService
    {
        [MasterService] partial class AccountTitleMasterClient { }
    }
}

