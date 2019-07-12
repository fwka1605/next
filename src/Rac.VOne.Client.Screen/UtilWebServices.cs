using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Common;
using System.Data;

namespace Rac.VOne.Client.Screen
{
    public static partial class Util
    {
        /// <summary>
        /// マスターインポート用設定取得
        /// </summary>
        /// <param name="login"></param>
        /// <param name="fileType"><see cref="Import.ImportFileType"/>の定数を利用</param>
        /// <returns></returns>
        public static async Task<ImportSetting> GetMasterImportSettingAsync(ILogin login, int fileType)
            => await ServiceProxyFactory.DoAsync(async (ImportSettingMasterService.ImportSettingMasterClient client) =>
                {
                    var result = await client.GetAsync(login.SessionKey, login.CompanyId, fileType);
                    if (result.ProcessResult.Result)
                        return result.ImportSetting;
                    return null;
                });

        public static async Task<GeneralSetting> GetGeneralSettingAsync(ILogin login, string code)
            => await ServiceProxyFactory.DoAsync(async (GeneralSettingMasterService.GeneralSettingMasterClient client) =>
                {
                    var result = await client.GetByCodeAsync(login.SessionKey, login.CompanyId, code);
                    if (result.ProcessResult.Result)
                        return result.GeneralSetting;
                    return null;
                });

        public static async Task<string> GetGeneralSettingServerPathAsync(ILogin login)
            => await GetGeneralSettingAsync(login, "サーバパス")
            .ContinueWith(x => x.Result?.Value);

        public static async Task<string> GetGeneralSettingFractionRoundingModeAsync(ILogin login)
            => await GetGeneralSettingAsync(login, "取込時端数処理").ContinueWith(x => x.Result?.Value);

        public static async Task<List<TaxClass>> GetTaxClassAsync(ILogin login)
            => await ServiceProxyFactory.DoAsync(async (TaxClassMasterService.TaxClassMasterClient client) =>
                {
                    var result = await client.GetItemsAsync(login.SessionKey);
                    if (result.ProcessResult.Result)
                        return result.TaxClass;
                    return new List<TaxClass>();
                });

        public static async Task<List<Currency>> GetCurrenciesAsync(ILogin login, string[] codes)
            => await ServiceProxyFactory.DoAsync(async (CurrencyMasterService.CurrencyMasterClient client) =>
            {
                var result = await client.GetByCodeAsync(login.SessionKey, login.CompanyId, codes);
                if (result.ProcessResult.Result)
                    return result.Currencies;
                return new List<Currency>();
            });
        public static async Task<IEnumerable<string>> GetLegalPersonaritiesAsync(ILogin login)
            => await ServiceProxyFactory.DoAsync(async (JuridicalPersonalityMasterService.JuridicalPersonalityMasterClient client) =>
            {
                var result = await client.GetItemsAsync(login.SessionKey, login.CompanyId);
                if (result.ProcessResult.Result)
                    return result.JuridicalPersonalities.Select(x => x.Kana);
                return null;
            });

        public static async Task<List<Category>> GetCategoriesByIdsAsync(ILogin login, int[] ids)
            => await ServiceProxyFactory.DoAsync(async (CategoryMasterService.CategoryMasterClient client) =>
            {
                var result = await client.GetAsync(login.SessionKey, ids);
                if (result.ProcessResult.Result)
                    return result.Categories;
                return new List<Category>();
            });
        public static async Task<List<Category>> GetCategoriesByCodesAsync(ILogin login, int type, string[] codes)
            => await ServiceProxyFactory.DoAsync(async (CategoryMasterService.CategoryMasterClient client) =>
            {
                var result = await client.GetByCodeAsync(login.SessionKey, login.CompanyId, type, codes);
                if (result.ProcessResult.Result)
                    return result.Categories;
                return new List<Category>();
            });

        public static async Task<List<Staff>> GetStaffByIdsAsync(ILogin login, int[] ids)
            => await ServiceProxyFactory.DoAsync(async (StaffMasterService.StaffMasterClient client) =>
            {
                var result = await client.GetAsync(login.SessionKey, ids);
                if (result.ProcessResult.Result)
                    return result.Staffs;
                return new List<Staff>();
            });

