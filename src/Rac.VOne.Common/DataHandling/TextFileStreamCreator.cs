using System.IO;
using System.Text;

namespace Rac.VOne.Common.DataHandling
{
    public class TextFileStreamCreator : IStreamCreator
    {
        public Stream Create(string source, Encoding encoding) => File.OpenRead(source);
    }
}
