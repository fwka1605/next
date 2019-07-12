using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import.ConcreteRecord
{
    public class MizuhoFactorWebFixed1Header : FixedLengthRecord
    {
        public override IEnumerable<int> FieldLengthList { get; } = new[] { 1, 3, 10, 40, 4, 62 };

        /// <summary>データ区分</summary>
        public int DataKubun { get; private set; }

        /// <summary>種別コード</summary>
        public string TypeCode { get; private set; }

        /// <summary>委託者コード</summary>
        public string ConsigneeCode { get; private set; }

        /// <summary>委託者名</summary>
        public string ConsigneeName { get; private set; }

        /// <summary>振替月日</summary>
        public string TransferDateMMDD { get; private set; }

        public int TransferMonth => int.Parse(TransferDateMMDD.Substring(0, 2));
        public int TransferDay => int.Parse(TransferDateMMDD.Substring(2, 2));

        ///// <summary>予備</summary>
        //public string Dummy { get; private set; }

        public MizuhoFactorWebFixed1Header(int lineNumber, string line)
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
            if (Fields.Count < 5)
            {
                throw new FormatException($"{recordInfo} FieldList.Count = {Fields.Count}");
            }

            //if (Fields[1] != "911")
            //{
            //    throw new FormatException($"{recordInfo} 種別コード = {Fields[1]}");
            //}
        }

        protected override void ParseFields()
        {
            DataKubun           = int.Parse(Fields[0]);
            TypeCode            =           Fields[1];
            ConsigneeCode       =           Fields[2];
            ConsigneeName       =           Fields[3];
            TransferDateMMDD    =           Fields[4];
            //Dummy               =           Fields[5];
        }
    }
}
