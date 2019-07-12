using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.EbData
{
    /// <summary>
    /// 複数レコードが改行なしで1行に結合された固定長フォーマットファイルのスプリッタ
    /// </summary>
    public class FileSplitterNoLineBreak : IFileSplitter
    {
        public Encoding Encoding { get; set; } = Encoding.GetEncoding(932); // shift-jis
        public ILineSplitter LineSplitter { get; set; }
        public bool IsPlainText { get; set; }

        /// <summary>固定レコード長</summary>
        /// <remarks>この長さで個々の行に分割する。</remarks>
        private int FixedRecordLength;

        public FileSplitterNoLineBreak(int fixedRecordLength = 200)
        {
            if (fixedRecordLength <= 0)
            {
                throw new ArgumentOutOfRangeException($"recordLength = {fixedRecordLength}");
            }

            FixedRecordLength = fixedRecordLength;
        }

        public Tuple<IEnumerable<string[]>, int, ImportResult> Split(string path)
        {
            var lines = (IEnumerable<string[]>)null;
            var fileSizeBytes = 0;
            var result = ImportResult.Success;

            if (!IsPlainText && !File.Exists(path))
            {
                return Tuple.Create(lines, fileSizeBytes, ImportResult.FileNotFound);
            }

            try
            {
                lines = SplitLine(path);
                fileSizeBytes = IsPlainText ? Encoding.GetByteCount(path) : (int)(new FileInfo(path)).Length;
            }
            catch
            {
                result = ImportResult.FileReadError;
            }

            return Tuple.Create(lines, fileSizeBytes, result);
        }

        private const string EndOfFile = "\u001a";

        private IEnumerable<string[]> SplitLine(string path)
        {
            var creator = IsPlainText ?
                (IStreamCreator)new PlainTextMemoryStreamCreator() :
                                new TextFileStreamCreator();
            using (var stream = creator.Create(path, Encoding))
            using (var reader = new StreamReader(stream, Encoding))
            {
                while (0 <= reader.Peek())
                {
                    var value = reader.ReadLine();
                    if (value.Contains(EndOfFile))
                    {
                        continue;
                    }

                    var lines = Split(value, FixedRecordLength);

                    foreach (var line in lines)
                    {
                        var fields = LineSplitter?.Split(line).Select(field => field.Trim());

                        yield return fields.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// 文字列を指定文字数で分割する。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="chunkSize">分割文字数(バイト数ではない)</param>
        /// <returns></returns>
        /// <remarks>
        /// 分割後の文字列は全て同じ長さである前提。そうでない(最後の文字列だけ文字数が足りない)場合は例外がスローされる。
        /// ※ 2バイト文字が含まれる場合は、実装を変更する必要がある
        /// </remarks>
        private IEnumerable<string> Split(string value, int chunkSize)
        {
            for (int i = 0; i < value.Length; i += chunkSize)
            {
                yield return value.Substring(i, chunkSize);
            }
        }
    }
}
