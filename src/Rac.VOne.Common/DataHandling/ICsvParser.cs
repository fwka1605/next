using System.Collections.Generic;
using System.Text;

namespace Rac.VOne.Common.DataHandling
{
    public interface ICsvParser
    {
        Encoding Encoding { get; set; }

        /// <summary>stream を作成する interface</summary>
        IStreamCreator StreamCreator { get; set; }
        /// <summary>csv の parse</summary>
        /// <param name="csv">source
        /// 通常 csv ファイルのパス filepath
        /// 直接 csv 形式のテキストの場合も有</param>
        /// <returns></returns>
        IEnumerable<string[]> Parse(string csv);
    }
}
