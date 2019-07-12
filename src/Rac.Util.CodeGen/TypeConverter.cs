using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Runtime.Serialization;

namespace Rac.Util.CodeGen
{
    public class TypeConverter
    {
        public string Convert(Type type)
        {
            var builder = new StringBuilder();
            var dic = new Dictionary<Type, string>();
            builder.AppendLine($@"export class {type.Name} {{");
            foreach (var property in type.GetProperties()
                .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(DataMemberAttribute))))
            {
                builder.Append(ConvertProperty(property));
                var premitiveType = GetPrimitiveType(property.PropertyType);
                if (!IsModels(premitiveType) || dic.ContainsKey(premitiveType)) continue;
                dic.Add(premitiveType, $@"import {{{premitiveType.Name}}} from './{premitiveType.GetTypeScriptFileName(false)}';");
            }
            builder.AppendLine("}");

            foreach (var import in dic.Values)
                builder.Insert(0, $"{import}{Environment.NewLine}");

            return builder.ToString();
        }

        private string ConvertProperty(PropertyInfo info)
        {
            var name = info.Name;
            var camelName = new string(name.Select((x, i) => i == 0 ? char.ToLower(x) : x).ToArray());
            var privateName = $"_{camelName}";
            var typeName = ConvertType(info.PropertyType);
            var builder = new StringBuilder($"    public {camelName}: {typeName};");
    //        if (info.CanRead) builder.Append($@"
    //public get {camelName}() : {typeName} {{
    //    return this.{privateName};
    //}}");
    //        if (info.CanWrite) builder.Append($@"
    //public set {camelName}({camelName}: {typeName}) {{
    //    this.{privateName} = {camelName};
    //}}");
            builder.AppendLine();
            return builder.ToString();
        }

        private string ConvertType(Type type)
        {
            // generics getgenericarguments
            // array element except byte[]
            var source = GetPrimitiveType(type);

            var typeName
                = source.IsNumber() ? "number"
                : source.IsString() ? "string"
                : source.IsAny()    ? "any"
                : source.Name;

            if (type.IsArrayType()) typeName += "[]";

            if (type.IsNullableType()) typeName += " | null";

            return typeName;
        }

        private Type GetPrimitiveType(Type type)
        {
            if (type != typeof(byte[]) && type.IsArray) return type.GetElementType();
            if (type.IsGenericType) return type.GetGenericArguments().First();
            return type;
        }

        private bool IsModels(Type type)
        {
            return type.Namespace.StartsWith(@"Rac.VOne.Web.Models");
        }
    }
}
