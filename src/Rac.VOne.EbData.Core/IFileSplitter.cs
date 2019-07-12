using System;
using System.Collections.Generic;

namespace Rac.VOne.EbData
{
    internal interface IFileSplitter
    {
        bool IsPlainText { get; set; }
        Tuple<IEnumerable<string[]>, int, ImportResult> Split(string path);
    }
}
