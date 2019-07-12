using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Rac.VOne.Common.DataHandling
{

    /// <summary>Dapper のパラメータを変換するための converter</summary>
    /// <remarks>
    /// http://www.newtonsoft.com/json/help/html/CustomJsonConverter.htm
    /// Dapper.TableValuedParameter の内部で保持している private DataTable table を取得する
    /// Dapper 内部で 上記変更された場合は、対応が必要
    /// パラメータ自体は Dapper.SqlMapper.ICustomQueryParameter の interface となる
    /// object -> Json は可能 逆は不可
    /// </remarks>
    public class JsonCustomConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => true;

        /// <summary><see cref="CanRead"/></summary>
        /// <remarks>NEVER to call <see cref="ReadJson(JsonReader, Type, object, JsonSerializer)"/></remarks>
        public override bool CanRead
            => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            const string TargetPropertyName = "ICustomQueryParameter";

            var token = JToken.FromObject(value);
            var obj = token as JObject;
            var customParameters = value.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.PropertyType.Name == TargetPropertyName);

            const string TargetFieldName = "table";
            foreach (var property in customParameters)
            {
                var innerValue = property.GetValue(value);
                var field = innerValue?.GetType()
                    .GetField(TargetFieldName, BindingFlags.NonPublic | BindingFlags.Instance);
                var table = field?.GetValue(innerValue) as DataTable;
                if (table == null) continue;
                var array = new JArray();
                foreach (DataRow row in table.Rows)
                {
                    var jrow = new JObject();
                    foreach (DataColumn column in table.Columns)
                    {
                        jrow.Add(column.ColumnName.Trim(), JToken.FromObject(row[column]));
                    }
                    array.Add(jrow);
                }
                obj[property.Name] = array;
            }
            obj?.WriteTo(writer);
        }
    }
}
