using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import.ConcreteRecord
{
    public class MizuhoFactorAspDsv1Header : DsvRecord
    {
        /// <summary>データ区分</summary>
        public int DataKubun { get; private set; }

        /// <summary>種別コード</summary>
        public string TypeCode { get; private set; }

        /// <summary>委託者コード</summary>
        public string ConsigneeCode { get; private set; }

        ///// <summary>予備</summary>
        //public string Dummy1 { get; private set; }

        /// <summary>委託者名</summary>
        public string ConsigneeName { get; private set; }

        /// <summary>振替月日</summary>
        public string TransferDateMMDD { get; private set; }

        public int TransferMonth => int.Parse(TransferDateMMDD.Substring(0, 2));
        public int TransferDay => int.Parse(TransferDateMMDD.Substring(2, 2));

        ///// <summary>予備</summary>
        //public string Dummy2 { get; private set; }

        public MizuhoFactorAspDsv1Header(string[] fields, int lineNumber)
            : base(fields, lineNumber)
        {
        }

        protected override void Validate()
        {
            var recordInfo = $"L.{LineNumber}";

            if (Fields.Count < 6)
            {
                throw new FormatException($"FieldList.Count = {Fields.Count}");
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
            //Dummy1              =           Fields[3];
            ConsigneeName       =           Fields[4];
            TransferDateMMDD    =           Fields[5];
            //Dummy2              =           Fields[6];
        }

    }
}
