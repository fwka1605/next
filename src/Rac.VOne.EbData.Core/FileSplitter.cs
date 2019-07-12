using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.EbData
{
    public class FileSplitter : IFileSplitter
    {
        public Encoding Encoding { get; set; } = Encoding.GetEncoding(932); // shift-jis
        public ILineSplitter LineSplitter { get; set; }
        public bool IsPlainText { get; set; }

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
            var creator = IsPlainText ? (IStreamCreator)new PlainTextMemoryStreamCreator() : new TextFileStreamCreator();
            using (var stream = creator.Create(path, Encoding))
            using (var reader = new StreamReader(stream, Encoding))
            {
                while (0 <= reader.Peek())
                {
                    var line = reader.ReadLine();
                    if (line.Contains(EndOfFile))
                    {
                        continue;
                    }

                    var fields = LineSplitter?.Split(line).Select(field => field.Trim()).ToArray();

                    yield return fields;
                }
            }
        }

    }

}
