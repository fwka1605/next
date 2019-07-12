using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import.ConcreteRecord
{
    public class MitsubishiUfjFactorDsv9End : DsvRecord
    {
        //public int DataKubun { get; set; }
        //public string Dummy { get; set; }

        public MitsubishiUfjFactorDsv9End(string[] fields, int lineNumber)
            : base(fields, lineNumber)
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
