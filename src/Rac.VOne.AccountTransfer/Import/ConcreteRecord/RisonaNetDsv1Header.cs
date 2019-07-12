using Rac.VOne.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import.ConcreteRecord
{
    public class RisonaNetDsv1Header : DsvRecord
    {
        /// <summary>1:データ区分</summary>
        public int DataKubun { get; private set; }

        /// <summary>2:委託者コード</summary>
        public string ConsigneeCode { get; private set; }

        /// <summary>3:委託者名</summary>
        public string ConsigneeName { get; private set; }

        /// <summary>4:引落日(MMdd)</summary>
        public string TransferDateMMDD { get; private set; }
        public int TransferMonth => int.Parse(TransferDateMMDD.Substring(0, 2));
        public int TransferDay => int.Parse(TransferDateMMDD.Substring(2, 2));

        /// <summary>5:取引銀行番号</summary>
        public string BankCode { get; private set; }

        /// <summary>6:取引支店番号</summary>
        public string BranchCode { get; private set; }

        /// <summary>7:委託者預金種目</summary>
        public int AccountTypeId { get; private set; }

        /// <summary>8:委託者口座番号</summary>
        public string AccountNumber { get; private set; }

        /// <summary>9:取引銀行名</summary>
        public string BankName { get; private set; }

        /// <summary>10:取引支店名</summary>
        public string BranchName { get; private set; }

        public RisonaNetDsv1Header(string[] fields, int lineNumber)
            : base(fields, lineNumber)
        { }

        protected override void Validate()
        {
            var recordInfo = $"L.{LineNumber}";

            if (Fields.Count < 10)
            {
                throw new FormatException($"{recordInfo} FieldList.Count = {Fields.Count}");
            }
        }

        protected override void ParseFields()
        {
            DataKubun           = int.Parse(Fields[0]);
            ConsigneeCode       =           Fields[1];
            ConsigneeName       =           Fields[2];
            TransferDateMMDD    =           Fields[3];
            BankCode            =           Fields[4].Right(4, '0');
            BranchCode          =           Fields[5].Right(3, '0');
            AccountTypeId       = int.Parse(Fields[6]);
            AccountNumber       =           Fields[7].Right(7, '0');
            BankName            =           Fields[8];
            BranchName          =           Fields[9];
        }
    }
}
