using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Common.Security
{
    public class CompanyPasswordCreator
    {
        private readonly Dictionary<string, string> KeyWordDictionary;

        public CompanyPasswordCreator()
        {
            this.KeyWordDictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

            this.KeyWordDictionary.Add("a", "i");
            this.KeyWordDictionary.Add("b", "b");
            this.KeyWordDictionary.Add("c", "t");
            this.KeyWordDictionary.Add("d", "e");
            this.KeyWordDictionary.Add("e", "7");
            this.KeyWordDictionary.Add("f", "y");
            this.KeyWordDictionary.Add("g", "h");
            this.KeyWordDictionary.Add("h", "6");
            this.KeyWordDictionary.Add("i", "d");
            this.KeyWordDictionary.Add("j", "a");
            this.KeyWordDictionary.Add("k", "9");
            this.KeyWordDictionary.Add("l", "4");
            this.KeyWordDictionary.Add("m", "j");
            this.KeyWordDictionary.Add("n", "k");
            this.KeyWordDictionary.Add("o", "w");
            this.KeyWordDictionary.Add("p", "q");
            this.KeyWordDictionary.Add("q", "n");
            this.KeyWordDictionary.Add("r", "f");
            this.KeyWordDictionary.Add("s", "8");
            this.KeyWordDictionary.Add("t", "o");
            this.KeyWordDictionary.Add("u", "r");
            this.KeyWordDictionary.Add("v", "5");
            this.KeyWordDictionary.Add("w", "s");
            this.KeyWordDictionary.Add("x", "g");
            this.KeyWordDictionary.Add("y", "u");
            this.KeyWordDictionary.Add("z", "m");
            this.KeyWordDictionary.Add("0", "i");
            this.KeyWordDictionary.Add("1", "b");
            this.KeyWordDictionary.Add("2", "t");
            this.KeyWordDictionary.Add("3", "e");
            this.KeyWordDictionary.Add("4", "7");
            this.KeyWordDictionary.Add("5", "y");
            this.KeyWordDictionary.Add("6", "h");
            this.KeyWordDictionary.Add("7", "6");
            this.KeyWordDictionary.Add("8", "d");
            this.KeyWordDictionary.Add("9", "a");

        }

        public string Convert(string s)
        {
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(s));
            System.Diagnostics.Debug.Assert(KeyWordDictionary.ContainsKey(s));
            var value = KeyWordDictionary[s];
            return value;
        }


        public string ConvertKeyWordToPassword(string keyword)
        {
            var result = "";
            for (int i = 0; i < keyword.Length; i++)
            {
                var t1 = keyword.Substring(i, 1);
                var t2 = Convert(t1);
                result += t2;
            }
            return result;
        }
    }
}
