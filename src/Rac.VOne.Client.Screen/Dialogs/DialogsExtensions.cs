using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public static class DialogsExtensions
    {
        private static CompareInfo compareInfo = CultureInfo.GetCultureInfo("ja-JP").CompareInfo;

        /// <summary>カナ種別無視 UPPER/lower case無視, 全角半角無視</summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool RoughlyContains(this string source, string target)
            => compareInfo.IndexOf(source, target,
                CompareOptions.IgnoreCase |
                CompareOptions.IgnoreKanaType |
                CompareOptions.IgnoreWidth) >= 0;
    }
}
