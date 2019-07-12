using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Rac.VOne.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> collection, string tableName = "")
        {
            var columnNames = new List<string>();
            Type t = typeof(T);
            PropertyInfo[] pia = t.GetProperties();
            foreach (PropertyInfo pi in pia)
            {
                columnNames.Add(pi.Name);
            }
            return collection.ToDataTable(columnNames, tableName);
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> collection, List<string> columnNames, string tableName = "")
        {
            if (string.IsNullOrEmpty(tableName)) tableName = typeof(T).Name;
            DataTable dt = new DataTable(tableName);

            Type t = typeof(T);

            var propList = t.GetProperties().Where(x => columnNames.Contains(x.Name)).ToList();
            propList.ForEach(x => dt.Columns.Add(x.Name, x.PropertyType));

            foreach (var item in collection)
            {
                DataRow dr = dt.NewRow();
                dr.BeginEdit();
                foreach (PropertyInfo pi in propList)
                {
                    dr[pi.Name] = pi.GetValue(item, null);
                }
                dr.EndEdit();
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary><see cref="IEnumerable{T}"/>をchunkSizeで分割</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        /// <remarks>
        /// https://github.com/dotnet/reactive/blob/master/Ix.NET/Source/System.Interactive/Buffer.cs
        /// </remarks>
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunkSize)
        {
            if (chunkSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(chunkSize), "Chunk size must be greater than 0.");
            IList<T> buffer = null;
            foreach (var x in source)
            {
                if (buffer == null)
                    buffer = new List<T>(chunkSize);
                buffer.Add(x);
                if (buffer.Count == chunkSize)
                {
                    yield return buffer;
                    buffer = null;
                }
            }
            if (buffer != null)
                yield return buffer;

        }
    }
}
