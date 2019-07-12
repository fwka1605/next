using Rac.VOne.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import.ConcreteRecord
{
    public class InternetJPBankFixed2Data : FixedLengthRecord, IAccountTransferInformation
    {
        public override IEnumerable<int> FieldLengthList { get; } = new[] { 1, 4, 15, 3, 20, 7, 30, 10, 1, 20, 1, 4, 2, 2 };

        /// <summary>1:データ区分</summary>
        public int DataKubun { get; set; }

        /// <summary>2:引落銀行番号</summary>
        public string TransferBankCode { get; set; }

        /// <summary>3:引落銀行名</summary>
        public string TransferBankName { get; set; }

        /// <summary>4:引落貯金記号</summary>
        public string TransferBranchCode { get; set; }

        ///// <summary>5:予備</summary>
        //public string Dummy { get; set; }

        /// <summary>6:引落貯金番号</summary>
        public string TransferAccountNumber { get; set; }

        /// <summary>7:預金者名</summary>
        public string TransferAccountName { get; set; }

        /// <summary>8:引落金額</summary>
        /// <remarks>振替不能時も振替依頼金額(請求金額)がセットされるのでプロパティ名はBillingAmountとしている</remarks>
        public decimal BillingAmount { get; set; }

        /// <summary>9:照会表示</summary>
        public string InquiryCode { get; set; }

        /// <summary>10:顧客番号</summary>
        public string TransferCustomerCode { get; set; }

        /// <summary>11:振替結果コード</summary>
        public int TransferResultCode { get; set; }

        ///// <summary>12:優先処理 - 年月</summary>
        //public string PriorityDateYYMM { get; set; }

        ///// <summary>13:優先処理 - コード</summary>
        //public string PriorityCode { get; set; }

        ///// <summary>14:補助文言表示</summary>
        //public string SupportWords { get; set; }

        public string TransferBranchName { get; set; }

        public InternetJPBankFixed2Data(int lineNumber, string line)
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
            if (Fields.Count < 11)
            {
                throw new FormatException($"{recordInfo} FieldList.Count = {Fields.Count}");
            }
        }

        protected override void ParseFields()
        {
            DataKubun               = int.Parse(    Fields[ 0]);
            TransferBankCode        =               Fields[ 1].Right(4, '0');
            TransferBankName        =               Fields[ 2];
            TransferBranchCode      =               Fields[ 3].Right(3, '0');
            //Dummy                   =               Fields[ 4];
            TransferAccountNumber   =               Fields[ 5].Right(7, '0');
            TransferAccountName     =               Fields[ 6];
            BillingAmount           = decimal.Parse(Fields[ 7]);
            InquiryCode             =               Fields[ 8];
            TransferCustomerCode    =               Fields[ 9];
            TransferResultCode      = int.Parse(    Fields[10]);
            //PriorityDateYYMM        =               Fields[11];
            //PriorityCode            =               Fields[12];
            //SupportWords            =               Fields[13];

            TransferBranchName = "口座記号:" + TransferBranchCode;
        }

    }
}
