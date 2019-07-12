using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rac.VOne.Common.DataHandling
{
    // EBデータ取込の共通処理
    public class EbDataHelper
    {
        /// <summary>漢字の包含をチェックする。</summary>
        public static bool ContainsKanji(string value)
        {
            // U+F900からU+FAFF   : CJK互換漢字     (CJK Compatibility Ideographs)
            // U+3400からU+4DBF   : CJK統合漢字拡張A(CJK Unified Ideographs Extension A)
            // U+20000からU+2A6DF : CJK統合漢字拡張B(CJK Unified Ideographs Extension B)
            const string range = @"[\p{IsCJKUnifiedIdeographs}" +
                @"\p{IsCJKCompatibilityIdeographs}" +
                @"\p{IsCJKUnifiedIdeographsExtensionA}]|" +
                @"[\uD840-\uD869][\uDC00-\uDFFF]|\uD869[\uDC00-\uDEDF]";

            return Regex.IsMatch(value, range);
        }

        /// <summary>ひらがな→カタカナ、全角→半角変換</summary>
        public static string ConvertToHankakuKatakana(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            value = Normalize(value);
            var chars = new List<char>();
            for (var i = 0; i < value.Length; i++)
            {
                var c = value[i];
                if (HiraToKanaDictionary.ContainsKey(c))
                    c = HiraToKanaDictionary[c];
                if (KataToHalfKanaDictionary.ContainsKey(c))
                    chars.AddRange(KataToHalfKanaDictionary[c]);
                else
                    chars.Add(c);
            }
            return new string(chars.ToArray());
        }

        /// <summary>文字列の Normalize</summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks>
        /// 実施している内容 全角英数 の 半角化 など
        /// </remarks>
        private static string Normalize(string value)
        {
            value = ConvertVoicedSoundMark(value); // 全角 濁音・半濁音を Normalize すると 半角スペースが混入する
            value = value.Normalize(NormalizationForm.FormKC); // Normalize すると 半角 濁音・半濁音 は 全角 へ変換される
            return value;
        }

        private static string ConvertVoicedSoundMark(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            var chars = new char[value.Length];
            for (var i = 0; i < value.Length; i++)
            {
                var c = value[i];
                chars[i] = VoicedSoundMarkDictionary.ContainsKey(c) ? VoicedSoundMarkDictionary[c] : c;
            }
            return new string(chars);
        }

        /// <summary>法人格情報削除処理</summary>
        /// <param name="value">削除を行う対象の文字列</param>
        /// <param name="leagalPersonalities">削除するカナの配列 Web.Models.JuridicalPersonality に登録されている Kana の配列 </param>
        /// <remarks>法人格削除後に、 Trim() を行い、前後の半角スペースを取り除く</remarks>
        public static string RemoveLegalPersonality(string value, IEnumerable<string> leagalPersonalities)
        {
            if (value == null) return null;
            if (string.IsNullOrEmpty(value)
                || !(leagalPersonalities?.Any() ?? false)) return value;

            var patterns = new string[] { "({0})", "{0})", "({0}" };
            foreach (var personality in leagalPersonalities.OrderByDescending(x => x.Length).ThenBy(x => x))
                foreach (var pattern in patterns)
                {
                    var target = string.Format(pattern, personality);
                    if (!value.Contains(target)) continue;
                    value = value.Replace(target, "");
                }
            return value.Trim();
        }

        /// <summary>禁止文字削除処理</summary>
        public static string RemoveProhibitCharacter(string value)
        {
            const string prohibit = @"][､\:;<>&\^%#\?@\$\|_'\+\*=!" + "\"";
            return Regex.Replace(value, "[" + prohibit + "]", string.Empty);
        }

        /// <summary>EBデータ使用不可文字変換処理</summary>
        public static string ConvertProhibitCharacter(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;
            return value.Replace("｡", ".")
                        .Replace("･", ".")
                        .Replace("ｰ", "-")
                        .Replace("ｧ", "ｱ")
                        .Replace("ｨ", "ｲ")
                        .Replace("ｩ", "ｳ")
                        .Replace("ｪ", "ｴ")
                        .Replace("ｫ", "ｵ")
                        .Replace("ｬ", "ﾔ")
                        .Replace("ｭ", "ﾕ")
                        .Replace("ｮ", "ﾖ")
                        .Replace("ｯ", "ﾂ")
                        .ToUpper();
        }

        /// <summary>小文字->大文字 lower to UPPER</summary>
        public static string ConvertToUpperCase(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;
            return value.ToUpper();
        }

        /// <summary>EB使用可能文字変換 + 法人格削除処理</summary>
        /// <param name="value">交換を行う対象の文字列</param>
        /// <param name="leagalPersonalities">削除するカナの配列 Web.Models.JuridicalPersonality に登録されている Kana の配列 </param>
        /// <remarks>ひらがな、全角→半角ｶﾅ、禁止文字を変換、禁止文字を削除、法人格を削除する</remarks>
        public static string ConvertToPayerName(string value, IEnumerable<string> legalPersonalities)
        {
            value = ConvertToValidEbKana(value);
            value = RemoveLegalPersonality(value, legalPersonalities);
            return value;
        }

        /// <summary>EB使用可能文字変換</summary>
        /// <param name="value">変換を行う対象の文字列</param>
        /// <remarks>法人格削除処理は行わない</remarks>
        public static string ConvertToValidEbKana(string value)
        {
            value = ConvertToHankakuKatakana(value);
            value = ConvertToUpperCase(value);
            value = ConvertProhibitCharacter(value);
            value = RemoveProhibitCharacter(value);
            return value;
        }

        /// <summary>
        /// 有効な文字のみで構成されているかチェックする。
        /// </summary>
        public static bool IsValidEBChars(string value)
        {
            return Regex.IsMatch(value, @"^[ 0-9A-Z\uFF62-\uFF9F!""#$%&'()-=^~\\|@`[{;+:*\]},<.>/?_｡｢｣､･]+$");
        }

        private static Encoding _shift_jis;
        public static Encoding ShiftJis {
            get {
                if (_shift_jis == null) _shift_jis = Encoding.GetEncoding(932);
                return _shift_jis;
            }
        }

        private static readonly Dictionary<char, char> VoicedSoundMarkDictionary = new Dictionary<char, char> {
            { '\u3099', '\uFF9E' },
            { '\u309A', '\uFF9F' },
            { '\u309B', '\uFF9E' },
            { '\u309C', '\uFF9F' },
        };

        private static readonly Dictionary<char, char> HiraToKanaDictionary = new Dictionary<char, char> {
                                    { '\u3041', '\u30A1' }, { '\u3042', '\u30A2' }, { '\u3043', '\u30A3' }, { '\u3044', '\u30A4' }, { '\u3045', '\u30A5' }, { '\u3046', '\u30A6' }, { '\u3047', '\u30A7' }, { '\u3048', '\u30A8' }, { '\u3049', '\u30A9' }, { '\u304A', '\u30AA' }, { '\u304B', '\u30AB' }, { '\u304C', '\u30AC' }, { '\u304D', '\u30AD' }, { '\u304E', '\u30AE' }, { '\u304F', '\u30AF' },
            { '\u3050', '\u30B0' }, { '\u3051', '\u30B1' }, { '\u3052', '\u30B2' }, { '\u3053', '\u30B3' }, { '\u3054', '\u30B4' }, { '\u3055', '\u30B5' }, { '\u3056', '\u30B6' }, { '\u3057', '\u30B7' }, { '\u3058', '\u30B8' }, { '\u3059', '\u30B9' }, { '\u305A', '\u30BA' }, { '\u305B', '\u30BB' }, { '\u305C', '\u30BC' }, { '\u305D', '\u30BD' }, { '\u305E', '\u30BE' }, { '\u305F', '\u30BF' },
            { '\u3060', '\u30C0' }, { '\u3061', '\u30C1' }, { '\u3062', '\u30C2' }, { '\u3063', '\u30C3' }, { '\u3064', '\u30C4' }, { '\u3065', '\u30C5' }, { '\u3066', '\u30C6' }, { '\u3067', '\u30C7' }, { '\u3068', '\u30C8' }, { '\u3069', '\u30C9' }, { '\u306A', '\u30CA' }, { '\u306B', '\u30CB' }, { '\u306C', '\u30CC' }, { '\u306D', '\u30CD' }, { '\u306E', '\u30CE' }, { '\u306F', '\u30CF' },
            { '\u3070', '\u30D0' }, { '\u3071', '\u30D1' }, { '\u3072', '\u30D2' }, { '\u3073', '\u30D3' }, { '\u3074', '\u30D4' }, { '\u3075', '\u30D5' }, { '\u3076', '\u30D6' }, { '\u3077', '\u30D7' }, { '\u3078', '\u30D8' }, { '\u3079', '\u30D9' }, { '\u307A', '\u30DA' }, { '\u307B', '\u30DB' }, { '\u307C', '\u30DC' }, { '\u307D', '\u30DD' }, { '\u307E', '\u30DE' }, { '\u307F', '\u30DF' },
            { '\u3080', '\u30E0' }, { '\u3081', '\u30E1' }, { '\u3082', '\u30E2' }, { '\u3083', '\u30E3' }, { '\u3084', '\u30E4' }, { '\u3085', '\u30E5' }, { '\u3086', '\u30E6' }, { '\u3087', '\u30E7' }, { '\u3088', '\u30E8' }, { '\u3089', '\u30E9' }, { '\u308A', '\u30EA' }, { '\u308B', '\u30EB' }, { '\u308C', '\u30EC' }, { '\u308D', '\u30ED' }, { '\u308E', '\u30EE' }, { '\u308F', '\u30EF' },
            { '\u3090', '\u30F0' }, { '\u3091', '\u30F1' }, { '\u3092', '\u30F2' }, { '\u3093', '\u30F3' }, { '\u3094', '\u30F4' }, { '\u3095', '\u30F5' }, { '\u3096', '\u30F6' },
        };

        private static readonly Dictionary<char, char[]> KataToHalfKanaDictionary = new Dictionary<char, char[]> {
            { '\u30A1', new[] { '\uFF67'           } },
            { '\u30A2', new[] { '\uFF71'           } },
            { '\u30A3', new[] { '\uFF68'           } },
            { '\u30A4', new[] { '\uFF72'           } },
            { '\u30A5', new[] { '\uFF69'           } },
            { '\u30A6', new[] { '\uFF73'           } },
            { '\u30A7', new[] { '\uFF6A'           } },
            { '\u30A8', new[] { '\uFF74'           } },
            { '\u30A9', new[] { '\uFF6B'           } },
            { '\u30AA', new[] { '\uFF75'           } },
            { '\u30AB', new[] { '\uFF76'           } },
            { '\u30AC', new[] { '\uFF76', '\uFF9E' } },
            { '\u30AD', new[] { '\uFF77'           } },
            { '\u30AE', new[] { '\uFF77', '\uFF9E' } },
            { '\u30AF', new[] { '\uFF78'           } },
            { '\u30B0', new[] { '\uFF78', '\uFF9E' } },
            { '\u30B1', new[] { '\uFF79'           } },
            { '\u30B2', new[] { '\uFF79', '\uFF9E' } },
            { '\u30B3', new[] { '\uFF7A'           } },
            { '\u30B4', new[] { '\uFF7A', '\uFF9E' } },
            { '\u30B5', new[] { '\uFF7B'           } },
            { '\u30B6', new[] { '\uFF7B', '\uFF9E' } },
            { '\u30B7', new[] { '\uFF7C'           } },
            { '\u30B8', new[] { '\uFF7C', '\uFF9E' } },
            { '\u30B9', new[] { '\uFF7D'           } },
            { '\u30BA', new[] { '\uFF7D', '\uFF9E' } },
            { '\u30BB', new[] { '\uFF7E'           } },
            { '\u30BC', new[] { '\uFF7E', '\uFF9E' } },
            { '\u30BD', new[] { '\uFF7F'           } },
            { '\u30BE', new[] { '\uFF7F', '\uFF9E' } },
            { '\u30BF', new[] { '\uFF80'           } },
            { '\u30C0', new[] { '\uFF80', '\uFF9E' } },
            { '\u30C1', new[] { '\uFF81'           } },
            { '\u30C2', new[] { '\uFF81', '\uFF9E' } },
            { '\u30C3', new[] { '\uFF6F'           } },
            { '\u30C4', new[] { '\uFF82'           } },
            { '\u30C5', new[] { '\uFF82', '\uFF9E' } },
            { '\u30C6', new[] { '\uFF83'           } },
            { '\u30C7', new[] { '\uFF83', '\uFF9E' } },
            { '\u30C8', new[] { '\uFF84'           } },
            { '\u30C9', new[] { '\uFF84', '\uFF9E' } },
            { '\u30CA', new[] { '\uFF85'           } },
            { '\u30CB', new[] { '\uFF86'           } },
            { '\u30CC', new[] { '\uFF87'           } },
            { '\u30CD', new[] { '\uFF88'           } },
            { '\u30CE', new[] { '\uFF89'           } },
            { '\u30CF', new[] { '\uFF8A'           } },
            { '\u30D0', new[] { '\uFF8A', '\uFF9E' } },
            { '\u30D1', new[] { '\uFF8A', '\uFF9F' } },
            { '\u30D2', new[] { '\uFF8B'           } },
            { '\u30D3', new[] { '\uFF8B', '\uFF9E' } },
            { '\u30D4', new[] { '\uFF8B', '\uFF9F' } },
            { '\u30D5', new[] { '\uFF8C'           } },
            { '\u30D6', new[] { '\uFF8C', '\uFF9E' } },
            { '\u30D7', new[] { '\uFF8C', '\uFF9F' } },
            { '\u30D8', new[] { '\uFF8D'           } },
            { '\u30D9', new[] { '\uFF8D', '\uFF9E' } },
            { '\u30DA', new[] { '\uFF8D', '\uFF9F' } },
            { '\u30DB', new[] { '\uFF8E'           } },
            { '\u30DC', new[] { '\uFF8E', '\uFF9E' } },
            { '\u30DD', new[] { '\uFF8E', '\uFF9F' } },
            { '\u30DE', new[] { '\uFF8F'           } },
            { '\u30DF', new[] { '\uFF90'           } },
            { '\u30E0', new[] { '\uFF91'           } },
            { '\u30E1', new[] { '\uFF92'           } },
            { '\u30E2', new[] { '\uFF93'           } },
            { '\u30E3', new[] { '\uFF6C'           } },
            { '\u30E4', new[] { '\uFF94'           } },
            { '\u30E5', new[] { '\uFF6D'           } },
            { '\u30E6', new[] { '\uFF95'           } },
            { '\u30E7', new[] { '\uFF6E'           } },
            { '\u30E8', new[] { '\uFF96'           } },
            { '\u30E9', new[] { '\uFF97'           } },
            { '\u30EA', new[] { '\uFF98'           } },
            { '\u30EB', new[] { '\uFF99'           } },
            { '\u30EC', new[] { '\uFF9A'           } },
            { '\u30ED', new[] { '\uFF9B'           } },
            { '\u30EE', new[] { '\uFF9C'           } },
            { '\u30EF', new[] { '\uFF9C'           } },
            { '\u30F0', new[] { '\uFF72'           } },
            { '\u30F1', new[] { '\uFF74'           } },
            { '\u30F2', new[] { '\uFF66'           } },
            { '\u30F3', new[] { '\uFF9D'           } },
            { '\u30F4', new[] { '\uFF73', '\uFF9E' } },
            { '\u30F5', new[] { '\uFF76'           } },
            { '\u30F6', new[] { '\uFF79'           } },
            { '\u30F7', new[] { '\uFF9C', '\uFF9E' } },
            { '\u30F8', new[] { '\uFF72', '\uFF9E' } },
            { '\u30F9', new[] { '\uFF74', '\uFF9E' } },
            { '\u30FA', new[] { '\uFF75', '\uFF9E' } },
            { '\u30FB', new[] { '\uFF65'           } },
            { '\u30FC', new[] { '\uFF70'           } },
            { '\u3001', new[] { '\uFF64'           } },
            { '\u3002', new[] { '\uFF61'           } },
            { '\u300C', new[] { '\uFF62'           } },
            { '\u300D', new[] { '\uFF63'           } },
            { '\u3099', new[] { '\uFF9E'           } },
            { '\u309A', new[] { '\uFF9F'           } },
            { '\u309B', new[] { '\uFF9E'           } },
            { '\u309C', new[] { '\uFF9F'           } },
        };

    }
}
