using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rac.VOne.Common.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// 文字列の 左側から 指定した文字列長を取得 paddingChar/forcePadding で 右側を paddingChar で指定文字長まで埋めて返す
        /// </summary>
        /// <param name="value">元となる文字列</param>
        /// <param name="length">文字列長</param>
        /// <param name="paddingChar">指定文字まで埋める文字 未指定時はなにもしない</param>
        /// <param name="forcePadding">元となる文字列が空文字の場合でも強制的に paddingChar で埋めるかどうか</param>
        /// <returns></returns>
        public static string Left(this string value, int length, char? paddingChar = null, bool forcePadding = false)
        {
            if (paddingChar.HasValue
                && (forcePadding || !string.IsNullOrEmpty(value))
                && (value?.Length ?? 0) < length)
            {
                value = (value ?? string.Empty)
                    + new string(paddingChar.Value, length);
            }
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return value.Length < length
                ? value : value.Substring(0, length);
        }
        /// <summary>
        /// 文字列の 右側から 指定した文字列長を取得 paddingChar/forcePadding で 左側を paddingChar で指定文字列長まで埋めて返す
        /// </summary>
        /// <param name="value">元となる文字列</param>
        /// <param name="length">文字列長</param>
        /// <param name="paddingChar">指定文字まで埋める文字 未指定時は何もしない</param>
        /// <param name="forcePadding">元となる文字列が空文字の場合でも強制的に paddingChar で埋めるかどうか</param>
        /// <returns></returns>
        public static string Right(this string value, int length, char? paddingChar = null, bool forcePadding = false)
        {
            if (paddingChar.HasValue
                && (forcePadding || !string.IsNullOrEmpty(value))
                && (value?.Length ?? 0) < length)
            {
                value = new string(paddingChar.Value, length)
                    + (value ?? string.Empty);
            }
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return value.Length < length
                ? value : value.Substring(value.Length - length, length);
        }

        /// <summary>
        /// bytecount で PadRight の実施
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        public static string PadRightMultiByte(this Encoding encoding, string value, int length, char padding = ' ')
        {
            value = value ?? string.Empty;
            var byteCount = encoding.GetByteCount(value);
            if (length < byteCount)
            {
                value = new string(value
                        .TakeWhile((c, i) =>
                            encoding.GetByteCount(value.Substring(0, i + 1)) <= length)
                        .ToArray());
                byteCount = encoding.GetByteCount(value);
            }
            return value.PadRight(length - (byteCount - value.Length), padding);
        }

    }
}
