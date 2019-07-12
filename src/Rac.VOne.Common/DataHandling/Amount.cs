using System;
using System.Collections.Generic;
using System.Text;

namespace Rac.VOne.Common.DataHandling
{
    /// <summary>金額計算</summary>
    public static class Amount
    {
        /// <summary>端数処理</summary>
        /// <param name="type">端数処理種別</param>
        /// <param name="value">値</param>
        /// <param name="precision">桁</param>
        /// <returns>計算結果。処理種別が取込不可の場合、切り捨て（エラー処理は外部で行うこと）</returns>
        public static decimal? Calc(this RoundingType type, decimal value, int precision)
        {
            decimal powered = value * Pow10(precision);
            decimal result = 0;
            switch (type)
            {
                case RoundingType.Floor:
                case RoundingType.Error:
                    result = Math.Abs(Math.Floor(powered)) * Math.Sign(powered);
                    break;
                case RoundingType.Ceil:
                    result = Math.Abs(Math.Ceiling(powered)) * Math.Sign(powered);
                    break;
                case RoundingType.Round:
                    result = Math.Round(powered, MidpointRounding.AwayFromZero);
                    break;
            }
            return result * Pow10(-precision);
        }

        /// <summary>10の累乗を求める</summary>
        /// <param name="y">乗数</param>
        /// <returns>計算結果</returns>
        public static decimal Pow10(int y)
        {
            decimal x = 1;
            decimal value = y > 0 ? 10 : 0.1M;
            int times = Math.Abs(y);

            while (times-- > 0)
            {
                x *= value;
            }
            return x;
        }
    }
}
