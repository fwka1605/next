using System.IO;
using System.Text;

namespace Rac.VOne.Common.DataHandling
{
    public class PlainTextMemoryStreamCreator : IStreamCreator
    {
        public Stream Create(string source, Encoding encoding) => new MemoryStream(encoding.GetBytes(source));

    }
}
