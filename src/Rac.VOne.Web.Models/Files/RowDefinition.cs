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
    /// <summary>重複データに対する振る舞い</summary>
    public enum DuplicateAdoption
    {
        /// <summary>最初のデータを優先する</summary>
        First,
        //Last,
        /// <summary>両者をエラーとする</summary>
        BothAreErrors,
    }

    /// <summary>重複データの扱い</summary>
    public enum TreatDuplicateAs
    {
        /// <summary>エラーとしてログ出力する</summary>
        Error,
        /// <summary>なかったものとして無視する</summary>
        Ignore,
    }

    public class RowDefinition<TModel>
        where TModel : class, new()
    {
        public string FileNameToken { get; protected set; }
        public string DataTypeToken { get; protected set; }
        public bool OutputHeader { get; set; } = true;
        public DuplicateAdoption DuplicateAdoption { get; set; } = DuplicateAdoption.BothAreErrors;
        public TreatDuplicateAs TreatDuplicateAs { get; set; } = TreatDuplicateAs.Error;

        public List<IFieldDefinition<TModel>> Fields { get; private set; }
                = new List<IFieldDefinition<TModel>>();
        public List<IFieldDefinition<TModel>> KeyFields { get; private set; }
                = new List<IFieldDefinition<TModel>>();

        public DataExpression DataExpression { get; private set; }

        public RowDefinition(DataExpression dataExpression)
        {
            DataExpression = dataExpression;
            Delimiter = ",";
        }

        public string Delimiter { get; set; }

        public void SetupFields()
        {
            int index = 1;
            foreach (IFieldDefinition<TModel> field in Fields.Where(f => !f.Ignored).OrderBy(f => f.FieldNumber))
            {
                field.FieldIndex = index++;
            }
        }

        public virtual string DefaultFileName { get { return string.Empty; } }

        public int StartLineNumber { get; set; }
        public virtual string PrintHeader()
        {
            Func<string, string> escape = null;
            if (Delimiter.Contains(","))
            {
                escape = value => (value.Contains("\"") || value.Contains(","))
                    ? $"\"{(value.Replace("\"", "\"\""))}\""
                    : value;
            }
            return string.Join(Delimiter,
                GetHeaderAry().Select(x => escape?.Invoke(x) ?? x).ToArray());
        }

        public string[] GetHeaderAry()
        {
            return OutputHeader ? Fields.Where(f => !f.Ignored)
                    .OrderBy(f => f.FieldNumber)
                    .Select(f => f.FieldName)
                    .ToArray()
                   : null;
        }

        public virtual bool Do(IFieldVisitor<TModel> visitor)
        {
            bool valid = true;
            foreach (var def in Fields)
            {
                if (!def.Ignored) valid &= def.Accept(visitor);
            }

            return valid;
        }

        public void SetFieldsSetting(
            IEnumerable<Web.Models.GridSetting> settings,
            Func<string, IFieldDefinition<TModel>> fieldSelector,
            int startNumber = 1)
        {
            if (settings == null
                || fieldSelector == null) return;
            foreach (var item in settings)
            {
                var column = item.ColumnName;
                var ignore = item.DisplayWidth == 0;
                var fieldNumber = item.DisplayOrder + startNumber;
                var field = fieldSelector(column);

                if (field == null) continue;
                field.FieldNumber = fieldNumber;
                field.Ignored = ignore;
                field.FieldName = ignore ? "" : item.ColumnNameJp;
            }
        }
    }
}
