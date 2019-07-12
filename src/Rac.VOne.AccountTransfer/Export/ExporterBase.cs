using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.AccountTransfer.Export
{
    /// <summary>
    /// 口座振替依頼データ作成 base class
    /// </summary>
    public abstract class ExporterBase : IAccountTransferExporter
    {
        /// <summary>データ区分 1 : ヘッダーレコード</summary>
        protected const int DivisionHeader = 1;
        /// <summary>データ区分 2 : データレコード</summary>
        protected const int DivisionData = 2;
        /// <summary>データ区分 8 : トレーラーレコード</summary>
        protected const int DivisionTrailer = 8;
        /// <summary>データ区分 9 : エンドレコード</summary>
        protected const int DivisionEnd = 9;

        public Action<Exception> LogError { get; set; }
        public PaymentAgency PaymentAgency { get; set; }
        public Company Company { get; set; }
        public int TotalCount { get; set; }
        public decimal TotalAmount { get; set; }
        public Encoding Encoding { get; set; } = Encoding.GetEncoding(932);
        public DateTime DueAt { get; set; }
        public virtual void Export(string path, IEnumerable<AccountTransferDetail> source)
        {
            var builder = new StringBuilder();
            builder.Append(GetHeaderRecord());
            foreach (var detail in source)
            {
                TotalCount++;
                TotalAmount += detail.BillingAmount;
                builder.Append(GetDataRecord(detail));
            }
            builder.Append(GetTrailerRecord());
            builder.Append(GetEndRecord());
            try
            {
                System.IO.File.WriteAllText(path, builder.ToString(), Encoding);
            }
            catch (Exception ex)
            {
                LogError?.Invoke(ex);
            }
        }
        public virtual string GetHeaderRecord() => string.Empty;
        public virtual string GetDataRecord(AccountTransferDetail detail) => string.Empty;
        public virtual string GetTrailerRecord() => string.Empty;
        public virtual string GetEndRecord() => string.Empty;

        protected string Join(params object[] args)
            => string.Join(",", args) + Environment.NewLine;

        protected string Join(params FixedField[] args)
            => string.Concat(args.Select(x => x.ToString())) + Environment.NewLine;
        protected FixedField GetStrField(int length, object value, char padding = ' ', bool rightPadding = true)
            => new FixedField(length, value, padding, rightPadding);
        protected FixedField GetNmbField(int length, object value)
            => new FixedField(length, value, '0', false);
        /// <summary>
        /// 空文字 or 指定した文字数分 半角スペース
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        protected string Dummy(int length = 0)
            => length == 0 ? string.Empty : new string(' ', length);
    }
}
