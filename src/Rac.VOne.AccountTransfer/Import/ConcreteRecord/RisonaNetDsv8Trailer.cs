using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import.ConcreteRecord
{
    public class RisonaNetDsv8Trailer : DsvRecord
    {
        ///// <summary>1:データ区分</summary>
        //public int DataKubun { get; set; }

        ///// <summary>2:合計件数</summary>
        //public int TotalRecordCount { get; set; }

        ///// <summary>3:合計金額</summary>
        //public decimal TotalAmount { get; set; }

        ///// <summary>4:振替済件数</summary>
        //public int TransferredRecordCount { get; set; }

        ///// <summary>5:振替済金額</summary>
        //public decimal TransferredAmount { get; set; }

        ///// <summary>6:振替不能件数</summary>
        //public int TransferErrorRecordCount { get; set; }

        ///// <summary>7:振替不能金額</summary>
        //public decimal TransferErrorAmount { get; set; }

        public RisonaNetDsv8Trailer(string[] fields, int lineNumber)
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
