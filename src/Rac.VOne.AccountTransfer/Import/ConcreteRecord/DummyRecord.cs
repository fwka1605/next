using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import.ConcreteRecord
{
    // フォーマット上存在しないレコードに使う。

    // AccountTransferResultクラスはTHeader,TData,TTrailer,TEndの型を指定する必要があるが、
    // 例えば三菱UFJニコスのフォーマットにはエンドレコードが無いため、TEndのレコード型としてDummyRecordを指定する。

    public class DummyRecord : Record
    {
        public DummyRecord(int lineNumber)
            : base(lineNumber)
        {
        }

        protected override void SplitLine() { }
        protected override void Validate() { }
        protected override void ParseFields() { }
    }
}