        public static async Task<List<Staff>> GetStaffByCodesAsync(ILogin login, string[] codes)
            => await ServiceProxyFactory.DoAsync(async (StaffMasterService.StaffMasterClient client) =>
            {
                var result = await client.GetByCodeAsync(login.SessionKey, login.CompanyId, codes);
                if (result.ProcessResult.Result)
                    return result.Staffs;
                return new List<Staff>();
            });

        public static async Task<List<Customer>> GetCustomerByCodesAsync(ILogin login, string[] codes)
            => await ServiceProxyFactory.DoAsync(async (CustomerMasterService.CustomerMasterClient client) =>
            {
                var result = await client.GetByCodeAsync(login.SessionKey, login.CompanyId, codes);
                if (result.ProcessResult.Result)
                    return result.Customers;
                return new List<Customer>();
            });
        public static async Task<List<Customer>> GetCustomerAsync(ILogin login)
            => await ServiceProxyFactory.DoAsync(async (CustomerMasterService.CustomerMasterClient client) =>
            {
                var result = await client.GetItemsAsync(login.SessionKey, login.CompanyId, null);
                if (result.ProcessResult.Result)
                    return result.Customers;
                return new List<Customer>();
            });

        public static async Task<List<Department>> GetDepartmentByCodesAsync(ILogin login, string[] codes)
            => await ServiceProxyFactory.DoAsync(async (DepartmentMasterService.DepartmentMasterClient client) =>
            {
                var result = await client.GetByCodeAsync(login.SessionKey, login.CompanyId, codes);
                if (result.ProcessResult.Result)
                    return result.Departments;
                return new List<Department>();
            });

        public static async Task<List<Department>> GetDepartmentByIdsAsync(ILogin login, int[] ids)
            => await ServiceProxyFactory.DoAsync(async (DepartmentMasterService.DepartmentMasterClient client) =>
            {
                var result = await client.GetAsync(login.SessionKey, ids);
                if (result.ProcessResult.Result)
                    return result.Departments;
                return new List<Department>();
            });

        public static async Task<List<Department>> GetDepartmentByLoginUserIdAsync(ILogin login)
            => await ServiceProxyFactory.DoAsync(async (DepartmentMasterService.DepartmentMasterClient client) =>
            {
                var result = await client.GetByLoginUserIdAsync(login.SessionKey, login.CompanyId, login.UserId);
                if (result.ProcessResult.Result)
                    return result.Departments;
                return new List<Department>();
            });

        public static async Task<List<Section>> GetSectionByCodesAsync(ILogin login, string[] codes)
            => await ServiceProxyFactory.DoAsync(async (SectionMasterService.SectionMasterClient client) =>
            {
                var result = await client.GetByCodeAsync(login.SessionKey, login.CompanyId, codes);
                if (result.ProcessResult.Result)
                    return result.Sections;
                return new List<Section>();
            });

        public static async Task<List<Section>> GetSectionByLoginUserIdAsync(ILogin login)
            => await ServiceProxyFactory.DoAsync(async (SectionMasterService.SectionMasterClient client) =>
            {
                var result = await client.GetByLoginUserIdAsync(login.SessionKey, login.UserId);
                if (result.ProcessResult.Result)
                    return result.Sections;
                return new List<Section>();
            });

        public static async Task<List<AccountTitle>> GetAccountTitleByCodesAsync(ILogin login, string[] codes)
            => await ServiceProxyFactory.DoAsync(async (AccountTitleMasterService.AccountTitleMasterClient client) =>
            {
                var result = await client.GetByCodeAsync(login.SessionKey, login.CompanyId, codes);
                if (result.ProcessResult.Result)
                    return result.AccountTitles;
                return new List<AccountTitle>();
            });

        public static async Task<List<HolidayCalendar>> GetHolidayCalendarsAsync(ILogin login, HolidayCalendarSearch option = null)
            => await ServiceProxyFactory.DoAsync(async (HolidayCalendarMasterService.HolidayCalendarMasterClient client) =>
            {
                var result = await client.GetItemsAsync(login.SessionKey, option ?? new HolidayCalendarSearch { CompanyId = login.CompanyId });
                if (result.ProcessResult.Result)
                    return result.HolidayCalendars;
                return new List<HolidayCalendar>();
            });

