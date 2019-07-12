using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Common.Extensions;

namespace Rac.VOne.AccountTransfer.Import
{
    public class FixedFileReader
    {
        public IStreamCreator StreamCreator { get; set; } = new TextFileStreamCreator();
        public Encoding Encoding { get; set; } = Encoding.GetEncoding(932);

        private const string EndOfFile = "\u001a";
        public string[] Read(string file, int fixedSize = 0)
        {
            var lines = new List<string>();
            using (var stream = StreamCreator.Create(file, Encoding))
            using (var reader = new StreamReader(stream, Encoding))
            {
                while (0 <= reader.Peek())
                {
                    var line = reader.ReadLine();
                    if (line.Contains(EndOfFile)) continue;
                    lines.Add(line);
                }
            }
            return lines.ToArray();
        }
    }
}
