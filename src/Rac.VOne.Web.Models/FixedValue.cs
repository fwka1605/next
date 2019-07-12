using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    /// <summary>
    /// フリーインポーター 固定値の検索結果（二択など）用
    /// </summary>
    [DataContract]
    public class FixedValue
    {
        [DataMember] public string Key { get; set; }
        [DataMember] public string Value { get; set; }

        public FixedValue() { }
        public FixedValue(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
