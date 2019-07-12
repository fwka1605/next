using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import.ConcreteRecord
{
    public class RicohLeaseCollectDsvData : DsvRecord, IAccountTransferInformation
    {
        /// <summary>顧客番号</summary>
        public string TransferCustomerCode { get; set; }

        /// <summary>顧客名カナ</summary>
        public string TransferCustomerName { get; set; }

        /// <summary>引落年月日</summary>
        public DateTime TransferDate { get; set; }

        /// <summary>銀行コード</summary>
        public string TransferBankBranchCode { get; set; }
        /// <summary>銀行コード</summary>
        public string TransferBankCode { get { return TransferBankBranchCode.Substring(0, 4); } }
        /// <summary>支店コード</summary>
        public string TransferBranchCode { get { return TransferBankBranchCode.Substring(5, 3); } }

        /// <summary>預金種別コード</summary>
        public int TransferAccountTypeId { get; set; }

        /// <summary>口座番号</summary>
        public string TransferAccountNumber { get; set; }

        /// <summary>預金者名カナ</summary>
        public string TransferAccountName { get; set; }

        /// <summary>引落依頼金額</summary>
        /// <remarks>振替不能時も振替依頼金額(請求金額)がセットされるのでプロパティ名はBillingAmountとしている</remarks>
        public decimal BillingAmount { get; set; }

        /// <summary>振替結果コード</summary>
        public int TransferResultCode { get; set; }
        public string TransferBankName { get; set; }
        public string TransferBranchName { get; set; }

        public RicohLeaseCollectDsvData(string[] fields, int lineNumber)
            : base(fields, lineNumber)
        {
        }

        protected override void Validate()
        {
            var recordInfo = $"L.{LineNumber}";

            if (Fields.Count < 9)
            {
                throw new FormatException($"{recordInfo} FieldList.Count = {Fields.Count}");
            }
        }

        protected override void ParseFields()
        {
            TransferCustomerCode    =               Fields[0];
            TransferCustomerName    =               Fields[1];
            TransferDate            = DateTime.ParseExact(  Fields[2], "yyyyMMdd", null);
            TransferBankBranchCode  =               Fields[3];
            TransferAccountTypeId   = int    .Parse(Fields[4]);
            TransferAccountNumber   =               Fields[5];
            //TransferAccountName     =               Fields[6];
            BillingAmount           = decimal.Parse(Fields[7]);
            TransferResultCode      = int    .Parse(Fields[8]);
        }

    }
}
