using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import.ConcreteRecord
{
    public class MizuhoFactorAspDsv8Trailer : DsvRecord
    {
        ///// <summary>データ区分</summary>
        //public int DataKubun { get; set; }

        ///// <summary>合計件数</summary>
        //public int TotalRecordCount { get; set; }

        ///// <summary>合計金額</summary>
        //public decimal TotalAmount { get; set; }

        ///// <summary>振替済件数</summary>
        //public int TransferredRecordCount { get; set; }

        ///// <summary>振替済金額</summary>
        //public decimal TransferredAmount { get; set; }

        ///// <summary>振替不能件数</summary>
        //public int TransferErrorRecordCount { get; set; }

        ///// <summary>振替不能金額</summary>
        //public decimal TransferErrorAmount { get; set; }

        ///// <summary>予備</summary>
        //public string Dummy { get; set; }

        public MizuhoFactorAspDsv8Trailer(string[] fields, int lineNumber)
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
