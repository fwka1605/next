using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Rac.Util.CodeGen
{
    public static class CodeGenExtensions
    {
        /// <summary>
        /// 数値型 かどうかの判定
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <remarks>定義が足りない場合は追加 基本的に Model で使用されうる型を指定しておけばOK</remarks>
        public static bool IsNumber(this Type type)
            => type == typeof(int)
            || type == typeof(short)
            || type == typeof(long)
            || type == typeof(float)
            || type == typeof(double)
            || type == typeof(decimal)
            || type == typeof(System.Drawing.Color)
            ;



        public static bool IsString(this Type type)
            => type == typeof(string)
            || type == typeof(byte[])
            || type == typeof(DateTime);

        // json では Date を定義していないので、文字列へ
        /// <summary>
        /// <see cref="DateTime"/>型の場合は、 Date とする 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDate(this Type type)
            => type == typeof(DateTime);

        /// <summary>
        /// <see cref="object"/>型の場合は any とする
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsAny(this Type type)
            => type == typeof(object);


        /// <summary>
        ///  <see cref="Nullable{T}"/>かどうかの判定
        ///  <see cref="string"/>は参照型なので、null を許容 ← 要検討
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/nullable-types/how-to-identify-a-nullable-type
        /// </remarks>
        public static bool IsNullableType(this Type type)
            => (type.IsString() && !type.IsDate())
            || Nullable.GetUnderlyingType(type) != null;

        /// <summary>
        /// 配列型かどうかの判定
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsArrayType(this Type type)
            => !type.IsString()
            && type.GetInterfaces().Any(x
                => x.IsGenericType
                && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));

        public static string GetTypeScriptFileName(this Type type, bool requireExtension = true)
            => type.Name.GetTypeScriptFileName(requireExtension);

        public static string GetTypeScriptFileName(this string name, bool requireExtension = true)
        {
            var fileName = new string(name.SelectMany((x, i) =>
            {
                var requrireHypen = (
                    i != 0
                    && char.IsUpper(x)
                    && (
                        char.IsLower(name[i - 1])
                     || i < name.Length - 1 && char.IsLower(name[i + 1])
                    )
                );

                return requrireHypen ?
                    new[] { '-', char.ToLower(x) } :
                    new[] { char.ToLower(x) };
            }).ToArray())
            + ".model"
            + (requireExtension ? ".ts" : "");
            return fileName;
        }
    }

}
