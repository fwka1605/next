using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import.ConcreteRecord
{
    // 検証やパースの不要なDSVヘッダレコード(項目名表示行)などに使う。
    // 検証やパースが必要な場合、当クラスは使わずDsvRecordを継承して個別のヘッダレコードクラスを実装する。

    public class DsvHeader : DsvRecord
    {
        public DsvHeader(string[] fields, int lineNumber)
            : base(fields, lineNumber)
        {
        }

        protected override void Validate() { }
        protected override void ParseFields() { }

    }
}
