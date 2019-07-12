using Rac.VOne.Common.Extensions;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.EbData
{
    public class ParserBizStationZenmeisai : IParser
    {
        public Helper Helper { get; set; }
        public FileInformation FileInformation { get; set; }
        public string BankCode { get; set; } = Constants.BTMUCode;

        public string[] NotImportKubun { get; } = { "利息", "ご融資" };

        public async Task< Tuple<ImportFileLog, ImportResult> > ParseAsync(IEnumerable<string[]> fields, CancellationToken token = default(CancellationToken))
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
            BankAccount bank = null;

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
                    bank = null;

                    if (field.Length < 11)
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }
                    var bankCode        = BankCode;
                    var bankName        = string.Empty;
                    var branchCode      = field[1];
                    var branchName      = field[2];
                    var kamokucode      = field[3];
                    var accountNumber   = field[6];
                    var accountName     = field[7];
                    var sousabi         = field[10];

                    var accountTypeId = 0;
                    if (!int.TryParse(kamokucode, out accountTypeId)
                        || !(Constants.ImportableAccountTypeIds.Contains(accountTypeId))
                        || !(Helper.ValidateBranchCode(ref branchCode))
                        || !(Helper.ValidateAccountNumber(ref accountNumber)))
                    {
                        parseResult = ImportResult.BankAccountFormatError;
                        break;
                    }

                    if (!Helper.TryParseDateTime(sousabi, out workDay))
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }

                    bank = Helper.IsAsync ?
                        await Helper.GetBankAccountAsync(bankCode, branchCode, accountTypeId, accountNumber, token) :
                              Helper.GetBankAccount     (bankCode, branchCode, accountTypeId, accountNumber);

                    if (bank == null || !bank.ReceiptCategoryId.HasValue)
                    {
                        parseResult = ImportResult.BankAccountMasterError;
                        FileInformation.BankInformation
                            = $"銀行コード：{bankCode}, 支店コード：{branchCode}, 預金種別：{Helper.GetAccountTypeNameById(accountTypeId)}, 口座番号：{accountNumber}";
                        break;
                    }

                    bankName = bank.BankName.Left(30);
                    branchName = Helper.ConvertToValidEbCharacter(branchName).Left(30);
                    accountName = Helper.ConvertToValidEbCharacter(accountName).Left(140);

                    receiptCategoryId = bank.ReceiptCategoryId.Value;


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
                    header.AccountTypeId = accountTypeId;
                    header.AccountNumber = accountNumber;
                    header.AccountName = accountName;
                    header.CreateBy = Helper.LoginUserId;
                    header.UpdateBy = Helper.LoginUserId;


                }
                if (datakubun == Constants.DataKubun.Data)
                {
                    if (parseSkipping) { continue; }

                    fileLog.ReadCount++;

                    if (field.Length < 6)
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }

                    var torihikibi          = field[1];
                    var torihikikubun       = field[2];
                    var payerName           = field[3];
                    var sourceBankName      = string.Empty;
                    var sourceBranchName    = string.Empty;
                    var kingaku             = field[5];

                    DateTime recordedAt;
                    if (!Helper.TryParseDateTime(torihikibi, out recordedAt))
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }

                    decimal receiptAmount;
                    if (!decimal.TryParse(kingaku, out receiptAmount)
                        || Constants.MaxAmount < Math.Abs(receiptAmount))
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }

                    if (receiptAmount == 0M)
                    {
                        continue; // 支払は0M 処理スキップ
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
