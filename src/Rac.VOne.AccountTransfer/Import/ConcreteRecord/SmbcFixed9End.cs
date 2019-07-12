using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import.ConcreteRecord
{
    public class SmbcFixed9End : FixedLengthRecord
    {
        public override IEnumerable<int> FieldLengthList { get; } = new[] { 1, 119 };

        //public int DataKubun { get; set; }
        //public string Dummy { get; set; }

        public SmbcFixed9End(int lineNumber, string line)
            : base(lineNumber, line)
        {
        }

        protected override void Validate()
        {
        }

        protected override void ParseFields()
        {
        }

    }
}