        public static async Task<List<BillingDivisionContract>> GetBillingDivisionContractByCustomerIdsAsync(ILogin login, int[] ids)
            => await ServiceProxyFactory.DoAsync(async (BillingDivisionContractMasterService.BillingDivisionContractMasterClient client) =>
            {
                var result = await client.GetByCustomerIdsAsync(login.SessionKey, login.CompanyId, ids);
                if (result.ProcessResult.Result)
                    return result.BillingDivisionContracts;
                return new List<BillingDivisionContract>();
            });

        public static async Task LoadColumnNameSettingAsync(ILogin login, string tableName, Action<IEnumerable<ColumnNameSetting>> setter)
            => await ServiceProxyFactory.DoAsync(async (ColumnNameSettingMasterService.ColumnNameSettingMasterClient client) =>
            {
                var result = await client.GetItemsAsync(login.SessionKey, login.CompanyId);
                if (result?.ProcessResult.Result ?? false)
                {
                    setter?.Invoke(result.ColumnNames.Where(x => x.TableName == tableName));
                }
            });

        public static async Task<CollationSetting> GetCollationSettingAsync(ILogin login)
            => await ServiceProxyFactory.DoAsync(async (CollationSettingMasterService.CollationSettingMasterClient client) =>
            {
                var result = await client.GetAsync(login.SessionKey, login.CompanyId);
                if (result?.ProcessResult.Result ?? false)
                    return result.CollationSetting;
                return new CollationSetting();
            });

        public static async Task<bool> IsControlInputNoteAsync(ILogin login)
            => await ServiceProxyFactory.DoAsync(async (InvoiceSettingService.InvoiceSettingServiceClient client) =>
            {
                var result = await client.GetInvoiceCommonSettingAsync(login.SessionKey, login.CompanyId);
                if (result.ProcessResult.Result)
                    return result.InvoiceCommonSetting == null ? false :
                            result.InvoiceCommonSetting.ControlInputCharacter == 1;
                else
                    return false;
            });

        /// <summary>
        /// セッションキー取得 認証 共通処理
        /// </summary>
        /// <param name="key">認証キー</param>
        /// <param name="code">認証会社コード</param>
        /// <param name="retryCount"></param>
        /// <returns></returns>
        public static string Authenticate(string key, string code, int retryCount = 2)
        {
            System.Diagnostics.Debug.Assert(1 <= retryCount && retryCount < 10);
            return AuthenticateRetry(key, code, retryCount);
        }

        private static string AuthenticateRetry(string key, string code, int retryCount)
        {
            string sessionKey = null;
            var args = GetAuthenticationArgs(key, code);
            AuthenticationResult result = null;
            try
            {
                var count = 0;
                do
                {
                    Exception error = null;
                    count++;
                    try
                    {
                        result = new AuthenticationResult(AuthenticateInner(args));
                    }
                    catch (Exception ex)
                    {
                        error = ex;
                        if (count > 1)
                            NLogHandler.WriteErrorLog(typeof(Util), ex, null);
                    }

                    if (result?.Success ?? false)
                    {
                        sessionKey = result.SessionKey;
                        break;
                    }
                    else if (result != null)
                    {
                        NLogHandler.WriteErrorLog(typeof(Util), result.ErrorMessage);
                    }

                } while (count <= retryCount);
            }
            finally
            {
                args?.Dispose();
            }
            return sessionKey;
        }

        /// <summary>認証処理(Authentication.svc:Anthenticate)の結果情報を格納する。</summary>
        private class AuthenticationResult
        {
            /// <summary>ProcessResult.Result</summary>
            public readonly bool Success;

            /// <summary>セッションキー(認証成功時)</summary>
            public readonly string SessionKey;

            /// <summary>ProcessResult.ErrorCode</summary>
            public readonly string ErrorCode = "0";

            /// <summary>ProcessResult.ErrorMessage</summary>
            public readonly string ErrorMessage = "";

            public AuthenticationResult(DataSet svcResult)
            {
                if (svcResult == null
                    || svcResult.Tables.Count == 0
                    || svcResult.Tables[0].Rows.Count == 0)
                {
                    return; // Result = false, others = null
                }
                var row = svcResult.Tables[0].Rows[0];

                Success = row.Field<bool>("Result");
                if (Success)
                {
                    SessionKey = row.Field<string>("SessionKey");
                }
                else
                {
                    ErrorCode = row.Field<string>("ErrorCode");
                    ErrorMessage = row.Field<string>("ErrorMessage");
                }
            }
        }

