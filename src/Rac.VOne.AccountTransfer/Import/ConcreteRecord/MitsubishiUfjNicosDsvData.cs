using Rac.VOne.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import.ConcreteRecord
{
    public class MitsubishiUfjNicosDsvData : DsvRecord, IAccountTransferInformation
    {
        /// <summary>不能理由サイン</summary>
        public int TransferResultCode { get; set; }

        /// <summary>顧客番号</summary>
        public string TransferCustomerCode { get; set; }

        /// <summary>引落依頼金額</summary>
        /// <remarks>振替不能時も振替依頼金額(請求金額)がセットされるのでプロパティ名はBillingAmountとしている</remarks>
        public decimal BillingAmount { get; set; }

        /// <summary>銀行コード</summary>
        public string TransferBankCode { get; set; }

        /// <summary>支店コード</summary>
        public string TransferBranchCode { get; set; }

        /// <summary>預金種別</summary>
        public int TransferAccountTypeId { get; set; }

        /// <summary>口座番号</summary>
        public string TransferAccountNumber { get; set; }

        /// <summary>口座名義人</summary>
        public string TransferAccountName { get; set; }

        /// <summary>新規サイン</summary>
        public string TransferNewCode { get; set; }

        //// 以下、変数名を思いつかなかったのと処理でも使っていないのでNoNameとしている

        ///// <summary>制度／商品コード</summary>
        //public string NoName1 { get; set; }

        ///// <summary>口座合算有無</summary>
        //public string NoName2 { get; set; }

        ///// <summary>会員番号</summary>
        //public string NoName3 { get; set; }

        ///// <summary>送付サイン</summary>
        //public string NoName4 { get; set; }
        public string TransferBankName { get; set; }
        public string TransferBranchName { get; set; }

        public MitsubishiUfjNicosDsvData(string[] fields, int lineNumber)
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
            TransferResultCode      = int    .Parse(Fields[ 0]);
            TransferCustomerCode    =               Fields[ 1];
            BillingAmount           = decimal.Parse(Fields[ 2]);
            TransferBankCode        =               Fields[ 3].Right(4, '0');
            TransferBranchCode      =               Fields[ 4].Right(3, '0');
            TransferAccountTypeId   = int    .Parse(Fields[ 5]);
            TransferAccountNumber   =               Fields[ 6].Right(7, '0');
            TransferAccountName     =               Fields[ 7];
            TransferNewCode         =               Fields[ 8];
            //NoName1                 =               Fields[ 9];
            //NoName2                 =               Fields[10];
            //NoName3                 =               Fields[11];
            //NoName4                 =               Fields[12];
        }

    }
}
