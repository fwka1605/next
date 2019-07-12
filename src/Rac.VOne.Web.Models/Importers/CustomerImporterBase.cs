using Rac.VOne.Common.DataHandling;
using Rac.VOne.Common.Extensions;
using Rac.VOne.Common.Importer.Customer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Web.Models.Importers
{
    public class CustomerImporterBase
    {

        /// <summary>月末日 代替:99</summary>
        private const int LastDayOfMonth = 99;

        protected int CollectCategoryType => Rac.VOne.Common.CategoryType.Collect;
        protected ApplicationControl ApplicationControl { get; set; }

        public int CompanyId { get; set; }
        public string CompanyCode { get; set; }

        public int LoginUserId { get; set; }

        public List<string> ErrorList { get; private set; } = new List<string>();
        private int ErrorCount { get; set; } = 0;
        public int ValidRecordCount { get; set; }
        public int InvalidRecordCount { get; set; }
        private List<Customer> ValidCustomerData { get; set; } = new List<Customer>();
        private List<Customer> InvalidCustomerData { get; set; } = new List<Customer>();
        protected RoundingType roundingType;
        private int DecimalScale { get { return 0; } }

        private IEnumerable<string> LegalPersonalities { get; set; }

        private List<Category> CollectCategoryList { get; set; }
        private List<Staff> StaffsList { get; set; }

        private List<Customer> dbCustomers { get; set; }

        /// <summary> 得意先マスター フリーインポーター 設定ヘッダ</summary>
        private ImporterSetting ImporterSetting { get; set; }
        /// <summary> 得意先マスター フリーインポーター 詳細 </summary>
        private List<ImporterSettingDetail> ImporterSettingDetails { get; set; }
        private int StartLineCount { get { return ImporterSetting?.StartLineCount ?? 0; } }
        private bool IgnoreLastLine { get { return ImporterSetting?.IgnoreLastLine == 1; } }

        public string PatternNo { get; set; }
        private int FormatId { get { return (int)FreeImporterFormatType.Customer; } }

        public CustomerImporterBase(ApplicationControl control)
        {
            ApplicationControl = control;
        }

        public async Task InitializeAsync()
        {
            var headerTask = GetImporterSettingAsync(FormatId, PatternNo);
            var detailsTask = GetImporterSettingDetailAsync(FormatId, PatternNo);
            await Task.WhenAll(headerTask, detailsTask);

            ImporterSetting = headerTask.Result;
            ImporterSettingDetails = detailsTask.Result;
        }

        public bool IsImporterSettingRegistered
        {
            get
            {
                return ImporterSetting != null
                  && ImporterSettingDetails != null
                  && ImporterSettingDetails.Any(x => x.FieldIndex > 0);
            }
        }

        public string GetInitialFilePath()
        {
            var dir = ImporterSetting?.InitialDirectory;
            if (string.IsNullOrEmpty(dir)
                || !Directory.Exists(dir))
                dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var fileName = $"得意先マスター{DateTime.Today:yyyyMMdd}.csv";
            return Path.Combine(dir, fileName);
        }

        private async Task LoadDataAsync()
        {
            var tasks = new List<Task>();
            tasks.Add(Task.Run(async () => roundingType = await GetRoundingTypeAsync()));
            tasks.Add(Task.Run(async () => CollectCategoryList = await GetCollectCategoryAsync()));
            tasks.Add(Task.Run(async () => StaffsList = await GetStaffAsync()));
            tasks.Add(Task.Run(async () => LegalPersonalities = await GetLeagalPersonaritiesAsync()));
            tasks.Add(Task.Run(async () => dbCustomers = await GetCustomerAsync()));
            await Task.WhenAll(tasks);
        }

        public async Task<ImportResult> ImportAsync(string filePath, int importMode, string errorLogPath)
        {
            var result = new ImportResult { ProcessResult = new ProcessResult(), };
            await LoadDataAsync();
            await ConfirmItems(filePath, importMode, errorLogPath);
            result.ValidItemCount = ValidCustomerData.Count;
            result.InvalidItemCount = InvalidCustomerData.Count;
            if (ValidCustomerData.Count == 0) return result;
            if (ValidRecordCount == 0) return result;

            var details = ImporterSettingDetails;

            try
            {
                var insertCustomerList = new List<Customer>();
                var deleteCustomerList = new List<Customer>();
                var updateCustomerList = new List<Customer>();

                if (importMode == 0)
                {
                    deleteCustomerList.AddRange(dbCustomers);
                    foreach (var customer in ValidCustomerData)
                    {
                        var dbCustomer = dbCustomers.FirstOrDefault(x => x.Code == customer.Code);
                        if (dbCustomer != null)
                        {
                            updateCustomerList.Add(CreateUpdateList(details, customer, dbCustomer));
                            deleteCustomerList.Remove(dbCustomer);
                        }
                        else
                        {
                            insertCustomerList.Add(customer);
                        }
                    }
                }
                else if (importMode == 1)
                {
                    insertCustomerList.AddRange(ValidCustomerData);
                }
                else if (importMode == 2)
                {
                    foreach (var customer in ValidCustomerData)
                    {
                        var dbCustomer = dbCustomers.FirstOrDefault(x => x.Code == customer.Code);
                        if (dbCustomer != null)
                        {
                            updateCustomerList.Add(CreateUpdateList(details, customer, dbCustomer));
                        }
                        else
                        {
                            insertCustomerList.Add(customer);
                        }
                    }
                }

                result = await ImportCustomerAsync(insertCustomerList, updateCustomerList, deleteCustomerList);

                result.ValidItemCount = ValidCustomerData.Count;
                result.InvalidItemCount = InvalidCustomerData.Count;
            }
            catch (Exception ex)
            {
                if (LogError != null)
                    LogError?.Invoke(ex);
                else
                    throw;
            }
            return result;
        }

        public Customer CreateUpdateList(List<ImporterSettingDetail> details,  Customer csvCustomer, Customer previousCustomer)
        {
            var updateCustomer = new Customer();
            Func<ImporterSettingDetail, Customer, Customer, Customer> selector
                = (detail, db, csv) => detail.UpdateKey == 0 ? db : csv;
            Action<Customer, Customer> setter = null;
            foreach (var detail in details)
            {
                var field = (Fields)detail.Sequence;
                if (field == Fields.CompanyCode              ) setter = (trg, src) => trg.CompanyId                 = src.CompanyId;
                if (field == Fields.CustomerCode             ) setter = (trg, src) => trg.Code                      = src.Code;
                if (field == Fields.CustomerName             ) setter = (trg, src) => trg.Name                      = src.Name;
                if (field == Fields.CustomerKana             ) setter = (trg, src) => trg.Kana                      = src.Kana;
                if (field == Fields.ExclusiveBankCode        ) setter = (trg, src) => trg.ExclusiveBankCode         = src.ExclusiveBankCode;
                if (field == Fields.ExclusiveBankName        ) setter = (trg, src) => trg.ExclusiveBankName         = src.ExclusiveBankName;
                if (field == Fields.ExclusiveBranchCode      ) setter = (trg, src) => trg.ExclusiveBranchCode       = src.ExclusiveBranchCode;
                if (field == Fields.ExclusiveBranchName      ) setter = (trg, src) => trg.ExclusiveBranchName       = src.ExclusiveBranchName;
                if (field == Fields.ExclusiveAccountNumber   ) setter = (trg, src) => trg.ExclusiveAccountNumber    = src.ExclusiveAccountNumber;
                if (field == Fields.ExclusiveAccountTypeId   ) setter = (trg, src) => trg.ExclusiveAccountTypeId    = src.ExclusiveAccountTypeId;
                if (field == Fields.ShareTransferFee         ) setter = (trg, src) => trg.ShareTransferFee          = src.ShareTransferFee;
                if (field == Fields.CreditLimit              ) setter = (trg, src) => trg.CreditLimit               = src.CreditLimit;
                if (field == Fields.ClosingDay               ) setter = (trg, src) => trg.ClosingDay                = src.ClosingDay;
                if (field == Fields.CollectCategoryId        ) setter = (trg, src) =>
                {
                    trg.CollectCategoryId   = src.CollectCategoryId;
                    trg.CollectCategoryCode = src.CollectCategoryCode;
                };
                if (field == Fields.CollectOffsetMonth       ) setter = (trg, src) => trg.CollectOffsetMonth        = src.CollectOffsetMonth;
                if (field == Fields.CollectOffsetDay         ) setter = (trg, src) => trg.CollectOffsetDay          = src.CollectOffsetDay;
                if (field == Fields.CollectOffsetDayPerBilling) setter = (trg, src) => trg.CollectOffsetDay          = src.CollectOffsetDay;
                if (field == Fields.StaffCode                ) setter = (trg, src) =>
                {
                    trg.StaffId     = src.StaffId;
                    trg.StaffCode   = src.StaffCode;
                };
                if (field == Fields.IsParent                 ) setter = (trg, src) => trg.IsParent                  = src.IsParent;
                if (field == Fields.PostalCode               ) setter = (trg, src) => trg.PostalCode                = src.PostalCode;
                if (field == Fields.Address1                 ) setter = (trg, src) => trg.Address1                  = src.Address1;
                if (field == Fields.Address2                 ) setter = (trg, src) => trg.Address2                  = src.Address2;
                if (field == Fields.Tel                      ) setter = (trg, src) => trg.Tel                       = src.Tel;
                if (field == Fields.Fax                      ) setter = (trg, src) => trg.Fax                       = src.Fax;
                if (field == Fields.CustomerStaffName        ) setter = (trg, src) => trg.CustomerStaffName         = src.CustomerStaffName;
                if (field == Fields.Note                     ) setter = (trg, src) => trg.Note                      = src.Note;
                if (field == Fields.UseFeeLearning           ) setter = (trg, src) => trg.UseFeeLearning            = src.UseFeeLearning;
                if (field == Fields.SightOfBill              ) setter = (trg, src) => trg.SightOfBill               = src.SightOfBill;
                if (field == Fields.DensaiCode               ) setter = (trg, src) => trg.DensaiCode                = src.DensaiCode;
                if (field == Fields.CreditCode               ) setter = (trg, src) => trg.CreditCode                = src.CreditCode;
                if (field == Fields.CreditRank               ) setter = (trg, src) => trg.CreditRank                = src.CreditRank;
                if (field == Fields.TransferBankCode         ) setter = (trg, src) => trg.TransferBankCode          = src.TransferBankCode;
                if (field == Fields.TransferBankName         ) setter = (trg, src) => trg.TransferBankName          = src.TransferBankName;
                if (field == Fields.TransferBranchCode       ) setter = (trg, src) => trg.TransferBranchCode        = src.TransferBranchCode;
                if (field == Fields.TransferBranchName       ) setter = (trg, src) => trg.TransferBranchName        = src.TransferBranchName;
                if (field == Fields.TransferAccountNumber    ) setter = (trg, src) => trg.TransferAccountNumber     = src.TransferAccountNumber;
                if (field == Fields.TransferAccountTypeId    ) setter = (trg, src) => trg.TransferAccountTypeId     = src.TransferAccountTypeId;
                if (field == Fields.TransferCustomerCode     ) setter = (trg, src) => trg.TransferCustomerCode      = src.TransferCustomerCode;
                if (field == Fields.TransferNewCode          ) setter = (trg, src) => trg.TransferNewCode           = src.TransferNewCode;
                if (field == Fields.TransferAccountName      ) setter = (trg, src) => trg.TransferAccountName       = src.TransferAccountName;
                if (field == Fields.ThresholdValue           ) setter = (trg, src) => trg.ThresholdValue            = src.ThresholdValue;
                if (field == Fields.LessThanCollectCategoryId       ) setter = (trg, src) =>
                {
                    trg.LessThanCollectCategoryId   = src.LessThanCollectCategoryId;
                    trg.LessThanCollectCategoryCode = src.LessThanCollectCategoryCode;
                };
                if (field == Fields.GreaterThanCollectCategoryId1   ) setter = (trg, src) =>
                {
                    trg.GreaterThanCollectCategoryId1   = src.GreaterThanCollectCategoryId1;
                    trg.GreaterThanCollectCategoryCode1 = src.GreaterThanCollectCategoryCode1;
                };
                if (field == Fields.GreaterThanRate1                ) setter = (trg, src) => trg.GreaterThanRate1           = src.GreaterThanRate1;
                if (field == Fields.GreaterThanRoundingMode1        ) setter = (trg, src) => trg.GreaterThanRoundingMode1   = src.GreaterThanRoundingMode1;
                if (field == Fields.GreaterThanSightOfBill1         ) setter = (trg, src) => trg.GreaterThanSightOfBill1    = src.GreaterThanSightOfBill1;
                if (field == Fields.GreaterThanCollectCategoryId2   ) setter = (trg, src) =>
                {
                    trg.GreaterThanCollectCategoryId2   = src.GreaterThanCollectCategoryId2;
                    trg.GreaterThanCollectCategoryCode2 = src.GreaterThanCollectCategoryCode2;
                };
                if (field == Fields.GreaterThanRate2                ) setter = (trg, src) => trg.GreaterThanRate2           = src.GreaterThanRate2;
                if (field == Fields.GreaterThanRoundingMode2        ) setter = (trg, src) => trg.GreaterThanRoundingMode2   = src.GreaterThanRoundingMode2;
                if (field == Fields.GreaterThanSightOfBill2         ) setter = (trg, src) => trg.GreaterThanSightOfBill2    = src.GreaterThanSightOfBill2;
                if (field == Fields.GreaterThanCollectCategoryId3   ) setter = (trg, src) =>
                {
                    trg.GreaterThanCollectCategoryId3   = src.GreaterThanCollectCategoryId3;
                    trg.GreaterThanCollectCategoryCode3 = src.GreaterThanCollectCategoryCode3;
                };
                if (field == Fields.GreaterThanRate3                ) setter = (trg, src) => trg.GreaterThanRate3           = src.GreaterThanRate3;
                if (field == Fields.GreaterThanRoundingMode3        ) setter = (trg, src) => trg.GreaterThanRoundingMode3   = src.GreaterThanRoundingMode3;
                if (field == Fields.GreaterThanSightOfBill3         ) setter = (trg, src) => trg.GreaterThanSightOfBill3    = src.GreaterThanSightOfBill3;
                if (field == Fields.UseKanaLearning                 ) setter = (trg, src) => trg.UseKanaLearning            = src.UseKanaLearning;
                if (field == Fields.HolidayFlag                     ) setter = (trg, src) => trg.HolidayFlag                = src.HolidayFlag;
                if (field == Fields.UseFeeTolerance                 ) setter = (trg, src) => trg.UseFeeTolerance            = src.UseFeeTolerance;
                if (field == Fields.PrioritizeMatchingIndividually  ) setter = (trg, src) => trg.PrioritizeMatchingIndividually  = src.PrioritizeMatchingIndividually;
                if (field == Fields.CollationKey                    ) setter = (trg, src) => trg.CollationKey               = src.CollationKey;
                if (field == Fields.ExcludeInvoicePublish           ) setter = (trg, src) => trg.ExcludeInvoicePublish      = src.ExcludeInvoicePublish;
                if (field == Fields.ExcludeReminderPublish          ) setter = (trg, src) => trg.ExcludeReminderPublish     = src.ExcludeReminderPublish;
                if (field == Fields.DestinationDepartmentName       ) setter = (trg, src) => trg.DestinationDepartmentName  = src.DestinationDepartmentName;
                if (field == Fields.Honorific                       ) setter = (trg, src) => trg.Honorific                  = src.Honorific;

                setter?.Invoke(updateCustomer, selector(detail, previousCustomer, csvCustomer));
            }
            updateCustomer.Id = previousCustomer.Id;
            updateCustomer.ReceiveAccountId1 = previousCustomer.ReceiveAccountId1;
            updateCustomer.ReceiveAccountId2 = previousCustomer.ReceiveAccountId2;
            updateCustomer.ReceiveAccountId3 = previousCustomer.ReceiveAccountId3;

            return updateCustomer;
        }

        public Func<List<Customer>, List<Customer>, List<Customer>, Task<ImportResult>> ImportCustomerAsync { get; set; }

        public ICsvParser Parser { get; set; } = new CsvParser {
            IgnoreBlankLines = false,
        };

        private async Task ConfirmItems(string filePath, int importType, string errorLogPath)
        {
            ValidCustomerData.Clear();
            InvalidCustomerData.Clear();
            ErrorList.Clear();
            ErrorCount = 0;
            var validCount = 0;
            var invalidCount = 0;
            var dupeCheck = new HashSet<string>();

            try
            {
                var records = Parser.Parse(filePath).ToArray();
                for (var index = 0; index < records.Length; index++)
                {
                    var i = index + 1;
                    if (i < StartLineCount) continue;
                    if (IgnoreLastLine && i == records.Length) break;

                    ErrorCount = 0;
                    var fields = records[index];

                    var customer = new Customer();

                    var normalDueAt = false;

                    foreach (var detail in ImporterSettingDetails.OrderBy(x => x.Sequence))
                    {
                        var field = (Fields)detail.Sequence;
                        var attribute = detail.AttributeDivision ?? 0;
                        if (field == Fields.CompanyCode)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     会社コード              空白のためインポートできません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                OwnCompanyCode(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0)
                            {
                                customer.CompanyId = CompanyId;
                            }
                        }
                        else if (field == Fields.CustomerCode)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           得意先コードがありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                CustomerCode(customer, fields[detail.FieldIndex.Value - 1], i);
                                var code = customer.Code;
                                if (importType == 1)
                                {
                                    var existCode = dbCustomers.Any(x => x.Code.Equals(customer.Code));
                                    if (existCode)
                                    {
                                        ErrorCount++;
                                        ErrorList.Add(string.Format("{0:00000000}", i) + "行目     得意先コード          既に登録されている得意先のため、インポートできません。");
                                    }
                                }
                                if (dupeCheck.Contains(code))
                                {
                                    ErrorList.Add(string.Format("{0:00000000}", i) + "行目     得意先コード          重複しているため、インポートできません。");
                                    ErrorCount++;
                                }
                                else
                                    dupeCheck.Add(code);

                            }
                            if (detail.ImportDivision == 0)
                            {
                                customer.Code = fields[detail.FieldIndex.Value - 1];
                            }
                        }
                        else if (field == Fields.CustomerName)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           得意先名がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                CustomerName(customer, fields[detail.FieldIndex.Value - 1], i, attribute);
                            }
                            else if (detail.ImportDivision == 0)
                            {
                                customer.Name = "";
                            }
                        }
                        else if (field == Fields.CustomerKana)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           得意先名カナがありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                CustomerNameKana(customer, fields[detail.FieldIndex.Value - 1], i, attribute);
                            }
                            else if (detail.ImportDivision == 0)
                            {
                                customer.Kana = "";
                            }
                        }
                        else if (field == Fields.ExclusiveBankCode)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           専用銀行コードがありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                ExclusiveBankCode(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.ExclusiveBankCode = "";
                            }
                        }
                        else if (field == Fields.ExclusiveBankName)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           専用銀行名がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                ExclusiveBankName(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.ExclusiveBankName = "";
                            }
                        }
                        else if (field == Fields.ExclusiveBranchCode)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           専用支店コードがありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                ExclusiveBranchCode(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.ExclusiveBranchCode = "";
                            }
                        }
                        else if (field == Fields.ExclusiveBranchName)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           専用支店名がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                ExclusiveBranchName(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.ExclusiveBranchName = "";
                            }
                        }
                        else if (field == Fields.ExclusiveAccountNumber)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           専用入金口座番号がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                ExclusiveAccountNumber(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.ExclusiveAccountNumber = "";
                            }
                        }
                        else if (field == Fields.ExclusiveAccountTypeId)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           預金種別がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                ExclusiveAccountTypeId(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.ExclusiveAccountTypeId = null;
                            }
                        }
                        else if (field == Fields.ShareTransferFee)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           手数料負担区分がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                ShareTransferFee(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.ShareTransferFee = int.Parse(detail.FixedValue);
                            }
                        }
                        else if (field == Fields.CreditLimit)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           与信限度額がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                CreditLimit(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.CreditLimit = 0;
                            }

                        }
                        else if (field == Fields.ClosingDay)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           締日がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                ClosingDay(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.ClosingDay = int.Parse(detail.FixedValue);
                            }

                            normalDueAt = customer.ClosingDay != 0;

                        }
                        else if (field == Fields.CollectCategoryId)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           回収方法がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                CollectCategoryId(customer, fields[detail.FieldIndex.Value - 1], i, attribute);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.CollectCategoryId = CollectCategoryList.Where(x => x.Code == detail.FixedValue).Select(x => x.Id).FirstOrDefault();
                                customer.CollectCategoryCode = detail.FixedValue;
                            }
                        }
                        else if (field == Fields.CollectOffsetMonth)
                        {
                            if (!normalDueAt)
                            {
                                customer.CollectOffsetMonth = 0;
                                continue;
                            }

                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           回収予定（月）がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                CollectOffsetMonth(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 2 && detail.FixedValue != "")
                            {
                                customer.CollectOffsetMonth = int.Parse(detail.FixedValue);
                            }
                        }
                        else if (field == Fields.CollectOffsetDay)
                        {
                            if (!normalDueAt) continue;

                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           回収予定（日）がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                CollectOffsetDay(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 2 && detail.FixedValue != "")
                            {
                                customer.CollectOffsetDay = int.Parse(detail.FixedValue);
                            }
                        }
                        else if (field == Fields.CollectOffsetDayPerBilling)
                        {
                            if (normalDueAt) continue;

                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 > fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           回収予定（都度請求）がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                CollectOffsetDayPerBilling(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 2 && detail.FixedValue != "")
                            {
                                customer.CollectOffsetDay = int.Parse(detail.FixedValue);
                            }
                        }
                        else if (field == Fields.StaffCode)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           営業担当者がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                StaffCode(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.StaffCode = detail.FixedValue;
                                List<int> staffId = StaffsList.Where(c => c.Code == customer.StaffCode).Select(x => x.Id).ToList();
                                customer.StaffId = Convert.ToInt32(staffId[0]);
                            }
                        }
                        else if (field == Fields.IsParent)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           債権代表者フラグがありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                IsParent(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.IsParent = int.Parse(detail.FixedValue);
                            }
                        }
                        else if (field == Fields.PostalCode)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           郵便番号がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                PostalCode(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.PostalCode = "";
                            }
                        }
                        else if (field == Fields.Address1)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           住所1がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                Address1(customer, fields[detail.FieldIndex.Value - 1], i, attribute);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.Address1 = "";
                            }

                        }
                        else if (field == Fields.Address2)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           住所2がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                Address2(customer, fields[detail.FieldIndex.Value - 1], i, attribute);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.Address2 = "";
                            }

                        }
                        else if (field == Fields.Tel)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           TEL番号がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                Tel(customer, fields[detail.FieldIndex.Value - 1], i, attribute);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.Tel = "";
                            }

                        }
                        else if (field == Fields.Fax)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           FAX番号がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                Fax(customer, fields[detail.FieldIndex.Value - 1], i, attribute);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.Fax = "";
                            }
                        }
                        else if (field == Fields.CustomerStaffName)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           相手先担当者名がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                CustomerStaffName(customer, fields[detail.FieldIndex.Value - 1], i, attribute);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.CustomerStaffName = "";
                            }
                        }
                        else if (field == Fields.Note)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           備考がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                Note(customer, fields[detail.FieldIndex.Value - 1], i, attribute);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.Note = "";
                            }
                        }
                        else if (field == Fields.UseFeeLearning)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           手数料自動学習がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                UseFeeLearning(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.UseFeeLearning = customer.ShareTransferFee == 0 ? 0 : int.Parse(detail.FixedValue);
                            }
                        }
                        else if (field == Fields.SightOfBill)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           回収サイトがありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                SightOfBill(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.SightOfBill = null;
                            }
                            else
                            {
                                var limitDate = CollectCategoryList.Where(x => x.Code == customer.CollectCategoryCode).Select(x => x.UseLimitDate).FirstOrDefault();
                                if (limitDate == 1)
                                {
                                    ErrorList.Add(string.Format("{0:00000000}", i) + "行目     回収サイト            空白のためインポートできません。");
                                    ErrorCount++;
                                }
                            }
                        }
                        else if (field == Fields.DensaiCode)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           電子手形用企業コードがありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                DensaiCode(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.DensaiCode = "";
                            }
                        }
                        else if (field == Fields.CreditCode)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           信用調査用企業コードがありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                CreditCode(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.CreditCode = "";
                            }
                        }
                        else if (field == Fields.CreditRank)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           与信ランクがありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                CreditRank(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.CreditRank = "";
                            }
                        }
                        else if (field == Fields.TransferBankCode)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           口座振替用銀行コードがありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                TransferBankCode(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.TransferBankCode = "";
                            }
                        }
                        else if (field == Fields.TransferBankName)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           口座振替用銀行名がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                TransferBankName(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.TransferBankName = "";
                            }
                        }
                        else if (field == Fields.TransferBranchCode)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           口座振替用支店コードがありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                TransferBranchCode(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.TransferBranchCode = "";
                            }
                        }
                        else if (field == Fields.TransferBranchName)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           口座振替用支店名がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                TransferBranchName(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.TransferBranchName = "";
                            }

                        }
                        else if (field == Fields.TransferAccountNumber)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           口座振替用口座番号がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                TransferAccountNumber(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.TransferAccountNumber = "";
                            }
                        }
                        else if (field == Fields.TransferAccountTypeId)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           口座振替用預金種別がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                TransferAccountTypeId(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.TransferAccountTypeId = null;
                            }
                        }
                        else if (field == Fields.TransferCustomerCode)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           口座振替用顧客コードがありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                TransferCustomerCode(customer, fields[detail.FieldIndex.Value - 1], i, attribute);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.TransferCustomerCode = "";
                            }
                        }
                        else if (field == Fields.TransferNewCode)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           口座振替用新規コードがありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                TransferNewCode(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.TransferNewCode = "";
                            }
                        }
                        else if (field == Fields.TransferAccountName)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           口座振替用預金者名がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                TransferAccountName(customer, fields[detail.FieldIndex.Value - 1], i, attribute);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.TransferAccountName = "";
                            }
                        }
                        else if (field == Fields.ThresholdValue)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           約定金額がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                ThresholdValue(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.ThresholdValue = 0;
                            }
                        }
                        else if (field == Fields.LessThanCollectCategoryId)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           約定金額未満がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                LessThanCollectCategoryId(customer, fields[detail.FieldIndex.Value - 1], i, attribute);
                            }
                        }
                        else if (field == Fields.GreaterThanCollectCategoryId1)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           約定金額以上1がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                GreaterThanCollectCategoryId1(customer, fields[detail.FieldIndex.Value - 1], i, attribute);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.GreaterThanCollectCategoryId1 = 0;
                            }
                        }
                        else if (field == Fields.GreaterThanRate1)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           分割1がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                GreaterThanRate1(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                        }
                        else if (field == Fields.GreaterThanRoundingMode1)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           端数1がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                GreaterThanRoundingMode1(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                        }
                        else if (field == Fields.GreaterThanSightOfBill1)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           回収サイト1がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                GreaterThanSightOfBill1(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                        }
                        else if (field == Fields.GreaterThanCollectCategoryId2)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           約定金額以上2がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                GreaterThanCollectCategoryId2(customer, fields[detail.FieldIndex.Value - 1], i, attribute);
                            }
                        }
                        else if (field == Fields.GreaterThanRate2)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           分割2がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                GreaterThanRate2(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                        }
                        else if (field == Fields.GreaterThanRoundingMode2)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           端数2がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                GreaterThanRoundingMode2(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                        }
                        else if (field == Fields.GreaterThanSightOfBill2)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           回収サイト2がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                GreaterThanSightOfBill2(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                        }
                        else if (field == Fields.GreaterThanCollectCategoryId3)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           約定金額以上3がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                GreaterThanCollectCategoryId3(customer, fields[detail.FieldIndex.Value - 1], i, attribute);
                            }
                        }
                        else if (field == Fields.GreaterThanRate3)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           分割3がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                GreaterThanRate3(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                        }
                        else if (field == Fields.GreaterThanRoundingMode3)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           端数3がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                GreaterThanRoundingMode3(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                        }
                        else if (field == Fields.GreaterThanSightOfBill3)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           回収サイト3がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                GreaterThanSightOfBill3(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                        }
                        else if (field == Fields.UseKanaLearning)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           カナ自動学習がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                UseKanaLearning(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.UseKanaLearning = int.Parse(detail.FixedValue);
                            }
                        }
                        else if (field == Fields.HolidayFlag)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           休業日設定がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                HolidayFlag(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.HolidayFlag = int.Parse(detail.FixedValue);
                            }
                        }
                        else if (field == Fields.UseFeeTolerance)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           手数料誤差利用がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                UseFeeTolerance(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.UseFeeTolerance = customer.ShareTransferFee == 0 ? 0 : int.Parse(detail.FixedValue);
                            }
                        }
                        else if (field == Fields.PrioritizeMatchingIndividually)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           一括消込対象外がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                PrioritizeMatchingIndividually(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.PrioritizeMatchingIndividually = int.Parse(detail.FixedValue);
                            }
                        }
                        else if (field == Fields.CollationKey)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           照合番号がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                CollationKey(customer, fields[detail.FieldIndex.Value - 1], i, attribute);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.CollationKey = "";
                            }
                        }
                        else if (field == Fields.ExcludeInvoicePublish)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           請求書発行対象外がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                ExcludeInvoicePublish(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.ExcludeInvoicePublish = int.Parse(detail.FixedValue);
                            }
                        }
                        else if (field == Fields.ExcludeReminderPublish)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           督促状発行対象外がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                ExcludeReminderPublish(customer, fields[detail.FieldIndex.Value - 1], i);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.ExcludeReminderPublish = int.Parse(detail.FixedValue);
                            }
                        }
                        else if (field == Fields.DestinationDepartmentName)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           相手先部署がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                DestinationDepartmentName(customer, fields[detail.FieldIndex.Value - 1], i, attribute);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.DestinationDepartmentName = "";
                            }
                        }
                        else if (field == Fields.Honorific)
                        {
                            if (detail.ImportDivision == 1 && detail.FieldIndex - 1 >= fields.Length)
                            {
                                ErrorList.Add(string.Format("{0:00000000}", i) + "行目                           敬称がありません。");
                                ErrorCount++;
                                break;
                            }
                            if (detail.ImportDivision == 1 && detail.FieldIndex != 0)
                            {
                                Honorific(customer, fields[detail.FieldIndex.Value - 1], i, attribute);
                            }
                            else if (detail.ImportDivision == 0 && detail.FixedValue != "")
                            {
                                customer.Honorific = "";
                            }
                        }
                    }//foreach

                    customer.ReceiveAccountId1 = 1;
                    customer.ReceiveAccountId2 = 1;
                    customer.ReceiveAccountId3 = 1;
                    customer.UpdateBy = LoginUserId;
                    customer.CreateBy = LoginUserId;
                    customer.LineNo = i;

                    if (ErrorCount > 0)
                    {
                        invalidCount += 1;
                        InvalidCustomerData.Add(customer);
                    }
                    else
                    {
                        validCount += 1;
                        ValidCustomerData.Add(customer);
                    }

                }

                var customerData = ValidCustomerData.Concat(InvalidCustomerData).ToList();
                var hiddenFlag = 0;
                var previousErrorCount = invalidCount;
                var codes = customerData.Select(x => x.Code).Where(c => !string.IsNullOrEmpty(c)).Distinct().ToArray();

                var arrayindex = new List<int>();

                for (var i = 0; i < ValidCustomerData.Count; i++)
                {
                    ErrorCount = 0;
                    if (ValidCustomerData[i].CollectCategoryCode == "00")
                    {
                        var totalGreaterThanRate = ValidCustomerData[i].GreaterThanRate1 + ValidCustomerData[i].GreaterThanRate2 + ValidCustomerData[i].GreaterThanRate3;
                        int showFlg = 0;

                        if (totalGreaterThanRate != 100)
                        {
                            if (previousErrorCount != 0)
                            {
                                hiddenFlag++;
                            }
                            else
                            {
                                ErrorList.Add(string.Format("{0:00000000}", ValidCustomerData[i].LineNo) + "行目     分割1                 分割の合計値が100にならないため、インポートできません。");
                                ErrorList.Add(string.Format("{0:00000000}", ValidCustomerData[i].LineNo) + "行目     分割2                 分割の合計値が100にならないため、インポートできません。");
                                ErrorList.Add(string.Format("{0:00000000}", ValidCustomerData[i].LineNo) + "行目     分割3                 分割の合計値が100にならないため、インポートできません。");
                                ErrorCount++;
                            }
                        }

                        if (ValidCustomerData[i].GreaterThanRoundingMode1 == 0 && ValidCustomerData[i].GreaterThanRoundingMode2 == 0 && ValidCustomerData[i].GreaterThanRoundingMode3 == 0) showFlg++;
                        else if (ValidCustomerData[i].GreaterThanRoundingMode1 == 0 && ValidCustomerData[i].GreaterThanRoundingMode2 == 0 && ValidCustomerData[i].GreaterThanRoundingMode3 != 0) showFlg++;
                        else if (ValidCustomerData[i].GreaterThanRoundingMode1 == 0 && ValidCustomerData[i].GreaterThanRoundingMode2 != 0 && ValidCustomerData[i].GreaterThanRoundingMode3 == 0) showFlg++;
                        else if (ValidCustomerData[i].GreaterThanRoundingMode1 != 0 && ValidCustomerData[i].GreaterThanRoundingMode2 == 0 && ValidCustomerData[i].GreaterThanRoundingMode3 == 0) showFlg++;
                        else if (ValidCustomerData[i].GreaterThanRoundingMode1 != 0 && ValidCustomerData[i].GreaterThanRoundingMode2 != 0 && ValidCustomerData[i].GreaterThanRoundingMode3 != 0)
                        {
                            if (previousErrorCount != 0)
                            {
                                hiddenFlag++;
                            }
                            else
                            {
                                ErrorList.Add(string.Format("{0:00000000}", ValidCustomerData[i].LineNo) + "行目     端数1                 端数処理が無いため、インポートできません。");
                                ErrorList.Add(string.Format("{0:00000000}", ValidCustomerData[i].LineNo) + "行目     端数2                 端数処理が無いため、インポートできません。");
                                ErrorList.Add(string.Format("{0:00000000}", ValidCustomerData[i].LineNo) + "行目     端数3                 端数処理が無いため、インポートできません。");
                                ErrorCount++;
                            }
                        }

                        if (showFlg > 0)
                        {
                            if (previousErrorCount != 0)
                            {
                                hiddenFlag++;
                            }
                            else
                            {
                                ErrorList.Add(string.Format("{0:00000000}", ValidCustomerData[i].LineNo) + "行目     端数1                 端数処理が複数あるため、インポートできません。");
                                ErrorList.Add(string.Format("{0:00000000}", ValidCustomerData[i].LineNo) + "行目     端数2                 端数処理が複数あるため、インポートできません。");
                                ErrorList.Add(string.Format("{0:00000000}", ValidCustomerData[i].LineNo) + "行目     端数3                 端数処理が複数あるため、インポートできません。");
                                ErrorCount++;
                            }
                        }
                    }

                    if (ErrorCount > 0 || hiddenFlag > 0)
                    {
                        invalidCount += 1;
                        validCount -= 1;
                        InvalidCustomerData.Add(ValidCustomerData[i]);
                        arrayindex.Add(i);
                    }
                }

                for (var i = arrayindex.Count - 1; i >= 0; i--)
                {
                    var index = arrayindex[i];
                    ValidCustomerData.Remove(ValidCustomerData[index]);
                }

                if (importType == 0)
                {
                    var parentResult = await GetMasterDataForCustomerGroupParentAsync(codes);
                    if (parentResult.Count > 0)
                    {
                        foreach (var data in parentResult)
                        {
                            var message = $"債権代表者マスターに存在する{data.Code}：{data.Name}が存在しないため、インポートできません。";
                            ErrorList.Add(message);
                        }
                        ErrorCount++;
                        invalidCount++;
                        validCount = 0;
                    }

                    var childResult = await GetMasterDataForCustomerGroupChildAsync(codes);
                    if (childResult.Count > 0)
                    {
                        foreach (var data in childResult)
                        {
                            var message = $"債権代表者マスターに存在する{data.Code}：{data.Name}が存在しないため、インポートできません。";
                            ErrorList.Add(message);
                        }
                        ErrorCount++;
                        invalidCount++;
                        validCount = 0;
                    }

                    var kanaHistoryResult = await GetMasterDataForKanaHistoryAsync(codes);
                    if (kanaHistoryResult.Count > 0)
                    {
                        foreach (var data in kanaHistoryResult)
                        {
                            var message = $"カナ学習履歴に存在する{data.Code}：{data.Name}が存在しないため、インポートできません。";
                            ErrorList.Add(message);
                        }
                        ErrorCount++;
                        invalidCount++;
                        validCount = 0;
                    }

                    var billingResult = await GetMasterDataForBillingAsync(codes);
                    if (billingResult.Count > 0)
                    {
                        foreach (var data in billingResult)
                        {
                            var message = $"請求データに存在する{data.Code}：{data.Name}が存在しないため、インポートできません。";
                            ErrorList.Add(message);
                        }
                        ErrorCount++;
                        invalidCount++;
                        validCount = 0;
                    }

                    var receiptResult = await GetMasterDataForReceiptAsync(codes);
                    if (receiptResult.Count > 0)
                    {
                        foreach (var data in receiptResult)
                        {
                            var message = $"入金データに存在する{data.Code}：{data.Name}が存在しないため、インポートできません。";
                            ErrorList.Add(message);
                        }
                        ErrorCount++;
                        invalidCount++;
                        validCount = 0;
                    }

                    var nettingResult = await GetMasterDataForNettingAsync(codes);
                    if (nettingResult.Count > 0)
                    {
                        foreach (var data in nettingResult)
                        {
                            var message = $"入金予定相殺データに存在する{data.Code}：{data.Name}が存在しないため、インポートできません。";
                            ErrorList.Add(message);
                        }
                        ErrorCount++;
                        invalidCount++;
                        validCount = 0;
                    }
                }

                InvalidRecordCount = invalidCount;
                if (importType == 0 && invalidCount > 0)
                {
                    validCount = 0;
                    ValidCustomerData.Clear();
                }
                ValidRecordCount = validCount;

                if (invalidCount > 0 && !string.IsNullOrEmpty(errorLogPath))
                {
                    OutputErrorLog?.Invoke(errorLogPath, ErrorList, filePath);
                }
            }
            catch (Exception ex)
            {
                if (LogError != null)
                    LogError.Invoke(ex);
                else
                    throw;
            }
        }

        /// <summary>例外エラーログの出力</summary>
        public Action<Exception> LogError { get; set; }

        /// <summary>検証エラーログ出力
        /// string : エラーログの path
        /// IEnumerable{string} : エラーログ
        /// string : 取込元の file path
        /// </summary>
        public Action<string, IEnumerable<string>, string> OutputErrorLog { get; set; }

        public bool IsNullOrWhiteSpaceWithTrim(string value)
        {
            if (string.IsNullOrWhiteSpace(value.Trim())) return true;
            return false;
        }

        private delegate bool TryParse<TValue>(string input, out TValue output);

        private TValue TryParseOrDefault<TValue>(TryParse<TValue> tryParse, string input)
        {
            if (tryParse == null)
                throw new ArgumentNullException(nameof(tryParse));
            TValue outvalue;
            return tryParse(input, out outvalue) ? outvalue : default(TValue);
        }
        private string GetErrorMessage(int lineNumber, string itemName, string message, Encoding encoding = null)
            => $"{lineNumber:D8}行目  {(encoding ?? ShiftJIS).PadRightMultiByte(itemName, 30)}{message}";
        private Encoding ShiftJIS { get { return Encoding.GetEncoding(932); } }

        #region 単発 0編集が必要なものは、customer のインスタンス渡しているので対応可能
        private void OwnCompanyCode(Customer customerData, string value, int i)
        {
            if (IsNullOrWhiteSpaceWithTrim(value))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     会社コード            ログインしている会社コードではないためインポートできません。");
                ErrorCount++;
                return;
            }

            var expression = new DataExpression(ApplicationControl);
            value = expression.CompanyCodeType == 0 ? value.PadLeft(4, '0') : value;
            if (value != CompanyCode)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     会社コード            ログインしている会社コードではないためインポートできません。");
                ErrorCount++;
                return;
            }
            customerData.CompanyId = CompanyId;
        }

        private void CustomerCode(Customer customerData, string value, int i)
        {
            if (IsNullOrWhiteSpaceWithTrim(value))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     得意先コード          空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            var codeLength = ApplicationControl.CustomerCodeLength;
            if (value.Length > codeLength)
            {
                string message = $"行目     得意先コード          {codeLength}文字以上のためインポートできません。";
                ErrorList.Add(string.Format("{0:00000000}", i) + message);
                ErrorCount++;
                return;
            }

            var codeType = ApplicationControl.CustomerCodeType;
            var pattern = CustomerHelper.CustomerPermission(codeType);

            value = EbDataHelper.ConvertToUpperCase(value);
            value = EbDataHelper.ConvertToHankakuKatakana(value);

            if (!Regex.IsMatch(value, pattern))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     得意先コード          不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }
            if (codeType == 0) value = value.PadLeft(codeLength, '0');
            customerData.Code = value;
        }

        private void CustomerName(Customer customerData, string value, int i, int attributes)
        {
            bool nullCustomerName = IsNullOrWhiteSpaceWithTrim(value);

            if (nullCustomerName)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     得意先名              空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            bool overLimit = value.Length > 140;

            if (overLimit && attributes == 1)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     得意先名              140文字以上のためインポートできません。");
                ErrorCount++;
                return;
            }

            customerData.Name = (overLimit && attributes == 2) ? value.Substring(0, 140) : value;
        }

        private void CustomerNameKana(Customer customerData, string value, int i, int attributes)
        {
            if (IsNullOrWhiteSpaceWithTrim(value))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     得意先名カナ          空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (EbDataHelper.ContainsKanji(value))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     得意先名カナ          不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            value = EbDataHelper.ConvertToPayerName(value, LegalPersonalities);
            if (string.IsNullOrEmpty(value))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     得意先名カナ          文字変換処理によって空白になったためインポートできません。");
                ErrorCount++;
                return;
            }

            bool overLimit = value.Length > 140;

            if (overLimit && attributes == 1)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     得意先名カナ          140文字以上のためインポートできません。");
                ErrorCount++;
                return;
            }

            customerData.Kana = (overLimit && attributes == 2) ? value.Substring(0, 140) : value;
        }

        private void ExclusiveBankCode(Customer customerData, string value, int i)
        {
            if (string.IsNullOrWhiteSpace(value)) return;

            if (!Regex.IsMatch(value, CustomerHelper.DigitPermissionPattern))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     専用銀行コード        不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            if (value.Length > 4)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     専用銀行コード        4文字以上のためインポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.ExclusiveBankCode = value.PadLeft(4, '0');
            }
        }

        private void ExclusiveBankName(Customer customerData, string value, int i)
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            customerData.ExclusiveBankName = value.Length > 30 ? value.Substring(0, 30) : value;
        }

        private void ExclusiveBranchCode(Customer customerData, string value, int i)
        {
            if (string.IsNullOrWhiteSpace(value)) return;

            if (!Regex.IsMatch(value, CustomerHelper.DigitPermissionPattern))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     専用支店コード        不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            if (value.Length > 3)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     専用支店コード        3文字以上のためインポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.ExclusiveBranchCode = value.PadLeft(3, '0');
            }
        }

        private void ExclusiveBranchName(Customer customerData, string value, int i)
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            customerData.ExclusiveBranchName = value.Length > 30 ? value.Substring(0, 30) : value;
        }

        private void ExclusiveAccountNumber(Customer customerData, string value, int i)
        {
            if (string.IsNullOrWhiteSpace(value)) return;

            if (!Regex.IsMatch(value, CustomerHelper.DigitPermissionPattern))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     専用入金口座番号      不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            if (value.Length > 10)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     専用入金口座番号      10文字以上のためインポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.ExclusiveAccountNumber = value.PadLeft(10, '0');
            }
        }

        private void ExclusiveAccountTypeId(Customer customerData, string value, int i)
        {
            bool nullExclusiveAccountTypeId = IsNullOrWhiteSpaceWithTrim(value);
            if (nullExclusiveAccountTypeId) return;

            if (value != "1" && value != "2" && value != "4" && value != "5")
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     預金種別              不正な文字が入力されているためインポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.ExclusiveAccountTypeId = TryParseOrDefault<int>(int.TryParse, value);
            }
        }

        private void ShareTransferFee(Customer customerData, string value, int i)
        {
            bool nullShareTransferFee = IsNullOrWhiteSpaceWithTrim(value);

            if (nullShareTransferFee)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     手数料負担区分        空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (value != "1" && value != "0")
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     手数料負担区分        不正な文字が入力されているためインポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.ShareTransferFee = TryParseOrDefault<int>(int.TryParse, value);
            }
        }

        private void CreditLimit(Customer customerData, string value, int i)
        {
            bool nullCreditLimit = IsNullOrWhiteSpaceWithTrim(value);
            if (nullCreditLimit) return;

            if (!Regex.IsMatch(value, CustomerHelper.DigitDecPermissionPattern))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     与信限度額            不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            decimal decValues = TryParseOrDefault<decimal>(decimal.TryParse, value);
            if (decValues < 0 || decValues > 999999999)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     与信限度額            不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            if (value.Contains('.') && roundingType == RoundingType.Error)
            {
                ErrorCount++;
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     与信限度額            小数が含まれているためインポートできません。");
            }
            else
            {
                customerData.CreditLimit = roundingType.Calc(decValues, DecimalScale).Value;
            }
        }

        private void ClosingDay(Customer customerData, string value, int i)
        {
            if (IsNullOrWhiteSpaceWithTrim(value))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     締日                  空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (!Regex.IsMatch(value, "^[0-9]{1,2}$"))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     締日                  不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            var result = TryParseOrDefault<int>(int.TryParse, value);
            var min = ClosingDayMinValue;
            var max = ClosingDayMaxValue;
            if (!(min <= result && result <= max))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     締日                  不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            if (result > 27) result = LastDayOfMonth;
            customerData.ClosingDay = result;
        }

        private void CollectCategoryId(Customer customerData, string value, int i, int attribute)
        {
            bool nullCollectCategoryCode = IsNullOrWhiteSpaceWithTrim(value);

            if (nullCollectCategoryCode)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     回収方法              空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            var category = GetCategory(value, attribute);
            if (category == null)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     回収方法              存在しないため、インポートできません。");
                ErrorCount++;
                return;
            }

            customerData.CollectCategoryId = category.Id;
            customerData.CollectCategoryCode = category.Code;
        }

        private void CollectOffsetMonth(Customer customerData, string value, int i)
        {
            bool nullCollectOffsetMonth = IsNullOrWhiteSpaceWithTrim(value);
            if (nullCollectOffsetMonth)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     回収予定（月）        空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (!Regex.IsMatch(value, CustomerHelper.DigitPermissionPattern))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     回収予定（月）        不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            var result = TryParseOrDefault<int>(int.TryParse, value);
            if (!(0 <= result && result <= 9))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     回収予定（月）        不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            customerData.CollectOffsetMonth = result;
        }

        private void CollectOffsetDay(Customer customerData, string value, int i)
        {
            bool nullCollectOffsetDay = IsNullOrWhiteSpaceWithTrim(value);

            if (nullCollectOffsetDay)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     回収予定（日）        空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (!Regex.IsMatch(value, "^[0-9]{1,2}$"))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     回収予定（日）        不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            var result = TryParseOrDefault<int>(int.TryParse, value);
            if (!(1 <= result && result <= LastDayOfMonth))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     回収予定（日）        不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            if (result > 27) result = LastDayOfMonth;
            customerData.CollectOffsetDay = result;
        }

        private void CollectOffsetDayPerBilling(Customer customer, string value, int i)
        {
            if (IsNullOrWhiteSpaceWithTrim(value))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     回収予定（都度請求）  空白のためインポートできません。");
                ErrorCount++;
                return;
            }
            if (!Regex.IsMatch(value, "^[0-9]{1,2}$"))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     回収予定（都度請求）  不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            var result = TryParseOrDefault<int>(int.TryParse, value);
            var maxOffsetDays = 99;
            if (!(0 <= result && result <= maxOffsetDays))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     回収予定（都度請求）  不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            customer.CollectOffsetMonth = 0;
            customer.CollectOffsetDay = result;
        }

        private void StaffCode(Customer customerData, string value, int i)
        {
            bool nullStaffCode = IsNullOrWhiteSpaceWithTrim(value);

            if (nullStaffCode)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     営業担当者            空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            var expression = new DataExpression(ApplicationControl);

            if (expression.StaffCodeFormatString == "9")
                value = value.PadLeft(expression.StaffCodeLength, '0');

            var code = StaffsList.Exists(c => c.Code == value);

            if (!code)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     営業担当者            存在しないため、インポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.StaffId = StaffsList.Where(x => x.Code == value).Select(x => x.Id).FirstOrDefault();
            }
        }

        private void IsParent(Customer customerData, string value, int i)
        {
            bool nullIsParent = IsNullOrWhiteSpaceWithTrim(value);

            if (nullIsParent)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     債権代表者フラグ      空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (value != "1" && value != "0")
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     債権代表者フラグ      不正な文字が入力されているためインポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.IsParent = TryParseOrDefault<int>(int.TryParse, value);
            }
        }

        private void PostalCode(Customer customerData, string value, int i)
        {
            bool nullPostalCode = IsNullOrWhiteSpaceWithTrim(value);
            if (nullPostalCode) return;

            customerData.PostalCode = value.Replace("-", string.Empty);

            if (!Regex.IsMatch(customerData.PostalCode, CustomerHelper.TelFaxPostNumberPermissionPattern))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     郵便番号              不正な文字が入力されているためインポートできません。");
                ErrorCount++;
            }
            else if (customerData.PostalCode.Length > 7)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     郵便番号              7文字以上のためインポートできません。");
                ErrorCount++;
            }
            else
            {
                var postalCode = customerData.PostalCode.PadLeft(7, '0');
                customerData.PostalCode = postalCode.Substring(0, 3) + "-" + postalCode.Substring(3, 4);
            }
        }

        private void Address1(Customer customerData, string value, int i, int attribute)
        {
            bool nullAddress1 = IsNullOrWhiteSpaceWithTrim(value);
            if (nullAddress1) return;

            bool overLimit = value.Length > 40;

            if (overLimit && attribute == 1)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     住所1                 40文字以上のためインポートできません。");
                ErrorCount++;
                return;
            }

            customerData.Address1 = (overLimit && attribute == 2) ? value.Substring(0, 40) : value;
        }

        private void Address2(Customer customerData, string value, int i, int attribute)
        {
            bool nullAddress2 = IsNullOrWhiteSpaceWithTrim(value);
            if (nullAddress2) return;

            bool overLimit = value.Length > 40;

            if (overLimit && attribute == 1)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     住所2                 40文字以上のためインポートできません。");
                ErrorCount++;
                return;
            }

            customerData.Address2 = (overLimit && attribute == 2) ? value.Substring(0, 40) : value;
        }

        private void Tel(Customer customerData, string value, int i, int attribute)
        {
            bool nullTel = IsNullOrWhiteSpaceWithTrim(value);
            if (nullTel) return;

            if (!Regex.IsMatch(value, CustomerHelper.TelFaxPostNumberPermissionPattern))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     TEL番号               不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            bool overLimit = value.Length > 20;

            if (overLimit && attribute == 1)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     TEL番号               20文字以上のためインポートできません。");
                ErrorCount++;
                return;
            }

            customerData.Tel = (overLimit && attribute == 2) ? value.Substring(0, 20) : value;
        }

        private void Fax(Customer customerData, string value, int i, int attribute)
        {
            bool nullFax = IsNullOrWhiteSpaceWithTrim(value);
            if (nullFax) return;

            if (!Regex.IsMatch(value, CustomerHelper.TelFaxPostNumberPermissionPattern))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     FAX番号               不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            bool overLimit = value.Length > 20;

            if (overLimit && attribute == 1)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     FAX番号               20文字以上のためインポートできません。");
                ErrorCount++;
                return;
            }

            customerData.Fax = (overLimit && attribute == 2) ? value.Substring(0, 20) : value;
        }

        private void CustomerStaffName(Customer customerData, string value, int i, int attribute)
        {
            bool nullCustomerStaffName = IsNullOrWhiteSpaceWithTrim(value);
            if (nullCustomerStaffName) return;

            bool overLimit = value.Length > 40;

            if (overLimit && attribute == 1)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     相手先担当者名        40文字以上のためインポートできません。");
                ErrorCount++;
                return;
            }

            customerData.CustomerStaffName = (overLimit && attribute == 2) ? value.Substring(0, 40) : value;
        }

        private void Note(Customer customerData, string value, int i, int attribute)
        {
            bool nullNote = IsNullOrWhiteSpaceWithTrim(value);
            if (nullNote) return;

            bool overLimit = value.Length > 100;

            if (overLimit && attribute == 1)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     備考                  100文字以上のためインポートできません。");
                ErrorCount++;
                return;
            }

            customerData.Note = (overLimit && attribute == 2) ? value.Substring(0, 100) : value;
        }

        private void UseFeeLearning(Customer customerData, string value, int i)
        {
            if (customerData.ShareTransferFee == 0) return;

            bool nullUseFeeLearning = IsNullOrWhiteSpaceWithTrim(value);
            if (nullUseFeeLearning)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     手数料自動学習        空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (!(value == "0" || value == "1"))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     手数料自動学習        不正な文字が入力されているためインポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.UseFeeLearning = TryParseOrDefault<int>(int.TryParse, value);
            }
        }

        private void SightOfBill(Customer customerData, string value, int i)
        {
            if (string.IsNullOrEmpty(customerData.CollectCategoryCode)) return;

            var limitDate = CollectCategoryList.Where(x => x.Code == customerData.CollectCategoryCode).Select(x => x.UseLimitDate).FirstOrDefault();
            if (limitDate != 1) return;

            bool nullSightOfBill = IsNullOrWhiteSpaceWithTrim(value);
            if (nullSightOfBill)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     回収サイト            空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (!Regex.IsMatch(value, CustomerHelper.DigitPermissionPattern))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     回収サイト            不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            int intValues = TryParseOrDefault<int>(int.TryParse, value);
            if (intValues < 1 || intValues > 999)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     回収サイト            不正な文字が入力されているためインポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.SightOfBill = intValues;
            }
        }

        private void DensaiCode(Customer customerData, string value, int i)
        {
            bool nullDensaiCode = IsNullOrWhiteSpaceWithTrim(value);
            if (nullDensaiCode) return;

            if (value.Length > 9)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     電子手形用企業コード  9文字以上のためインポートできません。");
                ErrorCount++;
            }
            if (!Regex.IsMatch(value, CustomerHelper.DigitAlphabetPermissionPattern))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     電子手形用企業コード  不正な文字が入力されているためインポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.DensaiCode = value;
            }
        }

        private void CreditCode(Customer customerData, string value, int i)
        {
            bool nullCreditCode = IsNullOrWhiteSpaceWithTrim(value);
            if (nullCreditCode) return;

            if (value.Length > 15)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     信用調査用企業コード  15文字以上のためインポートできません。");
                ErrorCount++;
            }
            if (!Regex.IsMatch(value, CustomerHelper.DigitAlphabetPermissionPattern))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     信用調査用企業コード  不正な文字が入力されているためインポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.CreditCode = value;
            }
        }

        private void CreditRank(Customer customerData, string value, int i)
        {
            bool nullCreditRank = IsNullOrWhiteSpaceWithTrim(value);
            if (nullCreditRank) return;

            if (value.Length > 2)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     与信ランク            2文字以上のためインポートできません。");
                ErrorCount++;
            }
            if (!Regex.IsMatch(value, CustomerHelper.DigitAlphabetPermissionPattern))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     与信ランク            不正な文字が入力されているためインポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.CreditRank = value;
            }
        }

        private void TransferBankCode(Customer customerData, string value, int i)
        {
            bool nullTransferBankCode = IsNullOrWhiteSpaceWithTrim(value);
            if (nullTransferBankCode) return;

            if (value.Length > 4)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     口座振替用銀行コード  4文字以上のためインポートできません。");
                ErrorCount++;
            }
            if (!Regex.IsMatch(value, CustomerHelper.DigitPermissionPattern))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     口座振替用銀行コード  不正な文字が入力されているためインポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.TransferBankCode = value.PadLeft(4, '0');
            }
        }

        private void TransferBankName(Customer customerData, string value, int i)
        {
            if (IsNullOrWhiteSpaceWithTrim(value)) return;

            var converted = EbDataHelper.ConvertToValidEbKana(value);
            var length = 30;
            if (converted.Length > length)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     口座振替用銀行名      30文字以上のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (!EbDataHelper.IsValidEBChars(converted))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     口座振替用銀行名      不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            customerData.TransferBankName = converted.Length > 30 ? converted.Substring(0, 30) : converted;
        }

        private void TransferBranchCode(Customer customerData, string value, int i)
        {
            bool nullTransferBranchCode = IsNullOrWhiteSpaceWithTrim(value);
            if (nullTransferBranchCode) return;

            if (value.Length > 3)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     口座振替用支店コード  3文字以上のためインポートできません。");
                ErrorCount++;
            }
            if (!Regex.IsMatch(value, CustomerHelper.DigitPermissionPattern))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     口座振替用支店コード  不正な文字が入力されているためインポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.TransferBranchCode = value.PadLeft(3, '0');
            }
        }

        private void TransferBranchName(Customer customerData, string value, int i)
        {
            if (IsNullOrWhiteSpaceWithTrim(value)) return;

            var converted = EbDataHelper.ConvertToValidEbKana(value);
            var length = 30;
            if (converted.Length > length)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     口座振替用銀行名      30文字以上のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (!EbDataHelper.IsValidEBChars(converted))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     口座振替用銀行名      不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            customerData.TransferBranchName = converted.Length > 30 ? converted.Substring(0, 30) : converted;
        }

        private void TransferAccountNumber(Customer customerData, string value, int i)
        {
            bool nullTransferAccountNumber = IsNullOrWhiteSpaceWithTrim(value);
            if (nullTransferAccountNumber) return;

            if (value.Length > 7)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     口座振替用口座番号    7文字以上のためインポートできません。");
                ErrorCount++;
            }
            if (!Regex.IsMatch(value, CustomerHelper.DigitPermissionPattern))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     口座振替用口座番号    不正な文字が入力されているためインポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.TransferAccountNumber = value.PadLeft(7, '0');
            }
        }

        private void TransferAccountTypeId(Customer customerData, string value, int i)
        {
            bool nullTransferAccountTypeId = IsNullOrWhiteSpaceWithTrim(value);
            if (nullTransferAccountTypeId) return;

            if (!Regex.IsMatch(value, "^[1239]$"))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     口座振替用預金種別    不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            customerData.TransferAccountTypeId = TryParseOrDefault<int>(int.TryParse, value);
        }

        private void TransferCustomerCode(Customer customer, string value, int i, int attribute)
        {
            if (IsNullOrWhiteSpaceWithTrim(value)) return;
            var maxLength = 20;
            value = EbDataHelper.ConvertToValidEbKana(value);
            if (attribute == 1 && value.Length > maxLength)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     口座振替用顧客コード  20文字以上のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (!EbDataHelper.IsValidEBChars(value))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     口座振替用顧客コード  不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }
            if (attribute == 2 && value.Length > maxLength) value = value.Substring(0, maxLength);
            customer.TransferCustomerCode = value;
        }

        private void TransferNewCode(Customer customer, string value, int i)
        {
            if (IsNullOrWhiteSpaceWithTrim(value)) return;

            var maxLength = 1;
            if (value.Length > maxLength)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     口座振替用新規コード  1文字以上のためインポートできません。");
                ErrorCount++;
                return;
            }
            if (!Regex.IsMatch(value, CustomerHelper.DigitPermissionPattern))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     口座振替用新規コード  不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }
            customer.TransferNewCode = value;
        }

        private void TransferAccountName(Customer customer, string value, int i, int attribute)
        {
            if (IsNullOrWhiteSpaceWithTrim(value)) return;
            var maxLength = 30;
            value = EbDataHelper.ConvertToValidEbKana(value);

            if (attribute == 1 && value.Length > maxLength)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     口座振替用預金者名    30文字以上のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (!EbDataHelper.IsValidEBChars(value))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     口座振替用預金者名    不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            if (attribute == 2 && value.Length > maxLength) value = value.Substring(0, maxLength);
            customer.TransferAccountName = value;
        }

        private void ThresholdValue(Customer customerData, string value, int i)
        {
            if (customerData.CollectCategoryCode == "00")
            {
                bool nullThresholdValue = IsNullOrWhiteSpaceWithTrim(value);

                if (nullThresholdValue)
                {
                    ErrorList.Add(string.Format("{0:00000000}", i) + "行目     約定金額              空白のためインポートできません。");
                    ErrorCount++;
                    return;
                }

                if (!Regex.IsMatch(value, CustomerHelper.DigitDecPermissionPattern))
                {
                    ErrorList.Add(string.Format("{0:00000000}", i) + "行目     約定金額              不正な文字が入力されているためインポートできません。");
                    ErrorCount++;
                    return;
                }

                decimal decValues = TryParseOrDefault<decimal>(decimal.TryParse, value);
                if (decValues < 0 || decValues > 99999999999)
                {
                    ErrorList.Add(string.Format("{0:00000000}", i) + "行目     約定金額              不正な文字が入力されているためインポートできません。");
                    ErrorCount++;
                    return;
                }

                if (value.Contains('.') && roundingType == RoundingType.Error)
                {
                    ErrorCount++;
                    ErrorList.Add(string.Format("{0:00000000}", i) + "行目     約定金額              小数が含まれているためインポートできません。");
                }
                else
                {
                    customerData.ThresholdValue = decValues;
                }
            }
        }

        private void LessThanCollectCategoryId(Customer customerData, string value, int i, int attribute)
        {
            if (customerData.CollectCategoryCode != "00") return;
            if (IsNullOrWhiteSpaceWithTrim(value))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     約定金額未満          空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            var category = GetCategory(value, attribute);
            if (category == null)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     約定金額未満          存在しないため、インポートできません。");
                ErrorCount++;
                return;
            }
            if (category.Code == "00")
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     約定金額未満          約定となっているため、インポートできません。");
                ErrorCount++;
                return;
            }
            if (category.UseLimitDate == 1)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     約定金額未満          期日ありのため、インポートできません。");
                ErrorCount++;
                return;
            }
            customerData.LessThanCollectCategoryId = category.Id;
            customerData.LessThanCollectCategoryCode = category.Code;

        }

        private void GreaterThanCollectCategoryId1(Customer customerData, string value, int i, int attribute)
        {
            if (customerData.CollectCategoryCode != "00") return;
            if (IsNullOrWhiteSpaceWithTrim(value))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     約定金額以上1         空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            var category = GetCategory(value, attribute);
            if (category == null)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     約定金額以上1         存在しないため、インポートできません。");
                ErrorCount++;
                return;
            }

            if (category.Code == "00")
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     約定金額以上1         約定となっているため、インポートできません。");
                ErrorCount++;
                return;
            }

            customerData.GreaterThanCollectCategoryId1 = category.Id;
            customerData.GreaterThanCollectCategoryCode1 = category.Code;

        }

        private void GreaterThanRate1(Customer customerData, string value, int i)
        {
            if (string.IsNullOrWhiteSpace(customerData.GreaterThanCollectCategoryCode1)) return;

            bool nullGreaterThanRate1 = IsNullOrWhiteSpaceWithTrim(value);

            if (nullGreaterThanRate1)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     分割1                 空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (!Regex.IsMatch(value, CustomerHelper.DigitDecPermissionPattern))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     分割1                 不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            decimal decValues = TryParseOrDefault<decimal>(decimal.TryParse, value);
            if (decValues < 0.1M || decValues > 100M)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     分割1                 不正な文字が入力されているためインポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.GreaterThanRate1 = decValues;
            }
        }

        private void GreaterThanRoundingMode1(Customer customerData, string value, int i)
        {
            if (string.IsNullOrWhiteSpace(customerData.GreaterThanCollectCategoryCode1)) return;

            bool nullGreaterThanRoundingMode1 = IsNullOrWhiteSpaceWithTrim(value);

            if (nullGreaterThanRoundingMode1)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     端数1                 空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (!Regex.IsMatch(value, CustomerHelper.DigitPermissionPattern))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     端数1                 不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            int intValues = TryParseOrDefault<int>(int.TryParse, value);
            if (intValues < 0 || intValues > 6)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     端数1                 不正な文字が入力されているためインポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.GreaterThanRoundingMode1 = intValues;
            }
        }

        private void GreaterThanSightOfBill1(Customer customerData, string value, int i)
        {
            if (string.IsNullOrWhiteSpace(customerData.GreaterThanCollectCategoryCode1)) return;

            var limitDate = CollectCategoryList.Where(x => x.Code == customerData.GreaterThanCollectCategoryCode1).Select(x => x.UseLimitDate).FirstOrDefault();

            if (limitDate != 1) return;

            bool nullGreaterThanSightOfBill1 = IsNullOrWhiteSpaceWithTrim(value);

            if (nullGreaterThanSightOfBill1)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     回収サイト1           空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (!Regex.IsMatch(value, CustomerHelper.DigitPermissionPattern))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     回収サイト1           不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            int intValues = TryParseOrDefault<int>(int.TryParse, value);
            if (intValues < 1 || intValues > 999)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     回収サイト1           不正な文字が入力されているためインポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.GreaterThanSightOfBill1 = intValues;
            }
        }

        private void GreaterThanCollectCategoryId2(Customer customerData, string value, int i, int attribute)
        {
            if (string.IsNullOrWhiteSpace(customerData.GreaterThanCollectCategoryCode1)) return;

            if (IsNullOrWhiteSpaceWithTrim(value)) return;

            var category = GetCategory(value, attribute);
            if (category == null)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     約定金額以上2         存在しないため、インポートできません。");
                ErrorCount++;
                return;
            }
            if (category.Code == "00")
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     約定金額以上2         約定となっているため、インポートできません。");
                ErrorCount++;
                return;
            }
            customerData.GreaterThanCollectCategoryId2 = category.Id;
            customerData.GreaterThanCollectCategoryCode2 = category.Code;
        }

        private void GreaterThanRate2(Customer customerData, string value, int i)
        {
            if (string.IsNullOrWhiteSpace(customerData.GreaterThanCollectCategoryCode2)) return;

            bool nullGreaterThanRate2 = IsNullOrWhiteSpaceWithTrim(value);

            if (nullGreaterThanRate2)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     分割2                 空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (!Regex.IsMatch(value, CustomerHelper.DigitDecPermissionPattern))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     分割2                 不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            decimal decValues = TryParseOrDefault<decimal>(decimal.TryParse, value);
            if (decValues < 0.1M || decValues > 100M)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     分割2                 不正な文字が入力されているためインポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.GreaterThanRate2 = decValues;
            }
        }

        private void GreaterThanRoundingMode2(Customer customerData, string value, int i)
        {
            if (string.IsNullOrWhiteSpace(customerData.GreaterThanCollectCategoryCode2)) return;

            bool nullGreaterThanRoundingMode2 = IsNullOrWhiteSpaceWithTrim(value);

            if (nullGreaterThanRoundingMode2)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     端数2                 空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (!Regex.IsMatch(value, CustomerHelper.DigitPermissionPattern))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     端数2                 不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            int intValues = TryParseOrDefault<int>(int.TryParse, value);
            if (intValues < 0 || intValues > 6)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     端数2                 不正な文字が入力されているためインポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.GreaterThanRoundingMode2 = intValues;
            }
        }

        private void GreaterThanSightOfBill2(Customer customerData, string value, int i)
        {
            if (string.IsNullOrWhiteSpace(customerData.GreaterThanCollectCategoryCode2)) return;

            var limitDate = CollectCategoryList.Where(x => x.Code == customerData.GreaterThanCollectCategoryCode2).Select(x => x.UseLimitDate).FirstOrDefault();

            if (limitDate != 1) return;

            bool nullGreaterThanSightOfBill2 = IsNullOrWhiteSpaceWithTrim(value);

            if (nullGreaterThanSightOfBill2)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     回収サイト2           空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (!Regex.IsMatch(value, CustomerHelper.DigitPermissionPattern))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     回収サイト2           不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            int intValues = TryParseOrDefault<int>(int.TryParse, value);
            if (intValues < 1 || intValues > 999)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     回収サイト2           不正な文字が入力されているためインポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.GreaterThanSightOfBill2 = intValues;
            }
        }

        private void GreaterThanCollectCategoryId3(Customer customerData, string value, int i, int attribute)
        {
            if (string.IsNullOrWhiteSpace(customerData.GreaterThanCollectCategoryCode2)) return;

            if (IsNullOrWhiteSpaceWithTrim(value)) return;

            var category = GetCategory(value, attribute);
            if (category == null)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     約定金額以上3         存在しないため、インポートできません。");
                ErrorCount++;
                return;
            }

            if (category.Code == "00")
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     約定金額以上3         約定となっているため、インポートできません。");
                ErrorCount++;
                return;
            }
            customerData.GreaterThanCollectCategoryId3 = category.Id;
            customerData.GreaterThanCollectCategoryCode3 = category.Code;
        }

        private void GreaterThanRate3(Customer customerData, string value, int i)
        {
            if (string.IsNullOrWhiteSpace(customerData.GreaterThanCollectCategoryCode3)) return;

            bool nullGreaterThanRate3 = IsNullOrWhiteSpaceWithTrim(value);
            if (nullGreaterThanRate3)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     分割3                 空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (!Regex.IsMatch(value, CustomerHelper.DigitDecPermissionPattern))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     分割3                 不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            decimal decValues = TryParseOrDefault<decimal>(decimal.TryParse, value);
            if (decValues < 0.1M || decValues > 100M)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     分割3                 不正な文字が入力されているためインポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.GreaterThanRate3 = decValues;
            }
        }

        private void GreaterThanRoundingMode3(Customer customerData, string value, int i)
        {
            if (string.IsNullOrWhiteSpace(customerData.GreaterThanCollectCategoryCode3)) return;

            bool nullGreaterThanRoundingMode3 = IsNullOrWhiteSpaceWithTrim(value);

            if (nullGreaterThanRoundingMode3)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     端数3                 空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (!Regex.IsMatch(value, CustomerHelper.DigitPermissionPattern))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     端数3                 不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            int intValues = TryParseOrDefault<int>(int.TryParse, value);
            if (intValues < 0 || intValues > 6)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     端数3                 不正な文字が入力されているためインポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.GreaterThanRoundingMode3 = intValues;
            }
        }

        private void GreaterThanSightOfBill3(Customer customerData, string value, int i)
        {
            if (string.IsNullOrWhiteSpace(customerData.GreaterThanCollectCategoryCode3)) return;

            var limitDate = CollectCategoryList.Where(x => x.Code == customerData.GreaterThanCollectCategoryCode3).Select(x => x.UseLimitDate).FirstOrDefault();

            if (limitDate != 1) return;

            bool nullGreaterThanSightOfBill3 = IsNullOrWhiteSpaceWithTrim(value);

            if (nullGreaterThanSightOfBill3)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     回収サイト3           空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (!Regex.IsMatch(value, CustomerHelper.DigitPermissionPattern))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     回収サイト3           不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            int intValues = TryParseOrDefault<int>(int.TryParse, value);
            if (intValues < 1 || intValues > 999)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     回収サイト3           不正な文字が入力されているためインポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.GreaterThanSightOfBill3 = intValues;
            }
        }

        private void UseKanaLearning(Customer customerData, string value, int i)
        {
            bool nullUseKanaLearning = IsNullOrWhiteSpaceWithTrim(value);

            if (nullUseKanaLearning)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     カナ自動学習          空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (value != "1" && value != "0")
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     カナ自動学習          不正な文字が入力されているためインポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.UseKanaLearning = TryParseOrDefault<int>(int.TryParse, value);
            }
        }

        private void HolidayFlag(Customer customerData, string value, int i)
        {
            bool nullHolidayFlag = IsNullOrWhiteSpaceWithTrim(value);

            if (nullHolidayFlag)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     休業日の設定          空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (value != "0" && value != "1" && value != "2")
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     休業日の設定          不正な文字が入力されているためインポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.HolidayFlag = TryParseOrDefault<int>(int.TryParse, value);
            }
        }

        private void UseFeeTolerance(Customer customerData, string value, int i)
        {
            if (customerData.ShareTransferFee != 1) return;

            bool nullUseFeeTolerance = IsNullOrWhiteSpaceWithTrim(value);
            if (nullUseFeeTolerance)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     手数料誤差利用        空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (value != "0" && value != "1")
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     手数料誤差利用        不正な文字が入力されているためインポートできません。");
                ErrorCount++;
            }
            else
            {
                customerData.UseFeeTolerance = TryParseOrDefault<int>(int.TryParse, value);
            }
        }

        private void PrioritizeMatchingIndividually(Customer customer, string value, int i)
        {
            if (IsNullOrWhiteSpaceWithTrim(value))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     一括消込対象外        空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (!Regex.IsMatch(value, "^[01]$"))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     一括消込対象外        不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            customer.PrioritizeMatchingIndividually = TryParseOrDefault<int>(int.TryParse, value);
        }

        private void CollationKey(Customer customer, string value, int i, int attribute)
        {
            if (IsNullOrWhiteSpaceWithTrim(value)) return;
            var maxLength = 48;
            if (attribute == 1 && value.Length > maxLength)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     照合番号              48文字以上のためインポートできません。");
                ErrorCount++;
                return;
            }
            if (!Regex.IsMatch(value, "^[0-9]+$"))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     照合番号              不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }
            if (attribute == 2 && value.Length > maxLength) value = value.Substring(0, maxLength);
            customer.CollationKey = value;
        }

        private void ExcludeInvoicePublish(Customer customer, string value, int i)
        {
            if (IsNullOrWhiteSpaceWithTrim(value))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     請求書発行対象外        空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (!Regex.IsMatch(value, "^[01]$"))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     請求書発行対象外        不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            customer.ExcludeInvoicePublish = TryParseOrDefault<int>(int.TryParse, value);
        }

        private void ExcludeReminderPublish(Customer customer, string value, int i)
        {
            if (IsNullOrWhiteSpaceWithTrim(value))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     督促状発行対象外        空白のためインポートできません。");
                ErrorCount++;
                return;
            }

            if (!Regex.IsMatch(value, "^[01]$"))
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     督促状発行対象外        不正な文字が入力されているためインポートできません。");
                ErrorCount++;
                return;
            }

            customer.ExcludeReminderPublish = TryParseOrDefault<int>(int.TryParse, value);
        }

        private void DestinationDepartmentName(Customer customerData, string value, int i, int attribute)
        {
            bool nullDestinationDepartmentName = IsNullOrWhiteSpaceWithTrim(value);
            if (nullDestinationDepartmentName) return;

            bool overLimit = value.Length > 40;

            if (overLimit && attribute == 1)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     相手先部署           40文字以上のためインポートできません。");
                ErrorCount++;
                return;
            }

            customerData.DestinationDepartmentName = (overLimit && attribute == 2) ? value.Substring(0, 40) : value;
        }

        private void Honorific(Customer customerData, string value, int i, int attribute)
        {
            bool nullHonorific = IsNullOrWhiteSpaceWithTrim(value);
            if (nullHonorific) return;

            bool overLimit = value.Length > 6;

            if (overLimit && attribute == 1)
            {
                ErrorList.Add(string.Format("{0:00000000}", i) + "行目     敬称                 6文字以上のためインポートできません。");
                ErrorCount++;
                return;
            }

            customerData.Honorific = (overLimit && attribute == 2) ? value.Substring(0, 6) : value;
        }

        private Category GetCategory(string value, int attribute)
        {
            if (attribute == 1) value = value.PadLeft(2, '0');
            Func<Category, string> keySelector = x => x.Code;
            if (attribute == 2) keySelector = x => x.ExternalCode;
            return CollectCategoryList.FirstOrDefault(x => keySelector(x) == value);
        }

        /// <summary>
        /// 都度請求 可能
        /// </summary>
        private bool CanDueAtPerBilling
        {
            get
            {
                return ImporterSettingDetails?
                    .Any(x => x.Sequence == (int)Fields.CollectOffsetDayPerBilling
                    && x.ImportDivision != 0) ?? false;
            }
        }
        /// <summary>
        /// 通常 入金予定日算出可能
        /// </summary>
        private bool CanDueAtNormal
        {
            get
            {
                return ImporterSettingDetails?
                    .Where(x => x.Sequence == (int)Fields.CollectOffsetMonth
                             || x.Sequence == (int)Fields.CollectOffsetDay)
                    .Any(x => x.ImportDivision != 0) ?? false;
            }
        }
        private int ClosingDayMinValue
        { get { return CanDueAtPerBilling ? 0 : 1; } }
        private int ClosingDayMaxValue
        { get { return CanDueAtNormal ? LastDayOfMonth : 0; } }
        #endregion

        #region delegates for get db items

        public Func<Task<RoundingType>> GetRoundingTypeAsync { get; set; }

        public Func<Task<List<Category>>> GetCollectCategoryAsync { get; set; }

        public Func<Task<List<Staff>>> GetStaffAsync { get; set; }

        public Func<Task<List<Customer>>> GetCustomerAsync { get; set; }
        public Func<Task<IEnumerable<string>>> GetLeagalPersonaritiesAsync { get; set; }

        /// <summary>インポート設定ヘッダー取得
        /// int : formatId
        /// string : code
        /// </summary>
        public Func<int, string, Task<ImporterSetting>> GetImporterSettingAsync { get; set; }

        /// <summary>インポート設定明細取得
        /// int : formatId
        /// string : code
        /// </summary>
        public Func<int, string, Task<List<ImporterSettingDetail>>> GetImporterSettingDetailAsync { get; set; }


        public Func<string[], Task<List<MasterData>>> GetMasterDataForCustomerGroupParentAsync { get; set; }

        public Func<string[], Task<List<MasterData>>> GetMasterDataForCustomerGroupChildAsync { get; set; }

        public Func<string[], Task<List<MasterData>>> GetMasterDataForKanaHistoryAsync { get; set; }

        public Func<string[], Task<List<MasterData>>> GetMasterDataForBillingAsync { get; set; }

        public Func<string[], Task<List<MasterData>>> GetMasterDataForReceiptAsync { get; set; }

        public Func<string[], Task<List<MasterData>>> GetMasterDataForNettingAsync { get; set; }

        #endregion

    }
}
