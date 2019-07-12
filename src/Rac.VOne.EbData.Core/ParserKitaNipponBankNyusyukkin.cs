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
    public class ParserKitaNipponBankNyusyukkin : IParser
    {
        public Helper Helper { get; set; }
        public FileInformation FileInformation { get; set; }

        public string BankCode { get; set; } = Constants.KitaNipponBankCode;

        // [SAMPLE] 余分な空白を削除済み
        // ヘッダ日付書式: yyyyMMdd，明細日付書式: yyyy/M/d，マルチヘッダーなし，明細[3]が｢振込｣で始まる行を取り込み
        //
        // 1,20170501,流通センター支店,当座預金,1234567,ｺｳｻﾞﾒｲｷﾞ
        // 2,2017/4/29,振込ｶ)ﾌﾘｺﾐｲﾗｲﾆﾝﾒｲ,,"\25,974","\8,403,511"
        // 2,2017/4/30,ﾘｿｸ,"\22,102",,"\8,377,537"

        public async Task<Tuple<ImportFileLog, ImportResult>> ParseAsync(IEnumerable<string[]> lines, CancellationToken token = default(CancellationToken))
        {
            var parseResult = ImportResult.Success;

            var fileLog = new ImportFileLog
            {
                Id          = FileInformation.Index,         // 自動採番で置き換え
                CompanyId   = Helper.CompanyId,
                FileName    = FileInformation.Path,
                FileSize    = FileInformation.Size,
                CreateBy    = Helper.LoginUserId,
            };

            DateTime workDay = /* Dummy */ DateTime.MinValue;
            BankAccount account = null;
            ReceiptHeader header = null;

            foreach (var fields in lines)
            {
                if (fields.Length < 1)
                {
                    parseResult = ImportResult.FileFormatError;
                    break;
                }

                var datakubun = fields[0];

                // ヘッダデータ(ReceiptHeader)
                if (datakubun == Constants.DataKubun.Header)
                {
                    if (fields.Length < 6)
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }

                    var inqueryDateField    = fields[1]; // 照会日付
                    var branchNameField     = fields[2]; // 支店名
                    var accountTypeField    = fields[3]; // 預金種別
                    var accountNumberField  = fields[4]; // 口座番号
                    var accountNameField    = fields[5]; // 口座名義

                    if (!Helper.TryParseDateTime(inqueryDateField, out workDay))
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }

                    var accountTypeId = Helper.GetAccountTypeIdByName(accountTypeField);
                    if (!Constants.ImportableAccountTypeIds.Contains(accountTypeId))
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }

                    var accountNumber = accountNumberField.Right(7, '0', true);
                    var accountName = Helper.ConvertToValidEbCharacter(accountNameField).Left(140);

                    account = Helper.IsAsync ?
                        await Helper.GetBankAccountByBranchNameAndNumberAsync(BankCode, branchNameField, accountTypeId, accountNumber, token) :
                              Helper.GetBankAccountByBranchNameAndNumber     (BankCode, branchNameField, accountTypeId, accountNumber);
                    if (account == null || !account.ReceiptCategoryId.HasValue)
                    {
                        parseResult = ImportResult.BankAccountMasterError;
                        FileInformation.BankInformation
                            = $"銀行コード：{BankCode}, 支店名：{branchNameField}, 預金種別：{accountTypeField}, 口座番号：{accountNumberField}";
                        break;
                    }

                    if (account.ImportSkipping == 1)
                    {
                        continue;
                    }

                    header = new ReceiptHeader
                    {
                        Id              = fileLog.ReceiptHeaders.Count,      // 自動採番で置き換え
                        CompanyId       = Helper.CompanyId,
                        CurrencyId      = Helper.DefaultCurrency.Id,
                        ImportFileLogId = fileLog.Id,           // 採番後に置き換え
                        Workday         = workDay,
                        BankCode        = account.BankCode,
                        BankName        = account.BankName.Left(30),
                        BranchCode      = account.BranchCode,
                        BranchName      = account.BranchName.Left(30),
                        AccountTypeId   = account.AccountTypeId,
                        AccountNumber   = account.AccountNumber,
                        AccountName     = accountName,
                        CreateBy        = Helper.LoginUserId,
                        UpdateBy        = Helper.LoginUserId,
                    };
                    fileLog.ReceiptHeaders.Add(header);
                }

                // 明細データ(Receipt)
                else if (datakubun == Constants.DataKubun.Data)
                {
                    if (fields.Length < 6)
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }

                    if (account == null)
                    {
                        parseResult = ImportResult.BankAccountMasterError;
                        break;
                    }

                    if (account.ImportSkipping == 1) continue;

                    fileLog.ReadCount++;

                    var transactionDateField        = fields[1]; // 取引日
                    var transactionDescriptionField = fields[2]; // 取引内容
                    var paymentAmountField          = fields[3]; // 支払金額
                    var receiptAmountField          = fields[4]; // 入金金額
                    var accountBalanceField         = fields[5]; // 残高

                    // "振込"から始まる行のみ取込対象
                    if (!transactionDescriptionField.StartsWith("振込")) continue;

                    // "振込"という文言は取込対象外
                    transactionDescriptionField = transactionDescriptionField.Substring(2).Trim();
                    var payerNameRaw    = Helper.ConvertToValidEbCharacter(transactionDescriptionField).Left(140);
                    var payerName       = Helper.RemoveLegalPersonality(payerNameRaw);

                    if (!Helper.IsValidEBChars(payerName))
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }

                    DateTime recordedAt;
                    if (!Helper.TryParseDateTime(transactionDateField, out recordedAt))
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }

                    decimal receiptAmount;
                    if (!decimal.TryParse(receiptAmountField.TrimStart('\\'), out receiptAmount)
                        || receiptAmount == 0M
                        || Constants.MaxAmount < Math.Abs(receiptAmount))
                    {
                        parseResult = ImportResult.FileFormatError;
                        break;
                    }

                    var receipt = new Receipt
                    {
                        Id                  = header.Receipts.Count(),           // 自動採番
                        CompanyId           = Helper.CompanyId,
                        CurrencyId          = Helper.DefaultCurrency.Id,
                        ReceiptHeaderId     = header.Id,            // 自動採番後に置き換え
                        ReceiptCategoryId   = account.ReceiptCategoryId.Value,
                        CustomerId          = null,
                        SectionId           = account.SectionId,
                        InputType           = Constants.InputTypeEbImporter,
                        Apportioned         = Helper.UseApportion ? 0 : 1,
                        Approved            = 1,                           // 承認済
                        Workday             = workDay,
                        RecordedAt          = recordedAt,
                        ReceiptAmount       = receiptAmount,
                        PayerCode           = "",
                        PayerName           = payerName,
                        PayerNameRaw        = payerNameRaw,
                        SourceBankName      = "",
                        SourceBranchName    = "",
                        CreateBy = Helper.LoginUserId,
                        UpdateBy = Helper.LoginUserId,
                    };
                    header.Receipts.Add(receipt);

                    int? excludeCategoryId = Helper.UseApportion ? null : Helper.IsAsync ?
                        await Helper.GetExcludeCategoryIdAsync(payerName, token) :
                              Helper.GetExcludeCategoryId     (payerName);

                    // 振分画面を利用しない 且つ 除外カナマスターに登録あり
                    if (excludeCategoryId.HasValue)
                    {
                        receipt.ExcludeFlag = 1;
                        receipt.ExcludeCategoryId = excludeCategoryId;
                        receipt.ExcludeAmount = receiptAmount;
                        receipt.RemainAmount = 0M;
                        receipt.AssignmentFlag = 2;             // 消込済

                        header.ReceiptExcludes.Add(new ReceiptExclude
                        {
                            Id = receipt.Id,                    // 自動採番
                            ReceiptId = receipt.Id,             // Receipt.Id採番後に設定
                            ExcludeCategoryId = excludeCategoryId.Value,
                            ExcludeAmount = receiptAmount,
                            OutputAt = null,
                            CreateBy = Helper.LoginUserId,
                            UpdateBy = Helper.LoginUserId,
                        });
                    }

                    // 振分画面を利用する 又は 除外カナマスターに登録なし
                    else
                    {
                        receipt.ExcludeFlag = 0;
                        receipt.ExcludeCategoryId = null;
                        receipt.ExcludeAmount = 0M;
                        receipt.RemainAmount = receiptAmount;
                        receipt.AssignmentFlag = 0;             // 未消込
                    }

                    header.ImportCount++;
                    header.ImportAmount += receiptAmount;
                    fileLog.ImportCount++;
                    fileLog.ImportAmount += receiptAmount;
                }
            }

            if (parseResult == ImportResult.Success && fileLog?.ImportCount == 0)
            {
                parseResult = ImportResult.ImportDataNotFound;
            }

            return Tuple.Create(fileLog, parseResult);
        }
    }
}
