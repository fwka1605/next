using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.AccountTransfer.Import.ConcreteRecord;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;


namespace Rac.VOne.AccountTransfer.Import.Reader
{
    using Bundle = ReadResultBundle<InternetJPBankFixed1Header, InternetJPBankFixed2Data, InternetJPBankFixed8Trailer, InternetJPBankFixed9End>;

    public class InternetJPBankFixedFileReader : IReader, IBundle<InternetJPBankFixed1Header, InternetJPBankFixed2Data, InternetJPBankFixed8Trailer, InternetJPBankFixed9End>
    {
        public Encoding Encoding { get; set; } = Encoding.GetEncoding(932);

        public FixedFileReader FixedFileReader { get; set; }

        public bool IsAsync { get; set; }
        public bool IsPlainText { get; set; }
        public int CompanyId { get; set; }
        public int PaymentAgencyId { get; set; }
        public bool AggregateBillings { get; set; }
        public int TransferYear { get; set; }
        public Helper Helper { get; set; }
        public bool IsNoLineBreak { get; set; }
        public string FileName { get; set; }

        private Record RecordSelector(string line, int lineNumber)
        {
            var dataKubun = line?.FirstOrDefault() ?? default(char);
            switch (dataKubun)
            {
                case '1': return new InternetJPBankFixed1Header (lineNumber, line);
                case '2': return new InternetJPBankFixed2Data   (lineNumber, line);
                case '8': return new InternetJPBankFixed8Trailer(lineNumber, line);
                case '9': return new InternetJPBankFixed9End    (lineNumber, line);
                default: throw new FormatException($"dataKubun = {dataKubun}, lineNumber = {lineNumber}, line = {line}");
            }
        }
        private List<Bundle> Read(string file)
        {
            var fixedSize = IsNoLineBreak ? 120 : 0;
            var reader = new FixedFileReader {
                StreamCreator       = IsPlainText ? (IStreamCreator)new PlainTextMemoryStreamCreator() : new TextFileStreamCreator(),
                Encoding            = Encoding,
            };
            var lines = reader.Read(file,  fixedSize);

            var records = lines.Select((line, index) => RecordSelector(line, index + 1)).ToList();
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

        /// <summary>
        /// 口座振替結果データを請求データと照合する。
        /// </summary>
        /// <param name="bundle">口座振替結果データ</param>
        public async Task<List<AccountTransferSource>> CollateAsync(Bundle bundle)
        {
            var header = bundle.Header;
            var trailer = bundle.Trailer;

            var targetMonth = header.TransferDate2ndCount == 1
                ? header.TransferMonth
                : header.Transfer2ndMonth;
            var targetDay = header.TransferDate2ndCount == 1
                ? header.TransferDay
                : header.Transfer2ndDay;
            var dueAt = new DateTime(TransferYear, targetMonth, targetDay);

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
                 && x.TransferAccountNumber == dataRecord.TransferAccountNumber
                 && x.TransferCustomerCode  == dataRecord.TransferCustomerCode
                 && x.TotalBillingAmount    == dataRecord.BillingAmount
                );

                var source = Helper.CreateTransferSource(target?.Billings, dataRecord.TransferResultCode, dataRecord.BillingAmount, dataRecord);
                if (header.TransferDate2ndCount == 1)
                {
                    source.IgnoreInitialization = true;
                    source.NewDueAt = new DateTime(TransferYear, header.Transfer2ndMonth, header.Transfer2ndDay);
                }
                matchingResultList.Add(source);
            }

            return matchingResultList;
        }

    }
}
