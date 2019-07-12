using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import
{
    public abstract class DsvRecord : Record
    {

        public DsvRecord(string[] fields, int lineNumber) : base(lineNumber)
        {
            Fields = fields.ToList();

            Validate();
            ParseFields();
        }

    }
}
