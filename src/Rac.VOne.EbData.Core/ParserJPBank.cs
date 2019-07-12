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
    public class ParserJPBank : IParser
    {
        public Helper Helper { get; set; }
        public FileInformation FileInformation { get; set; }

        public async Task<Tuple<ImportFileLog, ImportResult>> ParseAsync(IEnumerable<string[]> fields, CancellationToken token = default(CancellationToken))
        {
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
            DateTime workDay = DateTime.Today;
            DateTime recordedAt = new DateTime(0);
            BankAccount bank = null;

            var parseSkipping = false;

            foreach (var field in fields)
            {
                if (field.Length < 1)
                {
                    parseResult = ImportResult.FileFormatError;
                    break;
                }
                var datakubun = field[0];
                if (datakubun == Constants.DataKubun.Header)
                {
                    if (field.Length < 5)
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }
                    bank = null;
                    var bankCode = Constants.JPBankCode;
                    var ukeirekouzakigou = field[2];
                    var ukeirekouzanumber = field[3];
                    var toriatsukaibi = field[4];
                    var branchName = ukeirekouzakigou + ukeirekouzanumber;

                    var branchCode = string.Empty;
                    var bankName = string.Empty;
                    var accountNumber = string.Empty;
                    var accountName = string.Empty;

                    if (!Helper.TryParseDateTime(toriatsukaibi, out recordedAt))
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }

                    bank = Helper.IsAsync ?
                        await Helper.GetBankAccountByBranchNameAsync(bankCode, branchName, token) :
                              Helper.GetBankAccountByBranchName     (bankCode, branchName);

                    if (bank == null || !bank.ReceiptCategoryId.HasValue)
                    {
                        parseResult = ImportResult.BankAccountMasterError;
                        FileInformation.BankInformation
                            = $"銀行コード：{bankCode}, 支店名：{branchName}";
                        break;
                    }

                    receiptCategoryId = bank.ReceiptCategoryId.Value;

                    branchCode = bank.BranchCode.Right(3, '0', true);
                    accountNumber = bank.AccountNumber.Right(7, '0', true);
                    bankName = bank.BankName.Left(30);

                    if (bank.ImportSkipping == 1)
                    {
                        parseSkipping = true;
                        header = null;
                        continue;
                    }
                    header = new ReceiptHeader();
                    fileLog.ReceiptHeaders.Add(header);
                    header.Id = fileLog.ReceiptHeaders.Count;
                    header.CompanyId = Helper.CompanyId;
                    header.CurrencyId = Helper.DefaultCurrency.Id;
                    header.ImportFileLogId = fileLog.Id; /* 採番後に置き換え */
                    header.Workday = workDay;
                    header.BankCode = bankCode;
                    header.BankName = bankName;
                    header.BranchCode = branchCode;
                    header.BranchName = branchName;
                    header.AccountTypeId = bank.AccountTypeId;
                    header.AccountNumber = accountNumber;
                    header.AccountName = string.Empty;
                    header.CreateBy = Helper.LoginUserId;
                    header.UpdateBy = Helper.LoginUserId;


                }
                if (datakubun == Constants.DataKubun.Data)
                {

                    if (parseSkipping) { continue; }

                    fileLog.ReadCount++;

                    if (field.Length < 11)
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }

                    var kingaku = field[5];
                    var payerName = field[10];
                    var sourceBankName = string.Empty;
                    var sourceBranchName = string.Empty;

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

                    int? sectionId = bank.SectionId; // Helper.GetSectionIdByPayerCode(payerCode);

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
                if (datakubun == Constants.DataKubun.Trailer) { }
                if (datakubun == Constants.DataKubun.End) { }

            }

            if (parseResult == ImportResult.Success && fileLog?.ImportCount == 0)
            {
                parseResult = ImportResult.ImportDataNotFound;
            }

            return Tuple.Create(fileLog, parseResult);
        }
    }
}
