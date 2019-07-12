using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Screen
{
    public static class DataExpressionExtensions
    {
        public static System.Windows.Forms.ImeMode CustomerCodeImeMode(this Rac.VOne.Web.Models.DataExpression expression)
            => expression.CustomerCodeFormatString.Contains("K") ?
            System.Windows.Forms.ImeMode.KatakanaHalf :
            System.Windows.Forms.ImeMode.Disable;
    }
}
