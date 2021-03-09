using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalSupport.Services
{
    public interface ICryptoProvider
    {
        public byte[] GetPasswordHash(string str_password, byte[] l_salt);
        public byte[] GetRandomSaltString();
        public byte[] GetSHA256Hash(byte[] target);
        public byte[] GetTokenBytes(string str_token);
    }
}
