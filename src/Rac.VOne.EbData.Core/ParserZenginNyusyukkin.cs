using Rac.VOne.Common.Extensions;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.EbData
{
    public class ParserZenginNyusyukkin : IParser
    {
        public Helper Helper { get; set; }
        public FileInformation FileInformation { get; set; }
        /// <summary>入払区分</summary>
        /// <remarks>[1]：入金 [2]：出金</remarks>
        public string[] ImportIriharaiKubun { get; set; } = { "1" };
        /// <summary>取引区分</summary>
        /// <remarks>
        /// [10]：現金, [11]：振込, [12]：他店券入金, [13]：交換（取立入金及び交換払い）,[14]：振替, [18]：その他, [19]：訂正
        /// </remarks>
        public string[] ImportTorihikiKubun { get; set; } = { "11" };

        public async Task<Tuple<ImportFileLog, ImportResult>> ParseAsync(IEnumerable<string[]> records, CancellationToken token = default(CancellationToken))
        {
            var settingKubun = FileInformation.GetImportableValues();
            if (settingKubun.Any()) ImportTorihikiKubun = settingKubun;

            var fileLog = new ImportFileLog
            {
                Id = FileInformation.Index, /* 自動採番で置き換え */
                CompanyId = Helper.CompanyId,
                FileName = FileInformation.Path,
                FileSize = FileInformation.Size,
                CreateBy = Helper.LoginUserId,
            };
            var parseResult = ImportResult.Success;
            ReceiptHeader header = null;
            var receiptCategoryId = 0;
            var parseSkipping = false;
            DateTime workDay = new DateTime(0);
            BankAccount bank = null;

            var ebExcludeAccountList = Helper.IsAsync ?
                await Helper.GetEBExcludeAccountSettingListAsync(token) :
                      Helper.GetEBExcludeAccountSettingList();
            if (ebExcludeAccountList == null)
            {
                return Tuple.Create(fileLog, ImportResult.DBError);
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
                    parseSkipping = false;
                    bank = null;
                    if (fields.Length < 14)
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }

                    var sakuseibi           = fields[3];
                    var bankCode            = fields[6];
                    var bankName            = fields[7];
                    var branchCode          = fields[8];
                    var branchName          = fields[9];
                    var accountTypeIdBuf    = fields[11];
                    var accountNumber       = fields[12];
                    var accountName         = fields[13];

                    var accountTypeId = 0;
                    if (!int.TryParse(accountTypeIdBuf, out accountTypeId)
                        || !(Constants.ImportableAccountTypeIds).Contains(accountTypeId))
                    {
                        parseResult = ImportResult.BankAccountFormatError;
                        break;
                    }
                    if (!Helper.TryParseJpDateTime(sakuseibi, out workDay))
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }

                    if (!Helper.ValidateBankCode(ref bankCode)
                        || !Helper.ValidateBranchCode(ref branchCode)
                        || !Helper.ValidateAccountNumber(ref accountNumber))
                    {
                        parseResult = ImportResult.BankAccountFormatError;
                        break;
                    }

                    bankName = Helper.ConvertToValidEbCharacter(bankName).Left(30);
                    branchName = Helper.ConvertToValidEbCharacter(branchName).Left(30);
                    accountNumber = Helper.ConvertToValidEbCharacter(accountNumber).Left(7);
                    accountName = Helper.ConvertToValidEbCharacter(accountName).Left(140);

                    bank = Helper.IsAsync ?
                        await Helper.GetBankAccountAsync(bankCode, branchCode, accountTypeId, accountNumber, token) :
                              Helper.GetBankAccount     (bankCode, branchCode, accountTypeId, accountNumber);
                    var defaultReceiptCategoryId = Helper.DefaultReceiptCategory?.Id;
                    if (bank != null && !bank.ReceiptCategoryId.HasValue
                        || !defaultReceiptCategoryId.HasValue)
                    {
                        parseResult = ImportResult.FileReadError;
                        break;
                    }

                    receiptCategoryId = bank != null && bank.ReceiptCategoryId.HasValue
                        ? bank.ReceiptCategoryId.Value
                        : defaultReceiptCategoryId.Value;

                    // skip する場合は、別のheader になるまで skip
                    if (bank != null && bank.ImportSkipping == 1)
                    {
                        parseSkipping = true;
                        header = null;
                        continue;
                    }

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

                }
                if (datakubun == Constants.DataKubun.Data)
                {
                    if (fields.Length < 17)
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }

                    if (parseSkipping) continue;

                    fileLog.ReadCount++;

                    var kanjyoubi           = fields[2];
                    var azukeirebi          = fields[3];
                    var iriharaikubun       = fields[4];
                    var torihikikubun       = fields[5];
                    var torihikikingaku     = fields[6];
                    var payerCode           = fields[13];
                    var payerName           = fields[14];
                    var sourceBankName      = fields[15];
                    var sourceBranchName    = fields[16];

                    if (!ImportIriharaiKubun.Contains(iriharaikubun)
                        || !ImportTorihikiKubun.Contains(torihikikubun))
                    {
                        continue;
                    }

                    DateTime recordedAt;
                    if (!Helper.TryParseJpDateTime(FileInformation.UseValueDate ? azukeirebi : kanjyoubi,
                        out recordedAt))
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }

                    decimal receiptAmount;
                    if (!decimal.TryParse(torihikikingaku, out receiptAmount)
                        || receiptAmount == 0M
                        || Constants.MaxAmount < Math.Abs(receiptAmount))
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }

                    if (!Helper.ValidatePayerCode(ref payerCode))
                    {
                        parseResult = ImportResult.PayerCodeFormatError;
                        break;
                    }

                    if (ebExcludeAccountList // EBデータ取込対象外口座設定
                        .Any(x => x.BankCode       == header.BankCode
                               && x.BranchCode     == header.BranchCode
                               && x.AccountTypeId  == header.AccountTypeId
                               && x.PayerCode      == payerCode))
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
                              Helper.GetSectionIdByPayerCode     (payerCode)) ?? bank?.SectionId;

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
