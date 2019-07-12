using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Common
{
    /// <summary>金額取込フォーマット</summary>
    public enum NumberFormat
    {
        /// <summary>1:符号は先頭(+,空白,数値以外はマイナスとする)</summary>
        SignFirst = 1,
        /// <summary>2:符号は末尾(+,空白,数値以外はマイナスとする)</summary>
        SignLast,
    }

    public static class NumberFormatExtension
    {
        /// <summary>取込フォーマット用数値変換処理</summary>
        public static decimal? ToDecimal(this NumberFormat format, string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;

            Predicate<char> isNumeric = delegate (char c) { return '0' <= c && c <= '9'; };
            char sign = '\0';
            bool numeric;
            string numberString = null;

            switch (format)
            {
                case NumberFormat.SignFirst:
                    sign = text[0];
                    numeric = isNumeric(sign);
                    numberString = numeric ? text : new string(text.Skip(1).ToArray());
                    break;
                case NumberFormat.SignLast:
                    sign = text.Last();
                    numeric = isNumeric(sign);
                    numberString = numeric ? text : text.Substring(0, text.Length - 1);
                    break;
                default:
                    return null;
            }

            if (!Regex.IsMatch(numberString, @"^[0-9,]+(\.[0-9,]+)?$")) return null;

            decimal result = 0;
            if (!decimal.TryParse(numberString, out result)) return null;

            return result * (numeric || sign == '+' ? 1 : -1);
        }
    }
}
