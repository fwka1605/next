using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Rac.VOne.AccountTransfer.Import.Reader
{
    internal static class ReaderExtension
    {
        /// <summary>マルチヘッダー対応用
        /// <see cref="Record"/>の IEnumerable を変換し、<see cref="ReadResultBundle{THeader, TData, TTrailer, TEnd}"/>の List を作成する拡張メソッド
        /// <see cref="IBundle{THeader, TData, TTrailer, TEnd}"/>から利用可能
        /// </summary>
        /// <typeparam name="THeader"></typeparam>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TTrailer"></typeparam>
        /// <typeparam name="TEnd"></typeparam>
        /// <param name="reader"></param>
        /// <param name="records"></param>
        /// <param name="recordSetType"></param>
        /// <param name="isMultiHeader"></param>
        /// <returns></returns>
        internal static List<ReadResultBundle<THeader, TData, TTrailer, TEnd>> CreateBundles<THeader, TData, TTrailer, TEnd>(
            this IBundle<THeader, TData, TTrailer, TEnd> reader,
            IEnumerable<Record> records,
            RecordSetType recordSetType,
            bool isMultiHeader)
            where THeader   : Record
            where TData     : Record
            where TTrailer  : Record
            where TEnd      : Record
        {
            var results = new List<ReadResultBundle<THeader, TData, TTrailer, TEnd>>();
            var idx = 0;
            var recordStateMachine = new RecordOrderStateMachine(recordSetType, isMultiHeader);

            ReadResultBundle<THeader, TData, TTrailer, TEnd> result = null;
            List<TData> dataList = null;

            foreach (var rec in records)
            {
                var recordInfo = $"L.{rec.LineNumber}";

                if (rec is THeader)
                {
                    var tr = recordStateMachine.ChangeState(RecordType.Header);
                    if (tr != RecordOrderStateTransitionResult.Success || recordStateMachine.Status != RecordOrderState.Header)
                    {
                        throw new FormatException($"{recordInfo} {tr}");
                    }

                    dataList = new List<TData>();
                    result = new ReadResultBundle<THeader, TData, TTrailer, TEnd>
                    {
                        Header = (THeader)rec,
                        DataList = dataList,
                    };
                    results.Add(result);
                }
                else if (rec is TData)
                {
                    var tr = recordStateMachine.ChangeState(RecordType.Data);
                    if (tr != RecordOrderStateTransitionResult.Success || recordStateMachine.Status != RecordOrderState.Data)
                    {
                        throw new FormatException($"{recordInfo} {tr}");
                    }

                    dataList.Add((TData)rec);
                }
                else if (rec is TTrailer)
                {
                    var tr = recordStateMachine.ChangeState(RecordType.Trailer);
                    if (tr != RecordOrderStateTransitionResult.Success || recordStateMachine.Status != RecordOrderState.Trailer)
                    {
                        throw new FormatException($"{recordInfo} {tr}");
                    }

                    result.Trailer = (TTrailer)rec;
                }
                else if (rec is TEnd)
                {
                    var tr = recordStateMachine.ChangeState(RecordType.End);
                    if (tr != RecordOrderStateTransitionResult.Success || recordStateMachine.Status != RecordOrderState.End)
                    {
                        throw new FormatException($"{recordInfo} {tr}");
                    }
                }
                else
                {
                    throw new InvalidOperationException(recordInfo);
                }

                idx++;
            }

            if (records.Any()) // レコード無しの場合があるのでガードする
            {
                var tr = recordStateMachine.ChangeState(RecordType.EOF);
                if (tr != RecordOrderStateTransitionResult.Success || recordStateMachine.Status != RecordOrderState.Completed)
                {
                    var last = records.Last();
                    var recordInfo = $"L.{last.LineNumber + 1}";
                    throw new FormatException($"{recordInfo} {tr}");
                }
            }

            return results;
        }


    }
}
