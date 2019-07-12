using System;
using System.Collections.Generic;
using System.Text;

namespace Rac.VOne.EbData
{
    internal static class Constants
    {

        /// <summary> 入力区分 1 : EBデータ取込 </summary>
        internal const int InputTypeEbImporter = 1;

        /// <summary> 三菱東京UFJ銀行 銀行コード </summary>
        internal const string BTMUCode = "0005";
        /// <summary> 常陽銀行 銀行コード </summary>
        internal const string JyouyouBankCode = "0130";
        /// <summary> 横浜銀行 銀行コード </summary>
        internal const string YokohamaBankCode = "0138";
        /// <summary> 福岡銀行 銀行コード </summary>
        internal const string FukuokaBankCode = "0177";
        /// <summary> 北日本銀行 銀行コード </summary>
        internal const string KitaNipponBankCode = "0509";
        /// <summary> ゆうちょ 銀行コード </summary>
        internal const string JPBankCode = "9900";

        /// <summary>取込可能な預金種別</summary>
        /// <remarks>1:普通, 2:当座, 4:貯蓄, 5:通知</remarks>
        internal static int[] ImportableAccountTypeIds { get; } = new int[] { 1, 2, 4, 5 };

        internal static decimal MaxAmount { get; } = 99999999999M;
        internal static class DataKubun
        {
            /// <summary>1 : ヘッダーレコード</summary>
            internal const string Header = "1";
            /// <summary>2 : データレコード</summary>
            internal const string Data = "2";
            /// <summary>3 : 詳細レコード JPB のみ</summary>
            internal const string Detail = "3";
            /// <summary>8 : トレーラーレコード</summary>
            internal const string Trailer = "8";
            /// <summary>9 : エンドレコード</summary>
            internal const string End = "9";

        }


    }
}
