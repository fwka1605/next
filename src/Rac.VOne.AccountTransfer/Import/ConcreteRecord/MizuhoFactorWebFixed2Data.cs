using Rac.VOne.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import.ConcreteRecord
{
    public class MizuhoFactorWebFixed2Data : FixedLengthRecord, IAccountTransferInformation
    {
        public override IEnumerable<int> FieldLengthList { get; } = new[] { 1, 4, 15, 3, 15, 2, 2, 1, 7, 30, 10, 1, 20, 1, 8 };

        /// <summary>データ区分</summary>
        public int DataKubun { get; set; }

        /// <summary>引落銀行番号</summary>
        public string TransferBankCode { get; set; }

        /// <summary>引落銀行名</summary>
        public string TransferBankName { get; set; }

        /// <summary>引落支店番号</summary>
        public string TransferBranchCode { get; set; }

        /// <summary>引落支店名</summary>
        public string TransferBranchName { get; set; }

        /// <summary>枝番</summary>
        public string BranchNumber { get; set; }

        ///// <summary>予備</summary>
        //public string Dummy1 { get; set; }

        /// <summary>預金種目</summary>
        public int TransferAccountTypeId { get; set; }

        /// <summary>口座番号</summary>
        public string TransferAccountNumber { get; set; }

        /// <summary>口座名義</summary>
        public string TransferAccountName { get; set; }

        /// <summary>請求金額</summary>
        public decimal BillingAmount { get; set; }

        /// <summary>新規コード</summary>
        public string TransferNewCode { get; set; }

        /// <summary>顧客番号</summary>
        public string TransferCustomerCode { get; set; }

        /// <summary>振替結果コード</summary>
        public int TransferResultCode { get; set; }

        ///// <summary>予備</summary>
        //public string Dummy2 { get; set; }

        public MizuhoFactorWebFixed2Data(int lineNumber, string line)
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
            if (Fields.Count < 14)
            {
                throw new FormatException($"{recordInfo} FieldList.Count = {Fields.Count}");
            }
        }

        protected override void ParseFields()
        {
            DataKubun               = int    .Parse(Fields[ 0]);
            TransferBankCode        =               Fields[ 1].Right(4, '0');
            TransferBankName        =               Fields[ 2];
            TransferBranchCode      =               Fields[ 3].Right(3, '0');
            TransferBranchName      =               Fields[ 4];
            BranchNumber            =               Fields[ 5];
            //Dummy1                  =               Fields[ 6];
            TransferAccountTypeId   = int    .Parse(Fields[ 7]);
            TransferAccountNumber   =               Fields[ 8].Right(7, '0');
            TransferAccountName     =               Fields[ 9];
            BillingAmount           = decimal.Parse(Fields[10]);
            TransferNewCode         =               Fields[11];
            TransferCustomerCode    =               Fields[12];
            TransferResultCode      = int    .Parse(Fields[13]);
            //Dummy2                  =               Fields[14];
        }
    }
}
