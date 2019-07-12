using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Common.DataHandling
{
    public static class Model
    {
        public static void CopyTo<TModel>(TModel from, TModel to, bool withUpdateAt, bool forceIdCopy = false)
            where TModel : class
        {
            if (from == null || to == null) return;

            // 追加/更新系プロパティ
            const string updateAtPropNames = "UpdateAt";
            IEnumerable<PropertyInfo> props = typeof(TModel).GetProperties();

            if (!forceIdCopy)
                props = props.Where(p => p.Name != "Id");

            if (!withUpdateAt)
                props = props.Where(p => p.Name != updateAtPropNames);

            foreach (PropertyInfo prop in props)
            {
                MethodInfo getter = prop.GetGetMethod();
                MethodInfo setter = prop.GetSetMethod();
                //object value = prop.GetValue(from, null);
                //prop.SetValue(to, value, null);
                if (getter != null && setter != null)
                {
                    object value = getter.Invoke(from, null);
                    setter.Invoke(to, new[] { value });
                }
            }
        }

        public static void SetUpdateBy<TModel>(IEnumerable<TModel> target, int userId)
            where TModel : class
        {
            const string updateByPropName = "UpdateBy";
            if (target == null || !target.Any()) return;

            PropertyInfo updateBy = typeof(TModel).GetProperty(updateByPropName);
            if (updateBy != null)
            {
                foreach (TModel model in target)
                {
                    updateBy.SetValue(model, userId);
                }
            }
        }

        public static List<TModel> CopyTo<TModel>(List<TModel> source)
            where TModel : class, new()
        {
            var result = new List<TModel>();
            foreach (var sourceItem in source)
            {
                var resultItem = new TModel();
                Model.CopyTo<TModel>(sourceItem, resultItem, true, forceIdCopy: true);
                result.Add(resultItem);
            }
            return result;
        }
    }
}
