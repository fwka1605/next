using System;
using System.Security.Cryptography;
using System.Text;

namespace Rac.VOne.Common.Security
{
    public class License
    {
        private const string decryptCodeHeader = "RAC";
        private const string password = "randac";

        public static string GetLicenseKey(string productKey, int num ) {
            var decryptCode = decryptCodeHeader + " " + productKey + " " + String.Format("{0:D5}", num);
            var provider = new RC2CryptoServiceProvider();
            byte[] key = null;
            byte[] iv = null;
            GenerateKeyFromPassword(password, provider.KeySize, ref key, provider.BlockSize, ref iv);
            provider.Key = key;
            provider.IV = iv;

            var str = decryptCode;
            var strBytes = System.Text.Encoding.UTF8.GetBytes(str);
            var encryptor = provider.CreateEncryptor(key, iv);
            var encBytes = encryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);
            encryptor.Dispose();
            return Convert.ToBase64String(encBytes);
        }

        private static void GenerateKeyFromPassword(
            string password,
            int keySize,
            ref byte[] key,
            int blockSize,
            ref byte[] iv)
        {
            byte[] salt = Encoding.UTF8.GetBytes("xxxxxxxx");
            var deriveBytes = new Rfc2898DeriveBytes(password, salt);
            deriveBytes.IterationCount = 1000;
            key = deriveBytes.GetBytes(keySize / 8);
            iv = deriveBytes.GetBytes(blockSize / 8);
        }

        public static bool CheckDecryptCode(string licenseKey, string productKey)
        {
            var decryptCode = rc2_decrypt(licenseKey);
            if (string.IsNullOrEmpty(decryptCode)) return false;
            if (decryptCode.StartsWith(decryptCodeHeader)
                && decryptCode.Substring(4, 7) == productKey
                && IsNumber(decryptCode.Substring(12, 5)))
            {
                return true;
            }
            return false;
        }

        private static bool IsNumber(object value)
        {
            var strVal = value == null ? string.Empty : Convert.ToString(value);
            var result = 0M;
            return decimal.TryParse(strVal, out result);
        }

        private static string rc2_decrypt(string licenseKey)
        {
            try
            {
                var provider = new RC2CryptoServiceProvider();
                byte[] key = null;
                byte[] iv = null;
                GenerateKeyFromPassword(password, provider.KeySize, ref key, provider.BlockSize, ref iv);
                provider.Key = key;
                provider.IV = iv;

                var strByte = Convert.FromBase64String(licenseKey);
                var decryptor = provider.CreateDecryptor();
                var decBytes = decryptor.TransformFinalBlock(strByte, 0, strByte.Length);
                decryptor.Dispose();

                return Encoding.UTF8.GetString(decBytes);
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
