using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import
{
    public abstract class Record
    {
        public readonly int LineNumber; // 固定長(改行なし)の場合は分割後の行番号
        public string Line { get; protected set; }

        /// <summary>
        /// 無加工のフィールド文字列リスト
        /// </summary>
        public List<string> RawFields { get; protected set; }

        /// <summary>
        /// 両端の空白やダブルクォーテーションマーク(DSVのみ)などをトリムしたフィールド文字列リスト
        /// </summary>
        public List<string> Fields { get; protected set; }

        public Record(int lineNumber)
        {
            LineNumber  = lineNumber;
            //Line        = line;
        }

        /// <summary>
        /// split <see cref="Line"/> to <see cref="RawFields"/>/<see cref="Fields"/>
        /// </summary>
        protected virtual void SplitLine() { }

        /// <summary>
        /// Validation Before Parsing <see cref="Fields"/>
        /// </summary>
        protected abstract void Validate();

        /// <summary>
        /// FieldList -> Concrete Record
        /// </summary>
        protected abstract void ParseFields();
    }
}
