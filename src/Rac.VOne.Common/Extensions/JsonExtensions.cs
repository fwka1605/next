using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.Common.Extensions
{
    public static class JsonExtensions
    {
        /// <summary>
        /// クエリパラメータの json 化
        /// パラメータ内部の table を シリアライズするため、JsonCustomConverter を利用する
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConvertParameterToJson(this object value)
        {
            if (value == null) return string.Empty;
            return JsonConvert.SerializeObject(value,
                Formatting.Indented,
                new JsonCustomConverter());
        }

        /// <summary>
        /// オブジェクトのjson 形式の文字列へシリアライズ
        /// 一般的に <see cref="System.Collections.Generic.KeyValuePair{TKey, TValue}"/>を利用すれば
        /// 単純なjson は poco を作成せずに対応可能
        /// </summary>
        /// <param name="value">シリアライズしたいインスタンス</param>
        /// <param name="ignoreNull">nullになっている項目をシリアライズしない
        /// 文字列プロパティの初期化処理に注意</param>
        /// <returns></returns>
        public static string ConvertToJson(this object value, bool ignoreNull = true)
        {
            if (value == null) return string.Empty;
            return JsonConvert.SerializeObject(value,
                new JsonSerializerSettings
                {
                    NullValueHandling = ignoreNull
                    ? NullValueHandling.Ignore
                    : NullValueHandling.Include
                });
        }

        /// <summary>
        /// json 形式の文字列から、dynamic へデシリアライズ
        /// json 内部の項目を事前に把握しておく必要がある
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static dynamic ConverToDynamic(this string json)
        {
            if (string.IsNullOrEmpty(json)) return null;
            return JObject.Parse(json) as dynamic;
        }

        /// <summary>
        /// json 形式の 文字列から オブジェクトへのデシリアライズ
        /// 配列/入れ子になっている json のデシリアライズに注意
        /// plane な テキストに json 形式の文字列を用意し、デシリアライズできるか単体でテストすることを推奨
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="ignoreNull"></param>
        /// <returns></returns>
        public static T ConvertToModel<T>(this string json, bool ignoreNull = true)
        {
            if (string.IsNullOrEmpty(json)) return default(T);
            return JsonConvert.DeserializeObject<T>(json,
                new JsonSerializerSettings
                {
                    NullValueHandling = ignoreNull
                    ? NullValueHandling.Ignore
                    : NullValueHandling.Include
                });
        }
    }
}
