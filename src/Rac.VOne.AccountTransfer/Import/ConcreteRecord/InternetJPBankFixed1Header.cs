using Rac.VOne.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import.ConcreteRecord
{
    public class InternetJPBankFixed1Header:FixedLengthRecord
    {
        public override IEnumerable<int> FieldLengthList { get; } = new[] { 1, 2, 1, 10, 40, 4, 4, 15, 3, 16, 7, 4, 1, 12 };

        /// <summary>1:データ区分</summary>
        public int DataKubun { get; private set; }

        /// <summary>2:契約種別コード</summary>
        public string ContractCode { get; private set; }

        ///// <summary>3:予備1</summary>
        //public string Dummy1 { get; private set; }

        /// <summary>4:委託者コード</summary>
        public string ConsigneeCode { get; private set; }

        /// <summary>5:委託者名</summary>
        public string ConsigneeName { get; private set; }

        /// <summary>6:引落日(MMdd)</summary>
        public string TransferDateMMDD { get; private set; }
        public int TransferMonth => int.Parse(TransferDateMMDD.Substring(0, 2));
        public int TransferDay => int.Parse(TransferDateMMDD.Substring(2, 2));

        /// <summary>7:取引銀行番号</summary>
        public string BankCode { get; private set; }

        /// <summary>8:取引銀行名</summary>
        public string BankName { get; private set; }

        /// <summary>9:取引口座記号</summary>
        public string BranchCode { get; private set; }

        ///// <summary>10:予備2</summary>
        //public string Dummy2 { get; private set; }

        /// <summary>11:委託者口座番号</summary>
        public string AccountNumber { get; private set; }

        /// <summary>12:再振替日</summary>
        public string TransferDate2ndMMDD { get; private set; }

        public int Transfer2ndMonth => int.Parse(TransferDate2ndMMDD.Substring(0, 2));
        public int Transfer2ndDay => int.Parse(TransferDate2ndMMDD.Substring(2, 2));

        /// <summary>13:再振替回数</summary>
        public int TransferDate2ndCount { get; private set; }

        ///// <summary>14:予備3</summary>
        //public string Dummy3 { get; private set; }

        public InternetJPBankFixed1Header(int lineNumber, string line)
            : base(lineNumber, line)
        {
        }

        protected override void Validate()
        {
            var recordInfo = $"L.{LineNumber}";

            if (Line.Length != 120)
            {
                throw new FormatException($"{recordInfo} RecordLength = {Line.Length}");
            }
            if (Fields.Count < 13)
            {
                throw new FormatException($"{recordInfo} FieldList.Count = {Fields.Count}");
            }
        }

        protected override void ParseFields()
        {
            DataKubun               = int.Parse(Fields[ 0]);
            ContractCode            =           Fields[ 1];
            //Dummy1                  =           Fields[ 2];
            ConsigneeCode           =           Fields[ 3];
            ConsigneeName           =           Fields[ 4];
            TransferDateMMDD        =           Fields[ 5];
            BankCode                =           Fields[ 6].Right(4, '0');
            BankName                =           Fields[ 7];
            BranchCode              =           Fields[ 8].Right(3, '0');
            //Dummy2                  =           Fields[ 9];
            AccountNumber           =           Fields[10].Right(7, '0');
            TransferDate2ndMMDD     =           Fields[11];
            TransferDate2ndCount    = int.Parse(Fields[12]);
            //Dummy3                  =           Fields[13];
        }

    }
}
