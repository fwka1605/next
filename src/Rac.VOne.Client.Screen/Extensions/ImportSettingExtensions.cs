using Rac.VOne.Web.Models;
using System;
using System.IO;

namespace Rac.VOne.Client.Screen.Extensions
{
    public static class ImportSettingExtensions
    {
        /// <summary>エラーログ出力用 ディレクトリ取得</summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static string GetErrorLogDirectory(this ImportSetting setting)
            => setting.ExportErrorLog == 1
                ? setting.ErrorLogDestination == 0
                ? GetUserFolder()
                : Path.GetDirectoryName(setting.ImportFileName) 
            : "";

        /// <summary>エラーログ出力用 ファイルパス取得</summary>
        /// <param name="setting"></param>
        /// <returns>
        /// <see cref="ImportSetting.ErrorLogDestination"/>にて指定されたディレクトリ +
        /// yyyyMMdd_Import.log のファイルパス
        /// </returns>
        public static string GetErrorLogPath(this ImportSetting setting)
            => setting.ExportErrorLog == 1
            ? Path.Combine(GetErrorLogDirectory(setting), $"{DateTime.Today:yyyyMMdd}_Import.log")
            : "";

        /// <summary>
        /// ユーザーフォルダ取得
        /// MyDocuments\RAC\V-One\Log
        /// </summary>
        /// <returns></returns>
        public static string GetUserFolder()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "RAC\\V-One\\Log");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

    }
}
