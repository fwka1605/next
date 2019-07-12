using System.IO;
using System.Text;

namespace Rac.VOne.Common.DataHandling
{
    public interface IStreamCreator
    {
        Stream Create(string source, Encoding encoding);
    }
}
