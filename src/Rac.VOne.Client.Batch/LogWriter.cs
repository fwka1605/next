using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Batch
{
    public class LogWriter
    {
        /// <summary>
        ///  失敗時 移動フォルダ
        /// </summary>
        public string FailureDirectory { private get; set; }

        public string GetOutputPath()
            => Path.Combine(GetOutputDirectory(), LogFileName);

        private string GetOutputDirectory()
            => (string.IsNullOrEmpty(FailureDirectory)
            || !Directory.Exists(FailureDirectory))
            ? GetUserFolder() : FailureDirectory;
        private Encoding Encoding { get; } = Encoding.GetEncoding(932);
        private string LogFileName { get; } = $"{DateTime.Today:yyyyMMdd}_Import.log";
        private string GetUserFolder()
            => Screen.Extensions.ImportSettingExtensions.GetUserFolder();

        public LogWriter() { }
        private List<string> LogCache { get; } = new List<string>();

        #region logging
        public void Log(string message)
            => LogCache.Add(message);
        public void Log(IEnumerable<string> messages)
            => LogCache.AddRange(messages);
        public void Log(Exception ex)
            => Log($"エラーが発生しました。{Environment.NewLine}{ex.Message}");

        public void WriteLog()
            => WriteLog(LogCache);
        private void WriteLog(IEnumerable<string> messages)
        {
            if (!(messages?.Any() ?? false)) return;

            var path = GetOutputPath();
            var exist = File.Exists(path);
            try
            {
                using (var writer = new StreamWriter(path, true, Encoding))
                {
                    if (exist) writer.WriteLine();
                    writer.WriteLine($"{DateTime.Now:yyyy年MM月dd日 HH時mm分ss秒}");
                    foreach (var line in messages)
                        writer.WriteLine(line);
                }
            }
            catch { }
        }

        #endregion
    }
}
