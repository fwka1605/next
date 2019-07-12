using Rac.VOne.AccountTransfer.Import.ConcreteRecord;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;

namespace Rac.VOne.AccountTransfer.Import.Reader
{
    using Bundle = ReadResultBundle<MitsubishiUfjFactorDsv1Header, MitsubishiUfjFactorDsv2Data, MitsubishiUfjFactorDsv8Trailer, MitsubishiUfjFactorDsv9End>;

    public class MitsubishiUfjFactorDsvFileReader : IReader, IBundle<MitsubishiUfjFactorDsv1Header, MitsubishiUfjFactorDsv2Data, MitsubishiUfjFactorDsv8Trailer, MitsubishiUfjFactorDsv9End>
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
            var dataKubun = fields?.FirstOrDefault();
            switch (dataKubun)
            {
                case "1": return new MitsubishiUfjFactorDsv1Header  (fields, lineNumber);
                case "2": return new MitsubishiUfjFactorDsv2Data    (fields, lineNumber);
                case "8": return new MitsubishiUfjFactorDsv8Trailer (fields, lineNumber);
                case "9": return new MitsubishiUfjFactorDsv9End     (fields, lineNumber);
                default:   throw new FormatException($"dataKubun = {dataKubun}, lineNumber = {lineNumber}");
            }
        }

        /// <summary>口座振替結果データファイルを読み込む。</summary>
        private List<Bundle> Read(string file)
        {
            var parser = new CsvParser {
                StreamCreator   = IsPlainText ? (IStreamCreator) new PlainTextMemoryStreamCreator() : new TextFileStreamCreator(),
                Encoding        = Encoding,
            };
            var lines = parser.Parse(file).ToArray();
            var records = lines.Select((x, index) => CreateRecord(x, index + 1)).ToArray();
            return this.CreateBundles(records, RecordSetType.HDTE, isMultiHeader: true);
        }


        public async Task<List<AccountTransferSource>> ReadAsync(string file)
        {
            var bundles = Read(file);

            return await CollateAsync(bundles);
        }

        /// <summary>
        /// 口座振替結果データを請求データと照合する。
        /// </summary>
        /// <param name="bundles">口座振替結果データ(マルチヘッダ)</param>
        public async Task<List<AccountTransferSource>> CollateAsync(List<Bundle> bundles)
        {
            var matchingResultList = new List<AccountTransferSource>();

            foreach (var result in bundles)
                matchingResultList.AddRange(await CollateAsync(result));

            return matchingResultList;
        }


        /// <summary>口座振替結果データを請求データと照合する。</summary>
        /// <param name="bundle">口座振替結果データ</param>
        /// <param name="PaymentAgencyId">照合対象請求データの検索パラメータ</param>
        /// <param name="AggregateBillings">同一得意先で纏めた請求データと照合するか否か</param>
        /// <param name="TransferYear">引落年</param>
        public async Task<List<AccountTransferSource>> CollateAsync(Bundle bundle)
        {
            var header = bundle.Header;
            var trailer = bundle.Trailer;

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
            foreach (var dataRecord in bundle.DataList)
            {
                var target = targets.SingleOrDefault(x =>
                    x.TransferBankCode      == dataRecord.TransferBankCode
                 && x.TransferBranchCode    == dataRecord.TransferBranchCode
                 && x.TransferAccountTypeId == dataRecord.TransferAccountTypeId
                 && x.TransferAccountNumber == dataRecord.TransferAccountNumber
                 && x.TransferCustomerCode  == dataRecord.TransferCustomerCode
                 && x.TotalBillingAmount    == dataRecord.BillingAmount
                );

                matchingResultList.Add(
                    Helper.CreateTransferSource(target?.Billings, dataRecord.TransferResultCode, dataRecord.BillingAmount, dataRecord));
            }

            return matchingResultList;
        }
    }
}
