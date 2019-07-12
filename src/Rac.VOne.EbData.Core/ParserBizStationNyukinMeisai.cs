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
    public class ParserBizStationNyukinMeisai : IParser
    {
        public Helper Helper { get; set; }
        public FileInformation FileInformation { get; set; }

        public string BankCode { get; set; } = Constants.BTMUCode;

        /// <summary>常陽銀行 の場合、 金額の項目番号が 8 となる 通常は 9
        /// オフセット用 正数プロパティを用意</summary>
        internal int Offset { private get; set; }

        public async Task<Tuple<ImportFileLog, ImportResult>> ParseAsync(IEnumerable<string[]> fields, CancellationToken token = default(CancellationToken))
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
                    account = null;
                    if (field.Length < 11)
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }
                    var bankCode        = BankCode;
                    var branchCode      = field[1];
                    var bankName        = string.Empty;
                    var branchName      = field[2];
                    var kamokucode      = field[3];
                    var accountNumber   = field[6];
                    var accountName     = field[7];
                    var sousabi         = field[10];

                    var accountTypeId = 0;
                    if (!int.TryParse(kamokucode, out accountTypeId)
                        || !Constants.ImportableAccountTypeIds.Contains(accountTypeId)
                        || !Helper.ValidateBranchCode(ref branchCode)
                        || !Helper.ValidateAccountNumber(ref accountNumber))
                    {
                        parseResult = ImportResult.BankAccountFormatError;
                        break;
                    }

                    if (!Helper.TryParseDateTime(sousabi, out workDay))
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }

                    branchCode = Helper.ConvertToValidEbCharacter(branchCode).Right(3, '0', true);
                    accountNumber = Helper.ConvertToValidEbCharacter(accountNumber).Right(7, '0', true);

                    account = Helper.IsAsync ?
                        await Helper.GetBankAccountAsync(bankCode, branchCode, accountTypeId, accountNumber, token) :
                              Helper.GetBankAccount     (bankCode, branchCode, accountTypeId, accountNumber);

                    if (account == null || !account.ReceiptCategoryId.HasValue)
                    {
                        parseResult = ImportResult.BankAccountMasterError;
                        FileInformation.BankInformation
                            = $"銀行コード：{bankCode}, 支店コード：{branchCode}, 預金種別：{Helper.GetAccountTypeNameById(accountTypeId)}, 口座番号：{accountNumber}";
                        break;
                    }

                    receiptCategoryId = account.ReceiptCategoryId.Value;

                    if (account.ImportSkipping == 1)
                    {
                        parseSkipping = true;
                        header = null;
                        continue;
                    }

                    bankName = Helper.ConvertToValidEbCharacter(account.BankName).Left(30);
                    branchName = Helper.ConvertToValidEbCharacter(branchName).Left(30);
                    accountName = Helper.ConvertToValidEbCharacter(accountName).Left(30);

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
                    fileLog.ReadCount++;

                    if (parseSkipping) { continue; }

                    if (field.Length < 10)
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }

                    var torihikibi          = field[1];
                    var shiteibi            = field[2];
                    var payerName           = field[4];
                    var sourceBankName      = field[5];
                    var sourceBranchName    = field[6];
                    var payerCode           = field[7];
                    var kingaku             = field[9 - Offset];

                    var symd = FileInformation.UseValueDate  ? shiteibi : torihikibi;
                    if (string.IsNullOrEmpty(symd)) symd = torihikibi;
                    DateTime recordedAt;
                    if (!Helper.TryParseDateTime(symd, out recordedAt))
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

                    if (!Helper.ValidatePayerCode(ref payerCode))
                    {
                        parseResult = ImportResult.PayerCodeFormatError;
                        break;
                    }

                    if (ebExcludeAccountList // EBデータ取込対象外口座設定
                        .Any(setting =>
                        {
                            return header.BankCode == setting.BankCode
                                && header.BranchCode == setting.BranchCode
                                && header.AccountTypeId == setting.AccountTypeId
                                && payerCode == setting.PayerCode;
                        }))
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
