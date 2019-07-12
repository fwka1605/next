using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using Dapper;
using Models = Rac.VOne.Web.Models;
using Rac.VOne.Data.Entities;
using System.Reflection;
using System.Linq;

namespace Rac.VOne.Data
{
    public static class DbHelperExtensions
    {
        public static T Do<T>(this IDbHelper helper,
            Func<IDbHelper, T> @do,
            IConnectionFactory factory)
        {
            var result = default(T);
            if (helper == null)
            {
                return result;
            }
            if (factory != null)
            {
                helper.SetConnectionFactory(factory);
            }
            try
            {
                result = @do(helper);
                return result;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
                throw;
            }
        }

        public static async Task<T> DoAsync<T>(this IDbHelper helper,
            Func<IDbHelper, Task<T>> @do,
            IConnectionFactory factory)
        {
            var result = default(T);
            if (helper == null)
            {
                return result;
            }
            if (factory != null)
            {
                helper.SetConnectionFactory(factory);
            }
            try
            {
                result = await @do(helper);
                return result;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
                throw;
            }
        }

        /// <summary><see cref="IEnumerable{T}"/>をテーブル型パラメータへ変換 </summary>
        /// <typeparam name="T"><see cref="int"/>または<see cref="long"/>または<see cref="string"/></typeparam>
        /// <param name="items"><see cref="IEnumerable{T}"/>の配列<see cref="T[]"/> または、<see cref="List{T}"/>など</param>
        /// <returns>
        /// <see cref="Dapper.SqlMapper.ICusomQueryParameter"/>となる
        /// そのため、匿名クラスでプロパティを定義する必要がある
        /// </returns>
        /// <remarks>
        /// 利用方法
        /// Id IN @Param となっていた箇所を修正
        /// 
        /// Id IN (
        ///  SELECT Id
        ///    FROM @TableParam )
        /// 
        /// helper.GetItem(query, new { TableParam = array.GetTableParameter() });
        /// 
        /// ※ テーブル型として定義した Ids(int), BigIds(long), Codes(string) は、それぞれ
        /// 項目を Primary Key として登録しているため、問合せ前に重複しないよう GroupBy などで
        /// ユニークな値の配列を連携するように注意してください。
        /// </remarks>
        public static SqlMapper.ICustomQueryParameter GetTableParameter<T>(this IEnumerable<T> items)
        {
            var type = typeof(T);
            var columnNames = new Dictionary<string, Type>();
            var typeName = string.Empty;
            PropertyInfo[] props = null;
            Type[] duplicationType = new [] {
                typeof(Models.BillingImportDuplication),
                typeof(Models.ReceiptImportDuplication)
            };

            if (type == typeof(int))
            {
                columnNames.Add("Id", type); typeName = "Ids";
            }
            else if (type == typeof(long))
            {
                columnNames.Add("Id", type); typeName = "BigIds";
            }
            else if (type == typeof(string))
            {
                columnNames.Add("Code", type); typeName = "Codes";
            }
            else
            {
                var modelType = duplicationType.FirstOrDefault(t => t.IsAssignableFrom(type));
                if (modelType != null)
                {
                    props = modelType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
                    foreach (var prop in props)
                    {
                        Type propType;
                        if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            propType = prop.PropertyType.GetGenericArguments().First();
                        else
                            propType = prop.PropertyType;
                        columnNames.Add(prop.Name, propType);
                    }
                    typeName = modelType.Name;
                }
                else
                {
                    throw new ArgumentException("parameter allowed int, long, string, XxxxImportDuplication");
                }
            }

            var table = new System.Data.DataTable();
            foreach (var pair in columnNames)
            {
                table.Columns.Add(pair.Key, pair.Value);
            }
            if (items == null)
                return null;

            if (props == null)
            {
                foreach (var value in items)
                    table.Rows.Add(value);
            }
            else
            {
                foreach (var value in items)
                    table.Rows.Add(props.Select(p => p.GetValue(value)).ToArray());
            }

            return table.AsTableValuedParameter(typeName);
        }

        /// <summary>
        ///  <see cref="IEnumerable{MatchingOrder}"/> of <see cref="MatchingOrder"/> から、ORDER BY 句を取得するメソッド
        /// </summary>
        /// <param name="orders"></param>
        /// <param name="converter"><see cref="MatchingOrder"/> から ソートを行う項目名の変更</param>
        /// <returns></returns>
        public static string GetOrderByQuery(this IEnumerable<Models.MatchingOrder> orders,
            Func<Models.MatchingOrder, string> converter)
        {
            var orderby = string.Empty;
            if (orders.Any())
            {
                orderby = " ORDER BY "
                    + string.Join("\n     , ",
                    orders.Select(x =>
                    {
                        var item = string.Empty;
                        item = converter?.Invoke(x);
                        if (string.IsNullOrEmpty(item)) return item;
                        return $"{item} {x.SortOrderDirection}";
                    }).Where(x => !string.IsNullOrEmpty(x))
                    .ToArray());
            }
            return orderby;
        }

        /// <summary>
        /// クエリがキャンセルされたことを確認する処理
        /// SQL Server(System.Data.SqlClient) への vendor lock-inがあるので注意
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="culter"></param>
        /// <returns></returns>
        public static bool HasCancelledException(this Exception exception, System.Globalization.CultureInfo culter = null)
        {
#if NETCOREAPP2_2
            return false;
#else
            const string KeyOperationCancelled = "SQL_OperationCancelled";
            var message = Manager.GetString(KeyOperationCancelled, culter ?? System.Threading.Thread.CurrentThread.CurrentCulture);
            var sqlException = exception as System.Data.SqlClient.SqlException;
            if (sqlException == null) return false;
            return sqlException.Errors.Cast<System.Data.SqlClient.SqlError>().Any(x => x.Message == message);
#endif
        }
        private static System.Resources.ResourceManager manager;
        private static System.Resources.ResourceManager Manager
        {
            get
            {
                if (manager == null)
                    manager = new System.Resources.ResourceManager("System.Data", typeof(System.Data.SqlClient.SqlCommand).Assembly);
                return manager;
            }
        }

    }
}
