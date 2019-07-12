using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import.ConcreteRecord
{
    public class MitsubishiUfjNicosDsvTrailer : DsvRecord
    {
        ///// <summary>空欄</summary>
        //public string Empty1 { get; set; }

        ///// <summary>請求件数</summary>
        //public int TotalRecordCount { get; set; }

        ///// <summary>請求金額</summary>
        //public decimal TotalAmount { get; set; }

        ///// <summary>入金件数</summary>
        //public int TransferredRecordCount { get; set; }

        ///// <summary>入金金額</summary>
        //public decimal TransferredAmount { get; set; }

        ///// <summary>未入金件数</summary>
        //public int TransferErrorRecordCount { get; set; }

        ///// <summary>未入金金額</summary>
        //public decimal TransferErrorAmount { get; set; }

        ///// <summary>空欄</summary>
        //public string Empty2 { get; set; }

        ///// <summary>空欄</summary>
        //public string Empty3 { get; set; }

        ///// <summary>空欄</summary>
        //public string Empty4 { get; set; }

        ///// <summary>空欄</summary>
        //public string Empty5 { get; set; }

        ///// <summary>空欄</summary>
        //public string Empty6 { get; set; }

        ///// <summary>空欄</summary>
        //public string Empty7 { get; set; }

        public MitsubishiUfjNicosDsvTrailer(string[] fields, int lineNumber)
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
