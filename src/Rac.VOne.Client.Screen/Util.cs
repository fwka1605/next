using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Common;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.IO.Compression;

namespace Rac.VOne.Client.Screen
{
    public static partial class Util
    {

        #region import 関連

        public static Dictionary<string, TModel> ConvertToDictionary<TModel>(IEnumerable<TModel> entities,
            Func<TModel, string> keySelector)
            => entities.ToDictionary(x => keySelector(x)) ?? new Dictionary<string, TModel>();

        public static List<Customer> GetCustomerList(ILogin login, IEnumerable<string> codes)
            => ServiceProxyFactory.Do((CustomerMasterService.CustomerMasterClient client) =>
            {
                var result = client.GetByCode(login.SessionKey, login.CompanyId, codes.ToArray());
                if (result.ProcessResult.Result)
                    return result.Customers;
                return new List<Customer>();
            });

        public static async Task<List<CustomerGroup>> GetCustomerGroupListAsync(ILogin login)
            => await ServiceProxyFactory.DoAsync(async (CustomerGroupMasterService.CustomerGroupMasterClient client) =>
            {
                var result = await client.GetItemsAsync(login.SessionKey, login.CompanyId);
                if (result.ProcessResult.Result)
                    return result.CustomerGroups;
                return new List<CustomerGroup>();
            });

        public static async Task<ImportResult> ImportCustomerGroupAsync(ILogin login,
            UnitOfWork<CustomerGroup> imported)
            => await ServiceProxyFactory.DoAsync(async (CustomerGroupMasterService.CustomerGroupMasterClient client) =>
            {
                var result = await client.ImportAsync(login.SessionKey,
                    imported.New.ToArray(), imported.Dirty.ToArray(), imported.Removed.ToArray());
                if (result.ProcessResult.Result) return result;
                return null;
            });

        public static async Task<ImporterSetting> GetImporterSettingAsync(ILogin login, int formatId, int importerSettingId)
            => await ServiceProxyFactory.DoAsync(async (ImporterSettingService.ImporterSettingServiceClient client) =>
            {
                var result = await client.GetHeaderAsync(login.SessionKey, login.CompanyId, formatId);
                return result?.ImporterSettings.FirstOrDefault(x => x.Id == importerSettingId) ?? new ImporterSetting();
            });

        public static async Task<List<ImporterSettingDetail>> GetImporterSettingDetailByIdAsync(ILogin login, int importerSettingId)
            => await ServiceProxyFactory.DoAsync(async (ImporterSettingService.ImporterSettingServiceClient client) =>
            {
                var result = await client.GetDetailByIdAsync(login.SessionKey, importerSettingId);
                if (result?.ProcessResult.Result ?? false)
                    return result.ImporterSettingDetails;
                return new List<ImporterSettingDetail>();
            });

        public static async Task<ImporterSetting> GetImporterSettingAsync(ILogin login, int formatId, string code)
            => await ServiceProxyFactory.DoAsync(async (ImporterSettingService.ImporterSettingServiceClient client) =>
            {
                var result = await client.GetHeaderByCodeAsync(login.SessionKey, login.CompanyId, formatId, code);
                if (result?.ProcessResult.Result ?? false)
                    return result.ImporterSetting;
                return new ImporterSetting();
            });

        public static async Task<List<ImporterSettingDetail>> GetImporterSettingDetailByCodeAsync(ILogin login, int formatId, string code)
            => await ServiceProxyFactory.DoAsync(async (ImporterSettingService.ImporterSettingServiceClient client) =>
            {
                var result = await client.GetDetailByCodeAsync(login.SessionKey, login.CompanyId, formatId, code);
                if (result?.ProcessResult.Result ?? false)
                    return result.ImporterSettingDetails;
                return new List<ImporterSettingDetail>();
            });

        public static async Task<List<Setting>> GetSettingAsync(ILogin login, string[] itemIds = null)
            => await ServiceProxyFactory.DoAsync(async (SettingMasterService.SettingMasterClient client) =>
            {
                var result = await client.GetItemsAsync(login.SessionKey, itemIds);
                if (result?.ProcessResult.Result ?? false)
                    return result.Settings;
                return new List<Setting>();
            });

        #endregion

        #region IO.Path 関連

