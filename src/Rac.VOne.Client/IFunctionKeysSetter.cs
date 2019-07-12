using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client
{
    public interface IFunctionKeysSetter
    {
        void SetBaseContext(IFunctionKeys baseContext);
        void OnFunctionKey01Click();
        void OnFunctionKey02Click();
        void OnFunctionKey03Click();
        void OnFunctionKey04Click();
        void OnFunctionKey05Click();
        void OnFunctionKey06Click();
        void OnFunctionKey07Click();
        void OnFunctionKey08Click();
        void OnFunctionKey09Click();
        void OnFunctionKey10Click();
        void OnFunctionKey11Click();
        void OnFunctionKey12Click();
    }
}
