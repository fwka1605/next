using Rac.VOne.Common.DataHandling;
using Rac.VOne.AccountTransfer.Import.ConcreteRecord;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import.Reader
{
    using Bundle = ReadResultBundle<SmbcFixed1Header, SmbcFixed2Data, SmbcFixed8Trailer, SmbcFixed9End>;

    public class SmbcFixedFileReader : IReader, IBundle<SmbcFixed1Header, SmbcFixed2Data, SmbcFixed8Trailer, SmbcFixed9End>
    {
        public Encoding Encoding { get; set; } = Encoding.GetEncoding(932);
        public bool IsAsync { get; set; }
        public bool IsPlainText { get; set; }
        public int CompanyId { get; set; }
        public int PaymentAgencyId { get; set; }
        public bool AggregateBillings { get; set; }
        public int TransferYear { get; set; }
        public Helper Helper { get; set; }
        public bool IsNoLineBreak { get; set; }
        public string FileName { get; set; }

        private Record CreateRecord(string line, int lineNumber)
        {
            var dataKubun = line?.FirstOrDefault() ?? default(char);
            switch (dataKubun)
            {
                case '1': return new SmbcFixed1Header   (lineNumber, line);
                case '2': return new SmbcFixed2Data     (lineNumber, line);
                case '8': return new SmbcFixed8Trailer  (lineNumber, line);
                case '9': return new SmbcFixed9End      (lineNumber, line);
                default: throw new FormatException($"dataKubun = {dataKubun}, lineNumber = {lineNumber}, line = {line}");
            }
        }

        private List<Bundle> Read(string file)
        {
            var fixedSize = IsNoLineBreak ? 120 : 0;
            var reader = new FixedFileReader {
                StreamCreator   = IsPlainText ? (IStreamCreator) new PlainTextMemoryStreamCreator() : new TextFileStreamCreator(),
                Encoding        = Encoding,
            };
            var lines = reader.Read(file, fixedSize);
            var records = lines.Select((line, index) => CreateRecord(line, index + 1)).ToArray();
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
        /// <param name="bundle">口座振替結果データ</param>
        private async Task<List<AccountTransferSource>> CollateAsync(Bundle bundle)
        {
            var header = bundle.Header;
            var trailer = bundle.Trailer;
            var dueAt = new DateTime(TransferYear, header.TransferMonth, header.TransferDay);

            var billings = IsAsync ?
                await Helper.GetBillingsAsync(CompanyId, PaymentAgencyId, dueAt) :
                      Helper.GetBillings     (CompanyId, PaymentAgencyId, dueAt);

            var customerids = billings.Select(x => x.CustomerId).Distinct().ToArray();

            var customerDictionary = IsAsync ?
                await Helper.GetCustomersAsync(customerids) :
                      Helper.GetCustomers     (customerids);

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

        public async Task<List<AccountTransferSource>> ReadAsync(string file)
        {
            var bundles = Read(file);
            return await CollateAsync(bundles);
        }

    }
}
