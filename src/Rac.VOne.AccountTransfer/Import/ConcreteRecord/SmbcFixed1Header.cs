using Rac.VOne.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import.ConcreteRecord
{
    public class SmbcFixed1Header : FixedLengthRecord
    {
        public override IEnumerable<int> FieldLengthList { get; } = new[] { 1, 2, 1, 2, 8, 40, 4, 4, 15, 3, 15, 1, 7, 17 };

        /// <summary>データ区分</summary>
        public int DataKubun { get; private set; }

        /// <summary>種別コード</summary>
        public string TypeCode { get; private set; }

        /// <summary>コード区分</summary>
        public int EncodingKubun { get; private set; }

        /// <summary>マルチコード</summary>
        public string MultiCode { get; private set; }

        /// <summary>委託者コード</summary>
        public string ConsigneeCode { get; private set; }

        /// <summary>委託者名</summary>
        public string ConsigneeName { get; private set; }

        /// <summary>引落日(MMdd)</summary>
        public string TransferDateMMDD { get; private set; }
        public int TransferMonth => int.Parse(TransferDateMMDD.Substring(0, 2));
        public int TransferDay => int.Parse(TransferDateMMDD.Substring(2, 2));

        /// <summary>取引銀行番号</summary>
        public string BankCode { get; private set; }

        /// <summary>取引銀行名</summary>
        public string BankName { get; private set; }

        /// <summary>取引支店番号</summary>
        public string BranchCode { get; private set; }

        /// <summary>取引支店名</summary>
        public string BranchName { get; private set; }

        /// <summary>委託者預金種目</summary>
        public int AccountTypeId { get; private set; }

        /// <summary>委託者口座番号</summary>
        public string AccountNumber { get; private set; }

        ///// <summary>予備</summary>
        //public string Dummy { get; private set; }

        public SmbcFixed1Header(int lineNumber, string line)
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
            if (Fields.Count != 14)
            {
                throw new FormatException($"{recordInfo} FieldList.Count = {Fields.Count}");
            }

            if (Fields[1] != "91")
            {
                throw new FormatException($"{recordInfo} 種別コード = {Fields[1]}");
            }
            if (Fields[2] != "1")
            {
                throw new FormatException($"{recordInfo} コード区分 = {Fields[2]}");
            }
        }

        protected override void ParseFields()
        {
            DataKubun               = int.Parse(Fields[ 0]);
            TypeCode                =           Fields[ 1];
            EncodingKubun           = int.Parse(Fields[ 2]);
            MultiCode               =           Fields[ 3];
            ConsigneeCode           =           Fields[ 4];
            ConsigneeName           =           Fields[ 5];
            TransferDateMMDD        =           Fields[ 6];
            BankCode                =           Fields[ 7].Right(4, '0');
            BankName                =           Fields[ 8];
            BranchCode              =           Fields[ 9].Right(3, '0');
            BranchName              =           Fields[10];
            AccountTypeId           = int.Parse(Fields[11]);
            AccountNumber           =           Fields[12].Right(7, '0');
            //Dummy                   =           Fields[13];
        }

    }
}
