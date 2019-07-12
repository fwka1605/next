using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.EbData
{
    public class FileSplitterCsvParser : IFileSplitter
    {
        public Encoding Encoding { get; set; } = Encoding.GetEncoding(932);
        public string Delimiter { get; set; } = ",";
        public bool IsPlainText { get; set; }

        public Tuple<IEnumerable<string[]>, int, ImportResult> Split(string path)
        {
            var lines = new List<string[]>();
            var size = 0;
            var result = ImportResult.Success;
            try
            {
                if (!IsPlainText && !File.Exists(path))
                {
                    return Tuple.Create(lines.AsEnumerable(), size, ImportResult.FileNotFound);
                }
                IStreamCreator creator = IsPlainText ?
                    (IStreamCreator)new PlainTextMemoryStreamCreator() :
                                    new TextFileStreamCreator();
                var parser = new CsvParser {
                    Delimiter       = Delimiter,
                    StreamCreator   = creator,
                };
                lines = parser.Parse(path).ToList();
                size = IsPlainText ? Encoding.GetByteCount(path) : (int)(new FileInfo(path)).Length;
            }
            catch
            {
                result = ImportResult.FileReadError;
            }
            return Tuple.Create(lines.AsEnumerable(), size, result);
        }
    }
}
