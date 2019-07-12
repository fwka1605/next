using Rac.VOne.Common.Extensions;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.EbData
{
    public class ParserZenginNyukinMeisai : IParser
    {
        public Helper Helper { get; set; }
        public FileInformation FileInformation { get; set; }
        public async Task<Tuple<ImportFileLog, ImportResult>> ParseAsync(IEnumerable<string[]> records, CancellationToken token = default(CancellationToken))
        {
            var fileLog = new ImportFileLog
            {
                Id          = FileInformation.Index,
                CompanyId   = Helper.CompanyId,
                FileName    = FileInformation.Path,
                FileSize    = FileInformation.Size,
                CreateBy    = Helper.LoginUserId,
            };
            var parseResult = ImportResult.Success;
            ReceiptHeader header = null;
            var receiptCategoryId = 0;
            var parseSkipping = false;
            DateTime workDay = new DateTime(0);
            BankAccount account = null;

            var ebExcludeAccountList = Helper.IsAsync ?
                await Helper.GetEBExcludeAccountSettingListAsync(token) :
                      Helper.GetEBExcludeAccountSettingList();
            if (ebExcludeAccountList == null)
            {
                return Tuple.Create(fileLog, ImportResult.FileReadError); // UNDONE: ひとまず FileReadError を返す
            }

            foreach (var fields in records)
            {

                if (fields.Length < 1)
                {
                    parseResult = ImportResult.FileFormatError;
                    break;
                }
                var datakubun = fields[0];
                if (datakubun == Constants.DataKubun.Header)
                {
                    account = null;
                    if (fields.Length < 13)
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }
                    var sakuseibi       = fields[3];
                    var bankCode        = fields[6];
                    var bankName        = fields[7];
                    var branchCode      = fields[8];
                    var branchName      = fields[9];
                    var yokinsyumoku    = fields[10];
                    var accountNumber   = fields[11];
                    var accountName     = fields[12];

                    if (!Helper.ValidateBankCode(ref bankCode)
                        || !Helper.ValidateBranchCode(ref branchCode)
                        || !Helper.ValidateAccountNumber(ref accountNumber))
                    {
                        parseResult = ImportResult.BankAccountFormatError;
                        break;
                    }

                    bankName = Helper.ConvertToValidEbCharacter(bankName).Left(30);
                    branchName = Helper.ConvertToValidEbCharacter(branchName).Left(30);
                    accountName = Helper.ConvertToValidEbCharacter(accountName).Left(30);

                    var accountTypeId   = 0;
                    if (!int.TryParse(yokinsyumoku, out accountTypeId)
                        || !(Constants.ImportableAccountTypeIds.Contains(accountTypeId)))
                    {
                        parseResult = ImportResult.BankAccountFormatError;
                        break;
                    }
                    if (!Helper.TryParseJpDateTime(sakuseibi, out workDay))
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }

                    account = Helper.IsAsync ?
                        await Helper.GetBankAccountAsync(bankCode, branchCode, accountTypeId, accountNumber, token) :
                              Helper.GetBankAccount     (bankCode, branchCode, accountTypeId, accountNumber);

                    var defaultReceiptCategroyId = Helper.DefaultReceiptCategory?.Id;
                    if (account != null && !account.ReceiptCategoryId.HasValue
                        || !defaultReceiptCategroyId.HasValue)
                    {
                        parseResult = ImportResult.FileReadError;
                        break;
                    }

                    receiptCategoryId = (account != null && account.ReceiptCategoryId.HasValue)
                        ? account.ReceiptCategoryId.Value
                        : defaultReceiptCategroyId.Value;
                    if (account?.ImportSkipping == 1)
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
                    header.AccountTypeId = accountTypeId;
                    header.AccountNumber = accountNumber;
                    header.AccountName = accountName;
                    header.CreateBy = Helper.LoginUserId;
                    header.UpdateBy = Helper.LoginUserId;

                }
                if (datakubun == Constants.DataKubun.Data)
                {
                    if (fields.Length < 12)
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }
                    if (parseSkipping) continue;

                    fileLog.ReadCount++;

                    var kanjyoubi           = fields[2];
                    var kisanbi             = fields[3];
                    var kingaku1            = fields[4];
                    var payerCode           = fields[6];
                    var payerName           = fields[7];
                    var sourceBankName      = fields[8];
                    var sourceBranchName    = fields[9];
                    var torikeshikubun      = fields[10];
                    var kingaku2            = fields[11];

                    if (torikeshikubun == "1")
                    {
                        continue;
                    }

                    DateTime recordedAt;
                    if (!Helper.TryParseJpDateTime(FileInformation.UseValueDate ? kisanbi : kanjyoubi, out recordedAt))
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }

                    decimal receiptAmount;
                    if ((!decimal.TryParse(kingaku1, out receiptAmount)
                            || receiptAmount == 0M && !decimal.TryParse(kingaku2, out receiptAmount)
                            || receiptAmount == 0M)
                        || Constants.MaxAmount < Math.Abs(receiptAmount))
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }

                    if(!Helper.ValidatePayerCode(ref payerCode))
                    {
                        parseResult = ImportResult.PayerCodeFormatError;
                        break;
                    }

                    if (ebExcludeAccountList // EBデータ取込対象外口座設定
                        .Any(x => x.BankCode        == header.BankCode
                               && x.BranchCode      == header.BranchCode
                               && x.AccountTypeId   == header.AccountTypeId
                               && x.PayerCode       == payerCode
                        ))
                    {
                        continue;
                    }

                    payerName = Helper.ConvertToValidEbCharacter(payerName).Left(140);
                    sourceBankName = Helper.ConvertToValidEbCharacter(sourceBankName).Left(140);
                    sourceBranchName = Helper.ConvertToValidEbCharacter(sourceBranchName).Left(15);

                    if (!Helper.IsValidEBChars(payerName))
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }

                    int? customerId = Helper.IsAsync ?
                        await Helper.GetCustomerIdByExclusiveInfoAsync(header.BankCode, header.BranchCode, payerCode, token) :
                              Helper.GetCustomerIdByExclusiveInfo     (header.BankCode, header.BranchCode, payerCode);

                    int? sectionId = (Helper.IsAsync ?
                        await Helper.GetSectionIdByPayerCodeAsync(payerCode, token) :
                              Helper.GetSectionIdByPayerCode     (payerCode)) ?? account?.SectionId;

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
