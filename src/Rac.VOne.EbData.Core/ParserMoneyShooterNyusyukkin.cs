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
    /// みずほAdvancedシューター 内 マネーシューター 入出金明細照会結果 対応のフォーマット
    /// </summary>
    public class ParserMoneyShooterNyusyukkin : IParser
    {
        public Helper Helper { get; set; }
        public FileInformation FileInformation { get; set; }

        /// <summary>データレコード 項目番号 7:取消区分 （取込可能）
        /// 0 : 通常, 1 : 取消, 2 : 欠番
        /// 0  のみ取込可とし、他データはスキップする
        /// </summary>
        private string[] ImportableTorikeshiKubun { get; set; } = new[] { "0" };

        /// <summary>データレコード 項目番号11 : 取引区分（取込可能）
        /// 1 : 振込入金, 2 : 取立入金, 3 : 入金, 4 : 出金
        /// 1 のみ取込可とする
        /// </summary>
        public string[] ImportableTorihikiKubun { get; set; } = new[] { "1" };

        /// <summary>データレコード 項番 12:取引金額区分
        /// 1 : プラス
        /// 2 : マイナス
        /// それ以外は フォーマットエラーとする
        /// 通常 プラスのみ連携される想定
        /// </summary>
        private decimal GetSign(string torihikikingakukubun)
            => torihikikingakukubun == "1" ? 1M
             : torihikikingakukubun == "2" ? -1M
             : 0M;

        public async Task<Tuple<ImportFileLog, ImportResult>> ParseAsync(IEnumerable<string[]> lines, CancellationToken token = default(CancellationToken))
        {
            var torihikiKubuns = FileInformation.GetImportableValues();
            if (torihikiKubuns.Any()) ImportableTorihikiKubun = torihikiKubuns;

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
            var parseSkipping = false;
            DateTime workDay = DateTime.MinValue;
            BankAccount account = null;

            foreach (var fields in lines)
            {
                if (fields.Length < 1)
                {
                    parseResult = ImportResult.FileFormatError;
                    break;
                }
                var datakubun = fields.First();
                if (datakubun == Constants.DataKubun.Header)
                {
                    if (fields.Length < 14)
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }
                    if (header != null && header.Receipts.Any())
                        fileLog.ReceiptHeaders.Add(header);

                    var sakuseibi       = fields[ 2];
                    var bankCode        = fields[ 8];
                    var bankName        = fields[ 9];
                    var branchCode      = fields[10];
                    var branchName      = fields[11];
                    var accountType     = fields[12];
                    var accountNumber   = fields[13];

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
                    if (!Helper.TryParseDateTime(sakuseibi, out workDay))
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }

                    account = Helper.IsAsync ?
                        await Helper.GetBankAccountAsync(bankCode, branchCode, accountTypeId, accountNumber, token) :
                              Helper.GetBankAccount     (bankCode, branchCode, accountTypeId, accountNumber);
                    if (account == null || !account.ReceiptCategoryId.HasValue)
                    {
                        parseResult = ImportResult.BankAccountMasterError;
                        FileInformation.BankInformation
                            = $"銀行コード：{bankCode}, 支店コード：{branchCode}, 預金種別：{accountType}, 口座番号：{accountNumber}";
                        break;
                    }

                    if (account.ImportSkipping == 1)
                    {
                        parseSkipping = true;
                        header = null;
                        continue;
                    }

                    parseSkipping = false;
                    receiptCategoryId = account.ReceiptCategoryId.Value;

                    bankName    = Helper.ConvertToValidEbCharacter(bankName)  .Left(30);
                    branchName  = Helper.ConvertToValidEbCharacter(branchName).Left(30);

                    header = new ReceiptHeader {
                        CompanyId           = Helper.CompanyId,
                        CurrencyId          = Helper.DefaultCurrency.Id,
                        ImportFileLogId     = fileLog.ReceiptHeaders.Count,
                        Workday             = workDay,
                        BankCode            = bankCode,
                        BankName            = bankName,
                        BranchCode          = branchCode,
                        BranchName          = branchName,
                        AccountTypeId       = accountTypeId,
                        AccountNumber       = accountNumber,
                        AccountName         = string.Empty,
                        CreateBy            = Helper.LoginUserId,
                        UpdateBy            = Helper.LoginUserId,
                    };
                }
                if (datakubun == Constants.DataKubun.Data)
                {
                    if (fields.Length < 19)
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }
                    if (parseSkipping) continue;
                    fileLog.ReadCount++;

                    var torikeshikubun          = fields[ 6];
                    var kanjyoubi               = fields[ 8];
                    var kisanbi                 = fields[ 9];
                    var torihikikubun           = fields[10];
                    var torihikikingakukubun    = fields[11];
                    var torihikikingaku         = fields[12];
                    var tekiyo1                 = fields[15];
                    var tekiyo2                 = fields[16];
                    var sourceBankName          = fields[17];
                    var sourceBranchName        = fields[18];

                    if (!ImportableTorikeshiKubun.Contains(torikeshikubun)
                        || !ImportableTorihikiKubun.Contains(torihikikubun)) continue;

                    var monthday = FileInformation.UseValueDate ? kisanbi : kanjyoubi;
                    monthday = monthday.Left(4, '0', true);
                    var month   = monthday.Substring(0, 2);
                    var day     = monthday.Substring(2);
                    var recordedAt = DateTime.MinValue;
                    if (!Helper.TryParseDateTimeAnser(month, day, out recordedAt))
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }
                    var sign = GetSign(torihikikingakukubun);
                    if (sign == 0M)
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
                    receiptAmount = sign * receiptAmount;

                    var payerNameRaw    = Helper.ConvertToValidEbCharacter((tekiyo1 + tekiyo2).Trim()).Left(140);
                    var payerName       = Helper.RemoveLegalPersonality(payerNameRaw);
                    sourceBankName      = Helper.ConvertToValidEbCharacter(sourceBankName)  .Left(140);
                    sourceBranchName    = Helper.ConvertToValidEbCharacter(sourceBranchName).Left(140);

                    int? customerId = null; // Helper.GetCustomerIdByExclusiveInfo(account.BankCode, account.BranchCode, "");

                    int? sectionId = null; // Helper.GetSectionIdByPayerCode(payerCode: "");

                    int? excludeCategoryId = Helper.UseApportion ? null : Helper.IsAsync ?
                        await Helper.GetExcludeCategoryIdAsync(payerName, token) :
                              Helper.GetExcludeCategoryId     (payerName);

                    var receipt = new Receipt {
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
                        receipt.ExcludeAmount   = receiptAmount;
                        receipt.RemainAmount    = 0M;
                        receipt.AssignmentFlag  = 2;
                        var exclude = new ReceiptExclude {
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
                if (datakubun == Constants.DataKubun.Trailer) { }
                if (datakubun == Constants.DataKubun.End) { }
            }
            if (header != null && header.Receipts.Any())
                fileLog.ReceiptHeaders.Add(header);

            if (parseResult == ImportResult.Success && !fileLog.ReceiptHeaders.Any())
                parseResult = ImportResult.ImportDataNotFound;

            return Tuple.Create(fileLog, parseResult);
        }

    }
}
