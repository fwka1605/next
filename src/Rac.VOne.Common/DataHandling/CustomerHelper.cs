using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Common.DataHandling
{
    public class CustomerHelper
    {
        #region pattern
        ///<summary> A : 大文字のアルファベット（A～Z）</summary>
        private const string UpperCasePermission = "A-Z";
        ///<summary> a : 小文字のアルファベット（a～z）</summary>
        private const string LowerCasePermission = "a-z";
        ///<summary> K : カタカナ（促音・拗音の小書き表記あり）</summary>
        private const string HankakuKanaKPermission = @"\uff61-\uff9f";
        ///<summary> N : カタカナ（促音・拗音の小書き表記なし）</summary>
        private const string HankakuKanaNPermission = @"\uff61-\uff66\uff70-\uff9f";
        ///<summary> 9 : 数字（0～9）</summary>
        private const string DigitPermission = "0-9";
        ///<summary> # : 数字および数字関連記号（0～9、+ - $ % \ , .）</summary>
        private const string NumberPermission = DigitPermission + @"-\+\$%\,\.";
        ///<summary> @ : 記号（! " # $ % & ' ( ) - = ^ ~ \ | @ ` [ { ; + : * ] } , < . > / ? _ ｡ ｢ ｣ ､ ･）</summary>
        private const string SymbolPermission = "][!\"" + @"#\$%&'\(\)-=\^~\\\|@`\{;\+\:\*\},<\.>/\?_｡｢｣､･";
        /// <summary> A9 : 英数 0-9A-Z</summary>
        private const string DigitCharPermission = DigitPermission + UpperCasePermission;
        /// <summary>\uff61-\uff9f0-9A-Z</summary>
        private const string KanaDigitCharPermission = HankakuKanaKPermission + DigitCharPermission + SymbolPermission;
        /// <summary>0-9A-Z</summary>
        private const string DigitAlphabetPermission = DigitPermission + UpperCasePermission;
        /// <summary>\uff61-\uff9f0-9A-Z</summary>
        private const string KanaDigitAlphabetPermission = HankakuKanaKPermission + DigitPermission + UpperCasePermission;
        /// <summary>数字のみに制限する正規表現パターン ^[0-9]+$</summary>
        public static string DigitPermissionPattern { get { return ConvertPattern(DigitPermission); } }
        public static string DigitDecPermissionPattern { get { return ConvertPattern(DigitPermission + @"\."); } }
        /// <summary>英数のみに制限する正規表現パターン ^[0-9A-Z]+$</summary>
        public string DigitCharPermissionPattern { get { return ConvertPattern("-" + DigitCharPermission); } }
        /// <summary>得意先コード用英数のみに制限する正規表現パターン ^[-_0-9A-Z]+$</summary>
        public static string CustomerCodeDigitCharPermissionPattern { get { return ConvertPattern("-" + "_" + DigitAlphabetPermission); } }
        /// <summary>得意先コード用カナ英数のみに制限する正規表現パターン ^[-_\uff61-\uff9f0-9A-Z]+$</summary>
        public static string CustomerCodeKanaDigitCharPermissionPattern { get { return ConvertPattern("-" + "_" + KanaDigitAlphabetPermission); } }
        /// <summary>口座振替用銀行名・支店名　カナ英数記号のみに制限する正規表現パターン ^[\uff61-\uff9f0-9A-Z@]+$</summary>
        public static string KanaDigitCharPermissionPattern { get { return ConvertPattern(KanaDigitCharPermission); } }
        /// <summary>英数 小文字許可 ^[0-9A-Z]+$</summary>
        public static string DigitAlphabetPermissionPattern { get { return ConvertPattern(DigitAlphabetPermission); } }
        /// <summary>電話番号 FAX番号 数字 "-" 許可</summary>
        public static string TelFaxPostNumberPermissionPattern { get { return ConvertPattern("-" + DigitPermission); } }
        /// <summary>正規表現用表記への変換メソッド </summary>
        /// <param name="value"></param>
        /// <returns>行頭から行末まで、入力されたvalue の繰り返しとなる正規表現 ^[{value}]+$</returns>
        public static string ConvertPattern(string value) => $@"^[{value}]+$";
        #endregion
                
        public static string CustomerPermission(int codeType)
        {
            return codeType == 0 ? DigitPermissionPattern : 
                   codeType == 1 ? CustomerCodeDigitCharPermissionPattern :
                                   CustomerCodeKanaDigitCharPermissionPattern;
        }
    }
}
