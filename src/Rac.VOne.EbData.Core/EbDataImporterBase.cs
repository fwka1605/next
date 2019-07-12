using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.EbData
{
    public class EbDataImporterBase : IDisposable
    {
        public readonly Helper Helper = new Helper();

        public int Year
        {
            get { return Helper.Year; }
            set { Helper.Year = value; }
        }

        /// <summary>非同期処理を行うかどうか</summary>
        public bool IsAsync
        {
            set { Helper.IsAsync = value; }
        }

        /// <summary>連携されるデータ が直接EBデータかファイルパスかどうか
        /// <see cref="IFileSplitter.IsPlainText"/> へ 影響を与える 既定値は false
        /// </summary>
        public bool IsPlainText { get; set; }

        public List<FileInformation> ReadFiles(List<FileInformation> files)
        {
            Helper.Initialize?.Invoke();
            for (var i = 0; i < files.Count(); i++)
            {
                var file = files[i];
                var target = IsPlainText ? file.File : file.Path;

                try
                {
                    var splitter = CreateSplitter(file);
                    var splitResult = splitter.Split(target);
                    if (splitResult.Item3 != ImportResult.Success)
                    {
                        file.Result = splitResult.Item3;
                        continue;
                    }
                    file.Size = splitResult.Item2;
                    var parser = CreateParser(file);
                    var parseResult = parser.ParseAsync(splitResult.Item1).Result;
                    file.ImportFileLog = parseResult.Item1;
                    if (!Helper.UseApportion)
                    {
                        foreach (var header in file.ImportFileLog.ReceiptHeaders)
                            header.AssignmentFlag = header.Receipts.All(x => x.ExcludeFlag == 1)
                                ? 1 : 0;
                    }
                    file.Result = parseResult.Item2;
                }
                catch
                {
                    var list = new List<FileInformation>();
                    file = new FileInformation { Result = ImportResult.FileReadError };
                    continue;
                }
            }
            return files;
        }

        public async Task<List<FileInformation>> ReadFilesAsync(List<FileInformation> files, CancellationToken token = default(CancellationToken))
        {
            await Helper.InitializeAsync?.Invoke(token);
            for (var i = 0; i < files.Count(); i++)
            {
                var file = files[i];
                var target = IsPlainText ? file.File : file.Path;
                try
                {
                    var splitter = CreateSplitter(file);
                    var splitResult = splitter.Split(target);
                    if (splitResult.Item3 != ImportResult.Success)
                    {
                        file.Result = splitResult.Item3;
                        continue;
                    }
                    file.Size = splitResult.Item2;
                    var parser = CreateParser(file);
                    var parseResult = await parser.ParseAsync(splitResult.Item1);
                    file.ImportFileLog = parseResult.Item1;
                    if (!Helper.UseApportion)
                    {
                        foreach (var header in file.ImportFileLog.ReceiptHeaders)
                            header.AssignmentFlag = header.Receipts.All(x => x.ExcludeFlag == 1)
                                ? 1 : 0;
                    }
                    file.Result = parseResult.Item2;
                }
                catch
                {
                    var list = new List<FileInformation>();
                    file = new FileInformation { Result = ImportResult.FileReadError };
                    continue;
                }
            }
            return files;

        }


        private IFileSplitter CreateSplitter(FileInformation information)
        {
            const string tabDelimiter = "\t";
            var format = information.Format;
            if (information.FileFieldType == FileFieldType.CommaDelimited)
                return new FileSplitterCsvParser { IsPlainText = IsPlainText };
            if (information.FileFieldType == FileFieldType.TabDelimited)
                return new FileSplitterCsvParser {
                    IsPlainText = IsPlainText,
                    Delimiter   = tabDelimiter,
                };


            var lineSplitter
                = format == EbFileFormat.ZenginNyukinMeisai ? new LineSplitterZenginNyukinMeisai()
                : format == EbFileFormat.ZenginNyusyukkin   ? new LineSplitterZenginNyusyukkin()
                : format == EbFileFormat.JPBank             ? new LineSplitterJPBank()
                : (ILineSplitter)null;

            if (lineSplitter == null) throw new ArgumentException();

            if (information.FileFieldType == FileFieldType.FixedWidth)
                return new FileSplitter {
                    IsPlainText     = IsPlainText,
                    LineSplitter    = lineSplitter,
                };

            if (information.FileFieldType == FileFieldType.FixedWidthNoLineBreak)
                return new FileSplitterNoLineBreak {
                    IsPlainText     = IsPlainText,
                    LineSplitter    = lineSplitter,
                };

            throw new ArgumentException();
        }

        private IParser CreateParser(FileInformation info)
        {
            IParser parser = null;
            var format = info.Format;
            if (format == EbFileFormat.ZenginNyukinMeisai               ) parser = new ParserZenginNyukinMeisai();
            if (format == EbFileFormat.ZenginNyusyukkin                 ) parser = new ParserZenginNyusyukkin();
            if (format == EbFileFormat.Anser                            ) parser = new ParserAnser();
            if (format == EbFileFormat.BizStationZenmeisai              ) parser = new ParserBizStationZenmeisai    { BankCode = info.BankCode };
            if (format == EbFileFormat.BizStationNyukinMeisai           ) parser = new ParserBizStationNyukinMeisai { BankCode = info.BankCode };
            if (format == EbFileFormat.JPBank                           ) parser = new ParserJPBank();
            if (format == EbFileFormat.RegionalBank                     ) parser = new ParserRegionalBankNyusyukkin { BankCode = info.BankCode };
            if (format == EbFileFormat.MoneyShooterNyusyukkin           ) parser = new ParserMoneyShooterNyusyukkin();
            if (format == EbFileFormat.ULineXtraVer2                    ) parser = new ParserULineXtraVer2();
            if (format == EbFileFormat.KitaNipponBankBiznetNyusyukkin   ) parser = new ParserKitaNipponBankNyusyukkin();
            if (format == EbFileFormat.DaishBank                        ) parser = new ParserDaishiBank();
            if (format == EbFileFormat.JyouyouBankNyusyukkin            ) parser = new ParserBizStationNyukinMeisai { BankCode = info.BankCode, Offset = 1 };
            if (format == EbFileFormat.KewpieNetNyusyukkin              ) parser = new ParserKewpieNetNyusyukkin();

            if (parser == null) throw new ArgumentException();
            parser.Helper = this.Helper;
            parser.FileInformation = info;
            return parser;
        }

        public List<FileInformation> SaveFiles(List<FileInformation> files)
        {
            var models = files.Select(x => x.ImportFileLog).ToList();
            List<ImportFileLog> results = null;
            try
            {
                results = Helper.SaveData(models);
            }
            catch (Exception)
            {

            }
            if (results != null && results.Count == files.Count)
            {
                return files.Zip(results, (file, result) =>
                {
                    file.ImportFileLog = result;
                    return file;
                }).ToList();
            }
            return files.Select(x =>
            {
                x.Result = ImportResult.DBError;
                return x;
            }).ToList();
        }

        public async Task<List<FileInformation>> SaveFilesAsync(List<FileInformation> files, CancellationToken token = default(CancellationToken))
        {
            var models = files.Select(x => x.ImportFileLog).ToList();
            List<ImportFileLog> results = null;
            try
            {
                results = await Helper.SaveDataAsync(models, token);
            }
            catch (Exception)
            {

            }
            if (results != null && results.Count == files.Count)
            {
                return files.Zip(results, (file, result) =>
                {
                    file.ImportFileLog = result;
                    return file;
                }).ToList();
            }
            return files.Select(x =>
            {
                x.Result = ImportResult.DBError;
                return x;
            }).ToList();
        }

        public List<FileInformation> ReadAndSaveFiles(List<FileInformation> files)
        {
            var readResult = ReadFiles(files);
            if (readResult.Any(x => x.Result != ImportResult.Success))
                return readResult;
            var saveResult = SaveFiles(readResult);
            return saveResult;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            disposed = true;

            if (disposing)
            {
                Helper.Dispose();
            }
        }

        ~EbDataImporterBase()
        {
            Dispose(false);
        }
    }
}
