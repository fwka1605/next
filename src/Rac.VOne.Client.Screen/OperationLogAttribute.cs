using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Screen
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class OperationLogAttribute : Attribute
    {
        public OperationLogAttribute(string funcName)
        {
            FunctionName = funcName;
        }
        public string FunctionName { get; set; }
    }
}
