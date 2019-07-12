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
    /// みずほAdvancedシューター 内 キューピーネット 入出金明細照会結果 対応のフォーマット
    /// </summary>
    public class ParserKewpieNetNyusyukkin : IParser
    {
        public Helper Helper { get; set; }
        private class KewpieNetItemIndex
        {
            // 0~1
            internal const int AccountName = 2;
            internal const int BankCode = 3;
            internal const int BankName = 4;
            internal const int BranchCode = 5;
            internal const int BranchName = 6;
            internal const int AccountType = 7;
            internal const int AccountNumber = 8;
            // 9
            internal const int Kanjobi = 10;
            internal const int Kisanbi = 11;
            internal const int IribaraiKubun = 12;
            internal const int TorihikiKubun = 13;
            internal const int TorihikiKingaku = 14;
            // 15~17
            internal const int PayerCode = 18;
            internal const int PayerName = 19;
            // 20
            internal const int SourceBankName = 21;
            internal const int SourceBranchName = 22;
            internal const int Sakuseibi = 23;

        }
        public FileInformation FileInformation { get; set; }

        /// <summary>項目番号 13:入払区分（取込可能）
        /// 1 : 入金・振込入金・取立入金, 2 : 出金
        /// 1  のみ取込可とし、他データはスキップする
        /// </summary>
        private string[] ImportableIribaraiKubun { get; set; } = new[] { "1" };

        /// <summary>項目番号14 : 取引区分（取込可能）
        /// 01 : 振込入金, 02 : 取立入金, 03 : 入金, 04 : 出金, 99 : 取消
        /// デフォルトは01のみ取込可とし、FileInformation.ImportableValuesが設定されている場合はそちらを優先
        /// </summary>
        public int[] ImportableTorihikiKubun { get; set; } = new[] { 1 };

        public async Task<Tuple<ImportFileLog, ImportResult>> ParseAsync(IEnumerable<string[]> lines, CancellationToken token = default(CancellationToken))
        {
            var settingKubun = FileInformation.GetImportableValues().Select(x => int.Parse(x)).ToArray();
            if (settingKubun.Any()) ImportableTorihikiKubun = settingKubun;

            var fileLog = new ImportFileLog {
                Id          = FileInformation.Index,
                CompanyId   = Helper.CompanyId,
                FileName    = FileInformation.Path,
                FileSize    = FileInformation.Size,
                CreateBy    = Helper.LoginUserId,
            };
            var parseResult = ImportResult.Success;

            ReceiptHeader header = null;
            var receiptCategoryId = 0;
            DateTime workDay = DateTime.MinValue;

            BankAccount bankBuf = null;
            DateTime workDayBuf = new DateTime(0);

            foreach (var fields in lines)
            {
                if (fields.Length < 25)
                {
                    parseResult = ImportResult.FileFormatError;
                    break;
                }

                var sakuseibi = fields[KewpieNetItemIndex.Sakuseibi];

                if (!Helper.TryParseJpDateTime(sakuseibi, out workDay))
                {
                    parseResult = ImportResult.FileFormatError;
                    break;
                }

                var bankCode = fields[KewpieNetItemIndex.BankCode];
                var bankName = fields[KewpieNetItemIndex.BankName];
                var branchCode = fields[KewpieNetItemIndex.BranchCode];
                var branchName = fields[KewpieNetItemIndex.BranchName];
                var accountType = fields[KewpieNetItemIndex.AccountType];
                var accountNumber = fields[KewpieNetItemIndex.AccountNumber];
                var accountName = fields[KewpieNetItemIndex.AccountName];

                if (!Helper.ValidateBankCode(ref bankCode)
                    || !Helper.ValidateBranchCode(ref branchCode)
                    || !Helper.ValidateAccountNumber(ref accountNumber))
                {
                    parseResult = ImportResult.BankAccountFormatError;
                    break;
                }

                var accountTypeId = 0;
                if (!int.TryParse(accountType, out accountTypeId)
                    || !Constants.ImportableAccountTypeIds.Contains(accountTypeId))
                {
                    parseResult = ImportResult.BankAccountFormatError;
                    break;
                }
                var account = Helper.IsAsync ?
                    await Helper.GetBankAccountAsync(bankCode, branchCode, accountTypeId, accountNumber, token) :
                          Helper.GetBankAccount     (bankCode, branchCode, accountTypeId, accountNumber);
                if (account == null || !account.ReceiptCategoryId.HasValue)
                {
                    parseResult = ImportResult.BankAccountMasterError;
                    FileInformation.BankInformation
                        = $"銀行コード：{bankCode}, 支店コード：{branchCode}, 預金種別：{accountType}, 口座番号：{accountNumber}";
                    break;
                }

                if (account.ImportSkipping == 1) continue;
                fileLog.ReadCount++;
                receiptCategoryId = account.ReceiptCategoryId.Value;

                bankName = bankName.Left(30);
                branchName = branchName.Left(30);
                accountName = accountName.Left(30);

                if (!(bankBuf?.Id == account.Id
                   && workDayBuf == workDay))
                {
                    header = new ReceiptHeader
                    {
                        CompanyId = Helper.CompanyId,
                        CurrencyId = Helper.DefaultCurrency.Id,
                        ImportFileLogId = fileLog.ReceiptHeaders.Count,
                        Workday = workDay,
                        BankCode = bankCode,
                        BankName = bankName,
                        BranchCode = branchCode,
                        BranchName = branchName,
                        AccountTypeId = accountTypeId,
                        AccountNumber = accountNumber,
                        AccountName = accountName,
                        CreateBy = Helper.LoginUserId,
                        UpdateBy = Helper.LoginUserId,
                    };

                    fileLog.ReceiptHeaders.Add(header);

                    bankBuf = account;
                    workDayBuf = workDay;
                }

                var kanjyoubi = fields[KewpieNetItemIndex.Kanjobi];
                var kisanbi = fields[KewpieNetItemIndex.Kisanbi];
                var iribaraiKubun = fields[KewpieNetItemIndex.IribaraiKubun];
                var torihikikubun = fields[KewpieNetItemIndex.TorihikiKubun];
                var torihikikingaku = fields[KewpieNetItemIndex.TorihikiKingaku];
                var sourceBankName = fields[KewpieNetItemIndex.SourceBankName];
                var sourceBranchName = fields[KewpieNetItemIndex.SourceBranchName];
                var sourcePayerName = fields[KewpieNetItemIndex.PayerName];

                var torihikikubunInt = 0;
                if (!int.TryParse(torihikikubun, out torihikikubunInt))
                {
                    parseResult = ImportResult.FileFormatError;
                    break;
                }
                if (!ImportableIribaraiKubun.Contains(iribaraiKubun)
                    || !ImportableTorihikiKubun.Contains(torihikikubunInt)) continue;

                DateTime recordedAt;
                if (!Helper.TryParseJpDateTime(FileInformation.UseValueDate ? kisanbi : kanjyoubi, out recordedAt))
                {
                    parseResult = ImportResult.FileFormatError;
                    break;
                }

                var receiptAmount = 0M;
                if (!decimal.TryParse(torihikikingaku, out receiptAmount)
                    || Constants.MaxAmount < Math.Abs(receiptAmount))
                {
                    parseResult = ImportResult.FileFormatError;
                    break;
                }

                var payerNameRaw = Helper.ConvertToValidEbCharacter((sourcePayerName).Trim()).Left(140);
                var payerName = Helper.RemoveLegalPersonality(payerNameRaw);
                sourceBankName = Helper.ConvertToValidEbCharacter(sourceBankName).Left(140);
                sourceBranchName = Helper.ConvertToValidEbCharacter(sourceBranchName).Left(140);

                int? customerId = null;

                int? sectionId = account?.SectionId;

                int? excludeCategoryId = Helper.UseApportion ? null : Helper.IsAsync ?
                    await Helper.GetExcludeCategoryIdAsync(payerName, token) :
                          Helper.GetExcludeCategoryId     (payerName);

                var receipt = new Receipt
                {
                    Id                  = header.Receipts.Count() + 1,
                    CompanyId           = Helper.CompanyId,
                    CurrencyId          = Helper.DefaultCurrency.Id,
                    ReceiptHeaderId     = header.Id,
                    ReceiptCategoryId   = receiptCategoryId,
                    CustomerId          = customerId,
                    SectionId           = sectionId,
                    InputType           = Constants.InputTypeEbImporter,
                    Apportioned         = Helper.UseApportion ? 0 : 1,
                    Approved            = 1,
                    Workday             = workDay,
                    RecordedAt          = recordedAt,
                    ReceiptAmount       = receiptAmount,
                    RemainAmount        = receiptAmount,
                    PayerCode           = string.Empty,
                    PayerName           = payerName,
                    PayerNameRaw        = payerNameRaw,
                    SourceBankName      = sourceBankName,
                    SourceBranchName    = sourceBranchName,
                    ExcludeFlag         = excludeCategoryId.HasValue ? 1 : 0,
                    ExcludeCategoryId   = excludeCategoryId,
                    CreateBy            = Helper.LoginUserId,
                    UpdateBy            = Helper.LoginUserId,
                };
                if (excludeCategoryId.HasValue)
                {
                    receipt.ExcludeAmount = receiptAmount;
                    receipt.RemainAmount = 0M;
                    receipt.AssignmentFlag = 2;
                    var exclude = new ReceiptExclude
                    {
                        Id                  = receipt.Id,
                        ReceiptId           = receipt.Id,
                        ExcludeCategoryId   = excludeCategoryId,
                        ExcludeAmount       = receiptAmount,
                        CreateBy            = Helper.LoginUserId,
                        UpdateBy            = Helper.LoginUserId,
                    };
                    header.ReceiptExcludes.Add(exclude);
                }
                header.Receipts.Add(receipt);

                header.ImportCount++;
                header.ImportAmount += receiptAmount;
                fileLog.ImportCount++;
                fileLog.ImportAmount += receiptAmount;
            }

            if (parseResult == ImportResult.Success && !fileLog.ReceiptHeaders.Any())
                parseResult = ImportResult.ImportDataNotFound;

            return Tuple.Create(fileLog, parseResult);
        }

    }
}
