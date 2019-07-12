using System;
using System.Collections.Generic;
using System.Text;

namespace Rac.VOne.AccountTransfer.Import.Reader
{
    /// <summary><see cref="Record"/>の IEnumerable を <see cref="ReadResultBundle{THeader, TData, TTrailer, TEnd}"/> へ
    /// 変換する拡張メソッドの ため だけの Interface
    /// generic class や generic interface は扱い辛い</summary>
    /// <typeparam name="THeader"></typeparam>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TTrailer"></typeparam>
    /// <typeparam name="TEnd"></typeparam>
    internal interface IBundle<THeader, TData, TTrailer, TEnd>
        where THeader   : Record
        where TData     : Record
        where TTrailer  : Record
        where TEnd      : Record
    {
    }
}
