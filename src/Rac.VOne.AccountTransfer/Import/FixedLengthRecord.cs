using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import
{
    public abstract class FixedLengthRecord : Record
    {
        public FixedLengthRecord(int lineNumber, string line) : base(lineNumber)
        {
            Line = line;
            SplitLine();
            Validate();
            ParseFields();
        }

        /// <summary>
        /// 固定長フィールド桁数リスト
        /// </summary>
        public abstract IEnumerable<int> FieldLengthList { get; }

        /// <summary>
        /// Line -> RawFieldList / FieldList
        /// </summary>
        protected override void SplitLine()
        {
            var fields = new List<string>();

            var index = 0;
            foreach (var len in FieldLengthList)
            {
                fields.Add(Line.Substring(index, len));
                index += len;
            }

            RawFields = fields;
            Fields = RawFields.Select(x => x.Trim()).ToList();
        }
    }
}
