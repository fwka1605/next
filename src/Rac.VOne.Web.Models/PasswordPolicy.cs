using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class PasswordPolicy : IByCompany
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int MinLength { get; set; }
        [DataMember] public int MaxLength { get; set; }
        [DataMember] public int UseAlphabet { get; set; }
        [DataMember] public int MinAlphabetUseCount { get; set; }
        [DataMember] public int UseNumber { get; set; }
        [DataMember] public int MinNumberUseCount { get; set; }
        [DataMember] public int UseSymbol { get; set; }
        [DataMember] public int MinSymbolUseCount { get; set; }
        [DataMember] public string SymbolType { get; set; } = string.Empty;
        [DataMember] public int CaseSensitive { get; set; }
        [DataMember] public int MinSameCharacterRepeat { get; set; }
        [DataMember] public int ExpirationDays { get; set; }
        [DataMember] public int HistoryCount { get; set; }


        public char[] FullSymbolChars
            => new char[]
            {
                '!',
                '#',
                '%',
                '+',
                '-',
                '*',
                '/',
                '$',
                '_',
                '~',
                '\\',
                ';',
                ':',
                '@',
                '&',
                '?',
                '^',
            };
        public PasswordValidateResult Validate(string password)
        {
            password = Convert(password);

            var matAlph = Regex.Match(password, "[a-zA-Z]");
            if (UseAlphabet == 0 && matAlph.Success)
                return PasswordValidateResult.ProhibitionAlphabetChar;

            var matNumb = Regex.Match(password, "[0-9]");
            if (UseNumber == 0 && matNumb.Success)
                return PasswordValidateResult.ProhibitionNumberChar;

            var fullSymbolCount = 0;
            var allowedSymbolCount = 0;
            var notAllowedSymbolCount = 0;
            var maxCharRepeatedCount = 0;
            var charBuf = '\0';
            var countBuf = 0;
            foreach (var ch in password)
            {
                if (FullSymbolChars.Any(symbol => symbol == ch))
                    fullSymbolCount++;
                if (SymbolType.Any(symbol => symbol == ch))
                    allowedSymbolCount++;
                if (FullSymbolChars.Any(symbol => symbol == ch)
                    && !SymbolType.Any(symbol => symbol == ch))
                    notAllowedSymbolCount++;

                if (ch == charBuf)
                    countBuf++;
                else
                    countBuf = 1;
                charBuf = ch;
                maxCharRepeatedCount = Math.Max(countBuf, maxCharRepeatedCount);
            }
            if (UseSymbol == 0
                && 0 < fullSymbolCount)
                return PasswordValidateResult.ProhibitionSymbolChar;

            if (0 < notAllowedSymbolCount)
                return PasswordValidateResult.ProhibitionNotAllowedSymbolChar;


            if (UseAlphabet == 1
                && 0 < MinAlphabetUseCount
                && matAlph.Length < MinAlphabetUseCount)
                return PasswordValidateResult.ShortageAlphabetCharCount;
            if (UseNumber == 1
                && 0 < MinNumberUseCount
                && matNumb.Length < MinNumberUseCount)
                return PasswordValidateResult.ShortageNumberCharCount;

            if (UseSymbol == 1
                && 0 < MinSymbolUseCount
                && allowedSymbolCount < MinSymbolUseCount)
                return PasswordValidateResult.ShortageSymbolCharCount;

            if (password.Length < MinLength)
                return PasswordValidateResult.ShortagePasswordLength;

            if (MaxLength < password.Length)
                return PasswordValidateResult.ExceedPasswordLength;

            if (1 < MinSameCharacterRepeat
                && MinSameCharacterRepeat < maxCharRepeatedCount)
                return PasswordValidateResult.ExceedSameRepeatedChar;

            return PasswordValidateResult.Valid;
        }

        public string Convert(string plane)
        {
            return CaseSensitive == 1 ?
                plane :
                plane.ToUpper();
        }
    }

    /// <summary> パスワードポリシーに基づく検証結果 </summary>
    public enum PasswordValidateResult
    {
        /// <summary>有効</summary>
        Valid = 0,
        /// <summary>アルファベット禁止</summary>
        ProhibitionAlphabetChar,
        /// <summary>数字禁止</summary>
        ProhibitionNumberChar,
        /// <summary>記号禁止</summary>
        ProhibitionSymbolChar,
        /// <summary>不許可の記号禁止</summary>
        ProhibitionNotAllowedSymbolChar,
        /// <summary>アルファベット文字数不足</summary>
        ShortageAlphabetCharCount,
        /// <summary>数字文字数不足</summary>
        ShortageNumberCharCount,
        /// <summary>記号文字数不足</summary>
        ShortageSymbolCharCount,
        /// <summary>パスワード文字数不足</summary>
        ShortagePasswordLength,
        /// <summary>パスワード文字数超過</summary>
        ExceedPasswordLength,
        /// <summary>連続同一文字 文字数超過</summary>
        ExceedSameRepeatedChar
    }

    [DataContract] public class PasswordPolicyResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public PasswordPolicy PasswordPolicy { get; set; }
    }
}