        /// <summary>認証処理</summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static DataSet AuthenticateInner(DataSet args)
            => ServiceProxyFactory.Do((AuthenticationService.AuthenticationClient client)
                => client.Authenticate(args));

        private static DataSet GetAuthenticationArgs(string key, string code)
        {
            var dset = new DataSet();
            var table = new DataTable();
            table.Columns.Add("AuthenticationKey");
            table.Columns.Add("CompanyCode");
            var row = table.NewRow();
            row.SetField("AuthenticationKey", key);
            row.SetField("CompanyCode", code);
            table.Rows.Add(row);
            dset.Tables.Add(table);
            return dset;
        }


        public static Company GetCompany(ILogin login, string companyCode)
            => ServiceProxyFactory.Do((CompanyMasterService.CompanyMasterClient client) =>
            {
                var result = client.GetByCode(login.SessionKey, companyCode);
                if (result.ProcessResult.Result)
                    return result.Company;
                return null;
            });

        public static ApplicationControl GetApplicationControl(ILogin login)
            => ServiceProxyFactory.Do((ApplicationControlMasterService.ApplicationControlMasterClient client) =>
            {
                var result = client.Get(login.SessionKey, login.CompanyId);
                if (result.ProcessResult.Result)
                    return result.ApplicationControl;
                return null;
            });

        public static List<TaskSchedule> GetTaskSchedule(ILogin login)
            => ServiceProxyFactory.Do((TaskScheduleMasterService.TaskScheduleMasterClient client) =>
            {
                var result = client.GetItems(login.SessionKey, login.CompanyId);
                if (result.ProcessResult.Result)
                    return result.TaskSchedules;
                return new List<TaskSchedule>();
            });

        public static TaskScheduleHistoryResult SaveTaskScheduleHistory(ILogin login, TaskScheduleHistory history)
            => ServiceProxyFactory.Do((TaskScheduleHistoryService.TaskScheduleHistoryServiceClient client) =>
            {
                var result = client.Save(login.SessionKey, history);
                return result;
            });

        public static EBFileSetting GetEBFileSetting(ILogin login, int id)
            => ServiceProxyFactory.Do((EBFileSettingMasterService.EBFileSettingMasterClient client) => {
                var result = client.GetItem(login.SessionKey, id);
                if (result.ProcessResult.Result)
                    return result.EBFileSetting;
                return null;
            });

        public static async Task SaveWorkDepartmentTargetAsync(ILogin login, byte[] ClientKey, int[] DepartmentIds)
            => await ServiceProxyFactory.DoAsync(async (MatchingService.MatchingServiceClient client)
                => await client.SaveWorkDepartmentTargetAsync(login.SessionKey, login.CompanyId, ClientKey, DepartmentIds));

        public static async Task SaveWorkSectionTargetAsync(ILogin login, byte[] ClientKey, int[] SectionIds)
            => await ServiceProxyFactory.DoAsync(async (MatchingService.MatchingServiceClient client)
                => await client.SaveWorkSectionTargetAsync(login.SessionKey, login.CompanyId, ClientKey, SectionIds));

        public static async Task<byte[]> CreateClientKey(ILogin login, string programId)
            => await ServiceProxyFactory.DoAsync(async (DBService.DBServiceClient client) =>
            {
                var result = await client.GetClientKeyAsync(login.SessionKey, programId, GetRemoteHostName(), login.CompanyCode, login.UserCode);
                if (result.ProcessResult.Result)
                    return result.ClientKey;
                return null;
            });

        public static List<LoginUserLicense> GetLoginUserLicenses(ILogin login, int? companyId = null)
           => ServiceProxyFactory.Do( (LoginUserLicenseMasterService.LoginUserLicenseMasterClient client) =>
           {
               var result =  client.GetItems(login.SessionKey, companyId ?? login.CompanyId);
               if (result == null || result.ProcessResult.Result == false) return null;
               return result.LoginUserLicenses;
           });

    }
}
