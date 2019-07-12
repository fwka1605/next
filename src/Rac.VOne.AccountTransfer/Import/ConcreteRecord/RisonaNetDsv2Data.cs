using Rac.VOne.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import.ConcreteRecord
{
    public class RisonaNetDsv2Data : DsvRecord, IAccountTransferInformation
    {
        /// <summary>1:データ区分</summary>
        public int DataKubun { get; set; }

        /// <summary>2:引落銀行番号</summary>
        public string TransferBankCode { get; set; }

        /// <summary>3:引落支店番号</summary>
        public string TransferBranchCode { get; set; }

        /// <summary>4:預金種目</summary>
        public int TransferAccountTypeId { get; set; }

        /// <summary>5:口座番号</summary>
        public string TransferAccountNumber { get; set; }

        /// <summary>6:引落銀行名</summary>
        public string TransferBankName { get; set; }

        /// <summary>7:引落支店名</summary>
        public string TransferBranchName { get; set; }

        /// <summary>8:預金者名</summary>
        public string TransferAccountName { get; set; }

        /// <summary>9:引落金額</summary>
        /// <remarks>振替不能時も振替依頼金額(請求金額)がセットされるのでプロパティ名はBillingAmountとしている</remarks>
        public decimal BillingAmount { get; set; }

        /// <summary>10:新規コード</summary>
        public string TransferNewCode { get; set; }

        /// <summary>11:顧客番号</summary>
        public string TransferCustomerCode { get; set; }

        /// <summary>12:振替結果コード</summary>
        public int TransferResultCode { get; set; }

        public RisonaNetDsv2Data(string[] fields, int lineNumber)
            : base(fields, lineNumber)
        {
        }

        protected override void Validate()
        {
            var recordInfo = $"L.{LineNumber}";

            if (Fields.Count < 12)
            {
                throw new FormatException($"{recordInfo} FieldList.Count = {Fields.Count}");
            }
        }

        protected override void ParseFields()
        {
            DataKubun               = int    .Parse(Fields[ 0]);
            TransferBankCode        =               Fields[ 1].Right(4, '0');
            TransferBranchCode      =               Fields[ 2].Right(3, '0');
            TransferAccountTypeId   = int    .Parse(Fields[ 3]);
            TransferAccountNumber   =               Fields[ 4].Right(7, '0');
            TransferBankName        =               Fields[ 5];
            TransferBranchName      =               Fields[ 6];
            TransferAccountName     =               Fields[ 7];
            BillingAmount           = decimal.Parse(Fields[ 8]);
            TransferNewCode         =               Fields[ 9];
            TransferCustomerCode    =               Fields[10];
            TransferResultCode      = int    .Parse(Fields[11]);
        }

    }
}
