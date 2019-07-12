using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Common.Extensions;
using Rac.VOne.Web.Models;

namespace Rac.VOne.EbData
{
    public class ParserAnser : IParser
    {
        public Helper Helper { get; set; }
        public FileInformation FileInformation { get; set; }

        public string[] ImportableRecordKubun { get; set; } = { "明細" };
        public string[] ImportableTorihikiName { get; set; } = { "振込", "振込入金", "入金" };
        public async Task<Tuple<ImportFileLog, ImportResult>> ParseAsync(IEnumerable<string[]> fields, CancellationToken token = default(CancellationToken))
        {
            var settingKubun = FileInformation.GetImportableValues();
            if (settingKubun.Any()) ImportableTorihikiName = settingKubun;

            var fileLog = new ImportFileLog
            {
                Id          = FileInformation.Index, /* 自動採番で置き換え */
                CompanyId   = Helper.CompanyId,
                FileName    = FileInformation.Path,
                FileSize    = FileInformation.Size,
                CreateBy    = Helper.LoginUserId,
            };
            var parseResult = ImportResult.Success;
            ReceiptHeader header = null;
            var receiptCategoryId = 0;
            DateTime workDay = new DateTime(0);


            BankAccount bankBuf = null;
            DateTime workDayBuf = new DateTime(0);

            foreach (var field in fields)
            {
                if (field.Length < 1)
                {
                    parseResult = ImportResult.FileFormatError;
                    break;
                }

                var recordKubun         = field[0];

                if (!ImportableRecordKubun.Contains(recordKubun))
                {
                    continue;
                }

                if (field.Length < 27)
                {
                    parseResult = ImportResult.FileFormatError;
                    break;
                }

                var torihikimei = field[12];

                if (!ImportableTorihikiName.Contains(torihikimei))
                {
                    continue;
                }

                var headerMonth         = field[1];
                var headerday           = field[2];
                var accountName         = field[5];
                var bankName            = field[6];
                var branchName          = field[7];
                var kouzasyubetsu       = field[9];
                var accountNumber       = field[10];
                var toriatsukaiMonth    = field[15];
                var toriatsukaiDay      = field[16];
                var kisanbiMonth        = field[17];
                var kisanbiDay          = field[18];
                var kingaku             = field[19];
                var payerName           = field[21];
                var sourceBankName      = field[25];
                var sourceBranchName    = field[26];

                var accountTypeId = Helper.GetAccountTypeIdByName(kouzasyubetsu);

                if (!Constants.ImportableAccountTypeIds.Contains(accountTypeId)
                    || !Helper.ValidateAccountNumber(ref accountNumber))
                {
                    parseResult = ImportResult.BankAccountFormatError;
                    break;
                }

                bankName = bankName.Left(30);
                branchName = branchName.Left(30);
                accountName = accountName.Left(30);

                var account = Helper.IsAsync ?
                    await Helper.GetBankAccountByBankNameAsync(bankName, branchName, accountTypeId, accountNumber, token) :
                          Helper.GetBankAccountByBankName     (bankName, branchName, accountTypeId, accountNumber);

                if (account == null || !account.ReceiptCategoryId.HasValue)
                {
                    parseResult = ImportResult.BankAccountMasterError;
                    FileInformation.BankInformation
                        = $"銀行名：{bankName} 支店名：{branchName}, 預金種別：{Helper.GetAccountTypeNameById(accountTypeId)}, 口座番号：{accountNumber}";
                    break;
                }

                fileLog.ReadCount++;

                if (account.ImportSkipping == 1)
                {
                    continue;
                }

                if (!Helper.TryParseDateTimeAnser(headerMonth, headerday, out workDay))
                {
                    parseResult = ImportResult.FileFormatError;
                    break;
                }

                var bankCode = Helper.ConvertToValidEbCharacter(account.BankCode).Right(4, '0', true);
                var branchCode = Helper.ConvertToValidEbCharacter(account.BranchCode).Right(3, '0', true);

                receiptCategoryId = account.ReceiptCategoryId.Value;

                if (!(bankBuf?.Id == account.Id
                    && workDayBuf == workDay))
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
                    header.AccountName = accountName;
                    header.CreateBy = Helper.LoginUserId;
                    header.UpdateBy = Helper.LoginUserId;

                    bankBuf = account;
                    workDayBuf = workDay;
                }

                DateTime recordedAt;
                var smonth = FileInformation.UseValueDate ? kisanbiMonth : toriatsukaiMonth;
                if (string.IsNullOrEmpty(smonth)) smonth = toriatsukaiMonth;
                var sday = FileInformation.UseValueDate ? kisanbiDay : toriatsukaiDay;
                if (string.IsNullOrEmpty(sday)) sday = toriatsukaiDay;
                if (!Helper.TryParseDateTimeAnser(smonth, sday,
                    out recordedAt))
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
    }
}
