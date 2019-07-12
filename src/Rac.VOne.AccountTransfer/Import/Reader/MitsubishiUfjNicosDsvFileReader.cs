using Rac.VOne.AccountTransfer.Import.ConcreteRecord;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;

namespace Rac.VOne.AccountTransfer.Import.Reader
{
    using Bundle = ReadResultBundle<DsvHeader, MitsubishiUfjNicosDsvData, MitsubishiUfjNicosDsvTrailer, DummyRecord>;

    public class MitsubishiUfjNicosDsvFileReader : IReader, IBundle<DsvHeader, MitsubishiUfjNicosDsvData, MitsubishiUfjNicosDsvTrailer, DummyRecord>
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

        private Record CreateRecord(string[] fields, int numberOfLines, int lineNumber)
        {
            // ヘッダ部
            if (lineNumber <= 1)
                return new DsvHeader(fields, lineNumber);

            // 最終行＝トレーラ部
            if (lineNumber == numberOfLines)
                return new MitsubishiUfjNicosDsvTrailer(fields, lineNumber);

            // データ部
            return new MitsubishiUfjNicosDsvData(fields, lineNumber);
        }

        private List<Bundle> Read(string file)
        {
            var parser = new CsvParser {
                StreamCreator   = IsPlainText ? (IStreamCreator) new PlainTextMemoryStreamCreator() : new TextFileStreamCreator(),
                Encoding        = Encoding,
            };
            var lines = parser.Parse(file).ToArray();
            var records = lines.Select((fields, index) => CreateRecord(fields, lines.Length, index + 1)).ToArray();
            return this.CreateBundles(records, RecordSetType.HDT, isMultiHeader: false);
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
        /// <param name="bundle">口座振替結果データ</param>
        private async Task<List<AccountTransferSource>> CollateAsync(Bundle bundle)
        {
            var header = bundle.Header;
            var trailer = bundle.Trailer;
            var transferDate = GetTransferDate(FileName);

            var billings = IsAsync ?
                await Helper.GetBillingsAsync(CompanyId, PaymentAgencyId, transferDate) :
                      Helper.GetBillings     (CompanyId, PaymentAgencyId, transferDate);

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

        /// <summary>
        /// 取り込みファイル名から引落日を取り出すための簡易パターン。
        /// 厳密な日付チェックは別段行う。
        /// </summary>
        private Regex FileNamePattern => new Regex(@"^(?<date>\d{8}).\.dat$", RegexOptions.IgnoreCase);

        private DateTime GetTransferDate(string fileName)
        {
            var match = FileNamePattern.Match(fileName);
            if (!match.Success)
            {
                throw new FormatException($"filename = {fileName}");
            }

            // 日付無効時はFormatExceptionがスローされる
            return DateTime.ParseExact(match.Groups["date"].Value, "yyyyMMdd", null);
        }


        public async Task<List<AccountTransferSource>> ReadAsync(string file)
        {
            var bundles = Read(file);

            return await CollateAsync(bundles);
        }

    }
}
