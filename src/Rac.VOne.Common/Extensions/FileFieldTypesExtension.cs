using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Common.Extensions
{
    public static class FileFieldTypesExtension
    {
        public static Dictionary<int, string> GetFileFieldTypesSource(this FileFieldTypes types)
        {
            var dic = new Dictionary<int, string>();
            if (types.HasFlag(FileFieldTypes.CommaDelimited))         dic.Add(0, "カンマ区切り");
            if (types.HasFlag(FileFieldTypes.TabDelimited))           dic.Add(1, "タブ区切り");
            if (types.HasFlag(FileFieldTypes.FixedLength))            dic.Add(2, "固定長（改行あり）");
            if (types.HasFlag(FileFieldTypes.FixedLengthNoLineBreak)) dic.Add(3, "固定長（改行なし）");
            return dic;

        }
    }
}
