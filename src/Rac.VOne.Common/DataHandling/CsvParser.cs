using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace Rac.VOne.Common.DataHandling
{
    public class CsvParser : ICsvParser
    {
        public Encoding Encoding { get; set; } = Encoding.GetEncoding(932);
        public string Delimiter { get; set; } = ",";
        public bool IgnoreBlankLines { get; set; } = true;

        public IStreamCreator StreamCreator { get; set; }
        public CsvParser()
        {
            StreamCreator = new TextFileStreamCreator();
        }

        public IEnumerable<string[]> Parse(string csv)
        {
            using (var stream = StreamCreator.Create(csv, Encoding))
            using (var reader = new System.IO.StreamReader(stream, Encoding))
            using (var parser = new CsvHelper.CsvParser(reader, new Configuration {
                IgnoreBlankLines    = IgnoreBlankLines,
                Delimiter           = Delimiter,
                TrimOptions         = TrimOptions.Trim | TrimOptions.InsideQuotes,
            }))
            {
                string[] fields;
                while ((fields = parser.Read()) !=  null)
                    yield return fields;
            }
        }
    }
}
