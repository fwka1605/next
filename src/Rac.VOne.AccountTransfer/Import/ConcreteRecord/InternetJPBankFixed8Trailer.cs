using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import.ConcreteRecord
{
    public class InternetJPBankFixed8Trailer : FixedLengthRecord
    {
        public override IEnumerable<int> FieldLengthList { get; } = new[] { 1, 6, 12, 6, 12, 6, 12, 6, 6, 6, 6, 12, 29 };

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

        ///// <summary>8:照会請求件数</summary>
        //public int InquiryAllRecordCount { get; set; }

        ///// <summary>9:照会正常件数</summary>
        //public int  InquirySuccessRecordCount { get; set; }

        ///// <summary>10:照会事故件数</summary>
        //public int InquiryErrorRecordCount { get; set; }

        ///// <summary>11:再振替処理件数</summary>
        //public int Transferred2ndRecordCount { get; set; }

        ///// <summary>12:再振替処理金額</summary>
        //public decimal Transferred2ndAmount { get; set; }

        ///// <summary>13:予備</summary>
        //public string Dummy { get; set; }

        public InternetJPBankFixed8Trailer(int lineNumber, string line)
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
