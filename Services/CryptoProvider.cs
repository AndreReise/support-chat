using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Extensions;

namespace TechnicalSupport.Services
{
    public class CryptoProvider : ICryptoProvider
    {
        private readonly byte[] g_salt;
        public CryptoProvider()
        {
            //set g_salt
        }

        public byte[] GetPasswordHash(string str_password, byte[]  l_salt)
        {
            byte[] pass_bytes = Encoding.UTF8.GetBytes(str_password);

            var preHash = GetSHA256Hash(pass_bytes.AddBytes(l_salt));

            return GetSHA256Hash( preHash.AddBytes(g_salt) );
        }

        public byte[] GetRandomSaltString(int str_length)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            byte[] random_bytes = new byte[str_length];
            rng.GetBytes(random_bytes);

            return random_bytes;
        }

        public byte[] GetSHA256Hash(byte[] target)
        {
            using var encoder = SHA256Managed.Create();

            byte[] hash = encoder.ComputeHash(target);
            return hash;
        }
    }
}
