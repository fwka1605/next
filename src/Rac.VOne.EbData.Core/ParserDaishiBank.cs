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
    /// <summary>第四銀行、八十二銀行、北國銀行
    /// ナンバー銀行系？ Web サイトから DL できるCSV 形式
    /// </summary>
    /// <remarks>
    /// 念為 マルチヘッダー 対応
    /// 項目数で、ヘッダー、明細の判定を行う
    /// 入出金明細 のため、出金のみのデータを読み込んだ場合、取込対象のデータが無い旨、エラーを返す
    /// 振込依頼人番号 が無いため、除外設定の読込は行わない
    /// </remarks>
    public class ParserDaishiBank : IParser
    {
        public Helper Helper { get; set; }
        public FileInformation FileInformation { get; set; }

        public async Task<Tuple<ImportFileLog, ImportResult>> ParseAsync(IEnumerable<string[]> lines, CancellationToken token = default(CancellationToken))
        {
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

            const int HeaderFieldsCount = 7;
            const int DetailFieldsCount = 6;

            foreach (var fields in lines)
            {
                if (fields.Length < 1)
                {
                    parseResult = ImportResult.FileFormatError;
                    break;
                }

                if (fields.Length == HeaderFieldsCount)
                {
                    if (header != null && header.Receipts.Any())
                    {
                        fileLog.ReceiptHeaders.Add(header);
                    }
                    var bankName            = fields[0];
                    var branchName          = fields[1];
                    var accountTypeName     = fields[2];
                    var accountNumber       = fields[3];
                    var searchYMDFrom       = fields[4];
                    var searchYMDTo         = fields[5];
                    var downloadDateTime    = fields[6];
                    var accountTypeId       = Helper.GetAccountTypeIdByName(accountTypeName);
                    if (accountTypeId == 0)
                    {
                        parseResult = ImportResult.BankAccountFormatError;
                        break;
                    }
                    if (!Helper.ValidateAccountNumber(ref accountNumber))
                    {
                        parseResult = ImportResult.BankAccountFormatError;
                        break;
                    }
                    bankName                = bankName  .Left(30);
                    branchName              = branchName.Left(30);

                    account = Helper.IsAsync ?
                        await Helper.GetBankAccountByBankNameAsync(bankName, branchName, accountTypeId, accountNumber, token) :
                              Helper.GetBankAccountByBankName     (bankName, branchName, accountTypeId, accountNumber);
                    if (account == null || !account.ReceiptCategoryId.HasValue)
                    {
                        parseResult = ImportResult.BankAccountMasterError;
                        FileInformation.BankInformation
                            = $"銀行名：{bankName}, 支店名：{branchName}, 預金種別：{accountTypeName}, 口座番号：{accountNumber}";
                        break;
                    }

                    if (account.ImportSkipping == 1)
                    {
                        parseSkipping = true;
                        header = null;
                        continue;
                    }

                    if (!Helper.TryParseDateTime(downloadDateTime, out workDay, "yyyyMMddHHmmss"))
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }
                    workDay = workDay.Date;

                    parseSkipping = false;
                    receiptCategoryId = account.ReceiptCategoryId.Value;

                    header = new ReceiptHeader {
                        CompanyId       = Helper.CompanyId,
                        CurrencyId      = Helper.DefaultCurrency.Id,
                        ImportFileLogId = fileLog.ReceiptHeaders.Count,
                        Workday         = workDay,
                        BankCode        = account.BankCode,
                        BankName        = bankName,
                        BranchCode      = account.BranchCode,
                        BranchName      = branchName,
                        AccountTypeId   = accountTypeId,
                        AccountNumber   = accountNumber,
                        AccountName     = string.Empty,
                        CreateBy        = Helper.LoginUserId,
                        UpdateBy        = Helper.LoginUserId,
                    };
                }
                else if (fields.Length == DetailFieldsCount)
                {
                    if (parseSkipping) continue;

                    fileLog.ReadCount++;

                    var dealNumber  = fields[0];
                    var torihikibi  = fields[1];
                    var syukkingaku = fields[2];
                    var nyuukingaku = fields[3];
                    var tekiyou     = fields[4];
                    var zandaka     = fields[5];

                    var recordedAt = DateTime.MinValue;
                    if (!Helper.TryParseDateTime(torihikibi, out recordedAt))
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }

                    var receiptAmount = 0M;
                    if (string.IsNullOrWhiteSpace(nyuukingaku)) continue;
                    if (!decimal.TryParse(nyuukingaku, out receiptAmount)
                        || Constants.MaxAmount < Math.Abs(receiptAmount))
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }
                    if (receiptAmount == 0M) continue;

                    tekiyou = Helper.ConvertToValidEbCharacter(tekiyou);
                    tekiyou = string.Concat(tekiyou.Select(c => c.ToString()).Where(x => Helper.GetByteCount(x) == 1).ToArray()).Trim();
                    var payerNameRaw    = tekiyou.Left(140);
                    var payerName       = Helper.RemoveLegalPersonality(payerNameRaw);

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
                        SourceBankName      = string.Empty,
                        SourceBranchName    = string.Empty,
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
                else continue;
            }

            if (header != null && header.Receipts.Any())
                fileLog.ReceiptHeaders.Add(header);

            if (parseResult == ImportResult.Success && !fileLog.ReceiptHeaders.Any())
                parseResult = ImportResult.ImportDataNotFound;

            return Tuple.Create(fileLog, parseResult);
        }
    }
}
