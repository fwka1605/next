using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import
{
    /// <summary>
    /// 口座振替結果ファイルのヘッダ＋データ(複数)＋トレーラ情報を保持する。
    /// (マルチヘッダ時は、その中の単一のヘッダ＋データ＋トレーラ情報を保持)
    /// </summary>
    /// <typeparam name="THeader"></typeparam>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TTrailer"></typeparam>
    /// <typeparam name="TEnd"></typeparam>
    public class ReadResultBundle<THeader, TData, TTrailer, TEnd>
        where THeader   : Record
        where TData     : Record
        where TTrailer  : Record
        where TEnd      : Record
    {

        public THeader Header { get; set; }
        public IEnumerable<TData> DataList { get; set; }
        public TTrailer Trailer { get; set; }

    }
}
