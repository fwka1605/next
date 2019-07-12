using Rac.VOne.Common.Extensions;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.EbData
{
    /// <summary>
    /// 地方銀行系 入出金明細
    /// 福岡銀行/横浜銀行等
    /// </summary>
    public class ParserRegionalBankNyusyukkin : IParser
    {
        public Helper Helper { get; set; }
        public FileInformation FileInformation { get; set; }

        private const string SkipWord = "照会口座";

        private const string DateFormat = "yyyy年MM月dd日";

        public string BankCode { get; set; }

        public string[] ImportableTorihikiKubun { get; set; } = { "振込入金", "振込" };

        public async Task<Tuple<ImportFileLog, ImportResult>> ParseAsync(IEnumerable<string[]> fields, CancellationToken token = default(CancellationToken))
        {
            var torihikiKubuns = FileInformation.GetImportableValues();
            if (torihikiKubuns.Any()) ImportableTorihikiKubun = torihikiKubuns;
            var fileLog = new ImportFileLog {
                Id          = FileInformation.Index, /* 自動採番で置き換え */
                CompanyId   = Helper.CompanyId,
                FileName    = FileInformation.Path,
                FileSize    = FileInformation.Size,
                CreateBy    = Helper.LoginUserId,
            };
            var parseResult = ImportResult.Success;
            ReceiptHeader header = null;
            var receiptCategoryId = 0;
            DateTime workDay = DateTime.Today;
            BankAccount bankBuf = null;

            foreach (var field in fields)
            {
                if (field.Length < 13)
                {
                    parseResult = ImportResult.FileFormatError;
                    break;
                }

                var syoukaikouza        = field[ 0];
                var kanjyoubi           = field[ 2];
                var kisanbi             = field[ 3];
                var kingaku             = field[ 5];
                var torihikikubun       = field[ 8];
                var sourceBankName      = field[10];
                var sourceBranchName    = field[11];
                var payerName           = field[12];

                if (syoukaikouza == SkipWord) continue;

                var bankCode = BankCode;
                var branchCode = GetBranchCode(syoukaikouza).Right(3, '0', true);
                var accountTypeId = Helper.GetAccountTypeIdByName(syoukaikouza);

                if (!Constants.ImportableAccountTypeIds.Contains(accountTypeId))
                {
                    parseResult = ImportResult.FileFormatError;
                    break;
                }

                var accountNumber = GetAccountNumber(syoukaikouza);

                var account = Helper.IsAsync ?
                    await Helper.GetBankAccountAsync(bankCode, branchCode, accountTypeId, accountNumber, token) :
                          Helper.GetBankAccount     (bankCode, branchCode, accountTypeId, accountNumber);

                if (account == null || !account.ReceiptCategoryId.HasValue)
                {
                    parseResult = ImportResult.BankAccountMasterError;
                    FileInformation.BankInformation
                        = $"銀行コード：{bankCode}, 支店コード：{branchCode}, 預金種別：{Helper.GetAccountTypeNameById(accountTypeId)}, 口座番号：{accountNumber}";
                    break;
                }

                if (account.ImportSkipping == 1)
                {
                    // parameter
                    continue;
                }

                fileLog.ReadCount++;

                if (!ImportableTorihikiKubun.Contains(torihikikubun)) continue;

                var bankName = account.BankName.Left(30);
                var branchName = account.BranchName.Left(30);

                receiptCategoryId = account.ReceiptCategoryId.Value;

                if (!(bankBuf?.Id == account.Id))
                {
                    header = new ReceiptHeader();
                    fileLog.ReceiptHeaders.Add(header);
                    header.Id = fileLog.ReceiptHeaders.Count; /* 自動採番で置き換え */
                    header.CompanyId = Helper.CompanyId;
                    header.CurrencyId = Helper.DefaultCurrency.Id;
                    header.ImportFileLogId = fileLog.Id; /* 採番後に置き換え */
                    header.Workday = workDay;
                    header.BankCode = bankCode;
                    header.BankName = bankName;
                    header.BranchCode = branchCode;
                    header.BranchName = branchName;
                    header.AccountTypeId = accountTypeId;
                    header.AccountNumber = accountNumber;
                    header.AccountName = string.Empty;
                    header.CreateBy = Helper.LoginUserId;
                    header.UpdateBy = Helper.LoginUserId;

                    bankBuf = account;
                }

                kisanbi = kisanbi.Replace("（", "").Replace("）", "");
                var symd = FileInformation.UseValueDate ? kisanbi : kanjyoubi;
                if (string.IsNullOrEmpty(symd)) symd = kanjyoubi;
                DateTime recordedAt;
                if (!Helper.TryParseDateTime(symd, out recordedAt, DateFormat))
                {
                    parseResult = ImportResult.FileFormatError;
                    break;
                }

                decimal receiptAmount;
                if (!decimal.TryParse(kingaku, out receiptAmount)
                    || receiptAmount == 0M
                    || Constants.MaxAmount < Math.Abs(receiptAmount))
                {
                    parseResult = ImportResult.FileFormatError;
                    break;
                }
                var payerCode = string.Empty;

                payerName = Helper.ConvertToValidEbCharacter(payerName).Left(140);
                sourceBankName = Helper.ConvertToValidEbCharacter(sourceBankName).Left(140);
                sourceBranchName = Helper.ConvertToValidEbCharacter(sourceBranchName).Left(15);

                if (!Helper.IsValidEBChars(payerName))
                {
                    parseResult = ImportResult.FileFormatError;
                    break;
                }

                int? customerId = null; // Helper.GetCustomerIdByExclusiveInfo(header.BankCode, header.BranchCode, header.AccountType, payerCode);

                int? sectionId = account.SectionId; // Helper.GetSectionIdByPayerCode(payerCode);

                int? excludeCategoryId = Helper.UseApportion ? null : Helper.IsAsync ?
                    await Helper.GetExcludeCategoryIdAsync(payerName, token) :
                          Helper.GetExcludeCategoryId     (payerName);

                var receipt = new Receipt();
                header.Receipts.Add(receipt);
                receipt.Id = header.Receipts.Count(); /* 自動採番 */
                receipt.CompanyId = Helper.CompanyId;
                receipt.CurrencyId = Helper.DefaultCurrency.Id;
                receipt.ReceiptHeaderId = header.Id; /* 自動採番後に置き換え */
                receipt.ReceiptCategoryId = receiptCategoryId;
                receipt.CustomerId = customerId;
                receipt.SectionId = sectionId;
                receipt.InputType = Constants.InputTypeEbImporter;
                receipt.Apportioned = Helper.UseApportion ? 0 : 1;
                receipt.Approved = 1;
                receipt.Workday = workDay;
                receipt.RecordedAt = recordedAt;
                receipt.ReceiptAmount = receiptAmount;
                receipt.RemainAmount = receiptAmount;
                receipt.PayerCode = payerCode;
                receipt.PayerName = Helper.RemoveLegalPersonality(payerName);
                receipt.PayerNameRaw = payerName;
                receipt.SourceBankName = sourceBankName;
                receipt.SourceBranchName = sourceBranchName;

                receipt.ExcludeFlag = excludeCategoryId.HasValue ? 1 : 0;
                receipt.ExcludeCategoryId = excludeCategoryId;
                if (excludeCategoryId.HasValue)
                {
                    receipt.ExcludeAmount = receiptAmount;
                    receipt.RemainAmount = 0M;
                    receipt.AssignmentFlag = 2;

                    var receiptExclude = new ReceiptExclude();
                    receiptExclude.Id = receipt.Id;
                    receiptExclude.ReceiptId = receipt.Id;
                    receiptExclude.ExcludeCategoryId = excludeCategoryId.Value;
                    receiptExclude.ExcludeAmount = receiptAmount;
                    receiptExclude.CreateBy = Helper.LoginUserId;
                    receiptExclude.UpdateBy = Helper.LoginUserId;
                    header.ReceiptExcludes.Add(receiptExclude);
                }
                receipt.CreateBy = Helper.LoginUserId;
                receipt.UpdateBy = Helper.LoginUserId;

                header.ImportCount++;
                header.ImportAmount += receiptAmount;
                fileLog.ImportCount++;
                fileLog.ImportAmount += receiptAmount;

            }

            if (parseResult == ImportResult.Success && fileLog?.ImportCount == 0)
            {
                parseResult = ImportResult.ImportDataNotFound;
            }

            return Tuple.Create(fileLog, parseResult);
        }

        private string GetBranchCode(string value)
        {
            if (string.IsNullOrEmpty(value)
                || value.IndexOf("（") < 0
                || value.IndexOf("）") < 0)
                return string.Empty;
            var index = value.IndexOf("（") + 1;
            var length = value.IndexOf("）") - index;
            return Helper.ConvertToValidEbCharacter(value.Substring(index, length));
        }
        private string GetAccountNumber(string value)
        {
            var fields = value.Split(' ');
            if (fields.Length < 3) return string.Empty;
            return Helper.ConvertToValidEbCharacter(fields[2].Right(7, '0', true));
        }
    }
}