        /// <summary>
        /// 入力した <see cref="filePath"/> から、ディレクトリのパスを返す
        /// 直接ディレクトリを入力してある場合は、そのまま
        /// ファイル名を指定してある場合は、その直上のディレクトリのパス
        /// 入力したパスに 不正な文字などあった場合は DeskTop のパスを返す
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetDirectoryName(string filePath)
        {
            var dir = string.Empty;
            if (string.IsNullOrWhiteSpace(filePath))
                return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (System.IO.Directory.Exists(filePath))
                return filePath;
            try
            {
                dir = System.IO.Path.GetDirectoryName(filePath);
            }
            catch
            {
                dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            return dir;
        }

        /// <summary>
        /// ファイルパスから ファイル名を返す
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileName(string filePath)
        {
            var fileName = string.Empty;
            try
            {
                fileName = System.IO.Path.GetFileName(filePath);
            }
            catch
            {
            }
            return fileName;
        }

        /// <summary>
        /// 指定したファイル名 が有効かどうか検証
        /// <see cref="System.IO.Path.GetInvalidFileNameChars"/>で取得した文字が含まれる場合 false を返す
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool ValidateFileName(string fileName)
            => fileName?.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) < 0;

        /// <summary>
        /// 指定したファイルパスが有効かどうか検証
        /// <see cref="System.IO.Path.GetInvalidPathChars"/>で取得した文字が含まれる場合 false を返す
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool ValidateFilePath(string filePath)
            => filePath?.IndexOfAny(System.IO.Path.GetInvalidPathChars()) < 0;

        public static string GetUniqueFileName(string filePath)
        {
            var name = Path.GetFileNameWithoutExtension(filePath);
            var directory = Path.GetDirectoryName(filePath);
            var extension = Path.GetExtension(filePath);
            const string pattern1 = "(\\([0-9]+\\))$";
            const string pattern2 = "[0-9]+";

            while (File.Exists($"{directory}\\{name}{extension}"))
            {
                var value = Regex.Match(name, pattern1).Value;
                value = Regex.Match(value, pattern2).Value;
                int number;
                if (Int32.TryParse(value, out number))
                {
                    number++;
                    name = name.Replace($"({value})", $"({number.ToString()})");
                }
                else
                {
                    name = name + $" (2)";
                }
            }
            return $"{directory}\\{name}{extension}";
        }

        public static void CreateZip(string zipFileName, List<string> fileList)
        {
            ZipFile.Open(zipFileName, ZipArchiveMode.Create).Dispose();
            using (ZipArchive zipFile = ZipFile.Open(zipFileName, ZipArchiveMode.Update))
            {
                foreach (string fullPath in fileList)
                {
                    string fileName = Path.GetFileName(fullPath);

                    ZipArchiveEntry entry = zipFile.GetEntry(fileName);
                    if (entry != null) { entry.Delete(); }

                    zipFile.CreateEntryFromFile(fullPath, fileName);
                    File.Delete(fullPath);
                }
            }
        }

        public static void ArchivesAsZip(List<string> fileList, string path, string zipName, long? maxByte)
        {
            var tempList = new List<string>();
            var totalSize = 0L;

            for (var i = 0; i < fileList.Count; i++)
            {
                var size = new FileInfo(fileList[i]).Length;
                if (maxByte.HasValue && size > maxByte)
                {
                    if (tempList.Count > 0)
                    {
                        CreateZip(GetUniqueFileName($"{path}\\{zipName}.zip"),
                            tempList);
                    }

                    CreateZip(GetUniqueFileName($"{path}\\{zipName}.zip"),
                        new List<string>() { fileList[i] });

                    tempList = new List<string>();
                    totalSize = 0L;
                    continue;
                }

                totalSize += size;
                if (maxByte.HasValue && totalSize > maxByte)
                {
                    CreateZip(GetUniqueFileName($"{path}\\{zipName}.zip"), tempList);

                    tempList = new List<string>();
                    totalSize = 0L;
                }

                tempList.Add(fileList[i]);

                if (i == fileList.Count - 1
                    && tempList.Count > 0)
                {
                    CreateZip(GetUniqueFileName($"{path}\\{zipName}.zip"), tempList);
                }
            }
        }
        #endregion

        #region 動作環境の取得
        public static string GetRemoteHostName()
        {
            var hostName = Environment.GetEnvironmentVariable("CLIENTNAME");
            if (string.IsNullOrEmpty(hostName)) hostName = System.Net.Dns.GetHostName();
            return hostName;
        }
        #endregion
    }
}
