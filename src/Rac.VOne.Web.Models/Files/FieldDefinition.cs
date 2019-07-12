using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Models.Files
{
    public interface IFieldDefinition<TModel>
        where TModel : class, new()
    {
        string FieldName { get; set; }
        int FieldNumber { get; set; } // one-based
        int FieldIndex { get; set; } // runtimeのindex
        bool Ignored { get; set; }
        bool Required { get; set; }
        Func<IFieldVisitor<TModel>, bool> Accept { get; set; }
        Func<Dictionary<int, TModel>, object, IEnumerable<WorkingReport>> ValidateAdditional { get; set; }
        string GetModelPeopertyName();
    }

    // 現状、対応するのは数値型と文字列、日付のみ。
    public class FieldDefinition<TModel, TValue> : IFieldDefinition<TModel>
        where TModel : class, new()
    {
        public string FieldName { get; set; }
        public int FieldNumber { get; set; }
        public int FieldIndex { get; set; }
        public bool Ignored { get; set; }

        protected PropertyInfo Property { get; set; }

        public bool Required { get; set; } = false;

        public Func<IFieldVisitor<TModel>, bool> Accept { get; set; }
        public Func<Dictionary<int, TModel>, object, IEnumerable<WorkingReport>> ValidateAdditional { get; set; }

        public FieldDefinition(Expression<Func<TModel, TValue>> prop)
        {
            if (prop != null)
            {
                Property = GetProperty(prop);
            }
        }

        protected PropertyInfo GetProperty<TTargetModel, TPropType>(Expression<Func<TTargetModel, TPropType>> prop)
        {
            PropertyInfo info = null;
            var memberSelectorExpression = prop.Body as MemberExpression;
            if (memberSelectorExpression != null)
            {
                info = memberSelectorExpression.Member as PropertyInfo;
            }

            if (info == null)
            {
               throw new ArgumentException();
            }
            return info;
        }

        public void SetValue(TModel model, TValue value)
        {
            Property?.SetValue(model, value, null);
        }

        public TValue GetValue(TModel model)
        {
            return (TValue)Property?.GetValue(model, null);
        }

        public string GetModelPeopertyName()
        {
            return Property?.Name;
        }
    }

    /// <summary>数値/日付フィールド</summary>
    public class NumberFieldDefinition<TModel, TValue> : FieldDefinition<TModel, TValue>
        where TModel : class, new()
        where TValue : struct, IComparable<TValue>
    {
        public NumberFieldDefinition(Expression<Func<TModel, TValue>> prop)
            : this(prop, "", 0, false, null, null)
        {
        }
        public NumberFieldDefinition(Expression<Func<TModel, TValue>> selector,
            string fieldName = "", int fieldNumber = 0, bool required = false,
            Func<IFieldVisitor<TModel>, bool> accept = null,
            Func<TValue, string> formatter = null)
            : base (selector)
        {
            FieldName = fieldName;
            FieldNumber = fieldNumber;
            Required = required;
            Accept = accept;
            Format = formatter;
        }

        public TValue Min { get; set; }
        public TValue Max { get; set; }
        public Func<TValue, string> Format { get; set; }

        public virtual bool IsNullable { get; } = false;
    }

    public class NullableNumberFieldDefinition<TModel, TValue> : NumberFieldDefinition<TModel, TValue>
        where TModel : class, new()
        where TValue : struct, IComparable<TValue>
    {
        public NullableNumberFieldDefinition(Expression<Func<TModel, TValue?>> prop)
            : this(prop, "", 0, false, null, null)
        {
        }

        public NullableNumberFieldDefinition(Expression<Func<TModel, TValue?>> selector,
            string fieldName = "", int fieldNumber = 0, bool required = false,
            Func<IFieldVisitor<TModel>, bool> accept = null,
            Func<TValue, string> formatter = null )
            :base(null)
        {
            Property = GetProperty(selector);

            FieldName = fieldName;
            FieldNumber = fieldNumber;
            Required = required;
            Accept = accept;
            Format = formatter;
        }

        public override bool IsNullable { get; } = true;

        public void SetNullableValue(TModel model, TValue? value)
        {
            Property?.SetValue(model, value);
        }

        public TValue? GetNullableValue(TModel model)
        {
            return (TValue?)Property?.GetValue(model, null);
        }
    }

    /// <summary>外部キーフィールド</summary>
    public class ForeignKeyFieldDefinition<TModel, TValue, TForeign> : FieldDefinition<TModel, TValue>
        where TModel : class, new()
        where TValue : struct, IComparable<TValue>
    {
        public ForeignKeyFieldDefinition(
            Expression<Func<TModel, TValue>> prop,
            Expression<Func<TForeign, TValue>> foreignIdProp,
            Expression<Func<TModel, string>> codeProp,
            Expression<Func<TForeign, string>> foreignCodeProp)
            : this(prop, foreignIdProp, codeProp, foreignCodeProp, "", 0, false, null)
        {
        }

        public ForeignKeyFieldDefinition(
            Expression<Func<TModel, TValue>> prop,
            Expression<Func<TForeign, TValue>> foreignIdProp,
            Expression<Func<TModel, string>> codeProp,
            Expression<Func<TForeign, string>> foreignCodeProp,
            string fieldName = "",
            int fieldNumber = 0,
            bool required = false,
            Func<IFieldVisitor<TModel>, bool> accept = null)
            : base(prop)
        {
            if (codeProp != null)
                CodeProperty = GetProperty(codeProp);
            if (foreignIdProp != null)
                ForeignKeyProp = GetProperty(foreignIdProp);
            if (foreignCodeProp != null)
                ForeignCodeProp = GetProperty(foreignCodeProp);
            FieldName = fieldName;
            FieldNumber = fieldNumber;
            Required = required;
            Accept = accept;
        }

        public List<TForeign> ForeignModels { get; set; }
        public Func<string[], Dictionary<string, TForeign>> GetModelsByCode { get; set; }
        public Func<TValue[], Dictionary<TValue, TForeign>> GetModelsById { get; set; }

        public bool ModelHasCode { get { return CodeProperty != null; } }
        protected PropertyInfo ForeignKeyProp { get; set; }
        protected PropertyInfo ForeignCodeProp { get; set; }
        protected PropertyInfo CodeProperty { get; set; }
        public string GetCode(TModel model)
        {
            return CodeProperty?.GetValue(model) as string;
        }

        public void SetCode(TModel model, string value)
        {
            CodeProperty?.SetValue(model, value);
        }

        public TValue GetForeignKey(TForeign model)
        {
            return (TValue)ForeignKeyProp?.GetValue(model, null);
        }

        public string GetForeignCode(TForeign model)
        {
            return ForeignCodeProp?.GetValue(model, null).ToString();
        }

        public virtual bool IsNullable { get; } = false;
    }

    public class NullableForeignKeyFieldDefinition<TModel, TValue, TForeign> : ForeignKeyFieldDefinition<TModel, TValue, TForeign>
        where TModel : class, new()
        where TValue : struct, IComparable<TValue>
    {
        public NullableForeignKeyFieldDefinition(
            Expression<Func<TModel, TValue?>> prop,
            Expression<Func<TForeign, TValue>> foreignIdProp,
            Expression<Func<TModel, string>> codeProp,
            Expression<Func<TForeign, string>> foreignCodeProp)
            : this(prop, foreignIdProp, codeProp, foreignCodeProp, "", 0, false, null)
        {
        }

        public NullableForeignKeyFieldDefinition(
            Expression<Func<TModel, TValue?>> prop,
            Expression<Func<TForeign, TValue>> foreignIdProp,
            Expression<Func<TModel, string>> codeProp,
            Expression<Func<TForeign, string>> foreignCodeProp,
            string fieldName = "",
            int fieldNumber = 0,
            bool required = false,
            Func<IFieldVisitor<TModel>, bool> accept = null)
            :base(null, foreignIdProp, codeProp, foreignCodeProp, fieldName, fieldNumber, required, accept)
        {
            if (prop != null)
                Property = GetProperty(prop);
        }

        public override bool IsNullable { get; } = true;

        public void SetNullableValue(TModel model, TValue? value)
        {
            Property?.SetValue(model, value);
        }

        public TValue? GetNullableValue(TModel model)
        {
            return (TValue?)Property?.GetValue(model, null);
        }

        public TValue? GetNullableForeignKey(TForeign model)
        {
            return (TValue)ForeignKeyProp?.GetValue(model, null);
        }
    }

    public class StandardIdToCodeFieldDefinition<TModel, TForeign> : ForeignKeyFieldDefinition<TModel, int, TForeign>
        where TModel : class, new()
    {
        /// <summary>マスター系 Id ⇔ Code 変換 Field定義</summary>
        /// <param name="prop">自モデル内 他 Master の Id を指定する keySelector</param>
        /// <param name="foreignIdProp">他 Master モデルの Id を指定する keySelector</param>
        /// <param name="codeProp">自モデル 他 Master の Code を指定する keySelector</param>
        /// <param name="foreignCodeProp">他 Master モデルの Code を指定する KeySelector</param>
        /// <param name="fieldName"></param>
        /// <param name="fieldNumber"></param>
        /// <param name="required"></param>
        /// <param name="accept"></param>
        public StandardIdToCodeFieldDefinition(
            Expression<Func<TModel, int>> prop,
            Expression<Func<TForeign, int>> foreignIdProp,
            Expression<Func<TModel, string>> codeProp,
            Expression<Func<TForeign, string>> foreignCodeProp,
            string fieldName = "",
            int fieldNumber = 0,
            bool required = false,
            Func<IFieldVisitor<TModel>, bool> accept = null,
            Func<string[], Dictionary<string, TForeign>> getModelsByCode = null,
            Func<int[], Dictionary<int, TForeign>> getModelsById = null)
            :base(prop, foreignIdProp, codeProp, foreignCodeProp, fieldName, fieldNumber, required, accept)
        {
            GetModelsByCode = getModelsByCode;
            GetModelsById = getModelsById;
        }
    }
    public class StandardNullableIdToCodeFieldDefinition<TModel, TForeign> : NullableForeignKeyFieldDefinition<TModel, int, TForeign>
        where TModel : class, new()
    {
        public StandardNullableIdToCodeFieldDefinition(
            Expression<Func<TModel, int?>> prop,
            Expression<Func<TForeign, int>> foreignIdProp,
            Expression<Func<TModel, string>> codeProp,
            Expression<Func<TForeign, string>> foreignCodeProp,
            string fieldName = "",
            int fieldNumber = 0,
            bool required = false,
            Func<IFieldVisitor<TModel>, bool> accept = null,
            Func<string[], Dictionary<string, TForeign>> getModelsByCode = null,
            Func<int[], Dictionary<int, TForeign>> getModelsById = null)
            : base(prop, foreignIdProp, codeProp, foreignCodeProp, fieldName, fieldNumber, required, accept)
        {
            GetModelsByCode = getModelsByCode;
            GetModelsById = getModelsById;
        }

    }

    /// <summary>文字列フィールド</summary>
    public class StringFieldDefinition<TModel> : FieldDefinition<TModel, string>
        where TModel : class, new()
    {
        public StringFieldDefinition(Expression<Func<TModel, string>> prop)
            : this(prop, "", 0, false, 0, 0, null)
        {
        }

        public StringFieldDefinition(Expression<Func<TModel, string>> selector,
            string fieldName = "", int fieldNumber = 0, bool required = false,
            int minLength = 0, int maxLength = 0,
            Func<IFieldVisitor<TModel>, bool> accept = null)
            : base(selector)
        {
            FieldName = fieldName;
            FieldNumber = fieldNumber;
            Required = required;
            MinLength = minLength;
            MaxLength = maxLength;
            Accept = accept;
        }

        public int MinLength { get; set; }
        public int MaxLength { get; set; }
    }
}
