using Rac.VOne.Common.Extensions;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.EbData
{
    /// <summary>
    /// U-LINE Xtra Ver2 旧式の取込パターン
    /// </summary>
    /// <remarks>
    /// ヘッダー 4行
    /// 明細データの キャプションあり
    /// 
    /// 例）（半角スペースを _ で表現）
    /// 振込入金明細表_+
    /// お取扱店_ｷﾞﾝｺｳﾒｲ_ｼﾃﾝﾒｲ_+
    /// 口座番号_普通預金_+9999999
    /// 口座名_ｺｳｻﾞﾒｲ_+
    /// 日付_+,ご入金金額_+,
    /// 2000. 1. 1,99999,0,
    /// 2999.12.31,99999,0,
    /// </remarks>
    public class ParserULineXtraVer2 : IParser
    {
        public Helper Helper { get; set; }
        public FileInformation FileInformation { get; set; }

        private const string TitleRecordStart = "振込入金明細表";
        private const string HeaderRecord1Start = "お取扱店";
        private const string HeaderRecord2Start = "口座番号";
        private const string HeaderRecord3Start = "口座名";
        private const string DataCaptionRecordStart = "日付";
        private const string TrailerRecord1Start = "口座合計";
        private const string TrailerRecord2Start = "銀行合計";
        private const string EndRecordStart = "総 合 計";

        /// <summary>読込処理</summary>
        /// <remarks>
        /// ヘッダー は 口座名 レコード確認時に検証
        /// ヘッダー 1..3 が 正しい順序で連携される前提
        /// ヘッダー 3が連携されない場合は正常に取込不可
        /// 
        /// 以下備忘 Split(' ') → 右詰め 左詰め で 空白文字が連続すると、index でのアクセスが難しい
        /// 口座番号は 右詰めなので IEnumerable{string}.Last() を利用した
        /// </remarks>
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
            DateTime workDay = DateTime.Today;
            BankAccount account = null;

            var bankName = string.Empty;
            var branchName = string.Empty;
            var accountType = string.Empty;
            var accountTypeId = 0;
            var accountNumber = string.Empty;
            var accountName = string.Empty;
            foreach (var fields in lines)
            {
                if (!fields.Any()) continue;

                var content = fields.First();
                if (content.StartsWith(TrailerRecord1Start)
                    || content.StartsWith(TrailerRecord2Start)
                    || content.StartsWith(EndRecordStart)) continue;

                var headers = content.Split(' ').Select(x => x.Trim()).ToArray();
                if (content.StartsWith(HeaderRecord1Start))
                {
                    bankName    = headers[1].Trim();
                    branchName  = headers[2].Trim();
                }
                else if (content.StartsWith(HeaderRecord2Start))
                {
                    accountType = headers[1];
                    accountTypeId = Helper.GetAccountTypeIdByName(accountType);
                    accountNumber = headers.Last();
                }
                else if (content.StartsWith(HeaderRecord3Start))
                {
                    if (header != null && header.Receipts.Any())
                        fileLog.ReceiptHeaders.Add(header);

                    accountName = headers[1];
                    bankName = Helper.ConvertToValidEbCharacter(bankName).Left(30);
                    branchName = Helper.ConvertToValidEbCharacter(branchName).Left(30);
                    accountName = Helper.ConvertToValidEbCharacter(accountName).Left(140);

                    if (!Helper.ValidateAccountNumber(ref accountNumber))
                    {
                        parseResult = ImportResult.BankAccountFormatError;
                        break;
                    }

                    account = Helper.IsAsync ?
                        await Helper.GetBankAccountByBankNameAsync(bankName, branchName, accountTypeId, accountNumber, token) :
                              Helper.GetBankAccountByBankName     (bankName, branchName, accountTypeId, accountNumber);
                    if (account == null || !account.ReceiptCategoryId.HasValue)
                    {
                        parseResult = ImportResult.BankAccountMasterError;
                        FileInformation.BankInformation
                            = $"銀行名：{bankName}, 支店名：{branchName}, 預金種別：{accountType}, 口座番号：{accountNumber}";
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

                    header = new ReceiptHeader {
                        CompanyId           = Helper.CompanyId,
                        CurrencyId          = Helper.DefaultCurrency.Id,
                        ImportFileLogId     = fileLog.ReceiptHeaders.Count,
                        Workday             = workDay,
                        BankCode            = account.BankCode,
                        BankName            = bankName,
                        BranchCode          = account.BranchCode,
                        BranchName          = branchName,
                        AccountTypeId       = accountTypeId,
                        AccountNumber       = accountNumber,
                        AccountName         = accountName,
                        CreateBy            = Helper.LoginUserId,
                        UpdateBy            = Helper.LoginUserId,
                    };
                }
                else if (content.StartsWith(DataCaptionRecordStart)) continue;
                else
                {
                    // 項番 1 : 日付 の変換可否で、データレコードの判定
                    var recordedAt = DateTime.MinValue;
                    if (!Helper.TryParseDateTime(fields.First(), out recordedAt)) continue;
                    if (parseSkipping) continue;

                    fileLog.ReadCount++;

                    var nyukingaku          = fields[1];
                    var payerNameRaw        = fields[3];
                    var sourceBankName      = fields[4];
                    var sourceBranchName    = fields[5];

                    var receiptAmount = 0M;
                    if (!decimal.TryParse(nyukingaku, out receiptAmount)
                        || Constants.MaxAmount < Math.Abs(receiptAmount))
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }
                    if (receiptAmount == 0M) continue;

                    payerNameRaw  = Helper.ConvertToValidEbCharacter(payerNameRaw).Left(140);
                    var payerCode = string.Empty;
                    if (Regex.IsMatch(payerNameRaw, "^[0-9]{10} .*$"))
                    {
                        payerCode = payerNameRaw.Substring(0, 10);
                        payerNameRaw = payerNameRaw.Substring(10).Trim();
                    }
                    var payerName = Helper.RemoveLegalPersonality(payerNameRaw);

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
                        PayerCode           = payerCode,
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
            }
            if (header != null && header.Receipts.Any())
                fileLog.ReceiptHeaders.Add(header);

            if (parseResult == ImportResult.Success && !fileLog.ReceiptHeaders.Any())
                parseResult = ImportResult.ImportDataNotFound;

            return Tuple.Create(fileLog, parseResult);
        }
    }
}
