using Rac.VOne.Common.DataHandling;
using Rac.VOne.AccountTransfer.Import.ConcreteRecord;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import.Reader
{
    using Bundle = ReadResultBundle<MizuhoFactorAspDsv1Header, MizuhoFactorAspDsv2Data, MizuhoFactorAspDsv8Trailer, MizuhoFactorAspDsv9End>;

    public class MizuhoFactorAspDsvFileReader : IReader, IBundle<MizuhoFactorAspDsv1Header, MizuhoFactorAspDsv2Data, MizuhoFactorAspDsv8Trailer, MizuhoFactorAspDsv9End>
    {
        public Encoding Encoding { get; set; } = Encoding.GetEncoding(932);
        public bool IsAsync { get; set; }
        public bool IsPlainText { get; set; }
        public int CompanyId { get; set; }
        public int PaymentAgencyId { get; set; }
        public bool AggregateBillings { get; set; }
        public int TransferYear { get; set; }
        public Helper Helper { get; set; }
        public string FileName { get; set; }


        private Record CreateRecord(string[] fields, int lineNumber)
        {
            var dataKubun = fields.FirstOrDefault();

            switch (dataKubun)
            {
                case "1": return new MizuhoFactorAspDsv1Header  (fields, lineNumber);
                case "2": return new MizuhoFactorAspDsv2Data    (fields, lineNumber);
                case "8": return new MizuhoFactorAspDsv8Trailer (fields, lineNumber);
                case "9": return new MizuhoFactorAspDsv9End     (fields, lineNumber);
                default: throw new FormatException($"dataKubun = {dataKubun}, lineNumber = {lineNumber}");
            }
        }

        private List<Bundle> Read(string file)
        {
            var parser = new CsvParser {
                StreamCreator   = IsPlainText ? (IStreamCreator) new PlainTextMemoryStreamCreator() : new TextFileStreamCreator(),
                Encoding        = Encoding,
            };
            var lines = parser.Parse(file).ToArray();
            var records = lines.Select((fields, index) => CreateRecord(fields, index + 1)).ToArray();

            return this.CreateBundles(records, RecordSetType.HDTE, isMultiHeader: true);
        }
        /// <summary>
        /// 口座振替結果データを請求データと照合する。
        /// </summary>
        /// <param name="bundles">口座振替結果データ(マルチヘッダ)</param>
        private async Task<List<AccountTransferSource>> CollateAsync(List<Bundle> bundles)
        {
            var matchingResultList = new List<AccountTransferSource>();

            foreach (var result in bundles)
                matchingResultList.AddRange(await CollateAsync(result));

            return matchingResultList;
        }

        /// <summary>
        /// 口座振替結果データを請求データと照合する。
        /// </summary>
        /// <param name="bundles">口座振替結果データ</param>
        private async Task<List<AccountTransferSource>> CollateAsync(Bundle bundles)
        {
            var header = bundles.Header;
            var trailer = bundles.Trailer;
            var dueAt = new DateTime(TransferYear, header.TransferMonth, header.TransferDay);

            var billings = IsAsync ?
                await Helper.GetBillingsAsync(CompanyId, PaymentAgencyId, dueAt) :
                      Helper.GetBillings     (CompanyId, PaymentAgencyId, dueAt);

            var customerIds = billings.Select(x => x.CustomerId).Distinct().ToArray();

            var customerDictionary = IsAsync ?
                await Helper.GetCustomersAsync(customerIds) :
                      Helper.GetCustomers     (customerIds);

            var targets =
                Helper.ConvertToAccountTransferTargets(billings, customerDictionary, AggregateBillings);

            // 取込ファイルのデータレコードでループ
            var matchingResultList = new List<AccountTransferSource>();
            foreach (var dataRecord in bundles.DataList)
            {
                var target = targets.SingleOrDefault(x =>
                    x.TransferBankCode          == dataRecord.TransferBankCode
                 && x.TransferBranchCode        == dataRecord.TransferBranchCode
                 && x.TransferAccountTypeId     == dataRecord.TransferAccountTypeId
                 && x.TransferAccountNumber     == dataRecord.TransferAccountNumber
                 && x.TransferCustomerCode      == dataRecord.TransferCustomerCode
                 && x.TotalBillingAmount        == dataRecord.BillingAmount
                );

                matchingResultList.Add(
                    Helper.CreateTransferSource(target?.Billings, dataRecord.TransferResultCode, dataRecord.BillingAmount, dataRecord));
            }

            return matchingResultList;
        }

        public async Task<List<AccountTransferSource>> ReadAsync(string file)
        {
            var bundles = Read(file);
            return await CollateAsync(bundles);
        }

    }
}
