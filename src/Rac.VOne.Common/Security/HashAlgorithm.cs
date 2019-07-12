using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Rac.VOne.Common.Security
{
    public class HashAlgorithm : IHashAlgorithm
    {
        private readonly SHA256 _algorithm;

        public HashAlgorithm()
        {
            _algorithm = SHA256.Create();
        }
        public string Compute(string value)
        {
            var bytes = _algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
            return string.Concat(bytes.Select(x => x.ToString("X")));

        }
    }
}
